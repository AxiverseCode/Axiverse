cbuffer VsData : register(b0)
{
	float4x4 proj;
	float4x4 view;
	float4x4 world;
	float3	 camera;
};

struct VS_IN
{
	float4 position : POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
	float2 texcoord : TEXCOORD;
};

struct VS_OUT
{
	float4 screen : SV_POSITION;
	float3 cameraPosition : CAMERA_POSITION;
	float4 position : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD;
	float3 binormal : BINORMAL;
	float3 tangent : TANGENT;
	float3 color : COLOR;
	float4 shadowCoord : SHADOW_POSITION;
};

//texture
Texture2D textureMap;
SamplerState textureSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

VS_OUT VS(in VS_IN input)
{
	float3x3 wsTransform = (float3x3)world;

	VS_OUT output = (VS_OUT)0;
	output.position = mul(world, input.position);
	output.screen = mul(mul(proj, view), output.position);
	//output.positionCS = mul(mul(mul(proj, view), world), input.position);
	//output.positionCS = mul(input.position, mul(world, mul(view, proj)));
	output.normal = input.normal;
	//output.binormal = mul(input.normal, wsTransform);
	//output.tangent = mul(input.normal, wsTransform);
	output.uv = input.texcoord;
	output.color = float3(1.0f, 1.0f, 1.0f);
	output.shadowCoord = float4(0.0f, 0.0f, 0.0f, 0.0f); // output.shadowCoord = mul(output.position, depthBias);

	output.cameraPosition = camera;

	return output;
}

/*
PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.position = mul(mul(mul(proj, view), world), input.position);
	output.normal = input.normal;
	output.color = input.color;
	output.texcoord = input.texcoord;

	return output;
}
*/

struct Light
{
	float4 color;
	float3 position;
	float p0;
	float3 direction;
	float p1;
	float3 lightVector;
	float intensity;
};

cbuffer PsData : register(b0)
{
	Light sys_Light;
};

float4 PS(VS_OUT input) : SV_Target
{
	return textureMap.Sample(textureSampler, input.uv); // +float4(input.uv.x, 0, input.uv.y, 0);
}