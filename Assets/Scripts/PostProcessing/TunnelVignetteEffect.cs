using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace XRControls.Effects
{
    [Serializable]
    [PostProcess(typeof(TunnelVignetteEffectEffectRenderer), PostProcessEvent.AfterStack, "Hidden/Custom/TunnelVignetteEffect")]
    public sealed class TunnelVignetteEffect : PostProcessEffectSettings
    {
        public ColorParameter fadeColor = new ColorParameter { value = new Color(0f, 0f, 0f, 1f) };
        [Range(0f, 1f), Tooltip("Vignette Power.")]
        public FloatParameter fadePower = new FloatParameter { value = 0.5f };
        [Range(0f, 1f), Tooltip("Vignette Softness.")]
        public FloatParameter fadeSoftness = new FloatParameter { value = 0.5f };
        [Range(0f, 30f), Tooltip("Eye shape.")]
        public FloatParameter eyeShape = new FloatParameter { value = 5f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value
                && fadePower.value > 0f;
        }
    }

    public sealed class TunnelVignetteEffectEffectRenderer : PostProcessEffectRenderer<TunnelVignetteEffect>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/FadeEffect"));
            sheet.properties.SetFloat("_FadePower", 1 - settings.fadePower);
            sheet.properties.SetFloat("_FadeSoftness", settings.fadeSoftness);
            sheet.properties.SetFloat("_EyeShape", settings.eyeShape);
            sheet.properties.SetColor("_Color", settings.fadeColor);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}