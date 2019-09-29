Shader "Unlit/FoodShader"
{
	Properties
	{
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
        _Factor("Factor", Range(0, 3)) = 0

		_MainTex ("Texture", 2D) = "white" {}
        _Factor("Factor", Range(0, 5)) = 0
        _Color0 ("Color A", Color) = (1, 1, 1, 1)
        _Color1 ("Color B", Color) = (1, 1, 1, 1)
        _Color2 ("Color C", Color) = (1, 1, 1, 1)
        _Color3 ("Color D", Color) = (1, 1, 1, 1)
        _Color4 ("Color E", Color) = (1, 1, 1, 1)
        _Color5 ("Color F", Color) = (1, 1, 1, 1)
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

            float _Factor;
            fixed4 _Color0;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            fixed4 _Color4;
            fixed4 _Color5;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

                if(_Factor < 1){
                    col = lerp(_Color0, _Color1, _Factor);
                }
                else if(_Factor <2){
                    col = lerp(_Color1, _Color2, _Factor-1);
                }
                else if(_Factor <3){
                    col = lerp(_Color2, _Color3, _Factor-2);
                }
                else if(_Factor <4){
                    col = lerp(_Color3, _Color4, _Factor-3);
                }
                else{
                    col = lerp(_Color4, _Color5, _Factor-4);
                }
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
