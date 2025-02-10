Shader "Custom/CompletelyTransparent"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,0) // 알파값 0 (완전 투명)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        
        ZWrite Off  // 깊이 버퍼에 기록하지 않음
        Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 활성화

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(_Color.rgb, 0); // 완전 투명 (알파값 0)
            }
            ENDCG
        }
    }
}