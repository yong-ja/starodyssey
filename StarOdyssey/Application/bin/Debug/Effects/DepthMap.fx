#include <Include/Common.fx>
#include <Include/Materials.fx>
#include <Include/Lighting.fx>

#define ShaderModel

float4x4 mLightWorldViewProj;

 struct PSOut
 {
     float4 Color : COLOR0;
 };
 
struct ApplicationDataVSIn
{
	float4 Position			: POSITION;
	float2 TextureUV		: TEXCOORD0;
};

struct DepthVSOut
{
	float4 Position			: POSITION;
	float4 WorldPosition	: TEXCOORD0;
};
 
DepthVSOut DepthVS (ApplicationDataVSIn In)
{
	DepthVSOut Output;
	
	// Our standard WVP multiply, using the light's View and Projection matrices
	Output.Position = mul(In.Position, mLightWorldViewProj);

	// We also keep a copy of our position that is only multiplied by the world matrix. This is
	// because we do not want the camera's angle/position to affect our distance calculations
	
	Output.WorldPosition = mul(In.Position, mWorld);
	
	Output.WorldPosition /= Output.WorldPosition.w;
	//Output.WorldPosition = Output.Position;
	return Output;
}

PSOut DepthPS (DepthVSOut Input,
	uniform int TextureType)
{
	PSOut Out;
	// Get the distance from this pixel to the light, we divide by the 
	// far clip of the light so that it will fit into the 0.0-1.0 range
	// of pixel shader colours
	float depth = length(vLightPosition - Input.WorldPosition) / fLightRadius;
	
	if (TextureType == TextureF)
		Out.Color = float4(depth,0,0,1);
	else if (TextureType == Texture32i)
		Out.Color = PackDepthToARGB32(depth);
	else
		Out.Color = float4(PackDepthToRGB24(depth),1);
		
	return Out;
}

 
 // Depth Map in 32/16 fp texture
 technique DepthSSMf
 {
     pass Pass0
     {
		CullMode = CW;
        VertexShader = compile VSProfile DepthVS();
		PixelShader  = compile PSProfile DepthPS(TextureF);
     }
 }
 
 technique DepthSSM32i
 {
     pass Pass0
     {
		CullMode = CW;
        VertexShader = compile VSProfile DepthVS();
		PixelShader  = compile PSProfile DepthPS(Texture32i);
     }
 }
 
 technique DepthSSM24i
 {
     pass Pass0
     {
		CullMode = CW;
        VertexShader = compile VSProfile DepthVS();
		PixelShader  = compile PSProfile DepthPS(Texture24i);
     }
 }