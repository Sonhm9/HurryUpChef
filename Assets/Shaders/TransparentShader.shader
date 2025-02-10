Shader "Custom/TransparentShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)  // 기본 색상
        _MainTex("Base Map", 2D) = "white" {}            // 텍스처
        _Transparency("Transparency", Range(0, 1)) = 0.5 // 투명도 (0: 완전 불투명, 1: 완전 투명)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 사용

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

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _BaseColor;
            float _Transparency;

            // Vertex Shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader
            half4 frag(Varyings i) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                texColor *= _BaseColor; // 기본 색상 적용
                texColor.a = texColor.a * (1 - _Transparency); // 투명도 설정 (알파값 제어)
                return texColor;
            }
            ENDHLSL
        }
    }

    Fallback "Universal Forward"
}