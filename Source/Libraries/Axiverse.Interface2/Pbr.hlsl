// https://github.com/Nadrin/PBR/blob/master/data/shaders/hlsl/pbr.hlsl
// https://github.com/TheCherno/Sparky/blob/master/Sandbox/shaders/AdvancedLighting.hlsl

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
Texture2D albedoMap : register(t0);
Texture2D normalMap : register(t1);
Texture2D heightMap : register(t2);
Texture2D roughnessMap : register(t2);
Texture2D metallicMap : register(t4);
Texture2D alphaMap : register(t5);
Texture2D occlusionMap : register(t6);

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
	output.normal = mul(wsTransform, input.normal);
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
	float3 lightVector;
	float3 position;
	float p0;
	float3 direction;
	float p1;
	float intensity;
};

static const uint NUM_LIGHTS = 1;
cbuffer PsData : register(b0)
{
	Light lights[NUM_LIGHTS];
};

// Constant normal incidence Fresnel factor for all dielectrics.
static const float3 Fdielectric = 0.04;



float Diffuse(Light light, float3 eye)
{
	return 1;
}

float3 Specular(Light light, float3 eye)
{
	return (float3)1;
}

float4 PS(VS_OUT input) : SV_Target
{
	float3 albedo = albedoMap.Sample(textureSampler, input.uv).rgb;
	float3 normal = normalMap.Sample(textureSampler, input.uv).rgb;
	float metalness = metallicMap.Sample(textureSampler, input.uv).r;
	float roughness = roughnessMap.Sample(textureSampler, input.uv).r;
	float height = heightMap.Sample(textureSampler, input.uv).r;
	float alpha = alphaMap.Sample(textureSampler, input.uv).r;
	float occlusion = occlusionMap.Sample(textureSampler, input.uv).r;

	float3 eye = input.cameraPosition;

	// Outgoing light direction (vector from world-space fragment position to the "eye").
	float3 Lo = normalize(input.cameraPosition - input.position);

	// Get current fragment's normal and transform to world space.
	// normal = normalize(2.0 * normal - 1.0);
	normal = normalize(input.normal);

	// Angle between surface normal and outgoing light direction.
	float cosLo = max(0.0, dot(normal, Lo));

	// Specular reflection vector.
	float3 Lr = 2.0 * cosLo * normal - Lo;

	// Fresnel reflectance at normal incidence (for metals use albedo color).
	float3 F0 = lerp(Fdielectric, albedo, metalness);


	// Accumulators
	float4 diffuse = (float4)0;
	float3 specular = (float3)0;

	// Per light computation
	[unroll(NUM_LIGHTS)]
	for (uint i = 0; i < NUM_LIGHTS; i++)
	{
		Light light = lights[i];

		float NdotL = saturate(dot(normal, light.lightVector));

		// Diffuse Calculation
		diffuse += NdotL * Diffuse(light, eye) * light.color * light.intensity;
		// Specular Calculation
		specular += NdotL * Specular(light, eye) * light.color.xyz * light.intensity;
		//light.intensity /= 2.0;
		//light.lightVector = -light.lightVector;
		diffuse = NdotL;
	}

	// Shadow computation

	//float4 color = (float4)0;

	//color += albedoMap.Sample(textureSampler, input.uv);
	//color *= occlusionMap.Sample(textureSampler, input.uv);
	//color *= cosLo;
	//color.a = alpha;

	float3 color = (albedo.rgb * diffuse.rgb * occlusion) + (Lr + F0) * occlusion;
	return float4(color, alpha);
	//return lights[0].color;
	//return float4(lights[0].lightVector, alpha);
}