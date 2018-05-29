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
	float3 Position  : Position;
	float4 Color : Color;
	float2 Texture	 : Texture;
};

struct VertexOut
{
	float4 Position  : SV_Position;
	float4 Color : Color;
	float2 Texture	 : Texture;
};

VertexOut VSMain(VertexIn vin)
{
	VertexOut vout;

	// Transform to homogeneous clip space.
	vout.Position = mul(float4(vin.Position, 1.0f), gWorldViewProj);

	// Just pass vertex color into the pixel shader.
	vout.Color = vin.Color;
	vout.Texture = vin.Texture;

	return vout;
}

float4 PSMain(VertexOut pin) : SV_Target
{
	float4 color = g_texture.Sample(g_sampler, pin.Texture);
	return color * gColor;
	//return pin.Color;
}