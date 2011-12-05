// Position
struct VSIn
{
	float4 Position : POSITION;
};

// Position Colored
struct ColoredVerticesVSIn
{
	float4 Position : POSITION;
    float4 Color : COLOR;
};

struct ColoredVerticesVSOut
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR;
};

// TexturedVertices
struct TexturedVerticesVSOut
{
	float4 Position : SV_POSITION;
    float2 TextureUV : TEXCOORD;
};

struct TexturedVerticesVSIn
{
	float4 Position : POSITION;
    float2 TextureUV : TEXCOORD;
};

// Textured3DVertices
struct Textured3DVerticesVSOut
{
	float4 Position : SV_POSITION;
    float3 TextureUVW : TEXCOORD;
};

struct Textured3DVerticesVSIn
{
	float4 Position : POSITION;
    float3 TextureUVW : TEXCOORD;
};

// TexturedMeshVertices
struct TexturedMeshVerticesVSIn
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
	float3 Tangent: TANGENT;
	float3 Binormal: BINORMAL;
	float2 TextureUV: TEXCOORD;
};

struct TexturedMeshVerticesVSOut
{
    float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD0;
    float3 LightDirection : TEXCOORD1;
    float3 View: TEXCOORD2;
};

//Samplers
SamplerState triLinearSample
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

SamplerState triLinearCubeSample
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
	AddressW = Mirror;
};