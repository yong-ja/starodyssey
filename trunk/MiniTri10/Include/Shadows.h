float4x4 mLightWorldViewProj;
float4x4 mTextureBias;

texture tShadows;

sampler tShadowMapSampler = sampler_state 
{
	texture = <tShadows>;
	
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
	
	AddressU = CLAMP;
	AddressV = CLAMP;
};