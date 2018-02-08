struct VertexIn
{
	float3 IPosition  : POSITION;
};

struct VertexOut
{
	float4 Position  : SV_Position;
};

VertexOut VSMain(VertexIn vin)
{
	VertexOut vout;
	vout.Position = float4(vin.IPosition.xyz,1.0);
	return vout;
}

float4 PSMain(VertexOut pin) : SV_Target
{
	return float4(0.0,1.0,0.0,1.0);
}