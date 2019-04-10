cbuffer data :register(b0)
{
	float4x4 worldViewProj;
	float4x4 worldView;
	float4x4 proj;
	float4 color;
};

struct VS_IN
{
	float4 position : POSITION;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD;
};

struct PS_IN
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
	float4 add : COLOR1;
	float2 texcoord : TEXCOORD;
};

//texture
Texture2D textureMap;
SamplerState textureSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.position = mul(worldViewProj, input.position);
	output.color = input.color;
	output.add = color;
	output.texcoord = input.texcoord;
	
	return output;
}

float4 PS(PS_IN input) : SV_Target
{
	return textureMap.Sample(textureSampler, input.texcoord) * input.color + input.add;
}