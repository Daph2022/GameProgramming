Shader "UI/RoundedButton"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Radius ("Corner Radius", Range(0, 100)) = 20
        _Size ("Size", Vector) = (100,100,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float _Radius;
            float4 _Size;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pixelPos = i.uv * _Size.xy;
                float2 center = _Size.xy / 2.0;
                float2 diff = abs(pixelPos - center);
                float2 cornerSize = (_Size.xy / 2.0) - _Radius;

                float mask = smoothstep(0, 1, max(diff.x - cornerSize.x, diff.y - cornerSize.y));
                return fixed4(_Color.rgb, _Color.a * (1.0 - mask));
            }
            ENDCG
        }
    }
}
