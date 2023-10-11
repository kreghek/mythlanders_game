sampler inputTexture;

float DistanceX;
float DistanceY;

float4 MainPS(float2 textureCoordinates: TEXCOORD0) : COLOR0
{
	float2 textureCoordinates2 = float2(textureCoordinates.x + DistanceX, textureCoordinates.y + DistanceY);
	float4 color = tex2D(inputTexture, textureCoordinates2);
	return color;
}

technique BasicColorDrawing
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 MainPS();
	}
};
