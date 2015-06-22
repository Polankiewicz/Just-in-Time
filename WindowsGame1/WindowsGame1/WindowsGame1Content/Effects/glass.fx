float4x4 xWorldViewProjection;
float4x4 xLightsWorldViewProjection;
float4x4 xWorld;
float xAmbient;
Texture xTexture;
float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;
// TODO: add effect parameters here.

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 UV : TEXCOORD0;
    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV    : TEXCOORD0;
	float3 Normal        : TEXCOORD1;
	float4 Color : COLOR0;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	
	output.Position = mul(input.Position, xWorldViewProjection);
	output.UV = input.UV;
	output.Normal = input.Normal;
	float4 normal = mul(input.Normal, xLightsWorldViewProjection);
	float lightIntensity = dot(normal, DiffuseLightDirection);
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 baseColor =  tex2D(TextureSampler, input.UV);
	//baseColor.a = 0.3;
	//return baseColor;
	return saturate(input.Color + baseColor );
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.
		
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
