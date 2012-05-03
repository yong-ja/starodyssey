// Material Properties

cbuffer mBuffer {
	float kA = 0.0;
	float kD = 1.0;
	float kS = 1.0;
	float fMaterialSpecularPower = 16;
	float4 cMaterialAmbient = float4(1,1,1,1);
	float4 cMaterialDiffuse = float4(1,0,0,1);
	float4 cMaterialSpecular = float4(1,1,1,1);
}
