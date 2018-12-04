static const int MAX_BONES = 32;

//--------------------------------------------------------------------------------------
// Constant Buffers
//--------------------------------------------------------------------------------------
cbuffer cbPerObject : register(b0)
{
	float4x4 gWorldViewProj;
	float4 gColor;
};

cbuffer cbSkinningData : register(b1)
{
	float4x4 gSkinning[MAX_BONES];
}

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
	float3 Position  	: Position;
	float4 Color 		: Color;
	float2 Texture	 	: Texture;
	float2 Normal	 	: Normal;
    int4 BlendIndices 	: BLENDINDICES;
    float4 BlendWeights : BLENDWEIGHT;
};

struct VertexOut
{
	float4 Position  	: SV_Position;
	float4 Color 		: Color;
	float2 Texture	 	: Texture;
	float2 Normal	 	: Normal;
};

VertexOut VSMain(VertexIn vin)
{
	VertexOut vout;

	// Transform to homogeneous clip space.
	float4 position = float4(0, 0, 0, 0);
	float4 normal = float4(0, 0, 0, 0);

	for (int i = 0; i < 4; ++i)
	{
		position += float4(mul(vin.Position, gSkinning[int(vin.BlendIndices[i])]) * vin.BlendWeights[i]);
		normal += float4(mul(vin.Normal, gSkinning[int(vin.BlendIndices[i])]) * vin.BlendWeights[i]);
	}

	// Just pass vertex color into the pixel shader.
	vout.Position = position;
	vout.Normal = normal;

	vout.Color = vin.Color;
	vout.Texture = vin.Texture;
	return vout;
}

float4 PSMain(VertexOut pin) : SV_Target
{
	float2 dx = ddx(pin.Texture);
	float2 dy = ddy(pin.Texture);

	float4 color = g_texture.SampleGrad(g_sampler, pin.Texture, dx, dy);
	return color * gColor;
	//return pin.Color;
}