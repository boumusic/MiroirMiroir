using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(RecursiveRenderer), PostProcessEvent.AfterStack, "Custom/Recursive")]
public sealed class RecursivePP : PostProcessEffectSettings
{
    public TextureParameter texture = new TextureParameter { value = null };
}

public sealed class RecursiveRenderer : PostProcessEffectRenderer<RecursivePP>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Recursive"));
        if (settings.texture != null) sheet.properties.SetTexture("_Texture", settings.texture);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}