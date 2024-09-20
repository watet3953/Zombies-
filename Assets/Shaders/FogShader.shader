Shader "Custom/FogShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.7)
        _MainTex ("Canvas", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "LightMode"="ForwardBase" }
        Blend SrcAlpha OneMinusSrcAlpha
        lighting off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard noambient alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //o.Albedo = _Color.rgb * tex2D (_MainTex, IN.uv_MainTex).r;
            // Metallic and smoothness come from slider variables
            o.Alpha = _Color.a - tex2D (_MainTex, IN.uv_MainTex).r;
        }
        ENDCG
    }
    FallBack "Diffuse"
}