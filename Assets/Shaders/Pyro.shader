Shader "Unlit/Pyro"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _DistortTex ("Distort Tex", 2D) = "grey" {}
        _Intensity ("Intensity", Range(0,0.7)) = 0.01
        [HDR]_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex ooo
			#pragma fragment frag
			// make fog work

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            sampler2D _DistortTex;
            float _Intensity;
            fixed4 _Color;
			
			v2f ooo (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				// sample the texture
                
				fixed4 col = tex2D(_MainTex, i.uv + (tex2D(_DistortTex, i.uv + float2(0,_Time.x * 100) + float2(0, frac(i.vertex.y))) * _Intensity - fixed2(_Intensity, _Intensity)) * i.uv.y ) * _Color;
				// apply fog
                col.a *= pow(i.uv.y + 0.3, 4);
				return col;
			}
			ENDCG
		}
	}
}
