Shader "Custom/Sprite Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _TintR ("Tint Red", Range(0.0, 1.0)) = 1.0
        _TintG ("Tint Green", Range(0.0, 1.0)) = 1.0
        _TintB ("Tint Blue", Range(0.0, 1.0)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _TintR;
            float _TintG;
            float _TintB;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = col.rgb * float3(_TintR, _TintG, _TintB);
                col.rgb = col.rgb * _Color.rgb;
                col.a *= _Color.a;

                // Make black pixels transparent
                if (col.r == 0 && col.g == 0 && col.b == 0) {
                    col.a = 0;
                }

                return col;
            }
            ENDCG
        }
    }
}
