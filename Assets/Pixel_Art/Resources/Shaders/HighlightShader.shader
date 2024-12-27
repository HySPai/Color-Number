Shader "Custom/HighlightShader"
{
  Properties
  {
      _MainTex ("MainTex",2D) = "white"{}
      _ImageTex ("ImageTex",2D) = "white"{}
      _Alpha ("Alpha",Range(0,1)) = 0.6
      _CurrentColor ("Current Color",Color) = (1.0,1.0,1.0,1.0)
  }
	  SubShader{
		  Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		  Blend SrcAlpha OneMinusSrcAlpha
		  Pass{

		  CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		  sampler2D _MainTex;
		  sampler2D _ImageTex;
		  float _Alpha;
		  float4 _CurrentColor;

		  struct v2f {
			  float4  pos : SV_POSITION;
			  float2  uv : TEXCOORD0;
		  };

		  float4 _MainTex_ST;
		  float4 _ImageTex_ST;

		  v2f vert(appdata_base v)
		  {
			  v2f o;
			  o.pos = UnityObjectToClipPos(v.vertex);
			  o.uv = TRANSFORM_TEX(v.texcoord, _ImageTex);
			  return o;
		  }

		  half4 frag(v2f i) : COLOR
		  {
			  float4 texcol = tex2D(_ImageTex, i.uv);
              if ((int)(texcol.r * 1000) == (int)(_CurrentColor.r * 1000)
                && (int)(texcol.g * 1000) == (int)(_CurrentColor.g * 1000)
                && (int)(texcol.b * 1000) == (int)(_CurrentColor.b * 1000)) 
              { 
                texcol.a = 0.45;
              } 
              else 
              {
                texcol.a = 0;
              }
			  //texcol.a = ((texcol.r == _CurrentColor.r && texcol.g == _CurrentColor.g && texcol.b == _CurrentColor.b) ? 0.45 : 0);
			  texcol.rgb = float3(0.5, 0.5, 0.5);
			  return texcol;
		  }
			  ENDCG

		  }
	  }
		Fallback "VertexLit"
}