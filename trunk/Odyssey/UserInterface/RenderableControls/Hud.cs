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

#region Using directives
using System;
using System.Collections.Generic;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
#if (!SlimDX)
using Microsoft.DirectX.Direct3D;
#else
using SlimDX;
using SlimDX.Direct3D9;
#endif
#endregion


namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{

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
    public sealed partial class Hud : ContainerControl
    {
        const string ControlTag = "Hud";
        BaseControl captureControl;
        BaseControl clickedControl;
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
        public BaseControl CaptureControl
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
            IsFocusable = true;
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
                {
                    spriteList.Add(sCtl);
                    ctl.CreateShape();
                }


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

            int vertexIndex = 0;
            int vertexCount = 0, indexCount = 0, primitiveCount = 0;
            int baseVertex = 0, baseIndex = 0;
            int baseLabelIndex = 0, labelCount = 0;

            Comparison<ISpriteControl> spriteComparison =
                delegate(ISpriteControl i1, ISpriteControl i2) { return (((BaseControl)i1).Depth.CompareTo((i2 as BaseControl).Depth)); };

            Predicate<ISpriteControl> layerCheck = delegate(ISpriteControl iSpriteControl)
                                                       {
                                                           if (((BaseControl)iSpriteControl).Depth.ComponentLayer ==
                                                               currentComponentLayer)
                                                               return true;
                                                           else
                                                               return false;
                                                       };

            Predicate<ISpriteControl> windowCheck = delegate(ISpriteControl iSpriteControl)
                                                        {
                                                            if (((BaseControl)iSpriteControl).Depth.WindowLayer ==
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
                    labelCount = spriteList.FindIndex(baseLabelIndex, windowCheck);
                    //windowLayers++;
                    if (vertexCount != 0)
                        renderInfoList.Add(new RenderInfo(baseIndex, baseVertex, vertexCount, primitiveCount,
                                                          baseLabelIndex, labelCount));

                    baseIndex += indexCount;
                    baseVertex += vertexCount;
                    vertexCount = indexCount = primitiveCount = 0;
                    baseLabelIndex = (labelCount != -1) ? labelCount : 0;
                }
                if (shapeDescriptor.Depth.ComponentLayer > currentComponentLayer)
                {
                    currentComponentLayer = shapeDescriptor.Depth.ComponentLayer;
                    labelCount = spriteList.FindIndex(baseLabelIndex, layerCheck);

                    renderInfoList.Add(new RenderInfo(baseIndex, baseVertex, vertexCount, primitiveCount,
                                                      baseLabelIndex, labelCount));
                    baseIndex += indexCount;
                    baseVertex += vertexCount;
                    vertexCount = indexCount = primitiveCount = 0;
                    baseLabelIndex = (labelCount != -1) ? labelCount : 0;
                }

                shapeDescriptor.ArrayOffset = vertexIndex;
                indexCount += shapeDescriptor.Indices.Length;
                vertexIndex += shapeDescriptor.Vertices.Length;
                vertexCount += shapeDescriptor.Vertices.Length;
                primitiveCount += shapeDescriptor.NumPrimitives;
            }

            renderInfoList.Add(new RenderInfo(baseIndex, baseVertex, vertexCount, primitiveCount, labelCount, spriteList.Count));
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
            Controls.Sort();
            BuildVertexBuffer();
            DesignMode = false;
        }

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            // dispose managed components
            sprite.Dispose();
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                indexBuffer.Dispose();
            }
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