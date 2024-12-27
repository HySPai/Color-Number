// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.


Shader "Custom/3DColour" {
	Properties{
		_Blend("Blend", Range(0, 1)) = 0.0
		_BaseColor("Color", Color) = (1,1,1,1)
		_Color("Tint", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 1
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Highlighted("Highlighted", Range(0, 1)) = 1 
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
			Lighting On
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};
	 
	int _Highlighted;
	half _Blend;
	fixed4 _Color, _BaseColor;
	half _Glossiness;
	half _Metallic; 


	//UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * _Blend;
		o.Albedo = ((_BaseColor.r == _Color.r && _BaseColor.g == _Color.g && _BaseColor.b == _Color.b) ? _BaseColor.rgb : (lerp(_BaseColor, c.rgb, c.a)) - 0.25*_Highlighted);
		// Metallic and smoothness come from slider variables
		//o.Metallic = _Metallic;
		//o.Smoothness = _Glossiness;
		o.Alpha = 1;

			//// Albedo comes from a texture tinted by color
			//fixed4 m = tex2D(_MainTex, IN.uv_MainTex);
			//fixed4 c = m * _BaseColor * _Blend;
			//o.Albedo = c - 0.2 * _Highlighted;// lerp(_Color, c.rgb, c.a);
			//// Metallic and smoothness come from slider variables
			////o.Metallic = _Metallic;
			////o.Smoothness = _Glossiness;
			//o.Alpha = 1;
		}
	ENDCG
	}
	FallBack "Diffuse"
}
