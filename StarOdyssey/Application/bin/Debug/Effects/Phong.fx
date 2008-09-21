#include <Include/Common.fx>
#include <Include/Materials.fx>
#include <Include/Lighting.fx>
#include <Include/Shadows.fx>

uniform int iTextureType;

struct ApplicationDataVSIn
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
};

struct PhongVSOut
{
    float4 Position : POSITION;
    float3 Normal : TEXCOORD0;
    float3 LightDirection : TEXCOORD1;
    float3 View: TEXCOORD2;
};

struct PhongShadowsVSOut
{
    float4 Position : POSITION;
    float3 Normal : TEXCOORD0;
    float3 LightDirection : TEXCOORD1;
    float3 View: TEXCOORD2;
    float4 WorldPosition : TEXCOORD3;
    float4 ShadowProjection : TEXCOORD4; 
};

struct PSOut
{
    float4 Color : COLOR0;
};

PhongVSOut SpecularVS(ApplicationDataVSIn In)
{            
    PhongVSOut Out;
    
    Out.Position = mul(In.Position, mWorldViewProj);
    float3 worldPos = mul(In.Position, mWorld);
    // output LightDirection vector
    Out.LightDirection = vLightPosition.xyz - worldPos;
    Out.View = vEyePosition - worldPos;
    Out.Normal = mul(In.Normal, (float3x3)mWorld);
   
    return Out; 
}

PhongShadowsVSOut PhongShadowsVS(ApplicationDataVSIn In)
{
	PhongShadowsVSOut Out;
	Out.Position = mul(In.Position, mWorldViewProj);
    float4 worldPos = mul(In.Position, mWorld);
    Out.LightDirection = vLightPosition - worldPos;
    Out.View = vEyePosition - worldPos;
    Out.Normal = mul(In.Normal, (float3x3)mWorld);

    Out.ShadowProjection = mul(In.Position, mul(mLightWorldViewProj, mTextureBias));
    Out.WorldPosition = worldPos;
    Out.WorldPosition /= Out.WorldPosition.w;
    
    return Out;
}

PSOut SpecularPS(PhongVSOut In,
	uniform bool bDoSpecular) : COLOR0
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

technique PhongDiffuse
{
    pass p0
    {
		CullMode = CCW;
        VertexShader = compile vs_3_0 SpecularVS();
        PixelShader = compile ps_3_0 SpecularPS(false);
    }
}

technique PhongDiffuseSpecular
{
    pass p0
    {
		CullMode = CCW;
        VertexShader = compile vs_3_0 SpecularVS();
        PixelShader = compile ps_3_0 SpecularPS(true);
    }
}

technique PhongDiffuseSpecularSSM
{
    pass p0
    {
		//CullMode = CCW;
        VertexShader = compile vs_3_0 PhongShadowsVS();
        PixelShader = compile ps_3_0 SpecularShadowsPS(true,true,
			iTextureType, tShadowMapSampler);
    }
}
