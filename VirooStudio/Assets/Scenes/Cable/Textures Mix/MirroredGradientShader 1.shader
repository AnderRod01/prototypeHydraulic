Shader "Custom/MirroredGradientShader"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1,0,0,1) // Default to red
        _ColorB ("Color B", Color) = (0,1,0,1) // Default to green
        _X ("X", Range(0, 1)) = 0.33
        _Y ("Y", Range(0, 1)) = 0.66
        _Direction ("Direction", Vector) = (1, 0, 0, 0) // Default to horizontal gradient
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float4 _MainTex_ST;

            float4 _ColorA;
            float4 _ColorB;
            float _X;
            float _Y;
            float4 _Direction;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float gradientPosition = dot(i.uv, _Direction.xy);

                float4 color;
                if (gradientPosition < _X)
                {
                    color = _ColorA;
                }
                else if (gradientPosition < _Y)
                {
                    color = _ColorB;
                }
                else
                {
                    color = _ColorA;
                }

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}