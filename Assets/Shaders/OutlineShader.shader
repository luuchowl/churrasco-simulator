// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline and ScreenSpace texture"
{
    Properties
    {
        [Header(Outline)]
        _OutlineVal("Outline value", Range(0., 2.)) = 1.
        _OutlineCol("Outline color", color) = (1., 1., 1., 1.)
        [Header(Texture)]
        _Color ( "Color", color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _Zoom("Zoom", Range(0.5, 20)) = 1
        _SpeedX("Speed along X", Range(-1, 1)) = 0
        _SpeedY("Speed along Y", Range(-1, 1)) = 0
    }
        SubShader
        {
            Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }

            Pass
            {
                Cull Front
                ZTest Always

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct v2f {
                    float4 pos : SV_POSITION;
                };

                float _OutlineVal;

                v2f vert(appdata_base v) {
                    v2f o;

                    // Convert vertex to clip space
                    o.pos = UnityObjectToClipPos(v.vertex * _OutlineVal);

                    // Convert normal to view space (camera space)
                    //float3 normal = mul((float3x3) UNITY_MATRIX_IT_MV, v.normal);
                    float3 normal = v.normal;

                    /*
                    // Compute normal value in clip space
                    normal.x *= UNITY_MATRIX_P[0][0];
                    normal.y *= UNITY_MATRIX_P[1][1];
                    */
                    // Scale the model depending the previous computed normal and outline value
                    //o.pos.xyz += _OutlineVal * normal.xyz;
                    return o;
                }

                fixed4 _OutlineCol;

                fixed4 frag(v2f i) : SV_Target {
                    return _OutlineCol;
                }

                ENDCG
            }

            Pass
            {

                ZTest LEqual
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
                fixed4 _Color;
                
                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
                ENDCG
            }
        }
}