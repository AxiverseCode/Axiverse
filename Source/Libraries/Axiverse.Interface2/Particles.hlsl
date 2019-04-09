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
	output.position = mul(worldViewProj, input.position);
	output.color = input.color;
	output.texcoord = input.texcoord;
	return output;
}


[maxvertexcount(1)]
void GS_P(point GS_INPUT input[1], inout PointStream<PS_INPUT> outputStream)
{
	PS_INPUT vertices[1];

	vertices[0].position = input[0].position;
	vertices[0].position.x += 10.f;
	vertices[0].color = input[0].color;
	vertices[0].texcoord = float2(0, 0);

	outputStream.Append(vertices[0]);
}

[maxvertexcount(4)]
void GS(point GS_INPUT input[1], inout TriangleStream<PS_INPUT> outputStream)
{
	PS_INPUT vertices[4];
	float s = 1;
	float2 a = float2(-1, -1) * s;
	float2 b = float2(+1, -1) * s;
	float2 c = float2(-1, +1) * s;
	float2 d = float2(+1, +1) * s;

	vertices[0].color = input[0].color;
	vertices[1].color = input[0].color;
	vertices[2].color = input[0].color;
	vertices[3].color = input[0].color;

	vertices[0].texcoord = float2(0, 0);
	vertices[1].texcoord = float2(0, 1);
	vertices[2].texcoord = float2(1, 0);
	vertices[3].texcoord = float2(1, 1);

	vertices[0].position.zw = input[0].position.zw;
	vertices[1].position.zw = input[0].position.zw;
	vertices[2].position.zw = input[0].position.zw;
	vertices[3].position.zw = input[0].position.zw;

	vertices[0].position.xy = input[0].position.xy + a;
	vertices[1].position.xy = input[0].position.xy + b;
	vertices[2].position.xy = input[0].position.xy + c;
	vertices[3].position.xy = input[0].position.xy + d;

	outputStream.Append(vertices[0]);
	outputStream.Append(vertices[1]);
	outputStream.Append(vertices[2]);
	outputStream.Append(vertices[3]);
	outputStream.RestartStrip();
}

float4 PS(PS_INPUT input) : SV_TARGET
{
	//return diffuseMap.Sample(textureSampler, input.texcoord) * input.color;
	return input.color;
}
