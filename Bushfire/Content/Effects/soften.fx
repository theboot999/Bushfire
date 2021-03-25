texture Texture;
float weights[5];
float offsets[5];

sampler2D TextureSampler = sampler_state
{
	texture = <Texture>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};


float4 BlurHorizontal(float4 position : POSITION0, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : Color0
{
		float4 output = float4(0, 0, 0, 1);
		for (int i = 0; i < 4; i++)
		{
			output += tex2D(TextureSampler, texCoord + float2(offsets[i], 0)) * weights[i];
		}	
	
		for (int i = 0; i < 4; i++)
		{
			output += tex2D(TextureSampler, texCoord + float2(0, offsets[i])) * weights[i];
		}
		
        return output / 16;


}


technique Blur
{
	pass Horizontal
	{
		PixelShader = compile ps_4_0 BlurHorizontal();
	}
}