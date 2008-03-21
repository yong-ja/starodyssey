texture t1;
sampler s1 = sampler_state
{
    texture = <t1>;
    AddressU = Clamp;
    AddressV = Clamp;
    MinFilter = Point;
    MagFilter = Linear;
    MipFilter = Linear;
};

texture t2;
sampler s2 = sampler_state
{
    texture = <t2>;
    AddressU = Clamp;
    AddressV = Clamp;
    MinFilter = Point;
    MagFilter = Linear;
    MipFilter = Linear;
};

texture t3;
sampler s3 = sampler_state
{
    texture = <t3>;
    AddressU = Clamp;
    AddressV = Clamp;
    MinFilter = Point;
    MagFilter = Linear;
    MipFilter = Linear;
};

float4x4 mWorldViewProj;
float4x4 mWorld;
float3 vLightPos;
float4 vEye;
float cAmbient;

struct Output
{
    float4 Pos : POSITION;
    float2 Tex : TEXCOORD0;
    float3 Light : TEXCOORD1;
    float3 View : TEXCOORD2;
    float3 Att : TEXCOORD3;
};

struct VSInput {
    float4 Pos : POSITION;
    float2 Tex: TEXCOORD;
    float3 Normal: NORMAL;
    float3 Tangent: TANGENT;
};

struct PSInput {
    float2 Tex: TEXCOORD0;
    float3 Light: TEXCOORD1;
    float3 View: TEXCOORD2;
    float3 Att: TEXCOORD3;
};


Output VSMain(VSInput input)
{
  Output Out = (Output)0; 
  Out.Pos = mul(input.Pos, mWorldViewProj); // transform Position

  // compute the 3x3 tranform matrix 
  // to transform from world space to tangent space
  float3x3 worldToTangentSpace;
  worldToTangentSpace[0] = mul(input.Tangent, mWorld);
  worldToTangentSpace[1] = mul(cross(input.Tangent, input.Normal), mWorld);
  worldToTangentSpace[2] = mul(input.Normal, mWorld);

  Out.Tex = input.Tex.xy;

  Out.Light.xyz = mul(worldToTangentSpace, vLightPos); // L
  float3 PosWorld = normalize(mul(input.Pos, mWorld));
  float3 Viewer = vEye - PosWorld; // V
  Out.View = mul(worldToTangentSpace, Viewer);

  float LightRange = 0.000001;
    Out.Att = Out.Light * LightRange;
  
  return Out;
}

float4 DiffuseBumpClouds(PSInput input) : COLOR
{
  float4 colDiffuse = tex2D(s1, input.Tex); // fetch color map
  float4 colCloud = tex2D(s3, input.Tex);
  float4 color = colDiffuse * (1 - colCloud.a) + colCloud * colCloud.a;
  //float4 color = colDiffuse;
  
  float3 bump = tex2D(s2, input.Tex);
  float3 bumpNormal = normalize((bump * 2) - 1); // bump map

  float3 LightDir = normalize(input.Light); // L
  float3 ViewDir = normalize(input.View); // V

  float4 diff = saturate(dot(bumpNormal, LightDir)); // diffuse comp.
  float4 attenuation = mul(input.Att, input.Att);

  float shadow = saturate(4 * diff); // compute self-shadowing term

  float3 Reflect = normalize(2 * diff * bumpNormal - LightDir); // R

  // gloss map in color.w restricts spec reflection
  float4 spec = min(pow(saturate(dot(Reflect, ViewDir)), 16), color.w);

  return 0.2 * color + shadow * (color * diff + spec);
   //return (color * (shadow * diff + cAmbient) + shadow * spec) * (1 - attenuation);

}

float4 DiffuseBump(PSInput input) : COLOR
{
  float4 colDiffuse = tex2D(s1, input.Tex); // fetch color map
  //float4 colCloud = tex2D(s3, input.Tex);
  //float4 color = colDiffuse * (1 - colCloud.a) + colCloud * colCloud.a;
  //float4 color = colDiffuse;
  
  float3 bump = tex2D(s2, input.Tex);
  float3 bumpNormal = normalize((bump * 2) - 1); // bump map

  float3 LightDir = normalize(input.Light); // L
  float3 ViewDir = normalize(input.View); // V

  float4 diff = saturate(dot(bumpNormal, LightDir)); // diffuse comp.
  float4 attenuation = mul(input.Att, input.Att);

  float shadow = saturate(4 * diff); // compute self-shadowing term

  float3 Reflect = normalize(2 * diff * bumpNormal - LightDir); // R

  // gloss map in color.w restricts spec reflection
  float4 spec = min(pow(saturate(dot(Reflect, ViewDir)), 16), colDiffuse.w);

  return 0.2 * colDiffuse + shadow * (colDiffuse * diff + spec);
   //return (color * (shadow * diff + cAmbient) + shadow * spec) * (1 - attenuation);

}

technique SpecularBump
{
    pass p0
    {
        //CullMode = None;
        VertexShader = compile vs_2_0 VSMain();
        PixelShader = compile ps_2_0 DiffuseBumpClouds();
    }
}

technique DiffuseBumpNoClouds
{
    pass p0 
    {
        VertexShader = compile vs_2_0 VSMain();
        PixelShader = compile ps_2_0 DiffuseBump();
    }
}

