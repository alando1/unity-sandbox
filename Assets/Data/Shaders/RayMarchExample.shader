// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Raymarch Example"
{
	SubShader
	{
	  Pass
	  {
		Blend SrcAlpha Zero

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct v2f
		{
		  float2 uv : TEXCOORD0;
		  float4 vertex : SV_POSITION;
		};

		v2f vert(appdata_base v)
		{
		  v2f o;
		  o.vertex = UnityObjectToClipPos(v.vertex);
		  o.uv = v.texcoord;
		  return o;
		}

		float distFunc(float3 pos)
		{
		  const float sphereRadius = 1;
		  return length(pos) - sphereRadius;
		}

		fixed4 renderSurface(float3 pos)
		{
		  const float2 eps = float2(0.0, 0.01);

		  float ambientIntensity = 0.1;
		  float3 lightDir = float3(0, -0.5, 0.5);

		  float3 normal = normalize(float3(
			distFunc(pos + eps.yxx) - distFunc(pos - eps.yxx),
			distFunc(pos + eps.xyx) - distFunc(pos - eps.xyx),
			distFunc(pos + eps.xxy) - distFunc(pos - eps.xxy)));

		  float diffuse = ambientIntensity + max(dot(-lightDir, normal), 0);

		  return fixed4(diffuse, diffuse, diffuse, 1);
		}

		// New Code
		float3 _CamPos;
		float3 _CamRight;
		float3 _CamUp;
		float3 _CamForward;
		//float _AspectRatio;
		//float _FieldOfView;

		//fixed4 frag(v2f i) : SV_Target
		//{
		//  float2 uv = (i.uv - 0.5) * _FieldOfView;
		//  uv.x *= _AspectRatio;
		fixed4 frag(v2f i) : SV_Target
		{
		  float2 uv = i.uv - 0.5;
		  float3 pos = _CamPos;
		  float3 ray = _CamUp * uv.y + _CamRight * uv.x + _CamForward;

		//old code
		//fixed4 frag(v2f i) : SV_Target
		//{
		//  float2 uv = i.uv - 0.5;
		//  float3 camUp = float3(0, 1, 0);
		//  float3 camForward = float3(0, 0, 1);
		//  float3 camRight = float3(1, 0, 0);

		//  float3 pos = float3(0, 0, -5);
		//  float3 ray = camUp * uv.y + camRight * uv.x + camForward;

		  fixed4 color = 0;

		  for (int i = 0; i < 30; i++)
		  {
			float d = distFunc(pos);

			if (d < 0.01)
			{
			  color = renderSurface(pos);
			  break;
			}

			pos += ray * d;

			if (d > 40)
			{
				break;
			}
		  }

		  return color;
		}
		ENDCG
	  }
	}
}

//Shader "Custom/NewSurfaceShader" {
//	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
//	}
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//
//		CGPROGRAM
//		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard fullforwardshadows
//
//		// Use shader model 3.0 target, to get nicer looking lighting
//		#pragma target 3.0
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		half _Glossiness;
//		half _Metallic;
//		fixed4 _Color;
//
//		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
//		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
//		// #pragma instancing_options assumeuniformscaling
//		UNITY_INSTANCING_BUFFER_START(Props)
//			// put more per-instance properties here
//		UNITY_INSTANCING_BUFFER_END(Props)
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
//			// Metallic and smoothness come from slider variables
//			o.Metallic = _Metallic;
//			o.Smoothness = _Glossiness;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	}
//	FallBack "Diffuse"
//}
