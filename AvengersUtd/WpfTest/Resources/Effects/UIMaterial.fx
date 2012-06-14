#include <Include/Common.hlsl>
#include <Include/Structs.hlsl>

Texture2D tDiffuse;

ColoredVerticesVSOut VS(ColoredVerticesVSIn input )
{
	ColoredVerticesVSOut output = (ColoredVerticesVSOut)0;
	
	matrix mWorldViewProj = mul(mul(mWorld, mView), mProjection);
	
	output.Position = mul(input.Position,mWorldViewProj);
	output.Color = input.Color;
	
	return output;
}

float4 PS(ColoredVerticesVSOut input ) : SV_Target
{
	return input.Color;
}

technique10 UIMaterial
{
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}