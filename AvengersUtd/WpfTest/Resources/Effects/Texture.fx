#include <Include/Common.hlsl>
#include <Include/Structs.hlsl>

Texture2D tDiffuse;

TexturedVerticesVSOut VS(TexturedVerticesVSIn input )
{
	TexturedVerticesVSOut output = (TexturedVerticesVSOut)0;
	
	matrix mWorldViewProj = mul(mul(mWorld, mView), mProjection);
	
	output.Position = mul(input.Position,mWorldViewProj);
	output.TextureUV = input.TextureUV;
	
	return output;
}

float4 PS(TexturedVerticesVSOut input ) : SV_Target
{
	return tDiffuse.Sample(triLinearSample, input.TextureUV);
	//return float4(1,1,0,0);
}

technique10 Render
{
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}