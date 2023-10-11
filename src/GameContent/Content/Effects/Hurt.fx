sampler inputTexture;

float DistanceX;
float DistanceY;

float4 MainPS(float2 textureCoordinates: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(inputTexture, textureCoordinates);
	color.r = 0.5f;
	return color;
}

float4 MainPS2(float2 textureCoordinates: TEXCOORD0) : COLOR0
{
	float2 textureCoordinates2 = float2(textureCoordinates.x + DistanceX, textureCoordinates.y + DistanceY);
	float4 color = tex2D(inputTexture, textureCoordinates2);
	return color;
}

technique Techninque1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 MainPS();
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
	}
	
	pass Pass2
    {
        PixelShader = compile ps_3_0 MainPS2();
    }
};
