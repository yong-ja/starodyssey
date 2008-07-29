float4 vLightDir;
float4 vEyePos;
float4x4 mWorld;
float4x4 mWorldViewProj;
float4 cAmbient;
float4 cDiffuse;

struct VertexInput
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
};

struct VertexOutput
{
    float4 Position : POSITION;
    float3 Normal : TEXCOORD1;
    float3 Light : TEXCOORD2;
    float3 View: TEXCOORD3;
};

//vertex shader
VertexOutput VS(VertexInput In)
{            
    VertexOutput Out;

    Out.Position = mul(In.Position, mWorldViewProj);
    float3 worldPos = normalize(mul(In.Position, mWorld));
    Out.Light = vLightDir;
    Out.View = vEyePos - worldPos;
    Out.Normal = mul(In.Normal, mWorld);
   
    return Out; 
}

//pixel shader
float4 PS(VertexOutput In) : COLOR
{
    float3 Normal = normalize(In.Normal);
    float3 LightDir = normalize(In.Light);
    float3 ViewDir = normalize(In.View);
    float4 NdotL = saturate(dot(Normal, LightDir)); // diffuse component

    // R = 2 * (N.L) * N - L
    float3 Reflect = normalize(2 * NdotL * Normal - LightDir);
    float4 Specular = pow(saturate(dot(Reflect, ViewDir)), 8); // R.V^n

    // I = Acolor + Dcolor * N.L + (R.V)n
    return cAmbient + cDiffuse * NdotL + Specular;
}

technique Specular
{
    pass p0
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS();
    }
    
}