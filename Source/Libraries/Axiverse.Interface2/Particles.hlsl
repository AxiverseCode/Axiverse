cbuffer data :register(b0)
{
	float4x4 worldViewProj;
	float4x4 worldView;
	float4x4 proj;
}

struct VS_INPUT
{
	float4 position : POSITION;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD;
};

struct GS_INPUT
{
	float4 position : POSITION;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD;
};

struct PS_INPUT
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD;
};

Texture2D diffuseMap;
SamplerState textureSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};


GS_INPUT VS(VS_INPUT input)
{
	GS_INPUT output = (GS_INPUT)0;
	output.position = mul(worldView, input.position);
	output.color = input.color;
	output.texcoord = input.texcoord;
	return output;
}

[maxvertexcount(4)]
void GS(point GS_INPUT input[1], inout TriangleStream<PS_INPUT> outputStream)
{
	PS_INPUT vertices[4];
	float s = input[0].texcoord.x / 2;
	float t = input[0].texcoord.y / 2;
	float4 a = float4(-s, +t, 0, 0);
	float4 b = float4(+s, +t, 0, 0);
	float4 c = float4(-s, -t, 0, 0);
	float4 d = float4(+s, -t, 0, 0);

	vertices[0].color = input[0].color;
	vertices[1].color = input[0].color;
	vertices[2].color = input[0].color;
	vertices[3].color = input[0].color;

	vertices[0].texcoord = float2(0, 0);
	vertices[1].texcoord = float2(1, 0);
	vertices[2].texcoord = float2(0, 1);
	vertices[3].texcoord = float2(1, 1);

	vertices[0].position = mul(proj, input[0].position + a);
	vertices[1].position = mul(proj, input[0].position + b);
	vertices[2].position = mul(proj, input[0].position + c);
	vertices[3].position = mul(proj, input[0].position + d);

	outputStream.Append(vertices[0]);
	outputStream.Append(vertices[1]);
	outputStream.Append(vertices[2]);
	outputStream.Append(vertices[3]);
	outputStream.RestartStrip();
}

float4 PS(PS_INPUT input) : SV_TARGET
{
	return diffuseMap.Sample(textureSampler, input.texcoord) * input.color;
	//return input.color;
}
