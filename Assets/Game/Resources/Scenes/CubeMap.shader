Shader "Example/CubeMap" {
	Properties{
		_MainTex("Texture",2D) = "white" {}
		_BumpMap("Bumpmap",2D) = "bump" {}
		_Cube("Cubemap",CUBE) = "" {}
	}
	SubShader{
		Tags {"RenderType"="Opaque"}
		CGPROGRAM
#pragma surface surf Lambert
		struct Input {
			float2 uv_BumpMap;
			float2 uv_MainTex;
			float3 worldRefl;
			INTERNAL_DATA
		};
		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * 0.5; 
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Emission = texCUBE(_Cube, WorldReflectionVector(IN,o.Normal));
		}
		ENDCG
	}
	Fallback "Diffuse"
}
