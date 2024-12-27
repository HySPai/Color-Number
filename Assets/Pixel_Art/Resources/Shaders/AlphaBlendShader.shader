Shader "Custom/AlphaBlendShader"
{
	Properties
	{
		_MainTex ("Sprite Texture",2D) = "white"{}
		_MaxAlpha ("MaxAlpha",Range(0,1)) = 1
		_MinAlpha ("MinAlpha",Range(0,1)) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			SetTexture[_MainTex]
			{  
				combine texture * constant 
			}
		}
	}
}