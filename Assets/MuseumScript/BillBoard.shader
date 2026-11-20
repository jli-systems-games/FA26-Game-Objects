Shader "Custom/BillBoard"
{
    Properties
    {
        [MainTexture] _BaseMap("NPC Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        [Toggle] _VerticalBillboard("Full Billboard", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                float _VerticalBillboard;
            CBUFFER_END

            float3 RecalculateAsBillboard(float3 originalLocalPos)
            {
                float3 center = float3(0.0, 0.0, 0.0);
                float3 viewer = TransformWorldToObject(GetCameraPositionWS());

                float3 normalDir = viewer - center;
                normalDir.y = normalDir.y * _VerticalBillboard;

                if (dot(normalDir, normalDir) < 0.000001)
                {
                    normalDir = float3(0.0, 0.0, 1.0);
                }

                normalDir = normalize(normalDir);

                float3 upDir = abs(normalDir.y) > 0.999
                    ? float3(0, 0, 1)
                    : float3(0, 1, 0);

                float3 rightDir = normalize(cross(upDir, normalDir));
                upDir = normalize(cross(normalDir, rightDir));

                float3 centerOffset = originalLocalPos - center;

                float3 localPos =
                    center +
                    rightDir * centerOffset.x +
                    upDir * centerOffset.y +
                    normalDir * centerOffset.z;

                return localPos;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 localPos =
                    RecalculateAsBillboard(IN.positionOS.xyz);

                OUT.positionHCS =
                    TransformObjectToHClip(localPos);

                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(
                    _BaseMap,
                    sampler_BaseMap,
                    IN.uv
                );

                return color * _BaseColor;
            }

            ENDHLSL
        }
    }
}