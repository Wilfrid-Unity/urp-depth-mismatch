using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOverwriterFeature : ScriptableRendererFeature
{
    DepthOverwriterPass depthOverwriterPass;
    public override void Create()
    {
        depthOverwriterPass = new DepthOverwriterPass{ renderPassEvent = RenderPassEvent.AfterRenderingSkybox };
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(depthOverwriterPass);
    }

    public class DepthOverwriterPass : ScriptableRenderPass
    {
        RenderTexture myDepthTexture;

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            if (myDepthTexture == null)
            {
                var myDepthDescriptor = cameraTextureDescriptor;
                myDepthDescriptor.colorFormat = RenderTextureFormat.Depth;
                myDepthTexture = new RenderTexture(myDepthDescriptor);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("DepthOverwriterPass");

            // WriteSomethingTo(myDepthTexture);

            cmd.CopyTexture(myDepthTexture, renderingData.cameraData.renderer.cameraDepthTargetHandle);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}