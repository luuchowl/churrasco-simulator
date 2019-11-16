Shader "Unlit/FoodShaderTexture"
{
	Properties
	{
		
		 _Factor("Factor", Range(0, 3)) = 0

        _MainTex0 ("Texture0", 2D) = "white" {}
        _Color0 ("Color 0", Color) = (1, 1, 1, 1)
        _MainTex1 ("Texture1", 2D) = "white" {}
		[HDR]_Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _MainTex2 ("Texture2", 2D) = "white" {}
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        _MainTex3 ("Texture3", 2D) = "white" {}
        _Color3 ("Color 3", Color) = (1, 1, 1, 1)

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
            sampler2D _MainTex0;
            sampler2D _MainTex1;
            sampler2D _MainTex2;
            sampler2D _MainTex3;
			
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
				fixed4 col = tex2D(_MainTex0, i.uv);

                if(_Factor < 1){
                    col = lerp(_Color0 * tex2D(_MainTex0, i.uv), _Color1 * tex2D(_MainTex1, i.uv), _Factor);
                }
                else if(_Factor < 2){
                    col = lerp(_Color1 * tex2D(_MainTex1, i.uv), _Color2 * tex2D(_MainTex2, i.uv), _Factor-1);
                }
                else{
                    col = lerp(_Color2 * tex2D(_MainTex2, i.uv), _Color3 * tex2D(_MainTex3, i.uv), saturate(_Factor-2));
                }

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
