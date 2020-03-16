Shader "Hidden/Galaxy"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off// ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			//CBS
//Parallax scrolling fracal galaxy.
//Inspired by JoshP's Simplicity shader: https://www.shadertoy.com/view/lslGWr

// http://www.fracalforums.com/new-theories-and-research/very-simple-formula-for-fracal-patterns/
			float field(in float3 p, float s) 
			{
				float strength = 7. + .03 * log(1.e-6 + frac(sin(_Time.y) * 4373.11));
				float accum = s / 4.;
				float prev = 0.;
				float tw = 0.;
				for (int i = 0; i < 26; ++i) 
				{
					float mag = dot(p, p);
					p = abs(p) / mag + float3(-.5, -.4, -1.5);
					float w = exp(-float(i) / 7.);
					accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
					tw += w;
					prev = mag;
				}
				return max(0., 5. * accum / tw - .7);
			}

			// Less iterations for second layer
			float field2(in float3 p, float s) 
			{
				float strength = 7. + .03 * log(1.e-6 + frac(sin(_Time.y) * 4373.11));
				float accum = s / 4.;
				float prev = 0.;
				float tw = 0.;
				for (int i = 0; i < 18; ++i) 
				{
					float mag = dot(p, p);
					p = abs(p) / mag + float3(-.5, -.4, -1.5);
					float w = exp(-float(i) / 7.);
					accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
					tw += w;
					prev = mag;
				}
				return max(0., 5. * accum / tw - .7);
			}

			float3 nrand3(float2 co)
			{
				float3 a = frac(cos(co.x*8.3e-3 + co.y)*float3(1.3e5, 4.7e5, 2.9e5));
				float3 b = frac(sin(co.x*0.3e-3 + co.y)*float3(8.1e5, 1.0e5, 0.1e5));
				float3 c = lerp(a, b, 0.5);
				return c;
			}


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
			   float2 uv = -1.0 + 2.0 * i.uv;
			   float2 uvs = uv * _ScreenParams.xy / max(_ScreenParams.x, _ScreenParams.y);
				float3 p = float3(uvs / 4., 0) + float3(1., -1.3, 0.);
				p += .2 * float3(sin(_Time.y / 16.), sin(_Time.y / 12.),  sin(_Time.y / 128.));

				float freqs[4];
				//Sound
				freqs[0] = 0.5;//freqs[0] = texture(iChannel0, float2(0.01, 0.25)).x;
				freqs[1] = 0.5;//freqs[1] = texture(iChannel0, float2(0.07, 0.25)).x;
				freqs[2] = 0.5;//freqs[2] = texture(iChannel0, float2(0.15, 0.25)).x;
				freqs[3] = 0.5;//freqs[3] = texture(iChannel0, float2(0.30, 0.25)).x;

				float t = field(p,freqs[2]);
				float v = (1. - exp((abs(uv.x) - 1.) * 6.)) * (1. - exp((abs(uv.y) - 1.) * 6.));

				//Second Layer
				float3 p2 = float3(uvs / (4. + sin(_Time.y*0.11)*0.2 + 0.2 + sin(_Time.y*0.15)*0.3 + 0.4), 1.5) + float3(2., -1.3, -1.);
				p2 += 0.25 * float3(sin(_Time.y / 16.), sin(_Time.y / 12.),  sin(_Time.y / 128.));
				float t2 = field2(p2,freqs[3]);
				float4 c2 = lerp(.4, 1., v) * float4(1.3 * t2 * t2 * t2 ,1.8  * t2 * t2 , t2* freqs[0], t2);


				//Let's add some stars
				//Thanks to http://glsl.heroku.com/e#6904.0
				float2 seed = p.xy * 2.0;
				seed = floor(seed * _ScreenParams.x);
				float3 rnd = nrand3(seed);
				float4 starcolor = float4(pow(rnd.y,40.0), pow(rnd.y, 40.0), pow(rnd.y, 40.0), pow(rnd.y, 40.0));

				//Second Layer
				float2 seed2 = p2.xy * 2.0;
				seed2 = floor(seed2 * _ScreenParams.x);
				float3 rnd2 = nrand3(seed2);
				starcolor += float4(pow(rnd2.y,40.0), pow(rnd.y, 40.0), pow(rnd.y, 40.0), pow(rnd.y, 40.0));

				//fragColor = lerp(freqs[3] - .3, 1., v) * float4(1.5*freqs[2] * t * t* t , 1.2*freqs[1] * t * t, freqs[3] * t, 1.0) + c2 + starcolor;
				return lerp(-0.3, 1., v) * float4(1.5 * t * t* t, 1.2 * t * t, t, 1.0) + c2 + starcolor;

			}
			ENDCG
		}
	}
}
