sampler s0;
texture lightMask;
texture shadowMask;
float red;
float blue;
float green;
sampler lightSampler = sampler_state { Texture = <lightMask>; };
sampler shadowSampler = sampler_state { Texture = <shadowMask>; };

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords: TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);
    float4 lightColor = tex2D(lightSampler, coords);
	float4 shadowColor = tex2D(shadowSampler, coords);
	lightColor.r *= red;
    lightColor.g *= green;
	lightColor.b *= blue;
    return color * lightColor * shadowColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}


