Shader "Custom/Interactable"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
		_Tint ("Tint", Color) = (1, 1, 1, 1)

		[PerRendererData]_OutlineSize ("Outline Size", Range(0, 1)) = 1
		_OutlineWidth ("Outline Width", Float) = 0
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 0)
		_UseNormalOrPosition("Use Normal Or Position", Range(0, 1)) = 0

		[Header(Debug)][Space]
		[Toggle] _Debug ("Debug", Float) = 0
		_DebugOutlineSize ("Debug Outline Size", Range(0, 1)) = 1
    }

    SubShader
    {

		Tags { 
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}
       
        LOD 200
		

		//NORMAL RENDER
		Pass
		{
			Cull Off
			ZWrite On
			Ztest LEqual
			
			//We write "1" to the stencil buffer so the outline doesn't render above our model
			Stencil{
				Ref 1
				Comp Always
				Pass Replace
			}

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

        //OUTLINE
		Pass
        {
			Cull Off
            ZWrite On

			//We only render the outline where the model isn't being drawn
			Stencil{
				Ref 1
				Comp NotEqual
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			float _OutlineSize, _OutlineWidth, _UseNormalOrPosition;
			float4 _OutlineColor;
			float _Debug, _DebugOutlineSize;

            v2f vert (appdata v)
            {
                v2f o;
				float size = _OutlineWidth * (_Debug ? _DebugOutlineSize : _OutlineSize);
				float4 normalVert = v.vertex + (float4(v.normal, 0) * size);
                float4 posVert = v.vertex * (1 + size);
                float4 hullVertex = lerp(normalVert, posVert, _UseNormalOrPosition);
                o.vertex = UnityObjectToClipPos(hullVertex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _OutlineColor;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
