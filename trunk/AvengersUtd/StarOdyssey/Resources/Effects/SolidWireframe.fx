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

#include <cktools.fx>

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

float LineWidth = 1.5;
float FadeDistance = 50;
float PatternPeriod = 1.5;

float4 FillColor = float4(0.1, 0.2, 0.4, 1);
float4 WireColor = float4(1, 1, 1, 1);
float4 PatternColor = float4(1, 1, 0.5, 1);

//--------------------------------------------------------------------------------------

struct VSInput
{
    float4 Position  : POSITION;
    float3 Normal	 : NORMAL;
	float2 TextureUV : TEXCOORD0;
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
    noperspective float3 Heights : TEXCOORD1;
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
GSInput VS( VS_INPUT input )
{
    GSInput output;
    output.Position = ModelToProj( float4(input.Position, 1));
    output.PositionV = ModelToView( float4(input.Position, 1));
    return output;
}

//--------------------------------------------------------------------------------------
// Geometry Shader
//--------------------------------------------------------------------------------------

[maxvertexcount(3)] 
void GS( triangle GSInput input[3],
         inout TriangleStream<PS_INPUT> outStream )
{
    PS_INPUT output;

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

    // Shade and colour face.
    output.Color = shadeFace(input[0].PositionV, input[1].PositionV, input[2].PositionV);

    // Emit the 3 vertices
    // The Height attribute is based on the constant
    output.Position = input[0].Position;
    output.Heights = float3( 1, 0, 0 );
    outStream.Append( output );

    output.Position = input[1].Position;
    output.Heights = float3( 0, 1, 0 );
    outStream.Append( output );

    output.Position = input[2].Position;
    output.Heights = float3( 0, 0, 1 );
	outStream.Append( output );

    outStream.RestartStrip();
}

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 PSColor( PS_INPUT input) : SV_Target
{
    return input.Col;
}

float4 PSDXWire( PS_INPUT input) : SV_Target
{
    return WireColor;
}

float evalMinDistanceToEdges(in PSInputWire input)
{
    float dist;

	float3 ddxHeights = ddx( input.Heights );
	float3 ddyHeights = ddy( input.Heights );
	float3 ddHeights2 =  ddxHeights*ddxHeights + ddyHeights*ddyHeights;
	
    float3 pixHeights2 = input.Heights *  input.Heights / ddHeights2 ;
    
    dist = sqrt( min ( min (pixHeights2.x, pixHeights2.y), pixHeights2.z) );
    
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
    float4 color = WireColor;
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

    float4 color = WireColor * fading;
    color.a *= alpha;
    return color;
}

float det( float2 a, float2 b )
{
	return (a.x*b.y - a.y*b.x);
}

float4 PSSolidWirePattern( PSInputWire input) : SV_Target
{
    // Compute the shortest square distance between the fragment and the edges.
    float3 eDists;
    float3 vDists;
    uint3 order = uint3(0, 1, 2);

    float dist;

	float3 ddxHeights = ddx( input.Heights );
	float3 ddyHeights = ddy( input.Heights );
	float3 invddHeights = 1.0 / sqrt( ddxHeights*ddxHeights + ddyHeights*ddyHeights );

    eDists = input.Heights * invddHeights ;
    vDists = (1.0 - input.Heights) * invddHeights ;
    
	if (eDists[1] < eDists[0])
	{
		order.xy = order.yx;
	}
	if (eDists[2] < eDists[order.y])
	{
		order.yz = order.zy;
	}
	if (eDists[2] < eDists[order.x])
	{
		order.xy = order.yx;
	}
	
	// Now compute the coordinate of the fragment along each edges
   
	float2 hDirs[3] ;
	hDirs[0] = float2( ddxHeights[0], ddyHeights[0] ) * invddHeights[0] ;
 	hDirs[1] = float2( ddxHeights[1], ddyHeights[1] ) * invddHeights[1] ;
	hDirs[2] = float2( ddxHeights[2], ddyHeights[2] ) * invddHeights[2] ;
   
	float2 hTans[3] ;
	hTans[0] = float2( - hDirs[0].y, hDirs[0].x ) ;
 	hTans[1] = float2( - hDirs[1].y, hDirs[1].x ) ;
	hTans[2] = float2( - hDirs[2].y, hDirs[2].x ) ;
   
	float2 ePoints[3] ;
	ePoints[0] = input.Position.xy - hDirs[0]*eDists[0];
	ePoints[1] = input.Position.xy - hDirs[1]*eDists[1];
	ePoints[2] = input.Position.xy - hDirs[2]*eDists[2];

	float2 eCoords[3] ;
	eCoords[0].x = det( hTans[1], ePoints[0] - ePoints[1] ) / det( hTans[0], hTans[1] );
	eCoords[0].y = det( hTans[2], ePoints[0] - ePoints[2] ) / det( hTans[0], hTans[2] );
	
	eCoords[1].x = det( hTans[2], ePoints[1] - ePoints[2] ) / det( hTans[1], hTans[2] );
	eCoords[1].y = det( hTans[0], ePoints[1] - ePoints[0] ) / det( hTans[1], hTans[0] );

	eCoords[2].x = det( hTans[0], ePoints[2] - ePoints[0] ) / det( hTans[2], hTans[0] );
	eCoords[2].y = det( hTans[1], ePoints[2] - ePoints[1] ) / det( hTans[2], hTans[1] );
	

    float2 edgeCoord;
	
	// Current coordinate along closest edge in pixels
	edgeCoord.x = abs(eCoords[order.x].x);
	// Length of the closest edge in pixels
	edgeCoord.y = abs(eCoords[order.x].y - eCoords[order.x].x );
  
    dist = eDists[order.x];
    
    // Standard wire color
    float4 color = WireColor;
    float realLineWidth = 0.5*LineWidth;

    // if on the diagonal edge apply pattern
    if ( 2 == order.x )
    {
        if (dist > LineWidth+1) discard;

        float patternPos = (abs(edgeCoord.x - 0.5 * edgeCoord.y)) % (PatternPeriod * 2 * LineWidth) - PatternPeriod * LineWidth;
        dist = sqrt(patternPos*patternPos + dist*dist);

        color = PatternColor;
        realLineWidth = LineWidth;

        // Filling the corners near the vertices with the WireColor
        if ( eDists[order.y] < (0.5*LineWidth+1) )
        {
            dist = eDists[order.y];
            color = WireColor;
            realLineWidth = 0.5*LineWidth;
        }
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



