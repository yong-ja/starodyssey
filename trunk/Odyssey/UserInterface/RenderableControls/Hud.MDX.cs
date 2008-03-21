#if (!SlimDX)

#region Using directives - MDX version
using System;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TransformedColored = Microsoft.DirectX.Direct3D.CustomVertex.TransformedColored;
#endregion

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    // This source code is only compiled if "SlimDX" is not defined.

    public sealed partial class Hud
    {
        void BuildVertexBuffer()
        {
            t++;
            DebugManager.LogToScreen(string.Format("Times here: {0}", t));

            spriteList.Clear();
            PreCompute();


            if (ShapeDescriptors[0].Vertices.Length == 0)
                throw new InvalidOperationException("There are no vertices in the chosen HUD!");

            VertexBuffer tempVBuffer = new VertexBuffer(
                typeof(TransformedColored),
                ShapeDescriptors[0].Vertices.Length,
                OdysseyUI.Device,
                Usage.Dynamic | Usage.WriteOnly,
                TransformedColored.Format,
                Pool.Default);

            IndexBuffer tempIBuffer = new IndexBuffer(
                typeof(int),
                ShapeDescriptors[0].Indices.Length,
                OdysseyUI.Device,
                Usage.Dynamic | Usage.WriteOnly,
                Pool.Default);

            using (GraphicsStream vbStream = tempVBuffer.Lock(0, 0, LockFlags.Discard))
            {
                vbStream.Write(ShapeDescriptors[0].Vertices);
                tempVBuffer.Unlock();
            }

            using (GraphicsStream ibStream = tempIBuffer.Lock(0, 0, LockFlags.Discard))
            {
                ibStream.Write(ShapeDescriptors[0].Indices);
                tempIBuffer.Unlock();
            }

            if (vertexBuffer != null)
                vertexBuffer.Dispose();
            if (indexBuffer != null)
                indexBuffer.Dispose();

            vertexBuffer = tempVBuffer;
            indexBuffer = tempIBuffer;

            OnLoad(EventArgs.Empty);
        }

        void Update()
        {
            for (int i = 0; i < updateQueue.Count; i++)
            {
                BaseControl ctl = updateQueue[i];


                for (int j = 0; j < ctl.ShapeDescriptors.Length; j++)
                {
                    ShapeDescriptor sDesc = ctl.ShapeDescriptors[j];
                    Array.Copy(sDesc.Vertices, 0, ShapeDescriptors[0].Vertices, sDesc.ArrayOffset,
                               sDesc.Vertices.Length);
                    sDesc.IsDirty = false;
                }
                ctl.IsBeingUpdated = false;
            }
            DebugManager.LogToScreen(string.Format("Updated {0} shapeDescriptors ",
                                                   updateQueue.Count));

            updateQueue.Clear();

            using (GraphicsStream vbStream = vertexBuffer.Lock(0, 0, LockFlags.Discard | LockFlags.NoOverwrite))
            {
                vbStream.Write(ShapeDescriptors[0].Vertices);
                vertexBuffer.Unlock();
            }

            using (GraphicsStream ibStream = indexBuffer.Lock(0, 0, LockFlags.Discard | LockFlags.NoOverwrite))
            {
                ibStream.Write(ShapeDescriptors[0].Indices);
                indexBuffer.Unlock();
            }
        }

        /// <summary>
        /// Renders the user interface on screen.
        /// </summary>
        /// <remarks>These are the renderstate settings used:
        /// <code>
        /// device.RenderState.AlphaBlendOperation = BlendOperation.Add;
        /// device.RenderState.SourceBlend = Blend.SourceAlpha;
        /// device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
        /// </code></remarks>
        public void Render()
        {
            Device device = OdysseyUI.Device;

            device.RenderState.AlphaBlendOperation = BlendOperation.Add;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            device.SetStreamSource(0, vertexBuffer, 0);

            device.VertexFormat = TransformedColored.Format;
            device.Indices = indexBuffer;

            foreach (RenderInfo renderInfo in renderInfoList)
            {
                device.RenderState.AlphaBlendEnable = true;
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                             0, renderInfo.BaseVertex, renderInfo.VertexCount, renderInfo.BaseIndex,
                                             renderInfo.PrimitiveCount);
                device.RenderState.AlphaBlendEnable = false;
                device.SetTexture(0, null);

                sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.SortTexture);

                for (int i = renderInfo.BaseLabelIndex; i < renderInfo.LabelCount; i++)
                    spriteList[i].Render();
                sprite.End();
            }

            if (updateQueue.Count > 0)
                Update();
        }
    }
}
#endif
