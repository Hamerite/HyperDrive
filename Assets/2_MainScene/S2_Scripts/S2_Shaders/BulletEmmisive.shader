Shader "Custom/BulletEmmisive" {
    Properties {
        _Color ("Base Color", Color) = (1,1,1,1)
        [HDR] _EmissionColor ("Emission Color", Color) = (0, 0, 0, 0)

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        half4 _Color;
        half4 _EmissionColor;

        half _Glossiness;
        half _Metallic;

        struct Input { float4 color : COLOR; };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            o.Albedo = _Color.rbg * IN.color.rbg;
            o.Alpha = _Color.a * IN.color.a;

            o.Emission = _EmissionColor;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Standard"
}
