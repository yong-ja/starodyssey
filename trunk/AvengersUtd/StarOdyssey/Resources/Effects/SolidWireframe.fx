//----------------------------------------------------------------------------------
// File:   SolidWireframe.fx
// Author: Samuel Gateau
// Email:  sdkfeedback@nvidia.com
// 
// Copyright (c) 2007 NVIDIA Corporation. All rights reserved.
//
// TO  THE MAXIMUM  EXTENT PERMITTED  BY APPLICABLE  LAW, THIS SOFTWARE  IS PROVIDED
// *AS IS*  AND NVIDIA AND  ITS SUPPLIERS DISCLAIM  ALL WARRANTIES,  EITHER  EXPRESS
// OR IMPLIED, INCLUDING, BUT NOT LIMITED  TO, IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL  NVIDIA OR ITS SUPPLIERS
// BE  LIABLE  FOR  ANY  SPECIAL,  INCIDENTAL,  INDIRECT,  OR  CONSEQUENTIAL DAMAGES
// WHATSOEVER (INCLUDING, WITHOUT LIMITATION,  DAMAGES FOR LOSS OF BUSINESS PROFITS,
// BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY LOSS)
// ARISING OUT OF THE  USE OF OR INABILITY  TO USE THIS SOFTWARE, EVEN IF NVIDIA HAS
// BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
//
//
//----------------------------------------------------------------------------------
#include <Include/Common.hlsl>
#include <Include/Geometry.hlsl>
#include <Include/Materials.hlsl>

DepthStencilState DSSDepthWriteLess
{
  DepthEnable = true;
  DepthWriteMask = All;
  DepthFunc = Less;
};

DepthStencilState DSSDepthLessEqual
{
  DepthEnable = true;
  DepthWriteMask = 0x00;
  DepthFunc = Less_Equal;
};

RasterizerState RSFill
{
    FillMode = SOLID;
    CullMode = None;
    DepthBias = false;
    MultisampleEnable = true;
};

RasterizerState RSFillBiasBack
{
    FillMode = SOLID;
    CullMode = None;
    DepthBias = 1000;
    DepthBiasClamp = 10000;
    SlopeScaledDepthBias = 2;
    MultisampleEnable = true;
};

RasterizerState RSWireframe
{
    FillMode = WIREFRAME;
    CullMode = None;
    DepthBias = false;
    AntialiasedLineEnable = false;
    MultisampleEnable = true;
};

RasterizerState RSWireframeAntialiased
{
    FillMode = WIREFRAME;
    CullMode = None;
    DepthBias = false;
    AntialiasedLineEnable = true;
    MultisampleEnable = true;
};

BlendState BSNoColor
{
    BlendEnable[0] = false;
    RenderTargetWriteMask[0] = 0x00;
};

BlendState BSBlending
{
    BlendEnable[0] = TRUE;
    SrcBlend = SRC_ALPHA;
    DestBlend = INV_SRC_ALPHA ;
    BlendOp = ADD;
    SrcBlendAlpha = SRC_ALPHA;
    DestBlendAlpha = DEST_ALPHA;
    BlendOpAlpha = ADD;
    RenderTargetWriteMask[0] = 0x0F;
};

//--------------------------------------------------------------------------------------
// Constant Buffer Variables
//--------------------------------------------------------------------------------------

float4 LightVector = float4( 0, 0, 1, 0);

float LineWidth = 2.5;
float FadeDistance = 50;
float PatternPeriod = 1.5;

float4 FillColor = float4(0.1, 0.2, 0.4, 1);
//float4 WireColor = float4(1, 1, 1, 1);
float4 PatternColor = float4(1, 1, 0.5, 1);

uint infoA[]     = { 0, 0, 0, 0, 1, 1, 2 };
uint infoB[]     = { 1, 1, 2, 0, 2, 1, 2 };
uint infoAd[]    = { 2, 2, 1, 1, 0, 0, 0 };
uint infoBd[]    = { 2, 2, 1, 2, 0, 2, 1 }; 
uint infoEdge0[] = { 0, 2, 0, 0, 0, 0, 2 }; 

float4 ColorCases[] = {
    { 1, 1, 1, 1 }, 
    { 1, 1, 0, 1 },
    { 1, 0, 1, 1 },
    { 1, 0, 0, 1 },
    { 0, 1, 1, 1 },
    { 0, 1, 0, 1 },
    { 0, 0, 1, 1 }
}; 


//--------------------------------------------------------------------------------------

struct VSInput
{
    float3 Position  : POSITION;
    float3 Normal	 : NORMAL;
	float3 TextureUV : TEXCOORD0;
};

struct GSInput
{
    float4 Position  : POSITION;
    float4 PositionV : TEXCOORD0;
};

struct PSInput
{
    float4 Position : SV_POSITION;
    float4 Color : TEXCOORD0;
};

struct PSInputWire
{
    float4 Position : SV_POSITION;
    float4 Color : TEXCOORD0;
    noperspective float4 EdgeA: TEXCOORD1;
    noperspective float4 EdgeB: TEXCOORD2;
    uint Case : TEXCOORD3;
};

//--------------------------------------------------------------------------------------
// Utils Funtions
//--------------------------------------------------------------------------------------

// Compute the final color of a face depending on its facing of the light
float4 shadeFace(in float4 verA, in float4 verB, in float4 verC)
{
    // Compute the triangle face normal in view frame
    float3 normal = FaceNormal(verA, verB, verC);
    
    // Then the color of the face.
    float shade = 0.5*abs( dot( normal, LightVector) );
    
    return float4(FillColor.xyz*shade, 1);
}

//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
GSInput VS( VSInput input )
{
    GSInput output;
    output.Position = ModelToProj( float4(input.Position,1));
    output.PositionV = ModelToView( float4(input.Position,1));
    return output;
}

//--------------------------------------------------------------------------------------
// Geometry Shader
//--------------------------------------------------------------------------------------

[maxvertexcount(3)] 
void GS( triangle GSInput input[3],
         inout TriangleStream<PSInput> outStream )
{
    PSInput output;

    // Shade and colour face.
    output.Color = shadeFace(input[0].PositionV, input[1].PositionV, input[2].PositionV);

    output.Position = input[0].Position;
    outStream.Append( output );

    output.Position = input[1].Position;
    outStream.Append( output );

    output.Position = input[2].Position;
    outStream.Append( output );

    outStream.RestartStrip();
}

[maxvertexcount(3)]
void GSSolidWire( triangle GSInput input[3],
                         inout TriangleStream<PSInputWire> outStream )
{
    PSInputWire output;

    // Compute the case from the positions of point in space.
    output.Case = (input[0].Position.z < 0)*4 + (input[1].Position.z < 0)*2 + (input[2].Position.z < 0); 

    // If case is all vertices behind viewpoint (case = 7) then cull.
    if (output.Case == 7) return;

    // Shade and colour face just for the "all in one" technique.
    output.Color = shadeFace(input[0].PositionV, input[1].PositionV, input[2].PositionV);

   // Transform position to window space
    float2 points[3];
    points[0] = projToWindow(input[0].Position);
    points[1] = projToWindow(input[1].Position);
    points[2] = projToWindow(input[2].Position);

    // If Case is 0, all projected points are defined, do the
    // general case computation
    if (output.Case == 0) 
    {
        output.EdgeA = float4(0,0,0,0);
        output.EdgeB = float4(0,0,0,0);

        // Compute the edges vectors of the transformed triangle
        float2 edges[3];
        edges[0] = points[1] - points[0];
        edges[1] = points[2] - points[1];
        edges[2] = points[0] - points[2];

        // Store the length of the edges
        float lengths[3];
        lengths[0] = length(edges[0]);
        lengths[1] = length(edges[1]);
        lengths[2] = length(edges[2]);

        // Compute the cos angle of each vertices
        float cosAngles[3];
        cosAngles[0] = dot( -edges[2], edges[0]) / ( lengths[2] * lengths[0] );
        cosAngles[1] = dot( -edges[0], edges[1]) / ( lengths[0] * lengths[1] );
        cosAngles[2] = dot( -edges[1], edges[2]) / ( lengths[1] * lengths[2] );

        // The height for each vertices of the triangle
        float heights[3];
        heights[1] = lengths[0]*sqrt(1 - cosAngles[0]*cosAngles[0]);
        heights[2] = lengths[1]*sqrt(1 - cosAngles[1]*cosAngles[1]);
        heights[0] = lengths[2]*sqrt(1 - cosAngles[2]*cosAngles[2]);

        float edgeSigns[3];
        edgeSigns[0] = (edges[0].x > 0 ? 1 : -1);
        edgeSigns[1] = (edges[1].x > 0 ? 1 : -1);
        edgeSigns[2] = (edges[2].x > 0 ? 1 : -1);

        float edgeOffsets[3];
        edgeOffsets[0] = lengths[0]*(0.5 - 0.5*edgeSigns[0]);
        edgeOffsets[1] = lengths[1]*(0.5 - 0.5*edgeSigns[1]);
        edgeOffsets[2] = lengths[2]*(0.5 - 0.5*edgeSigns[2]);

        output.Position =( input[0].Position );
        output.EdgeA[0] = 0;
        output.EdgeA[1] = heights[0];
        output.EdgeA[2] = 0;
        output.EdgeB[0] = edgeOffsets[0];
        output.EdgeB[1] = edgeOffsets[1] + edgeSigns[1] * cosAngles[1]*lengths[0];
        output.EdgeB[2] = edgeOffsets[2] + edgeSigns[2] * lengths[2];
        outStream.Append( output );

        output.Position = ( input[1].Position );
        output.EdgeA[0] = 0;
        output.EdgeA[1] = 0;
        output.EdgeA[2] = heights[1];
        output.EdgeB[0] = edgeOffsets[0] + edgeSigns[0] * lengths[0];
        output.EdgeB[1] = edgeOffsets[1];
        output.EdgeB[2] = edgeOffsets[2] + edgeSigns[2] * cosAngles[2]*lengths[1];
        outStream.Append( output );

        output.Position = ( input[2].Position );
        output.EdgeA[0] = heights[2];
        output.EdgeA[1] = 0;
        output.EdgeA[2] = 0;
        output.EdgeB[0] = edgeOffsets[0] + edgeSigns[0] * cosAngles[0]*lengths[2];
        output.EdgeB[1] = edgeOffsets[1] + edgeSigns[1] * lengths[1];
        output.EdgeB[2] = edgeOffsets[2];
        outStream.Append( output );

        outStream.RestartStrip();
    }
    // Else need some tricky computations
    else
    {
        // Then compute and pass the edge definitions from the case
        output.EdgeA.xy = points[ infoA[output.Case] ];
        output.EdgeB.xy = points[ infoB[output.Case] ];

		output.EdgeA.zw = normalize( output.EdgeA.xy - points[ infoAd[output.Case] ] ); 
        output.EdgeB.zw = normalize( output.EdgeB.xy - points[ infoBd[output.Case] ] );
		
		// Generate vertices
        output.Position =( input[0].Position );
        outStream.Append( output );
     
        output.Position = ( input[1].Position );
        outStream.Append( output );

        output.Position = ( input[2].Position );
        outStream.Append( output );

        outStream.RestartStrip();
    }
}
//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 PSColor( PSInput input) : SV_Target
{
    return input.Color;
}

float4 PSDXWire( PSInput input) : SV_Target
{
    return cMaterialDiffuse;
}

float evalMinDistanceToEdges(in PSInputWire input)
{
    float dist;

    // The easy case, the 3 distances of the fragment to the 3 edges is already
    // computed, get the min.
    if (input.Case == 0)
    {
        dist = min ( min (input.EdgeA.x, input.EdgeA.y), input.EdgeA.z);
    }
    // The tricky case, compute the distances and get the min from the 2D lines
    // given from the geometry shader.
    else
    {
        // Compute and compare the sqDist, do one sqrt in the end.
        	
        float2 AF = input.Position.xy - input.EdgeA.xy;
        float sqAF = dot(AF,AF);
        float AFcosA = dot(AF, input.EdgeA.zw);
        dist = abs(sqAF - AFcosA*AFcosA);

        float2 BF = input.Position.xy - input.EdgeB.xy;
        float sqBF = dot(BF,BF);
        float BFcosB = dot(BF, input.EdgeB.zw);
        dist = min( dist, abs(sqBF - BFcosB*BFcosB) );
       
        // Only need to care about the 3rd edge for some cases.
        if (input.Case == 1 || input.Case == 2 || input.Case == 4)
        {
            float AFcosA0 = dot(AF, normalize(input.EdgeB.xy - input.EdgeA.xy));
			dist = min( dist, abs(sqAF - AFcosA0*AFcosA0) );
	    }

        dist = sqrt(dist);
    }

    return dist;
}

float4 PSSolidWire( PSInputWire input) : SV_Target
{
    // Compute the shortest distance between the fragment and the edges.
    float dist = evalMinDistanceToEdges(input);

    // Cull fragments too far from the edge.
    if (dist > 0.5*LineWidth+1) discard;

    // Map the computed distance to the [0,2] range on the border of the line.
    dist = clamp((dist - (0.5*LineWidth - 1)), 0, 2);

    // Alpha is computed from the function exp2(-2(x)^2).
    dist *= dist;
    float alpha = exp2(-2*dist);

    // Standard wire color
    float4 color = cMaterialDiffuse;
    color.a *= alpha;
	
    return color;
}

float4 PSSolidWireFade( PSInputWire input) : SV_Target
{
    // Compute the shortest square distance between the fragment and the edges.
    float dist = evalMinDistanceToEdges(input);

    // Cull fragments too far from the edge.
    if (dist > 0.5*LineWidth+1) discard;

    // Map the computed distance to the [0,2] range on the border of the line.
    dist = clamp((dist - (0.5*LineWidth - 1)), 0, 2);

    // Alpha is computed from the function exp2(-2(x)^2).
    dist *= dist;
    float alpha = exp2(-2*dist);

    // Standard wire color but faded by distance
    // Dividing by pos.w, the depth in view space
    float fading = clamp(FadeDistance / input.Position.w, 0, 1);

    float4 color = cMaterialDiffuse * fading;
    color.a *= alpha;
    return color;
}

float det( float2 a, float2 b )
{
	return (a.x*b.y - a.y*b.x);
}
float evalMinDistanceToEdgesExt(  in PSInputWire input, 
                                  out float3 edgeSqDists, 
                                  out float3 edgeCoords, 
                                  out uint3 edgeOrder)
{
    // The easy case, the 3 distances of the fragment to the 3 edges is already
    // computed, get the min.
    if (input.Case == 0)
    {
        edgeSqDists = input.EdgeA.xyz * input.EdgeA.xyz;
        edgeCoords = input.EdgeB.xyz;
        edgeOrder = uint3(0, 1, 2);

        if (edgeSqDists[1] < edgeSqDists[0])
        {
            edgeOrder.xy = edgeOrder.yx;
        }
        if (edgeSqDists[2] < edgeSqDists[edgeOrder.y])
        {
            edgeOrder.yz = edgeOrder.zy;
        }
        if (edgeSqDists[2] < edgeSqDists[edgeOrder.x])
        {
            edgeOrder.xy = edgeOrder.yx;
        }
    }
    // The tricky case, compute the distances and get the min from the 2D lines
    // given from the geometry shader.
    else
    {
        float2 AF = input.Position.xy - input.EdgeA.xy;
        float sqAF = dot(AF,AF);
        float AFcosA = dot(AF, input.EdgeA.zw);
        edgeSqDists[0] = abs(sqAF - AFcosA*AFcosA);
        edgeOrder = uint3(0, 1, 2);
        edgeCoords[0] = abs(AFcosA);

        float2 BF = input.Position.xy - input.EdgeB.xy;
        float sqBF = dot(BF,BF);
        float BFcosB = dot(BF, input.EdgeB.zw);
        edgeSqDists[1] = abs(sqBF - BFcosB*BFcosB);
        edgeCoords[1] = abs(BFcosB);

        if (edgeSqDists[1] < edgeSqDists[0])
        {
            edgeOrder.xy = edgeOrder.yx;
        }

        if (input.Case == 1 || input.Case == 2 || input.Case == 4)
        {
            float AFcosA0 = dot(AF, normalize(input.EdgeB.xy - input.EdgeA.xy));         
            edgeSqDists[2] = abs(sqAF - AFcosA0*AFcosA0);
            edgeCoords[2] = abs(AFcosA0);

            if (edgeSqDists[2] < edgeSqDists[edgeOrder.y])
            {
                edgeOrder.yz = edgeOrder.zy;
            }
            
            if (edgeSqDists[2] < edgeSqDists[edgeOrder.x])
            {
                edgeOrder.xy = edgeOrder.yx;
            }
        }
        else
        {
			edgeSqDists[2] = 0;
			edgeCoords[2] = 0;
		}
    }

    return sqrt(edgeSqDists[edgeOrder.x]);
}

float4 PSSolidWirePattern( PSInputWire input) : SV_Target
{
    // Compute the shortest square distance between the fragment and the edges.
    float3 edgeSqDists;
    float3 edgeCoords;
    uint3 edgeOrder;
    float dist = evalMinDistanceToEdgesExt(input, edgeSqDists, edgeCoords, edgeOrder);

    // Standard wire color
    float4 color = cMaterialDiffuse;
    float realLineWidth = 0.5*LineWidth;

    // Except if on the diagonal edge
    if ( infoEdge0[input.Case] == edgeOrder.x )
    {
        if (dist > LineWidth+1) discard;

        float patternPos = (abs(edgeCoords[edgeOrder.x]) % (PatternPeriod*2*LineWidth)) - LineWidth;
        dist = (patternPos*patternPos + dist*dist);

        color = PatternColor;
        realLineWidth = LineWidth;

        // Filling the corners near the vertices with the WireColor
        if ( edgeSqDists[edgeOrder.y] < pow(0.5*LineWidth+1, 2) )
        {
            dist = edgeSqDists[edgeOrder.y];
            color = cMaterialDiffuse;
            realLineWidth = 0.5*LineWidth;
        }
        dist = sqrt(dist);
    }
    // Cull fragments too far from the edge.
    else if (dist > 0.5*LineWidth+1) discard;

    // Map the computed distance to the [0,2] range on the border of the line.
    dist = clamp((dist - (realLineWidth - 1)), 0, 2);

    // Alpha is computed from the function exp2(-2(x)^2).
    dist *= dist;
    float alpha = exp2(-2*dist);

    color.a *= alpha;
    return color;
}

technique10 SolidWire
    <string info = "SolidWire technique";>
{     
    pass
    {
        SetDepthStencilState( DSSDepthLessEqual, 0 );
        SetRasterizerState( RSFill );
        SetBlendState( BSBlending, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF );
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetGeometryShader( CompileShader( gs_4_0, GSSolidWire() ) );
        SetPixelShader( CompileShader( ps_4_0, PSSolidWire() ) );
    }
}

technique10 SolidWireFade
    <string info = "SolidWireFade : SolidWire fragments faded depending on the depth";>
{     
    pass
    {
        SetDepthStencilState( DSSDepthLessEqual, 0 );
        SetRasterizerState( RSFill );
        SetBlendState( BSBlending, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF );
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetGeometryShader( CompileShader( gs_4_0, GSSolidWire() ) );
        SetPixelShader( CompileShader( ps_4_0, PSSolidWireFade() ) );
    }
}

technique10 SolidWirePattern
    <string info = "SolidWirePattern : SolidWire with a dot line pattern on diagonal edges";>
{
    pass
    {
        SetDepthStencilState( DSSDepthLessEqual, 0 );
        SetRasterizerState( RSFill );
        SetBlendState( BSBlending, float4( 0.0f, 0.0f, 0.0f, 0.0f ), 0xFFFFFFFF );
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetGeometryShader( CompileShader( gs_4_0, GSSolidWire() ) );
        SetPixelShader( CompileShader( ps_4_0, PSSolidWirePattern() ) );
    }
}



