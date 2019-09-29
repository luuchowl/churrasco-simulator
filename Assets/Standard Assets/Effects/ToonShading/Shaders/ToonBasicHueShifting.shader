// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/Basic" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
		_Color ("Color", Color) = (0.5,0.5,0.5,0.5)
		_ShadowColor ("Shadow Color", Color) = (0.0,0.0,0.0,1.0)
		_LightColor ("Light Color", Color) = (1.0,1.0,1.0,1.0)
	}


	SubShader {
		Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
		Pass {
			Name "BASE"
			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			uniform float4 _LightColor0;
			float4 _Color;
			float4 _ShadowColor;
			float4 _LightColor;


			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float3 cubenormal : TEXCOORD1;
				float4 normalDir: NORMAL;

				UNITY_FOG_COORDS(2)
			};

			v2f vert (appdata v)
			{
				
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				o.normalDir = normalize(mul ( float4(v.normal, 0.0), unity_WorldToObject).xyzw); 


				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 normalDirection = i.normalDir; 
				float4 lightDirection = normalize(_WorldSpaceLightPos0.xyzw);
				float atten = 1.0;
				float4 diffuseReflection = atten * _LightColor0.xyzw * max (0.0, dot(normalDirection, lightDirection));


				fixed4 col = _Color * tex2D(_MainTex, i.texcoord);
				fixed4 cube = texCUBE(_ToonShade, i.cubenormal);


				float4 lightIntensity = cube;
				float4 hueshift = float4 (((float4(1,1,1,1)-lightIntensity) - 0.6*pow(float4(1,1,1,1)-lightIntensity, 2) ) * _ShadowColor.xyz, 0) + float4 (float4(1,1,1,1)*pow(lightIntensity,2) * _LightColor.rgb, 0);
				//float4 hueshift = float4 (((float4(1)-lightIntensity) - 0.6*pow(float4(1)-lightIntensity, 2) ) * _ShadowColor.xyz, 0) + float4 (float4(1)*pow(lightIntensity,2) * _LightColor.rgb, 0);


				//fixed4 c = fixed4(2.0f * hueshift.rgb + col.rgb, col.a) ;

				fixed4 c = fixed4(2.0f * hueshift.rgb * col.rgb, col.a) ;
				UNITY_APPLY_FOG(i.fogCoord, c);
				//float4 hueshift = float4 (((1-lightIntensity) - 0.6*pow(1-lightIntensity, 2) ) * _ShadowColor.xyz, 0) + float4 (1*pow(lightIntensity,2) * _LightColor.rgb, 0) + float4(specularReflection, 1) ;
				return c ;
			}
			ENDCG			
		}
	} 

	Fallback "VertexLit"
}
