#include <Include/Common.h>
#include <Include/Structs.h>

TextureCube tDiffuse;

Textured3DVerticesVSOut VS(Textured3DVerticesVSIn input )
{
	Textured3DVerticesVSOut output = (Textured3DVerticesVSOut)0;
	
	float4 hPos = float4(input.Position.xyz, 1);
	Matrix mViewNT = mView;
	
	matrix mViewProj = mul(mViewNT, mProjection);
	output.Position = mul(hPos, mViewProj).xyww;
	output.TextureUVW = input.TextureUVW;
	return output;
}

float4 PS(Textured3DVerticesVSOut input ) : SV_Target
{
	return tDiffuse.Sample(triLinearCubeSample, input.TextureUVW);
}

technique10 SkyBoxCubeMap
{
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}