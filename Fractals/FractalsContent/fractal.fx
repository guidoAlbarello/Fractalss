float2 Viewport;

int maxIterations = 100;
float2 zoom = 3;
float2 Pan = float2(0.5,0);
float Aspect = 0.75;

float3 colors = float3(15,4,12);

float2 juliaSeed = float2(-0.03769230769230769230769230769231,0.692307692307692307692307692308);

float4 juliaShader(float2 pos : TEXCOORD) : COLOR
{
	float2 z = (pos-0.5) * zoom.x * float2(1,Aspect) - Pan;
	float2 c = juliaSeed;
	int counts = 0;
	int maxHere = maxIterations;
	int max = maxIterations;
	float2 squares = 0;
	
	for (int i = 0; i < maxHere; i++)
	{
		squares = float2(z.x * z.x, z.y*z.y);
		z = float2(squares.x - squares.y,z.x * z.y * 2) + c;
		counts++;
		if((maxHere == max) && (squares.x +squares.y) > 4)
		{
			break;
		}
	}
	float var = (float(counts) - (log(log(sqrt(squares.x+squares.y))) / log(2.0))) / float(max);
	return float4(sin(var * colors.x),sin(var * colors.y),sin(var*colors.z),1);
}



void vertexShader(inout float4 color : COLOR0, inout float4 texCoord : TEXCOORD0, inout float4 position : POSITION0)
{
	position.xy -= 0.5;
	position.xy /= Viewport;
	position.xy *= float2(2,-2);
	position.xy -= float2(1,-1);
}

technique julia
{
	pass P0
	{
		VertexShader = compile vs_3_0 vertexShader();
		PixelShader = compile ps_3_0 juliaShader();
	}
}