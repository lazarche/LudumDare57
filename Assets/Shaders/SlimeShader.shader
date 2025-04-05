Shader "Custom/SlimeShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _BlendRadius ("Blend Radius", Range(0, 1)) = 0.2
        _PixelSize ("Pixel Size", Range(0.01, 1)) = 0.1
        _SlimeColor ("Slime Color", Color) = (0.0, 1.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float _BlendRadius;
            float _PixelSize;
            float4 _SlimeColor;

            // Avoid conflict with Unity's _Time variable
            // Remove this if you don't need it
            float4 _CustomTime : TIME;

            // Vertex shader
            struct Attributes
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = UnityObjectToClipPos(v.position);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader
            half4 frag(Varyings i) : SV_Target
            {
                // Get the main texture color
                half4 baseColor = tex2D(_MainTex, i.uv);

                // Calculate the distance from the center of the "pixel cube" to the neighboring pixels
                float2 center = float2(0.5, 0.5); // Center of the texture space
                float2 dist = abs(i.uv - center);
                float distance = length(dist);

                // Smooth step for the blending effect based on distance
                float blend = smoothstep(_BlendRadius, _BlendRadius * 0.5, distance);

                // Blend the slime color with the base color
                half4 blendedColor = lerp(baseColor, _SlimeColor, blend);

                // Apply pixelation effect based on _PixelSize
                float2 pixelatedUV = floor(i.uv / _PixelSize) * _PixelSize;
                half4 pixelatedColor = tex2D(_MainTex, pixelatedUV);

                // Final color output with pixelation and blending
                return lerp(pixelatedColor, blendedColor, blend);
            }
            ENDHLSL
        }
    }
    Fallback "Unlit/Color"
}
