#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//Change this value depending on how much point lights you might have in one place in your project
static const int MAX_POINT_LIGHTS  = 16;

//Change this value depending on how much spot lights you might have in one place in your project
#define MAX_SPOT_LIGHTS 8

//Directional light can only have one ACTIVE instance per scene

matrix WorldViewProjection;

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float4 AmbientLightColor;

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

//point lights data. Not using structs because they tend to cause "shader has corrupt ctab data" error during shader compilation
float4 pointLightsColor[MAX_POINT_LIGHTS];
float3 pointLightsLocation[MAX_POINT_LIGHTS];
float pointLightsIntensity[MAX_POINT_LIGHTS];
bool pointLightsValid[MAX_POINT_LIGHTS];

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition,Projection);

	float4 normal = mul(input.Normal,WorldInverseTranspose);
	
	//output.Position = mul(input.Position, WorldViewProjection);
	output.Color = AmbientLightColor;

	output.TextureCoordinate=input.TextureCoordinate;
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 resultColor = float4(0,0,0,0);
	for(int i = 0; i < MAX_POINT_LIGHTS; i++)
	{
		if(pointLightsValid[i] == true)
		{
			//this prevents shader from being compiled for some reason, the issue is caused by pointLights[i].Color
			//resultColor += pointLights[i].Color * pointLights[i].Intensity;
			resultColor += pointLightsColor[i] * pointLightsIntensity[i];
		}
				
	}
	return tex2D(textureSampler, input.TextureCoordinate)*(AmbientLightColor + resultColor);
}

technique BasicTexture
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};