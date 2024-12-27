// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GreyTextureShader" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
		_ResTex("Overlay (RGB) Trans (A)",2D) = "black"{}
	}
	SubShader
	{
		Tags
		{
			"IGNOREPROJECTOR" = "true"
			"QUEUE" = "Transparent"
			"RenderType" = "Transparent"
		}
		Pass
		{
			ZWrite Off	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			 
			sampler2D _MainTex;
			sampler2D _ResTex;

			struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			float4 _MainTex_ST; 

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 texcol1 = tex2D(_MainTex, i.uv);
				half4 texcol2 = tex2D(_ResTex, i.uv);
				half4 output = half4(0, 0, 0, 1);
				output.rgb = dot(texcol1.rgb, float3(0.3, 0.59, 0.11));

				output = texcol2.a > 0 ? texcol2 : output;
				output.a = 1;
				return output;
			}
			ENDCG

		}
	}
	Fallback "VertexLit"
}