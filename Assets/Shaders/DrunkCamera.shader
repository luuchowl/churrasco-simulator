Shader "Hidden/DrunkCamera"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Float) = 0.3
        _Debug ("Debug", Range(0.1, 2)) = 0.1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

            float _Distortion;
            float _Intensity;
            float _Debug;

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
                float2 uv = i.uv;
                i.uv.x -= 0.5;
                //i.uv.x *= 1/ ;
                i.uv.x *= 1/(1 + _Intensity * _Distortion);
                i.uv.x += 0.5;
                //i.uv.x -= 0.5 -  _Intensity * _Distortion ;
				fixed4 col = tex2D(_MainTex, uv) * 0.5 + tex2D(_MainTex, i.uv) * 0.5;
                // fixed4 col = tex2D(_MainTex, i.uv) * 0.5;
				// just invert the colors
				col.rgb = col.rgb;
				return col;
			}
			ENDCG
		}
	}
}
