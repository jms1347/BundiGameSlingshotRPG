Shader "Custom/LeftHalfGauge"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _FillAmount("Fill Amount", Range(0, 1)) = 1
        _FillColor("Fill Color", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            Pass
            {
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
                float _FillAmount;
                float4 _FillColor;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 원의 중심을 (0.5, 0.5)로 맞춰서 UV를 변환
                    float2 uv = i.uv - 0.5;

                    // 픽셀이 원의 바깥쪽에 있다면 투명하게 처리
                    if (dot(uv, uv) > 0.2501) {
                        return fixed4(0, 0, 0, 0);
                    }

                    // 오른쪽 반원 픽셀은 투명하게 처리
                    if (uv.x > 0) {
                        return fixed4(0, 0, 0, 0);
                    }

                    // 9시 방향을 0도로, 12시를 90도로 만드는 각도 계산
                    // atan2(y, x) -> atan2(-uv.y, -uv.x)로 변경
                    float angle = atan2(-uv.x, -uv.y);

                    // 각도를 0 ~ 2*PI (0~360도) 범위로 정규화
                    if (angle < 0) {
                        angle += 2 * 3.14159265359;
                    }

                    // 게이지 채우기 로직
                    // 9시 ~ 12시 ~ 3시 (왼쪽 반원) 각도: 0 ~ 180도 (PI)
                    float maxAngle = 3.14159265359;
                    float fillAngle = _FillAmount * maxAngle;

                    // 채워지는 각도 범위: 0 ~ 180도
                    if (angle <= fillAngle) {
                        fixed4 col = tex2D(_MainTex, i.uv) * _FillColor;
                        return col;
                    }

                    return fixed4(0, 0, 0, 0);
                }
                ENDCG
            }
        }
}