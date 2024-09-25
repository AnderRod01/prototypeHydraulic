Shader "Custom/MirroredGradientPBRShaderWithNormals"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1,0,0,1) // Default to red
        _ColorB ("Color B", Color) = (0,1,0,1) // Default to green
        _MetallicA ("Metallic A", Range(0,1)) = 0.0
        _MetallicB ("Metallic B", Range(0,1)) = 1.0
        _RoughnessA ("Roughness A", Range(0,1)) = 1.0
        _RoughnessB ("Roughness B", Range(0,1)) = 0.0
        _NormalMapA ("Normal Map A", 2D) = "bump" {}
        _NormalMapB ("Normal Map B", 2D) = "bump" {}
        _X ("X", Range(0, 1)) = 0.33
        _Y ("Y", Range(0, 1)) = 0.66
        _Direction ("Direction", Vector) = (1, 0, 0, 0) // Default to horizontal gradient
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMapA;
            float2 uv_NormalMapB;
        };

        sampler2D _MainTex;
        float4 _ColorA;
        float4 _ColorB;
        float _MetallicA;
        float _MetallicB;
        float _RoughnessA;
        float _RoughnessB;
        sampler2D _NormalMapA;
        sampler2D _NormalMapB;
        float _X;
        float _Y;
        float4 _Direction;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float gradientPosition = dot(IN.uv_MainTex, _Direction.xy);

            if (gradientPosition < _X)
            {
                o.Albedo = _ColorA.rgb;
                o.Metallic = _MetallicA;
                o.Smoothness = 1.0 - _RoughnessA;
                o.Normal = UnpackNormal(tex2D(_NormalMapA, IN.uv_NormalMapA));
            }
            else if (gradientPosition < _Y)
            {
                o.Albedo = _ColorB.rgb;
                o.Metallic = _MetallicB;
                o.Smoothness = 1.0 - _RoughnessB;
                o.Normal = UnpackNormal(tex2D(_NormalMapB, IN.uv_NormalMapB));
            }
            else
            {
                o.Albedo = _ColorA.rgb;
                o.Metallic = _MetallicA;
                o.Smoothness = 1.0 - _RoughnessA;
                o.Normal = UnpackNormal(tex2D(_NormalMapA, IN.uv_NormalMapA));
            }

            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
