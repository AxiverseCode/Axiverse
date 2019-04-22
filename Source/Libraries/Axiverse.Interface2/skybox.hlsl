//===================================================================================== =
// Constant buffers
//======================================================================================

cbuffer VSConstants : register (b0)
{
	float4x4 View;
	float4x4 Projection;
	float3 Bias;
}

//======================================================================================
// Samplers
//======================================================================================

TextureCube	EnvironmentMap : register(t0);
SamplerState LinearSampler : register(s0);

SamplerState textureSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
	AddressW = Wrap;
};

//======================================================================================
// Input/Output structs
//======================================================================================

struct VSInput
{
	float3 PositionOS : POSITION;
};

struct VSOutput
{
	float4 PositionCS : SV_Position;
	float3 TexCoord : TEXCOORD;
	float3 Bias : BIAS;
};

//======================================================================================
// Vertex Shader
//======================================================================================
VSOutput VS(in VSInput input)
{
	VSOutput output;
	input.PositionOS = normalize(input.PositionOS);

	// Rotate into view-space, centered on the camera
	float3 positionVS = normalize(mul((float3x3)View, input.PositionOS.xyz));

	// Transform to clip-space
	output.PositionCS = mul(Projection, float4(positionVS, 1.0f));
	output.PositionCS.z = output.PositionCS.w;

	// Make a texture coordinate
	output.TexCoord = input.PositionOS;

	// Pass along the bias
	output.Bias = Bias;

	return output;
}

//======================================================================================
// Pixel Shader
//======================================================================================
float4 PS(in VSOutput input) : SV_Target
{
	input.TexCoord = normalize(input.TexCoord);
	// Sample the skybox
	float3 texColor = EnvironmentMap.Sample(textureSampler, input.TexCoord).rgb;

	// Apply the bias
	texColor = texColor * input.Bias;

	//return float4(input.TexCoord / 2 + 0.5, 1);
	return float4(texColor, 1.0f);
	//return float4(input.Bias, 1);
	//return (float4)1;
}