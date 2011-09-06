// Defines
#define TextureF	0
#define Texture32i	1
#define Texture24i	2

// Camera Position
float4 vEyePosition;

// Matrices
cbuffer cbUpdateEveryFrame {
	matrix mWorld;
	matrix mView;
	float4 cMaterialDiffuse;
};

matrix mProjection;
matrix mWorldInv;


// Helper functions

//pack the depth in a 32-bit rgba color
float4 PackDepthToARGB32(const float value)
{
    const float4 bitSh = float4(256.0 * 256.0 * 256.0, 256.0 * 256.0, 256.0, 1.0);
    const float4 mask = float4(0.0, 1.0 / 256.0, 1.0 / 256.0, 1.0 / 256.0);
    float4 res = frac(value * bitSh);
    res -= res.xxyz * mask;
    return res;
}

//unpack the depth from a 32-bit rgba color
float UnpackDepthFromARGB32(const float4 value)
{
    const float4 bitSh = float4(1.0 / (256.0 * 256.0 * 256.0), 1.0 / (256.0 * 256.0), 1.0 / 256.0, 1.0);
    return(dot(value, bitSh));
}

//pack the depth in a 24-bit rgb color
float3 PackDepthToRGB24(const float value)
{
    const float3 bitSh = float3(256.0 * 256.0, 256.0, 1.0);
    const float3 mask = float3(0.0, 1.0 / 256.0, 1.0 / 256.0);
    float3 res = frac(value * bitSh);
    res -= res.xyz * mask;
    return res;
}

//unpack the depth from a 24-bit rgb color
float UnpackDepthFromRGB24(const float3 value)
{
    const float3 bitSh = float3(1.0 / (256.0 * 256.0), 1.0 / 256.0, 1.0);
    return(dot(value, bitSh));
} 

