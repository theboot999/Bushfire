texture Texture;

sampler2D TextureSampler = sampler_state
{
	texture = <Texture>;

};


float4 BlurHorizontal(float4 position : POSITION0, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : Color0
{
   float4 pixel = tex2D(TextureSampler, texCoord);
	float4 output = float4(0, 0, 0, 1);
    
	if(pixel.r < 0.5) 
	{
	for (int x = -2; x < 3; x++)
	{		
		for (int y = -2; y < 3; y++)
		{		
			output += tex2D(TextureSampler, texCoord + float2(x, y));
			}
		}		
	}
	output = output / 5;
	return output;
}




technique Blur
{
	pass Horizontal
	{
		PixelShader = compile ps_4_0 BlurHorizontal();
	}
	
}