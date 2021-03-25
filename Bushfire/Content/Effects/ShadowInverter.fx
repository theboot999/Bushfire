float darkness;
texture shadowMask;
sampler lightSampler = sampler_state { Texture = <shadowMask>; };

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords: TEXCOORD0) : COLOR0
{
    float4 pixel = tex2D(lightSampler, coords);
	
	if(pixel.r < 0.5) 
	{
		pixel.r = 1;
		pixel.b = 1;
		pixel.g = 1;
			
	}
	else if (pixel.r > 0.5)  
	{
	    pixel.r = 1;
		pixel.b = 1;
		pixel.g = 1;	
		pixel.a = darkness;
		
	}
	return pixel;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}


