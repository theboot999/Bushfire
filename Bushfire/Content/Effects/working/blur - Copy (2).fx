sampler TextureSampler : register(s0);
Texture2D <float4> myTex2D;
#define SAMPLE_COUNT 15
float2 SampleOffsets[SAMPLE_COUNT];
float SampleWeights[SAMPLE_COUNT];
 
 
 
float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
   // float4 tex;
  //  tex = myTex2D.Sample(TextureSampler, texCoord.xy) * .6f;
  //  tex += myTex2D.Sample(TextureSampler, texCoord.xy + (0.005)) * .2f;
  //  return tex;
	
	
	
	
	 float4 c = 0;
    
    // Combine a number of weighted image filter taps.
    for (int i = 0; i < SAMPLE_COUNT; i++)
    {
        c += tex2D(TextureSampler, texCoord + SampleOffsets[i]) * SampleWeights[i];
	//	c += myTex2D.Sample(TextureSampler, texCoord + SampleOffsets[i]) * SampleWeights[i];
    }
    
    return c;
	
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();  
    }
}