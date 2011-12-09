#include <Include/Common.h>
#include <Include/Materials.h>
#include <Include/Lighting.h>
#include <Include/Structs.h>

uniform int iTextureType;

struct PhongShadowsVSOut
{
    float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD0;
    float3 LightDirection : TEXCOORD1;
    float3 View: TEXCOORD2;
    float4 WorldPosition : TEXCOORD3;
    float4 ShadowProjection : TEXCOORD4; 
};

struct PSOut
{
    float4 Color : SV_Target;
};

TexturedMeshVerticesVSOut SpecularVS(TexturedMeshVerticesVSIn In)
{            
    TexturedMeshVerticesVSOut Out;
    matrix mWorldViewProj = mul(mul(mWorld, mView), mProjection);
    Out.Position = mul(In.Position, mWorldViewProj);
    float3 worldPos = mul(In.Position, mWorld);
	
    // output LightDirection vector
    Out.LightDirection = vLightPosition - worldPos;
    Out.View = vEyePosition - worldPos;
    Out.Normal = mul(In.Normal, (float3x3)mWorld);
   
    return Out; 
}

PSOut SpecularPS(TexturedMeshVerticesVSOut In,
	uniform bool bDoSpecular) : SV_Target
{
	float4 cFinal = cLightAmbient * cMaterialDiffuse;
	
	cFinal += PhongLighting(
		bDoSpecular,
		In.LightDirection,
		In.Normal,
		In.View);
		
	cFinal *= ComputeAttenuation(In.LightDirection, fLightRadius);
	
	//cFinal *= GetSpotlightEffect(
	//	normalize(In.LightDirection),
	//	vSpotlightDirection,
	//	fSpotlightInnerConeCosine,
	//	fSpotlightOuterConeCosine,
	//	fLightFalloff);
					
	cFinal.a = 1;
	PSOut Out;
	
	Out.Color = cFinal;
	return Out;
}

PSOut SpecularShadowsPS(PhongShadowsVSOut In,
	uniform bool bDoSpecular,
	uniform bool bDoShadows,
	uniform int iTextureType,
	uniform sampler tShadowMapSampler) : COLOR0
{
	float4 cFinal = cLightAmbient * cMaterialDiffuse;
	
	cFinal += PhongLighting(
		bDoSpecular,
		In.LightDirection,
		In.Normal,
		In.View);
		
	cFinal *= ComputeAttenuation(In.LightDirection, fLightRadius);
	
	cFinal *= GetSpotlightEffect(
		normalize(In.LightDirection),
		vSpotlightDirection,
		fSpotlightInnerConeCosine,
		fSpotlightOuterConeCosine,
		fLightFalloff);
	
	if (bDoShadows)
		cFinal *= ComputeShadowsSSM(
			In.LightDirection,
			In.ShadowProjection,
			iTextureType,
			tShadowMapSampler);
			
	cFinal.a = 1;
	PSOut Out;
	
	Out.Color = cFinal;
	return Out;
}

technique10 PhongDiffuseSpecular
{
    pass p0
    {
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0,SpecularVS() ) );
		SetPixelShader( CompileShader( ps_4_0,SpecularPS(true) ) );
    }
}

//technique PhongDiffuseSpecularSSM
//{
//    pass p0
//    {
//		//CullMode = CCW;
//        VertexShader = compile vs_3_0 PhongShadowsVS();
//        PixelShader = compile ps_3_0 SpecularShadowsPS(true,true,
//			iTextureType, tShadowMapSampler);
//    }
//}
