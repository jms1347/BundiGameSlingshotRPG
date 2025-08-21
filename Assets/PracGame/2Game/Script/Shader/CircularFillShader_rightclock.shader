Shader "Custom/RightHalfGauge"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _FillAmount("Fill Amount", Range(0, 1)) = 0 // 0이면 12시, 1이면 6시까지
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

                    // 왼쪽 반원 픽셀은 투명하게 처리 (x축이 음수인 영역)
                    if (uv.x < 0) {
                        return fixed4(0, 0, 0, 0);
                    }

                    // 12시 방향부터 시계방향으로 각도를 계산하기 위한 보정
                    // atan2(y, x) -> atan2(x, -y)로 변경하면 12시가 0도가 되고, 시계방향으로 증가
                    float angle = atan2(uv.x, -uv.y);

                    // 각도를 0 ~ 2*PI (0~360도) 범위로 정규화
                    if (angle < 0) {
                        angle += 2 * 3.14159265359;
                    }

                    // 게이지 채우기 로직
                    float maxAngle = 3.14159265359; // PI (180도)
                    float fillAngle = _FillAmount * maxAngle;

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