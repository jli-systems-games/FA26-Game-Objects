Shader "Custom/ColorNoise"
{
    Properties
    {
        _Strength ("Noise Strength", Range(0, 0.2)) = 0.06
        _GrainSize ("Grain Size", Range(1, 4)) = 1
        _Speed ("Updates Per Second", Range(1, 60)) = 20
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Strength;
            float _GrainSize;
            float _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            float random(float2 p, float seed)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))
                    + seed) * 43758.5453);
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;
                float2 pixel = floor(
                    uv * _ScreenParams.xy / _GrainSize
                );

                float frame = floor(_Time.y * _Speed);

                float3 noise = float3(
                    random(pixel, frame + 1),
                    random(pixel, frame + 47),
                    random(pixel, frame + 113)
                );

                // 柔和的彩色颗粒
                float3 color = lerp(
                    float3(0.5, 0.5, 0.5), noise, 0.65
                );

                return float4(color, _Strength);
            }
            ENDHLSL
        }
    }
}