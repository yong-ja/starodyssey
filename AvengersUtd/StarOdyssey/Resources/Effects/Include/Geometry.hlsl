//--------------------------------------------------------------------------------------
// Standard Transform Functions
//--------------------------------------------------------------------------------------
float4 ModelToProj(in float4 pos)
{
	return mul(pos, mul(mul(mWorld,mView),mProjection));
}

float4 ModelToView(in float4 pos)
{
    return mul(pos, mul(mWorld,mView));
}

float4 ViewToProj(in float4 pos)
{
    return mul(pos, mProjection);
}

float4 Viewport = float4(1920,1080,0,0);
// From window pixel pos to projection frame at the specified z (view frame). 
float2 projToWindow(in float4 pos)
{
    return float2(  Viewport.x*0.5*((pos.x/pos.w) + 1) + Viewport.z,
                    Viewport.y*0.5*(1-(pos.y/pos.w)) + Viewport.w );
}

/*
float4 ViewToProj2(in float4 pos)
{
    return float4(	pos.x * 2 * Frustum.z / Frustum.x, 
					pos.y * 2 * Frustum.z / Frustum.y,
					(Frustum.w / (Frustum.w - Frustum.z)) * ( pos.z - pos.w*Frustum.z ),
					pos.z );
}



// From window pixel pos to projection frame at the specified z (view frame). 
float4 windowToProj(in float2 pos, float depth)
{
    return float4(  (((pos.x-Viewport.z)*2/Viewport.x)-1)*depth,
                    (((pos.y-Viewport.w)*2/Viewport.y)-1)*(-depth),
                    (depth - Frustum.z)*Frustum.w /(Frustum.w - Frustum.z),
                    depth );
}
*/

//--------------------------------------------------------------------------------------
// Standard Geometric Functions
//--------------------------------------------------------------------------------------

// Compute the triangle face normal from 3 points
float3 FaceNormal(in float3 posA, in float3 posB, in float3 posC)
{
    return normalize( cross(normalize(posB - posA), normalize(posC - posA)) );
}

