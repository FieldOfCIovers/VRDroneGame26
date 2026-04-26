Shader "Custom/FakeGodray"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Noise Texture", 2D) = "white" {}
        _Intensity ("Intensity", Float) = 1.5
        _Softness ("Edge Softness", Float) = 2.0
        _ScrollSpeed ("Scroll Speed", Float) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _Intensity;
            float _Softness;
            float _ScrollSpeed;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                uv.y += _Time.y * _ScrollSpeed;
                float noise = tex2D(_MainTex, uv).r;

                float edge = pow(1.0 - abs(i.uv.x - 0.5) * 2.0, _Softness);

                float alpha = noise * edge * _Intensity;

                return _Color * alpha;
            }
            ENDCG
        }
    }
}
