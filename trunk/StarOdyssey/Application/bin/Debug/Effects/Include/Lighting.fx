// Common light properties
float fLightRadius = 100;
float4 vLightPosition;
float4 cLightDiffuse = float4(1,1,1,1);
float4 cLightAmbient = float4(0.15, 0.15, 0.15, 1);
float4 cLightSpecular = float4(1,1,1,1);

// Spotlight Parameters
float fSpotlightInnerConeCosine;
float fSpotlightOuterConeCosine;
float3 vSpotlightDirection;
float fLightFalloff = 1;


float ComputeAttenuation(float3 vLightDirection, float fLightRadius)
{
   float x = vLightDirection.x;
   float y = vLightDirection.y;
   float z = vLightDirection.z;
   float r = fLightRadius;
   
   return 1 - ( pow(x/r,2) + pow(y/r,2) + pow(z/r,2));
}

// Gets Spotlight intensity
// cosTheta and cosPhi are cos(theta/2) and cos(phi/2)
float GetSpotlightEffect(
      float3 vLightDirection,
      float3 vSpotlightDirection,
      float cosTheta,
      float cosPhi,
      float falloff)
{
   float rho = dot(vLightDirection, vSpotlightDirection);
   
   if (rho > cosTheta)
      return 1;
   else if (rho <= cosPhi)
      return 0;
   else
      return pow(saturate(
          (rho - cosPhi) / (cosTheta - cosPhi)), 
          falloff); 
}

//float4 Prova(float4 vShadowProjection, uniform sampler2D tShadowMapSampler)
//{
	//float3 shadowColorValue = tex2Dproj(tShadowMapSampler, vShadowProjection).rgb;      
	//
	//if (isnan(shadowColorValue.x) || isnan(shadowColorValue.y) || isnan(shadowColorValue.z))
		//return float4(1,0,0,1);
	//else if (isinf(shadowColorValue.x) || isinf(shadowColorValue.y) || isinf(shadowColorValue.z))
		//return float4(0,0,1,1);
	//else
		//return float4(0,1,0,1);
	//
	////return float4(saturate(shadowColorValue),1);
//}

float ComputeShadowsSSM(
	float3 vLightDirection,
	float4 vShadowProjection,
	uniform int iTextureType,
	uniform sampler2D tShadowMapSampler)
{
	float shadowMapDepth=1.0;
	
	// Unpack depth if necessary
	if (iTextureType == TextureF)
	{
		shadowMapDepth = tex2Dproj(tShadowMapSampler, vShadowProjection).r; 
	}
	else if (iTextureType == Texture32i)
	{
		float4 shadowColorValue = tex2Dproj(tShadowMapSampler, vShadowProjection).argb;      
		shadowMapDepth = UnpackDepthFromARGB32(shadowColorValue);
	}
	else
	{
		float3 shadowMapColorValue = tex2Dproj(tShadowMapSampler, vShadowProjection).rgb; 
		shadowMapDepth = UnpackDepthFromRGB24(shadowMapColorValue);
	}
		
	float lightDepth =length(vLightDirection) / fLightRadius;
	float shadowOcclusion = 0;
	if (vShadowProjection.x > 0 && vShadowProjection.y > 0)
   	{
		shadowOcclusion = shadowMapDepth + 0.0005f >=lightDepth;
	}
		
	return shadowOcclusion;
}

// Calculates the full Phong equation
// Make sure that all components are in the same
// coordinate space
float4 PhongLighting(
				bool bDoSpecular,
               float3 vLightDirection,
               float3 vNormal, 
               float3 vViewDirection)
{
    // Calculate Phong components per-pixel
    // Eq.: kA + kD * NdotL + kS * RdotV^n
   float4 color;
    float3 L = normalize(vLightDirection);
    float3 V = normalize(vViewDirection);
    float3 N = normalize(vNormal);
   //float3 H = normalize(L + V);
    float3 R = reflect(-L,N);
    float NdotL = saturate(dot(N,L));
   
    float fSelfShadow = saturate (4 * NdotL);
    float4 cDiffuse = (kD * NdotL * cMaterialDiffuse * cLightDiffuse);
	
	
	if (bDoSpecular) 
	{
		float RdotV = saturate(dot(R,V));
		float4 cSpecular = (kS * pow(RdotV,fMaterialSpecularPower) * cMaterialSpecular * cLightSpecular);
		color = kA + (fSelfShadow * (cDiffuse + cSpecular));
	}
	else 
		color = kA + fSelfShadow * cDiffuse;
		
	return color;
}

