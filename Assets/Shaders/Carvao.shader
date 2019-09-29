Shader "Unlit/Carvao"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _EmissiveTex ("EmissiveTex", 2D) = "black" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        [HDR]_EmissiveColor("EmissiveColor", Color) = (1,1,1,1)

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                float4 pos : COLOR;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

            sampler2D _NoiseTex;

            sampler2D _EmissiveTex;
            fixed4 _Color;
            fixed4 _EmissiveColor;

            
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = mul (unity_ObjectToWorld, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.pos = o.vertex;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv) * _Color + tex2D(_EmissiveTex, i.uv + _Time.xx * 0.2) * _EmissiveColor * (sin(_Time.x * 40) + 1) / 2;
                fixed4 col = tex2D(_MainTex, i.uv) * _Color + tex2D(_EmissiveTex, i.uv ) * _EmissiveColor * (sin((_Time.x + tex2D(_NoiseTex, i.pos.xy / 5).r)* 10 )*25);
                //fixed4 col = tex2D(_NoiseTex, i.vertex.xy / 1000).r; 
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
