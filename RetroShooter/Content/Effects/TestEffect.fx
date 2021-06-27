#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

texture BaseTexture;

sampler2D textureSampler = sampler_state
{
	Texture = (BaseTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinate:TEXCOORD0;
	float4 Normal:NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition,Projection);

	float4 normal = mul(input.Normal,WorldInverseTranspose);
	
	//output.Position = mul(input.Position, WorldViewProjection);
	output.Color = float4(1,1,1,1);

	output.TextureCoordinate=input.TextureCoordinate;
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return tex2D(textureSampler, input.TextureCoordinate);
}

technique BasicTexture
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};