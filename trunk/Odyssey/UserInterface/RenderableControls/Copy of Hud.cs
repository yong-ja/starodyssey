#region Disclaimer

/* 
 * Hud
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    #region Using directives

    

    #endregion

    #region RenderInfo struct

    /// <summary>
    /// To render the UI in the right way, it has to be rendered from back to front.
    /// This struct helps the Hud to know how many vertices and sprites to render
    /// before stopping. This enables the UI to correctly handle circumstances in
    /// which certain elements should occlude others. 
    /// </summary>
    internal struct RenderInfo
    {
        int baseIndex;
        int baseLabelIndex;
        int baseVertex;
        int labelCount;
        int primitiveCount;
        int vertexCount;

        #region Properties

        public int VertexCount
        {
            get { return vertexCount; }
        }

        public int BaseIndex
        {
            get { return baseIndex; }
        }

        public int BaseVertex
        {
            get { return baseVertex; }
        }

        public int PrimitiveCount
        {
            get { return primitiveCount; }
        }

        public int BaseLabelIndex
        {
            get { return baseLabelIndex; }
        }

        public int LabelCount
        {
            get { return labelCount; }
        }

        #endregion

        /// <summary>
        /// Creates a RenderInfo struct.
        /// </summary>
        /// <param name="baseIndex">The array index for the first index to use when rendering.</param>
        /// <param name="baseVertex">The array index for the first vertex to use when rendering.</param>
        /// <param name="vertexCount">How many vertices to render.</param>
        /// <param name="primitiveCount">How many primitives these vertices form.</param>
        /// <param name="baseLabelIndex">The array index for the first label to render.</param>
        /// <param name="labelCount">How many labels to render.</param>
        public RenderInfo(int baseIndex, int baseVertex, int vertexCount, int primitiveCount, int baseLabelIndex,
                          int labelCount)
        {
            this.baseIndex = baseIndex;
            this.baseVertex = baseVertex;
            this.vertexCount = vertexCount;
            this.primitiveCount = primitiveCount;
            this.baseLabelIndex = baseLabelIndex;
            this.labelCount = labelCount;
        }
    }

    #endregion

    /// <summary>
    /// The Hud (short for Head-Up Display) is the control that contains the whole user
    /// interface. If rendered as the last element, it will provide your game or application
    /// with a user interface. Be sure to call the <see cref="Render"/> method in your render
    /// loop to visualize it.
    /// </summary>
    /// <remarks>The Hud, in Windows terminology, is the equivalent of your "desktop". That means that you
    /// should avoid having overlapping <b>controls</b> added to the Hud. If you want depth sorting
    /// of the controls you should add <see cref="Window"/> controls. Those controls are brought to the
    /// foreground when activated, while non-Window controls are not. In fact, they are not supposed to. As
    /// you can't have overlapping icons on your (Window OS) desktop, you should avoid placing non-Window 
    /// controls so that they overlap.</remarks>
    public sealed class Hud : ContainerControl
    {
        const string ControlTag = "Hud";
        BaseControl captureControl;
        BaseControl clickedControl;
        bool disposed;
        BaseControl enteredControl;
        BaseControl focusedControl;
        IndexBuffer indexBuffer;

        List<RenderInfo> renderInfoList;
        Sprite sprite;
        List<ISpriteControl> spriteList;
        int t;
        List<BaseControl> updateQueue;

        VertexBuffer vertexBuffer;

        WindowManager windowManager;

        #region Properties

        /// <summary>
        /// Gets a reference to the DirectX Sprite interface used to render sprites.
        /// </summary>
        /// <value>A DirectX sprite object.</value>
        public Sprite SpriteManager
        {
            get { return sprite; }
        }

        /// <summary>
        /// Gets the currently focused Control.
        /// </summary>
        /// <value>The currently focused control.</value>
        public BaseControl FocusedControl
        {
            get { return focusedControl; }
            internal set { focusedControl = value; }
        }

        /// <summary>
        /// Gets a reference to the last control that the mouse pointer entered.
        /// </summary>
        /// <value>The currently entered control.</value>
        internal BaseControl EnteredControl
        {
            get { return enteredControl; }
            set { enteredControl = value; }
        }

        /// <summary>
        /// Gets a reference to the control that has captured the mouse pointer.
        /// This control receives all mouse events whether or not the mouse pointer
        /// is inside its bounds. Useful for drag purposes.
        /// </summary>
        /// <value>The control that has currently captured the mouse pointer.</value>
        internal BaseControl CaptureControl
        {
            get { return captureControl; }
            set
            {
                if (value != null)
                    value.HasCaptured = true;
                else
                    captureControl.HasCaptured = false;
                captureControl = value;
            }
        }

        /// <summary>
        /// Gets a reference or sets the clicked control.
        /// </summary>
        /// <value>The currently clicked control.</value>
        internal BaseControl ClickedControl
        {
            get { return clickedControl; }
            set { clickedControl = value; }
        }

        /// <summary>
        /// Gets a reference to the <see cref="WindowManager"/>.
        /// </summary>
        /// <value>A <see cref="WindowManager"/> object..</value>
        public WindowManager WindowManager
        {
            get { return windowManager; }
        }

        #endregion

        static Hud()
        {
            EventLoad = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hud"/> class.
        /// </summary>
        public Hud()
        {
            ApplyControlStyle(ControlStyle.EmptyStyle);
            spriteList = new List<ISpriteControl>();
            renderInfoList = new List<RenderInfo>();
            updateQueue = new List<BaseControl>();
            sprite = new Sprite(OdysseyUI.Device);

            ShapeDescriptors = new ShapeDescriptorCollection(1);

            focusedControl = enteredControl = this;
            IsInside = true;
            windowManager = new WindowManager();
        }

        void PreCompute()
        {
            List<ShapeDescriptor>
                hudDescriptors = new List<ShapeDescriptor>();

            foreach (BaseControl ctl in TreeTraversal.PreOrderControlRenderingVisit(this))
            {
                ISpriteControl sCtl = ctl as ISpriteControl;
                if (sCtl != null)
                    spriteList.Add(sCtl);


                if (ctl.ControlStyle.Shape == Shape.None)
                    continue;
                else
                {
                    ctl.CreateShape();
                    hudDescriptors.AddRange(ctl.ShapeDescriptors.GetArray());
                }
            }

            hudDescriptors.Sort();

            GenerateRenderSteps(hudDescriptors);
            ShapeDescriptors[0] = ShapeDescriptor.Join(hudDescriptors.ToArray());
        }

        void GenerateRenderSteps(List<ShapeDescriptor> hudDescriptors)
        {
            int currentWindowLayer = 0, currentComponentLayer = 0;

            int vIndex = 0;
            int vCount = 0, iCount = 0, primitiveCount = 0;
            int vBase = 0, iBase = 0;
            int lIndex = 0, lBase = 0;

            Comparison<ISpriteControl> spriteComparison =
                delegate(ISpriteControl i1, ISpriteControl i2) { return ((i1 as BaseControl).Depth.CompareTo((i2 as BaseControl).Depth)); };

            Predicate<ISpriteControl> layerCheck = delegate(ISpriteControl iSpriteControl)
                                                       {
                                                           if ((iSpriteControl as BaseControl).Depth.ComponentLayer ==
                                                               currentComponentLayer)
                                                               return true;
                                                           else
                                                               return false;
                                                       };

            Predicate<ISpriteControl> windowCheck = delegate(ISpriteControl iSpriteControl)
                                                        {
                                                            if ((iSpriteControl as BaseControl).Depth.WindowLayer ==
                                                                currentWindowLayer)
                                                                return true;
                                                            else
                                                                return false;
                                                        };


            renderInfoList.Clear();

            spriteList.Sort(spriteComparison);


            foreach (ShapeDescriptor shapeDescriptor in hudDescriptors)
            {
                // This code loops through the sprite List to find the
                // last label index that belongs to the same layer
                // of the current layer. When it is found (if there is)
                // it saves that index and continues with the next layer.

                if (shapeDescriptor.Depth.WindowLayer > currentWindowLayer)
                {
                    currentWindowLayer = shapeDescriptor.Depth.WindowLayer;
                    lBase = spriteList.FindIndex(lIndex, windowCheck);
                    //windowLayers++;
                    if (vCount != 0)
                        renderInfoList.Add(new RenderInfo(iBase, vBase, vCount, primitiveCount,
                                                          lIndex, lBase));

                    iBase += iCount;
                    vBase += vCount;
                    vCount = iCount = primitiveCount = 0;
                    lIndex = (lBase != -1) ? lBase : 0;
                }
                if (shapeDescriptor.Depth.ComponentLayer > currentComponentLayer)
                {
                    currentComponentLayer = shapeDescriptor.Depth.ComponentLayer;
                    lBase = spriteList.FindIndex(lIndex, layerCheck);
                    //componentLayers++;

                    renderInfoList.Add(new RenderInfo(iBase, vBase, vCount, primitiveCount,
                                                      lIndex, lBase));
                    iBase += iCount;
                    vBase += vCount;
                    vCount = iCount = primitiveCount = 0;
                    lIndex = (lBase != -1) ? lBase : 0;
                }

                shapeDescriptor.ArrayOffset = vIndex;
                iCount += shapeDescriptor.Indices.Length;
                vIndex += shapeDescriptor.Vertices.Length;
                vCount += shapeDescriptor.Vertices.Length;
                primitiveCount += shapeDescriptor.NumPrimitives;
            }

            renderInfoList.Add(new RenderInfo(iBase, vBase, vCount, primitiveCount, lBase, spriteList.Count));
        }

        void BuildVertexBuffer()
        {
            t++;
            DebugManager.LogToScreen(string.Format("Times here: {0}", t));

            spriteList.Clear();
            PreCompute();


            if (ShapeDescriptors[0].Vertices.Length == 0)
                throw new InvalidDataException("There are no vertices in the chosen HUD!");

            VertexBuffer tempVBuffer = new VertexBuffer(
                typeof (CustomVertex.TransformedColored),
                ShapeDescriptors[0].Vertices.Length,
                OdysseyUI.Device,
                Usage.Dynamic | Usage.WriteOnly,
                CustomVertex.TransformedColored.Format,
                Pool.Default);

            IndexBuffer tempIBuffer = new IndexBuffer(
                typeof (int),
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

        /// <summary>
        /// Enqueues a control for update.
        /// </summary>
        /// <remarks>Multiple events in rapid succession may cause a control to update its appearance.
        /// This makes sure that a control is not added multiple times to the queue if it has not yet
        /// been updated.
        /// <para>When a control is updated, the entire vertex buffer is discared and rewritten, for
        /// performance purposes.</para></remarks>
        /// <param name="control">The control to update.</param>
        public void EnqueueForUpdate(BaseControl control)
        {
            if (!control.IsBeingUpdated)
            {
                updateQueue.Add(control);
                control.IsBeingUpdated = true;
            }
            else
                return;
        }

        void DebugQueue(List<ISpriteControl> spriteList)
        {
            foreach (BaseControl ctl in spriteList)
                Console.WriteLine(ctl.Id);
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
            device.VertexFormat = CustomVertex.TransformedColored.Format;
            device.SetStreamSource(0, vertexBuffer, 0);
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

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(Position, Size, cursorLocation);
        }


        /// <summary>
        /// Sets the <b>whole</b> interface in design mode. 
        /// </summary>
        /// <remarks>Always call this method before creating new controls.</remarks>
        /// <seealso cref="Hud.EndDesign"/>
        /// <seealso cref="BaseControl.DesignMode"/>
        public void BeginDesign()
        {
            DesignMode = true;
        }

        /// <summary>
        /// Signals the UI to end the design process.
        /// </summary>
        /// <remarks>When this method is called, the vertexbuffer is built depending
        /// on the controls you added to the <see cref="Hud"/>. Therefore, always
        /// call this method when you are finished with adding a new control.
        /// <para>Adding a new control at runtime requires you to enclose the call
        /// between <c>OdysseyUI.CurrentHud.BeginDesign()</c> and 
        /// <c>OdysseyUI.CurrentHud.EndDesign()</c> calls.</para></remarks>
        public void EndDesign()
        {
            // If there were still some controls to update, their update request will
            // be canceled and the queue will be emptied. In this way controls that
            // requested an update before the UI was recomputed, will still be updated.
            // Otherwise they'll be stuck in the 'IsBeingUpdated' state.

            if (updateQueue.Count > 0)
            {
                foreach (BaseControl control in updateQueue)
                    control.IsBeingUpdated = false;
            }

            updateQueue.Clear();
            BuildVertexBuffer();
            DesignMode = false;
            Controls.Sort();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    // dispose managed components
                    sprite.Dispose();
                    vertexBuffer.Dispose();
                    indexBuffer.Dispose();
                }

                // dispose unmanaged components
            }
            disposed = true;
        }

        #region Exposed Events

        static readonly object EventLoad;

        /// <summary>
        /// Occurs when the interface is built.
        /// </summary>
        public event EventHandler Load
        {
            add { Events.AddHandler(EventLoad, value); }
            remove { Events.RemoveHandler(EventLoad, value); }
        }

        /// <summary>
        /// Raises the <see cref="Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void OnLoad(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventLoad];
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}