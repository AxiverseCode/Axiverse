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
	float3 tangent   : TANGENT;
	float3 binormal : BINORMAL;
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
	float3x3 basis : BASIS;
	float3 color : COLOR;
	float4 shadowCoord : SHADOW_POSITION;
};

//texture
Texture2D albedoMap : register(t0);
Texture2D normalMap : register(t1);
Texture2D heightMap : register(t2);
Texture2D roughnessMap : register(t3);
Texture2D metallicMap : register(t4);
Texture2D alphaMap : register(t5);
Texture2D occlusionMap : register(t6);
TextureCube environmentMap : register(t7);

SamplerState textureSampler
{
	//Filter = MIN_MAG_MIP_LINEAR;
	Filter = ANISOTROPIC;
	AddressU = Wrap;
	AddressV = Wrap;
};

VS_OUT VS(in VS_IN input)
{
	float3x3 wsTransform = (float3x3)world;

	VS_OUT output = (VS_OUT)0;
	float4 position = float4(input.position.xyz, 1);
	output.position = mul(world, input.position);
	output.screen = mul(mul(proj, view), output.position);
	//output.screen = mul(mul(mul(proj, view), world), input.position);
	//output.screen = mul(mul(position, world), mul(view, proj));
	output.normal = mul(wsTransform, input.normal);
	//output.binormal = mul(input.normal, wsTransform);
	//output.tangent = mul(input.normal, wsTransform);

	//float3x3 TBN = float3x3(normalize(input.tangent), normalize(input.binormal), normalize(input.normal));
	float3x3 TBN = float3x3(input.tangent, input.binormal, input.normal);
	output.basis = mul(wsTransform, transpose(TBN));
	//output.basis = transpose(output.basis);
	//output.basis[0] = normalize(output.basis[0]);
	//output.basis[1] = normalize(output.basis[1]);
	//output.basis[2] = normalize(output.basis[2]);
	//output.basis = transpose(output.basis);

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

	// Inbound vector from the light position to the vertex position.
	float3 lightVector;
	float intensity;
	float3 position;
	float p0;
	float3 direction;
	float p1;
};

static const uint NUM_LIGHTS = 1;
cbuffer PsData : register(b0)
{
	Light lights[NUM_LIGHTS];
};

// Constant normal incidence Fresnel factor for all dielectrics.
static const float3 Fdielectric = 0.04;

#define PI 3.1415926535897932384626433832795f
#define GAMMA 2.2f

struct Properties {
	float3 position;
	float2 uv;
	float3 normal;
	float3 binormal;
	float3 tangent;
};

struct Material {
	float4 albedo;
	float3 specular;
	float roughness;
	float3 normal;
};




float FresnelSchlick(float f0, float fd90, float view)
{
	return f0 + (fd90 - f0) * pow(max(1.0f - view, 0.1f), 5.0f);
}

float Disney(Properties properties, Light light, Material material, float3 eye)
{
	float3 halfVector = normalize(light.lightVector + eye);

	float NdotL = saturate(dot(properties.normal, light.lightVector));
	float LdotH = saturate(dot(light.lightVector, halfVector));
	float NdotV = saturate(dot(properties.normal, eye));

	float energyBias = lerp(0.0f, 0.5f, material.roughness);
	float energyFactor = lerp(1.0f, 1.0f / 1.51f, material.roughness);
	float fd90 = energyBias + 2.0f * (LdotH * LdotH) * material.roughness;
	float f0 = 1.0f;

	float lightScatter = FresnelSchlick(f0, fd90, NdotL);
	float viewScatter = FresnelSchlick(f0, fd90, NdotV);

	return lightScatter * viewScatter * energyFactor;
}

float DistributionGGX(float3 N, float3 H, float a)
{
	float a2 = a * a;
	float NdotH = max(dot(N, H), 0.0);
	float NdotH2 = NdotH * NdotH;

	float nom = a2;
	float denom = (NdotH2 * (a2 - 1.0) + 1.0);
	denom = PI * denom * denom;

	return nom / denom;
}

float3 GGX(Properties properties, Light light, Material material, float3 eye)
{
	float3 h = normalize(light.lightVector + eye);
	float NdotH = saturate(dot(properties.normal, h));

	float rough2 = max(material.roughness * material.roughness, 2.0e-3f); // capped so spec highlights don't disappear
	float rough4 = rough2 * rough2;

	float d = (NdotH * rough4 - NdotH) * NdotH + 1.0f;
	float D = rough4 / (PI * (d * d));
	//float D = DistributionGGX(-properties.normal, h, material.roughness);

	// Fresnel
	//float3 h2 = normalize(-light.lightVector + eye);
	float3 reflectivity = material.specular;
	float fresnel = 1.0;
	float NdotL = saturate(dot(properties.normal, -light.lightVector));
	float LdotH = saturate(dot(-light.lightVector, h));
	float NdotV = saturate(dot(properties.normal, eye));
	float3 F = reflectivity + (fresnel - fresnel * reflectivity) * exp2((-5.55473f * LdotH - 6.98316f) * LdotH);

	// geometric / visibility
	float k = rough2 * 0.5f;
	float G_SmithL = NdotL * (1.0f - k) + k;
	float G_SmithV = NdotV * (1.0f - k) + k;
	float G = 0.25f / (G_SmithL * G_SmithV);

	return G * D * F;
	//return F;
}

float Diffuse(Properties properties, Light light, Material material, float3 eye)
{
	return Disney(properties, light, material, eye);
}

float3 Specular(Properties properties, Light light, Material material, float3 eye)
{
	return GGX(properties, light, material, eye);
}

//float3 RadianceIBLIntegration(float NdotV, float roughness, float3 specular)
//{
//	float2 preintegratedFG = u_PreintegratedFG.Sample(AnisoClamp, float2(roughness, 1.0f - NdotV)).rg;
//	return specular * preintegratedFG.r + preintegratedFG.g;
//}

float3 IBL(Properties properties, Material material, float3 eye)
{
	// Note: Currently this function assumes a cube texture resolution of 1024x1024
	float NdotV = max(dot(properties.normal, eye), 0.0f);

	float3 reflectionVector = normalize(reflect(-eye, properties.normal));
	float smoothness = 1.0f - material.roughness;
	float mipLevel = (1.0f - smoothness * smoothness) * 11.0f;
	float4 cs = environmentMap.SampleLevel(textureSampler, reflectionVector, mipLevel);
	//float4 cs = environmentMap.Sample(textureSampler, reflectionVector);
	float3 result = pow(cs.xyz, GAMMA) * material.specular; // RadianceIBLIntegration(NdotV, material.roughness, material.specular);
	//result = reflectionVector / 2 + 0.5;
	//result = properties.normal / 2 + 0.5;

	float3 diffuseDominantDirection = properties.normal;
	float diffuseLowMip = 9.6;
	float3 diffuseImageLighting = environmentMap.SampleLevel(textureSampler, diffuseDominantDirection, diffuseLowMip).rgb;
	//float3 diffuseImageLighting = environmentMap.Sample(textureSampler, diffuseDominantDirection).rgb;
	diffuseImageLighting = pow(diffuseImageLighting, GAMMA);

	return result + diffuseImageLighting * material.albedo.rgb;
}

float4 GammaCorrectTexture(Texture2D t, SamplerState s, float2 uv)
{
	float4 samp = t.Sample(s, uv);
	return float4(pow(samp.rgb, GAMMA), samp.a);
}

float3 GammaCorrectTextureRGB(Texture2D t, SamplerState s, float2 uv)
{
	float4 samp = t.Sample(s, uv);
	return float3(pow(samp.rgb, GAMMA));
}

float3 RestoreGamma(float3 color)
{
	return pow(color, 1.0f / GAMMA);
}

float4 PS(VS_OUT input) : SV_Target
{
	Properties properties;
	properties.position = input.position.xyz;
	properties.uv = input.uv;
	properties.normal = normalize(input.normal);
	//properties.binormal = input.binormal;
	//properties.tangent = input.tangent;

	Material material;
	material.albedo = GammaCorrectTexture(albedoMap, textureSampler, input.uv);
	//material.albedo.a = alphaMap.Sample(textureSampler, input.uv).r;
	material.specular = metallicMap.Sample(textureSampler, input.uv).rgb;// *material.albedo.rgb;
	material.roughness = roughnessMap.Sample(textureSampler, input.uv).r;
	//material.roughness = 1 - heightMap.Sample(textureSampler, input.uv).r;
	material.normal = normalize(normalMap.Sample(textureSampler, input.uv).rgb * 2 - 1);
	properties.normal = normalize(mul(input.basis, material.normal));

	float height = heightMap.Sample(textureSampler, input.uv).r;
	float2 occlusion = occlusionMap.Sample(textureSampler, input.uv).ra;

	float3 eye = normalize(input.cameraPosition - properties.position);

	// TODO: rotate normals?

	// Accumulators
	float4 diffuse = (float4)0;
	float3 specular = (float3)0;

	// Per light computation
	[unroll(NUM_LIGHTS)]
	for (uint i = 0; i < NUM_LIGHTS; i++)
	{
		Light light = lights[i];
		light.lightVector *= -1; // Convert to outbound light vector
		light.lightVector = normalize(light.position - properties.position);

		float NdotL = saturate(dot(properties.normal, light.lightVector));

		// Diffuse Calculation
		diffuse += NdotL * Diffuse(properties, light, material, eye) * light.color * light.intensity;

		// Specular Calculation
		specular += NdotL * Specular(properties, light, material, eye) * light.color.rgb * light.intensity;

		//diffuse = material.roughness;
	}

	// Shadow computation

	// Color composition
	float3 ibl = IBL(properties, material, eye);
	float3 color = (material.albedo.rgb * diffuse.rgb + specular + ibl) * (occlusion.x * occlusion.y +  (1 - occlusion.y));
	color = RestoreGamma(color);
	//color = properties.normal / 2 + 0.5;
	//color = input.basis[0] / 2 + 0.5;
	//color.rgb = float3(input.uv, 0);
	return float4(color, material.albedo.a);
}