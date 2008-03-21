#if (SlimDX)

#region Using directives - SlimDX version

using System;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D9;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
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
                OdysseyUI.Device,
                ShapeDescriptors[0].Vertices.Length*TransformedColored.SizeInBytes,
                Usage.Dynamic | Usage.WriteOnly,
                TransformedColored.Format,
                Pool.Default);

            IndexBuffer tempIBuffer = new IndexBuffer(
                OdysseyUI.Device,
                ShapeDescriptors[0].Indices.Length*TransformedColored.SizeInBytes,
                Usage.Dynamic | Usage.WriteOnly,
                Pool.Default,
                false);

            using (DataStream vbStream = tempVBuffer.Lock(0, 0, LockFlags.Discard))
            {
                vbStream.WriteRange<TransformedColored>(ShapeDescriptors[0].Vertices);
                tempVBuffer.Unlock();
            }

            using (DataStream ibStream = tempIBuffer.Lock(0, 0, LockFlags.Discard))
            {
                ibStream.WriteRange<int>(ShapeDescriptors[0].Indices);
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

            using (DataStream vbStream = vertexBuffer.Lock(0, 0, LockFlags.Discard | LockFlags.NoOverwrite))
            {
                vbStream.WriteRange<TransformedColored>(ShapeDescriptors[0].Vertices);
                vertexBuffer.Unlock();
            }

            using (DataStream ibStream = indexBuffer.Lock(0, 0, LockFlags.Discard | LockFlags.NoOverwrite))
            {
                ibStream.WriteRange<int>(ShapeDescriptors[0].Indices);
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
            device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            device.SetRenderState(RenderState.DestBlend, Blend.InvSourceAlpha);
            device.SetStreamSource(0, vertexBuffer, 0, 20);

            device.VertexFormat = TransformedColored.Format;
            device.SetIndices(indexBuffer);

            foreach (RenderInfo renderInfo in renderInfoList)
            {
                device.SetRenderState(RenderState.AlphaBlendEnable, true);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                                             0, renderInfo.BaseVertex, renderInfo.VertexCount, renderInfo.BaseIndex,
                                             renderInfo.PrimitiveCount);

                device.SetRenderState(RenderState.AlphaBlendEnable, false);
                device.SetTexture(0, null);

                sprite.Begin(SpriteFlags.AlphaBlend | SpriteFlags.Texture);
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