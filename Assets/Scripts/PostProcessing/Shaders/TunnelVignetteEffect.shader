Shader "Hidden/Custom/TunnelVignetteEffect"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float _FadePower;
		float _FadeSoftness;
		float _EyeShape;
		float3 _Color;
		
        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo);
			float2 texCoords = i.texcoord.xy;
			float2 dist2 = distance(texCoords.xy, float2(0.5, 0.5));
			float distSum = (_EyeShape * dist2.x * abs(texCoords.y - 0.5) + dist2.y) * 0.5;
			float vignette = smoothstep(_FadePower, _FadePower - _FadeSoftness, distSum);
            float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
			color.rgb = lerp(saturate(vignette * color.rgb), _Color * luminance.xxx, 1 - vignette.xxx);
            return color;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}