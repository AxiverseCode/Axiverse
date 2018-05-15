//--------------------------------------------------------------------------------------
// Constant Buffers
//--------------------------------------------------------------------------------------
cbuffer cbPerObject : register(b0)
{
	float4x4 gWorldViewProj;
	float4 gColor;
};

//--------------------------------------------------------------------------------------
// Textures
//--------------------------------------------------------------------------------------
Texture2D g_texture : register(t0);

//--------------------------------------------------------------------------------------
// Samplers
//--------------------------------------------------------------------------------------
SamplerState g_sampler : register(s0);

struct VertexIn
{
	float3 IPosition  : POSITION;
};

struct VertexOut
{
	float4 Position  : SV_Position;
	float4 Color     : Color;
};

VertexOut VSMain(VertexIn vin)
{
	VertexOut vout;
	vout.Color = gColor;
	vout.Position = float4(vin.IPosition.xyz,1.0);
	return vout;
}

float4 PSMain(VertexOut pin) : SV_Target
{
	return pin.Color;
}