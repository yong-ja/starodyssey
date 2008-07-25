float4x4 mWorldViewProj;

float4 invWavelength;
float4 vEyePos;
float4 vLightDir;

float cameraHeight; // The camera's current height
float cameraHeight2; // fCameraHeight^2
float outerRadius; // The outer (atmosphere) radius
float outerRadius2; // fOuterRadius^2
float innerRadius; // The inner (planetary) radius
//float innerRadius2; // fInnerRadius^2
float krESun; // Kr * ESun
float kmESun; // Km * ESun
float kr4PI; // Kr * 4 * PI
float km4PI; // Km * 4 * PI
float scale; // 1 / (fOuterRadius - fInnerRadius)
float scaleDepth; // The scale depth (i.e. the altitude at which the atmosphere's average density is found)
float scaleOverScaleDepth; // fScale / fScaleDepth
float g;
float g2;

struct vpconn
{
	float4 Position : POSITION;
	float3 t0 : TEXCOORD0;
	//float DepthBlur : TEXCOORD1;
	float3 c0 : TEXCOORD1; // The Rayleigh color
	float3 c1 : TEXCOORD2; // The Mie color
};

struct vsconn
{
	float4 rt0 : COLOR0;
	//float4 rt1 : COLOR1;
};


// The scale equation calculated by Vernier's Graphical Analysis
float expScale (float fCos)
{
	//float x = 1.0 - fCos;
	float x = 1 - fCos;
	return scaleDepth * exp(-0.00287 + x*(0.459 + x*(3.83 + x*(-6.80 + x*5.25))));

}

// Calculates the Mie phase function
float getMiePhase(float fCos, float fCos2, float g, float g2)
{
	return 1.5 * ((1.0 - g2) / (2.0 + g2)) * (1.0 + fCos2) / pow(1.0 + g2 - 2.0*g*fCos, 1.5);
}

// Calculates the Rayleigh phase function
float getRayleighPhase(float fCos2)
{
	return 0.75 + (1.0 + fCos2);
}

// Returns the near intersection point of a line and a sphere
float getNearIntersection(float3 v3Pos, float3 v3Ray, float fDistance2, float fRadius2)
{
	float B = 2.0 * dot(v3Pos, v3Ray);
	float C = fDistance2 - fRadius2;
	float fDet = max(0.0, B*B - 4.0 * C);
	return 0.5 * (-B - sqrt(fDet));
}

// Returns the far intersection point of a line and a sphere
float getFarIntersection(float3 v3Pos, float3 v3Ray, float fDistance2, float fRadius2)
{
	float B = 2.0 * dot(v3Pos, v3Ray);
	float C = fDistance2 - fRadius2;
	float fDet = max(0.0, B*B - 4.0 * C);
	return 0.5 * (-B + sqrt(fDet));
}

vpconn
AtmosphereFromSpaceVS(float4 vPos : POSITION )
{
    float3 pos = vPos.xyz;
	float3 ray = pos - vEyePos.xyz;
	pos = normalize(pos);
	
 	float far = length (ray);
	ray /= far;
	
	// check if this point is obscured by the planet
	float B = 2.0 * dot(vEyePos, ray);
	float C = cameraHeight2 - (innerRadius*innerRadius);
	float fDet = (B*B - 4.0 * C);
	if (fDet >= 0)
	{
		// compute the intersection if so
		far = 0.5 * (-B - sqrt(fDet));
	}

	float near = getNearIntersection (vEyePos, ray,	cameraHeight2, outerRadius2);

	float3 start = vEyePos + ray * near;
	far -= near;

	float startAngle = dot (ray, start) / outerRadius;
	float startDepth = exp (scaleOverScaleDepth * (innerRadius - cameraHeight));
	//float startDepth = exp ((innerRadius - cameraHeight) / scaleDepth);
	//float startDepth = exp (-(1.0f / 0.25));
	float startOffset = startDepth * expScale (startAngle);

	float sampleLength = far / 3.0f;
	float scaledLength = sampleLength * scale;
	float3 sampleRay = ray * sampleLength;
	float3 samplePoint = start + sampleRay * 0.5f;

	float3 frontColor = float3 (0,0,0);

	for (int i = 0; i < 3; i++)
	{
		float height = length (samplePoint);
		float depth = exp (scaleOverScaleDepth * (innerRadius - height));
		float lightAngle = dot (vLightDir, samplePoint) / height;
		float cameraAngle = dot(-ray, samplePoint) / height;
		float scatter = (startOffset + depth * (expScale (lightAngle) - expScale (cameraAngle)));

		float3 attenuate = exp (-scatter * (invWavelength.xyz * kr4PI + km4PI));

		frontColor += attenuate * (depth * scaledLength);
		samplePoint += sampleRay;
	}

	vpconn OUT;

//	OUT.DepthBlur = ComputeDepthBlur(mul (vPos, viewMatrix));
	OUT.t0 = vEyePos.xyz - vPos.xyz;
	OUT.Position = mul(vPos, mWorldViewProj);
	OUT.c0.xyz = frontColor * (invWavelength.xyz * krESun);
	OUT.c1.xyz = frontColor * kmESun;

	return OUT;
}

vsconn
AtmosphereFromSpacePS1(vpconn IN)
{
  float3 color = IN.c0;
  
  
  vsconn OUT;
  OUT.rt0.rgb = color.rgb;
  OUT.rt0.a = 1.0f;
  return OUT;
}

vsconn
AtmosphereFromSpacePS(vpconn IN)
{
	vsconn OUT;

	float cos = saturate(dot (vLightDir, IN.t0) / length (IN.t0));
	float cos2 = cos*cos;

	//float fMiePhase = 1.5 * ((1.0 - g2) / (2.0 + g2)) * (1.0 + cos*cos) / pow(1.0 + g2 - 2.0*g*cos, 1.5);
	float fMiePhase = getMiePhase(cos,cos2,g,g2);
	float fRayleighPhase = getRayleighPhase(cos2);
	//float fRayleighPhase = 0.75 * (1.0 + cos*cos);

	//OUT.rt0.rgb = (fRayleighPhase * IN.c0 + fMiePhase * IN.c1);
	float exposure = 0.850;
	OUT.rt0.rgb = 1.0 - exp(-exposure * (fRayleighPhase * IN.c0 + fMiePhase * IN.c1));
	OUT.rt0.a = OUT.rt0.b;

	//OUT.rt1 = ComputeDepthBlur (IN.DepthBlur);

	return OUT;
}

technique AtmosphereFromSpace
{
pass P0
	{
		//AlphaBlendEnable = true;
		//DestBlend = ONE;
		//SrcBlend = ONE;
		CullMode = CW;
		VertexShader = compile vs_3_0 AtmosphereFromSpaceVS();
		PixelShader = compile ps_3_0 AtmosphereFromSpacePS();
	}
}
