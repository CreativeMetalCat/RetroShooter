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
static const int MAX_SPOT_LIGHTS  = 16;

//Change this value depending on how much spot lights you might have in one place in your project
//note: while direcational lights need least amount of calculations, they are the kind that shines everywhere and this is 
//the only situation where limit is not to avoid performance drop but to prevent having hight amount of these in one level
static const int MAX_DIR_LIGHTS  = 2;

//Directional light can only have one ACTIVE instance per scene

matrix WorldViewProjection;

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float3 ViewVector = float3(1,0,0);

float4 AmbientLightColor;
float AmbientLightIntensity;

float Shininess = 0;
bool UseSpecularMap;

texture BaseTexture;
texture SpecualTexture;
texture NormalTexture;

float bumpConstant = 5;

sampler2D textureSampler = sampler_state
{
	Texture = (BaseTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};


sampler2D textureSamplerSpecular = sampler_state
{
	Texture = (BaseTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

sampler2D textureSamplerNormal = sampler_state
{
	Texture = (NormalTexture);
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
	float3 Tangent : TANGENT0;
    float3 Binormal : BINORMAL0;
	float4 Normal:NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
	float3 WorldPos : TEXCOORD2;
	float4 Normal:TEXCOORD3;
	float3 Tangent : TEXCOORD4;
    float3 Binormal : TEXCOORD5;
};

//point lights data. Not using structs because they tend to cause "shader has corrupt ctab data" error during shader compilation
float4 pointLightsColor[MAX_POINT_LIGHTS];
float3 pointLightsLocation[MAX_POINT_LIGHTS];
float  pointLightsIntensity[MAX_POINT_LIGHTS];
float  pointLightsRadius[MAX_POINT_LIGHTS];
bool   pointLightsValid[MAX_POINT_LIGHTS];

//spotlights data. Not using structs because they tend to cause "shader has corrupt ctab data" error during shader compilation
float4 spotLightsColor[MAX_SPOT_LIGHTS];
float3 spotLightsLocation[MAX_SPOT_LIGHTS];
float3 spotLightsDirection[MAX_SPOT_LIGHTS];
float  spotLightsIntensity[MAX_SPOT_LIGHTS];
float  spotLightsRadius[MAX_SPOT_LIGHTS];
float  spotLightsInnerCutoff[MAX_SPOT_LIGHTS];
float  spotLightsOuterCutoff[MAX_SPOT_LIGHTS];
bool   spotLightsValid[MAX_SPOT_LIGHTS];

//directional light data. Not using structs because they tend to cause "shader has corrupt ctab data" error during shader compilation
float3 dirLightsDirection[MAX_DIR_LIGHTS];
float4 dirLightsColor[MAX_DIR_LIGHTS];
float  dirLightsIntensity[MAX_DIR_LIGHTS];

float Vec3LenghtSquared(float3 vec)
{
	return vec.x*vec.x+vec.y*vec.y+vec.z*vec.z;
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition,Projection);

	float4 normal = mul(input.Normal,World);

	output.WorldPos = mul(input.Position, World).xyz;

	output.Color = AmbientLightColor*AmbientLightIntensity;

	output.Normal = normalize(mul(input.Normal,World));
	output.Tangent = normalize(mul(input.Tangent,World));
	output.Binormal = normalize(mul(input.Binormal,World));

	output.TextureCoordinate = input.TextureCoordinate;

	//output.Normal = normalize(normal);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float3 bump = bumpConstant*(tex2D(textureSamplerNormal,input.TextureCoordinate)) - float4(0.5,0.5,0.5,0);
	//float3 bumpNormal = input.Normal + (bump.x * input.Tangent + bump.y * input.Binormal);
	float3 bumpNormal = mul(bump,float3x3(input.Tangent,input.Binormal,input.Normal.xyz));
    bumpNormal = normalize(bumpNormal);
	
	float4 resultColor = float4(0,0,0,0);
	float4 specularColor = float4(0,0,0,0);

	[unroll]
	for(int i = 0; i < MAX_POINT_LIGHTS; i++)
	{
		if(pointLightsIntensity[i] > 0)
		{
			float3 pointLightDirection = input.WorldPos - pointLightsLocation[i];
			float distanceSq = Vec3LenghtSquared(pointLightDirection);
			float radius = pointLightsRadius[i];
			[branch]
			if(distanceSq < abs(radius*radius))
			{
				float distance = sqrt(distanceSq);
				float du = distance/(1-distanceSq/(radius*radius -1));
				float denom = du / abs(radius) + 1;
				float attenuetion = 1/(denom*denom);

				pointLightDirection /= distance;
				float dotProduct = dot(input.Normal,-pointLightDirection);
				float bumpInt = dot(normalize(pointLightDirection),bumpNormal);
				float bumpDotProduct = dot(bumpNormal,-pointLightDirection);

				resultColor += (bumpDotProduct*  pointLightsColor[i] * pointLightsIntensity[i] * attenuetion );
				//resultColor += (/*dotProduct**/  pointLightsColor[i] * pointLightsIntensity[i] * attenuetion )*bumpDotProduct;
				specularColor +=  UseSpecularMap?tex2D(textureSamplerSpecular,input.TextureCoordinate).r*pointLightsColor[i] *pow(saturate(dotProduct),Shininess):float4(0,0,0,0);
				
			}
		}
				
	}

	[unroll]
	for(int i = 0; i < MAX_SPOT_LIGHTS; i++)
	{
		[branch]
		if(spotLightsIntensity[i] > 0)
		{
			float3 spotLightDirection = input.WorldPos - spotLightsLocation[i];
			float theta = dot(spotLightDirection,normalize(-spotLightsDirection[i]));

			float distanceSq = Vec3LenghtSquared(spotLightDirection);
			float radius = spotLightsRadius[i];

			float eps = spotLightsInnerCutoff[i] - spotLightsOuterCutoff[i];
			float intensity = saturate((theta-spotLightsOuterCutoff[i])/eps);
			//do calculations
				
				
			float distance = sqrt(distanceSq);
			float du = distance/(1-distanceSq/(radius*radius -1));
			float denom = du / abs(radius) + 1;
			float attenuetion = 1/(denom*denom);

			spotLightDirection /= distance;
			float dotProduct = dot(input.Normal,-spotLightDirection);
			float bumpDotProduct = dot(bumpNormal,-spotLightDirection);
			//resultColor += saturate(bumpDotProduct)* spotLightsColor[i]*spotLightsIntensity[i]*intensity  * attenuetion;
			
			//specularColor += UseSpecularMap?tex2D(textureSamplerSpecular,input.TextureCoordinate).r*spotLightsColor[i]*pow(saturate(dotProduct),Shininess):float4(0,0,0,0);
			
		}
	}
	for(int d = 0; d < MAX_DIR_LIGHTS; d++)
	{
		float dotProduct = dot(input.Normal,-normalize(-dirLightsDirection[d]));
		resultColor += saturate(dotProduct)*dirLightsColor[d]*dirLightsIntensity[d];	
	}
	//return saturate((input.Color + resultColor ) * tex2D(textureSampler, input.TextureCoordinate)+ specularColor);
	//return saturate((input.Color + resultColor ) + specularColor);
	return  tex2D(textureSampler, input.TextureCoordinate);
}

technique BasicTexture
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};