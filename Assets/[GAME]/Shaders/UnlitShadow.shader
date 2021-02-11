Shader "Custom/Unlit Shadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tint ("Tint", Color) = (1, 1, 1, 1)
    }

    SubShader
    {

		Tags { 
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}
       
        LOD 200
		
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
				float2 uv: TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float2 uv: TEXCOORD0;
            };
			
			sampler2D _MainTex;
			float4 _Tint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o, o.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col); 
                return col;
            }
            ENDCG
		}
    }
    FallBack "Diffuse"
}
