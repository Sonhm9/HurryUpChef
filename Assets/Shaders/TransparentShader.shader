Shader "Custom/TransparentShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)  // �⺻ ����
        _MainTex("Base Map", 2D) = "white" {}            // �ؽ�ó
        _Transparency("Transparency", Range(0, 1)) = 0.5 // ���� (0: ���� ������, 1: ���� ����)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha // ���� ���� ���

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
                texColor *= _BaseColor; // �⺻ ���� ����
                texColor.a = texColor.a * (1 - _Transparency); // ���� ���� (���İ� ����)
                return texColor;
            }
            ENDHLSL
        }
    }

    Fallback "Universal Forward"
}