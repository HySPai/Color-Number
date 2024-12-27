Shader "Custom/AlphaReverseShader"
{
  Properties
  {
      _MainTex ("Sprite Texture",2D) = "white"{}
      _Color ("Color",Color) = (1,1,1,1)
  }
	  SubShader{
		  Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		  LOD 100

		  ZWrite Off
		  Blend SrcAlpha OneMinusSrcAlpha

		  Pass
	  {
		  SetTexture[_MainTex]{ constantColor(1,1,1,[_Alpha]) combine texture * constant }
	  }
	  }
}