sampler s0;
texture lightMask;
sampler lightSampler = sampler_state { Texture = <lightMask>; };
float red;
float blue;
float green;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords: TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);
    float4 lightColor = tex2D(lightSampler, coords);
	lightColor.r *= red;
    lightColor.g *= green;
	lightColor.b *= blue;
    return color * lightColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}


