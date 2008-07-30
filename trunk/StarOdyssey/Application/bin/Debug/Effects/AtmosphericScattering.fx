float4x4 mWorldViewProj : WORLDVIEWPROJECTION;
float4x4 mWorld : WORLD;

float4x4 mView : VIEW;

float4 invWavelength;
float4 vLightDir : LIGHTDIRECTION;
float4 vLightPos : LIGHTPOSITION;
float4 vEyePos : EYELOCATION;

float3 cAmbient : LIGHTAMBIENT = {0.2,0.2,0.2};
float3 cDiffuse : LIGHTDIFFUSE = {1,1,1};
float3 cSpecular = {0.25,0.25,0.25};

float cameraHeight; // The camera's current height
float cameraHeight2; // fCameraHeight^2
float outerRadius; // The outer (atmosphere) radius
float outerRadius2; // fOuterRadius^2
float innerRadius; // The inner (planetary) radius
float krESun; // Kr * ESun
float kmESun; // Km * ESun
float kr4PI; // Kr * 4 * PI
float km4PI; // Km * 4 * PI
float scale; // 1 / (fOuterRadius - fInnerRadius)
float scaleDepth; // The scale depth (i.e. the altitude at which the atmosphere's average density is found)
float scaleOverScaleDepth; // fScale / fScaleDepth
float g;
float g2;

float fDepth = 0.0015;
float fShine = 8.0;

texture tDiffuse : DIFFUSEMAP;
texture tNormal : NORMALMAP;
texture t1;
texture t2;

sampler2D tDiffuse_sampler = sampler_state
{
	Texture = <tDiffuse>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

sampler2D tNormal_sampler = sampler_state
{
	Texture = <tNormal>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

sampler2D tClouds_sampler = sampler_state
{
	Texture = <t1>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

sampler2D tSpecular_sampler = sampler_state
{
	Texture = <t2>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};


struct AtmosphereVSIn
{
	float4 pos : POSITION;
	float3 normal : NORMAL;
	float2 txcoord : TEXCOORD0;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
};

struct GroundFromSpaceVSOut
{
	float4 hpos : POSITION;
	float2 txcoord : TEXCOORD0;
	float3 vpos : TEXCOORD1;
	float3 tangent : TEXCOORD2;
	float3 binormal : TEXCOORD3;
	float3 normal : TEXCOORD4;
	float4 lightPosition : TEXCOORD5;
	float3 normal2 : TEXCOORD6;
	//float depthBlur : TEXCOORD6;
	float3 c0 : COLOR;
	float3 c1 : COLOR1;
};

struct PSOut
{
	float4 rt0 : COLOR0;
	//float4 rt1 : COLOR1;
};

struct AtmosphereVSOut
{
	float4 Position : POSITION;
	float3 t0 : TEXCOORD0;
	//float DepthBlur : TEXCOORD1;
	float3 c0 : TEXCOORD1; // The Rayleigh color
	float3 c1 : TEXCOORD2; // The Mie color
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

AtmosphereVSOut
AtmosphereFromSpaceVS(float4 vPos : POSITION )
{
    //float3 pos = vPos.xyz;
	float3 pos = (mul(vPos, mWorld));
	float3 ray = pos - vEyePos.xyz;
	//pos = normalize(pos);
	
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

	float sampleLength = far / 1.0f;
	float scaledLength = sampleLength * scale;
	float3 sampleRay = ray * sampleLength;
	float3 samplePoint = start + sampleRay * 0.5f;

	float3 frontColor = float3 (0,0,0);
	for (int i = 0; i < 1; i++)
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

	AtmosphereVSOut OUT;

//	OUT.DepthBlur = ComputeDepthBlur(mul (vPos, viewMatrix));
	OUT.t0 = vEyePos.xyz - vPos.xyz;
	OUT.Position = mul(vPos, mWorldViewProj);
	OUT.c0.xyz = frontColor * (invWavelength.xyz * krESun);

	OUT.c1.xyz = frontColor * kmESun;

	return OUT;
}

PSOut
AtmosphereFromSpacePS1(AtmosphereVSOut IN)
{
  float3 color = IN.c0;
  
  
  PSOut OUT;
  OUT.rt0.rgb = color.rgb;
  OUT.rt0.a = 1.0f;
  return OUT;
}

PSOut
AtmosphereFromSpacePS(AtmosphereVSOut IN)
{
	PSOut OUT;

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

GroundFromSpaceVSOut GroundFromSpaceVS(AtmosphereVSIn IN)
{
	GroundFromSpaceVSOut OUT;

	// vertex position in object space
	// float scalar = tex2Dlod (tNormal_sampler, float4 (IN.txcoord.xy, 0, 0)).a;
	// float3 ScaledDisplacementVector = IN.normal* (scalar * 125);
	// float4 pos = ; // float4(IN.pos.xyz + ScaledDisplacementVector.xyz, 1);

	// compute mWorldView rotation only part
	float4x4 mWorldView = mul(mWorld,mView);
	float3x3 mWorldViewrot;
	mWorldViewrot[0]=mWorldView[0].xyz;
	mWorldViewrot[1]=mWorldView[1].xyz;
	mWorldViewrot[2]=mWorldView[2].xyz;

	// vertex position in clip space
	OUT.hpos = mul(IN.pos,mWorldViewProj);

	// vertex position in mView space (with model transformations)
	OUT.vpos = mul(IN.pos,mWorldView).xyz;

	// light position in mView space
	OUT.lightPosition = mul(vLightPos,mView);

	// tangent space vectors in mView space (with model transformations)
	OUT.tangent = mul(IN.tangent,mWorldViewrot);
	OUT.binormal = mul(IN.binormal,mWorldViewrot);
	OUT.normal = mul(IN.normal,mWorldViewrot);
	
	OUT.normal2 = normalize(mul(IN.normal, mWorld));

	// begin scattering code
	float3 v3Pos = mul (IN.pos, mWorld);

	float3 v3Ray = v3Pos - vEyePos.xyz;
	float fFar = length(v3Ray);
	v3Ray = v3Ray / fFar;
	
		// check if this point is obscured by the planet
	float B = 2.0 * dot(vEyePos, v3Ray);
	float C = cameraHeight2 - (innerRadius*innerRadius);
	float fDet = (B*B - 4.0 * C);
	if (fDet >= 0)
	{
		// compute the intersection if so
		fFar = 0.5 * (-B - sqrt(fDet));
	}

	// Calculate the closest intersection of the ray with the outer atmosphere (which is the near point of the ray passing through the atmosphere)
	float fNear =getNearIntersection(vEyePos, v3Ray, cameraHeight2, outerRadius2);

	// Calculate the ray's starting position, then calculate its scattering offset
	float3 v3Start = vEyePos.xyz + (v3Ray.xyz * fNear);
	fFar -= fNear;
	float ffDepth = exp((innerRadius - outerRadius) / scaleDepth);
	float fCameraAngle = dot(-v3Ray, v3Pos) / length(v3Pos);
	float fLightAngle = dot(vLightDir, v3Pos) / length(v3Pos);
	float fCameraScale = expScale(fCameraAngle);
	float fLightScale = expScale(fLightAngle);
	float fCameraOffset = ffDepth*fCameraScale;
	float fTemp = (fLightScale + fCameraScale);

	// Initialize the scattering loop variables
	float fSampleLength = fFar /5;
	float fScaledLength = fSampleLength * scale;
	float3 v3SampleRay = v3Ray * fSampleLength;
	float3 v3SamplePoint = v3Start + (v3SampleRay * 0.5);

	// Now loop through the sample rays
	float3 v3FrontColor = float3(0.0, 0.0, 0.0);
	float3 v3Attenuate;
	for(int i=0; i<5; i++)
	{
		float fHeight = length(v3SamplePoint);
		float ftfDepth = exp(scaleOverScaleDepth * (innerRadius - fHeight));
		float fScatter = ftfDepth*fTemp - fCameraOffset;
		v3Attenuate = exp(-fScatter * (invWavelength.xyz * kr4PI + km4PI));
		v3FrontColor = v3FrontColor + (v3Attenuate * (ftfDepth * fScaledLength));
		v3SamplePoint = v3SamplePoint + v3SampleRay;
	}

	//OUT.fDepthBlur = ComputefDepthBlur(mul (v3Pos, mView));
	OUT.c0 = v3FrontColor * (invWavelength.xyz * krESun + kmESun);
	OUT.c1 = v3Attenuate;

	// copy color and texture coordinates

	OUT.txcoord = IN.txcoord.xy;
	return OUT;
}

float ray_intersect_rm(
	in sampler2D tNormal,
	in float2 dp,
	in float2 ds)
{
	const int linear_search_steps=15;
	const int binary_search_steps=5;
	float fDepth_step=1.0/linear_search_steps;

	// current size of search window
	float size=fDepth_step;
	// current fDepth position
	float fDepth=0.0;
	// best match found (starts with last position 1.0)
	float bestDepth=1.0;

	// search front to back for first point inside object
	for( int i=0;i<linear_search_steps-1;i++ )
	{
		fDepth+=size;
		float4 t=tex2D(tNormal,dp+ds*fDepth);

		if (bestDepth>0.996) // if no fDepth found yet
			if (fDepth>=t.w)
			bestDepth=fDepth; // store best fDepth
	}
	fDepth=bestDepth;

	// recurse around first point (fDepth) for closest match
	for( int j=0;j<binary_search_steps;j++ )
	{
		size*=0.5;
		float4 t=tex2D(tNormal,dp+ds*fDepth);
		if (fDepth>=t.w)
		{
			bestDepth=fDepth;
			fDepth-=2*size;
		}
		fDepth+=size;
	}

	return bestDepth;
	}

PSOut GroundFromSpacePS(
	GroundFromSpaceVSOut IN,
	uniform sampler2D tDiffuse,
	uniform sampler2D tNormal) : COLOR
{
	float4 t;
	float3 p,v,l,s,c;
	float2 dp,ds,uv;
	float d,a;

	// ray intersect in mView direction
	p = IN.vpos;
	v = normalize(p);
	a = dot(IN.normal,-v);
	s = normalize(float3(dot(v,IN.tangent),dot(v,IN.binormal),a));
	s *= fDepth/a;
	ds = s.xy;
	dp = IN.txcoord;
	d = ray_intersect_rm (tNormal,dp,ds);

	// get rm and color texture points
	uv=dp+ds*d;
	t=tex2D(tNormal,uv);
	c=tex2D(tDiffuse,uv);

	// expand normal from normal map in local polygon space
	t.xy=t.xy*2.0-1.0;
	float temp = dot(t.xy, t.xy);
	t.z=sqrt(1.0-temp);
	t.xyz=normalize(t.x*IN.tangent-t.y*IN.binormal+t.z*IN.normal);

	// compute light direction
	p += v*d*a;
	l=normalize(p-IN.lightPosition.xyz);

	// ray intersect in light direction
	dp+= ds*d;
	a = dot(IN.normal,-l);
	s = normalize(float3(dot(l,IN.tangent),dot(l,IN.binormal),a));
	s *= fDepth/a;
	ds = s.xy;
	dp-= ds*d;
	float dl = ray_intersect_rm(tNormal,dp,s.xy);
	float shadow = 1.0;
	float3 cAmbientColor = cAmbient;
	float3 cSpecularColor = cSpecular;

	if (dl<d-0.05) // if pixel in shadow
	{
		shadow=dot(cAmbientColor, 1)*0.333333;
		cSpecularColor=0;
	}

	// compute cDiffuse and cSpecular terms
	float att=saturate(dot(-l,IN.normal.xyz));
	float diff=shadow*saturate(dot(-l,t.xyz));
	float spec=saturate(dot(normalize(-l-v),t.xyz)) * tex2D(tSpecular_sampler,uv).r;
	
	float4 cloudColor = tex2D(tClouds_sampler,uv);

	// compute final color
	float4 finalcolor;
	finalcolor.xyz=cAmbientColor*c+att*(c*cDiffuse*diff+cSpecularColor.xyz*pow(spec,fShine));
	finalcolor.w=1.0;
	
	float fLuminance = 0.299f*IN.c1.r + 0.587f*IN.c1.g + 0.114f*IN.c1.b;;
	float3 vDiff = float3(fLuminance,fLuminance,fLuminance);

	finalcolor.xyz = IN.c0 +finalcolor.xyz * IN.c1;//+ cloudColor.xyz * IN.c1;
	finalcolor.xyz = lerp(finalcolor.xyz,vDiff,cloudColor.b);
	PSOut output;
	output.rt0 = finalcolor;
	//output.rt1 = ComputefDepthBlur (IN.fDepthBlur);
	//output.rt0 = float4(vDiff,1.0);

	return output;
}

technique AtmosphereFromSpace
{
	pass P0
	{
		AlphaBlendEnable = false;
		CullMode = CCW;
		VertexShader = compile vs_3_0 GroundFromSpaceVS();
		PixelShader = compile ps_3_0 GroundFromSpacePS(tDiffuse_sampler,tNormal_sampler);
	}
	pass P1
	{
		AlphaBlendEnable = true;
		DestBlend = ONE;
		SrcBlend = ONE;
		CullMode = CW;
		VertexShader = compile vs_3_0 AtmosphereFromSpaceVS();
		PixelShader = compile ps_3_0 AtmosphereFromSpacePS();
	}

}
