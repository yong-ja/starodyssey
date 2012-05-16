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
using System.Linq;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.UserInterface.Input;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D11;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
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
        private const string ControlTag = "Hud";

        private readonly SortedList<Keys, KeyBinding> keyBindings;
        private readonly List<RenderStep> renderInfoList;
        private readonly Queue<UpdateElement> updateQueue;
        private readonly List<ISpriteObject> spriteControls;
        private readonly List<ShapeDescription> hudShapes;
        private UserInterfaceRenderCommand uiRCommand;
        private UserInterfaceUpdateCommand uiUCommand;
        private BaseControl captureControl;
        private ShapeDescription hudInterface;
        private readonly UpdateElement recomputeAction;

        static Hud()
        {
            EventLoad = new object();
        }

        public static Hud FromDescription(Device device, HudDescription description)
        {
            Hud hud = new Hud
                          {
                              AbsolutePosition = Vector2.Zero,
                              Size = new Size(description.Width, description.Height),
                              HudDescription = description,
                              Depth = new Depth
                                          {
                                              WindowLayer = (int) description.ZFar,
                                              ComponentLayer = (int) description.ZFar,
                                              ZOrder = description.ZFar
                                          },
                          };
            Device = device;

            if (description.CameraEnabled)
            {
                QuaternionCam camera = Game.CurrentRenderer.Camera;

                hud.SetBinding(new KeyBinding(KeyAction.StrafeLeft, Keys.A, delegate() { camera.Strafe(-QuaternionCam.DefaultSpeed);}));
                hud.SetBinding(new KeyBinding(KeyAction.StrafeRight, Keys.D, delegate() { camera.Strafe(QuaternionCam.DefaultSpeed); }));
                hud.SetBinding(new KeyBinding(KeyAction.MoveForward, Keys.W, delegate() { camera.Move(QuaternionCam.DefaultSpeed); }));
                hud.SetBinding(new KeyBinding(KeyAction.MoveBackward, Keys.S, delegate() { camera.Move(-QuaternionCam.DefaultSpeed); }));
                hud.SetBinding(new KeyBinding(KeyAction.RotateLeft, Keys.Z, delegate() { camera.RotateY(QuaternionCam.DefaultRotationSpeed); }));
                hud.SetBinding(new KeyBinding(KeyAction.RotateRight, Keys.C, delegate() { camera.RotateY(-QuaternionCam.DefaultRotationSpeed); }));
                hud.SetBinding(new KeyBinding(KeyAction.HoverUp, Keys.Q, delegate() { camera.Hover(QuaternionCam.DefaultSpeed); }));
                hud.SetBinding(new KeyBinding(KeyAction.HoverDown, Keys.E, delegate() { camera.Hover(-QuaternionCam.DefaultSpeed); }));

                //hud.SetBinding(new KeyBinding(KeyAction.StrafeLeft, Game.CurrentRenderer.Camera.SetState, Keys.A, QuaternionCam.DefaultSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.StrafeRight, Game.CurrentRenderer.Camera.SetState, Keys.D, -QuaternionCam.DefaultSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.HoverUp, Game.CurrentRenderer.Camera.SetState, Keys.Q, QuaternionCam.DefaultSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.HoverDown, Game.CurrentRenderer.Camera.SetState, Keys.E, -QuaternionCam.DefaultSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.RotateLeft, Game.CurrentRenderer.Camera.SetState, Keys.Z, QuaternionCam.DefaultRotationSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.RotateRight, Game.CurrentRenderer.Camera.SetState, Keys.C, -QuaternionCam.DefaultRotationSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.MoveForward, Game.CurrentRenderer.Camera.SetState, Keys.W, QuaternionCam.DefaultSpeed));
                //hud.SetBinding(new KeyBinding(KeyAction.MoveBackward, Game.CurrentRenderer.Camera.SetState, Keys.S, -QuaternionCam.DefaultSpeed));
            }

            return hud;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hud"/> class.
        /// </summary>
        private Hud() : base(ControlTag, "Empty")
        {
            renderInfoList = new List<RenderStep>();
            updateQueue = new Queue<UpdateElement>();
            spriteControls = new List<ISpriteObject>();
            hudShapes = new List<ShapeDescription>();
            keyBindings = new SortedList<Keys, KeyBinding>();
            Shapes = new ShapeCollection(1);

            FocusedControl = EnteredControl = this;
            IsInside = true;
            IsFocusable = true;
            //WindowManager = new WindowManager();

            recomputeAction = new UpdateElement(this, UpdateAction.Recompute);
        }

        #region Properties

        internal BaseControl FocusedControl { get; set; }
        internal BaseControl EnteredControl { get; set; }
        internal BaseControl ClickedControl { get; set; }
        internal HudDescription HudDescription { get; set; }
        internal InterfaceMesh InterfaceMesh { get; set; }

        internal bool ShouldUpdateShapes
        {
            get { return updateQueue.Count > 0; }
        }

        internal bool ShouldUpdateSprites { get; private set; }
        internal RenderStep[] RenderSteps { get; private set; }
        internal ISpriteObject[] SpriteControls { get; private set; }

        internal BaseControl CaptureControl
        {
            get { return captureControl; }
            set
            {
                if (value != null)
                    value.HasCaptured = true;
                else if (captureControl != null)
                    captureControl.HasCaptured = false;
                captureControl = value;
            }
        }

        public IEnumerable<KeyBinding> KeyBindings
        {
            get { return keyBindings.Values; }
        }

        public static Device Device { get; set; }

        #endregion

        private void ComputeShapes(IContainer containerControl)
        {
            foreach (BaseControl ctl in TreeTraversal.PreOrderVisibleControlsVisit(containerControl))
            {
                ISpriteObject sCtl = ctl as ISpriteObject;
                if (sCtl != null && ctl.IsVisible)
                    spriteControls.Add(sCtl);

                if (ctl.IsVisible && ctl.Description.Shape != Shape.None)
                    hudShapes.AddRange(ctl.Shapes.Array);
            }
        }

        public void BuildInterfaceMesh()
        {
            int hiddenVertices = 0;
            int hiddenIndices = 0;
            foreach (ShapeDescription sDesc in
                TreeTraversal.PreOrderHiddenControlsVisit(this).SelectMany(control => control.Shapes))
            {
                hiddenVertices += sDesc.Vertices.Length;
                hiddenIndices += sDesc.Indices.Length;
            }


            if (hudInterface.Vertices.Length == 0)
                throw Error.InvalidOperation(Properties.Resources.ERR_HudNoControls);

            Shapes[0] = hudInterface;
            ColoredVertex[] vertices = new ColoredVertex[Shapes[0].Vertices.Length + hiddenVertices];
            ushort[] indices = new ushort[Shapes[0].Indices.Length + hiddenIndices];
            Array.Copy(Shapes[0].Vertices, vertices, Shapes[0].Vertices.Length);
            Array.Copy(Shapes[0].Indices, indices, Shapes[0].Indices.Length);

            if (InterfaceMesh != null)
                InterfaceMesh.Rebuild(vertices, indices);
            else
                InterfaceMesh = new InterfaceMesh(vertices, indices) {OwnerHud = this};

            OnLoad(EventArgs.Empty);
        }


        public void AddToScene(Renderer renderer, SceneManager scene)
        {
            TextMaterial textMaterial = new TextMaterial();
            UIMaterial uiMaterial = new UIMaterial();

            MaterialNode mTextNode = new MaterialNode(textMaterial);
            MaterialNode mUINode = new MaterialNode(uiMaterial);
            //UserInterfaceNode mUiNode = new UserInterfaceNode();
            RenderableNode rNode = new RenderableNode(InterfaceMesh);
            CameraOverlayNode caNode = new CameraOverlayNode(renderer.Camera);

            caNode.AppendChild(rNode);
            //caNode.AppendChild(mUiNode);
            //caNode.AppendChild(mTextNode);

            //mUiNode.AppendChild(rNode);

            uiRCommand = new UserInterfaceRenderCommand(renderer, this, rNode);
                //(renderer, mUINode, mUINode.RenderableCollection,
                // mTextNode, this);

            uiUCommand = new UserInterfaceUpdateCommand(this, uiRCommand);

            scene.Tree.RootNode.AppendChild(caNode);
            scene.CommandManager.AddUpdateCommand(uiUCommand);
            scene.CommandManager.AddBaseCommands(uiMaterial.PreRenderStates);
            scene.CommandManager.AddRenderCommand(uiRCommand);
            scene.CommandManager.AddBaseCommands(uiMaterial.PostRenderStates);
        }

        private void GenerateRenderSteps(IEnumerable<ShapeDescription> hudDescriptions)
        {
            float currentWindowLayer = 0;
            float currentComponentLayer = 0;

            int vertexCount = 0, indexCount = 0, primitiveCount = 0;
            int baseVertex = 0, baseIndex = 0;
            int baseLabelIndex = 0, labelCount = 0;

            Comparison<ISpriteObject> spriteComparison =
                (i1, i2) => (((BaseControl) i1).Depth.CompareTo(((BaseControl) i2).Depth));

            renderInfoList.Clear();
            spriteControls.Sort(spriteComparison);

            foreach (ShapeDescription shapeDescriptor in hudDescriptions)
            {
                // This code loops through the sprite List to find the
                // last label index that belongs to the same layer
                // of the current layer. When it is found (if there is)
                // it saves that index and continues with the next layer.

                if (shapeDescriptor.Depth.WindowLayer > currentWindowLayer)
                {
                    currentWindowLayer = shapeDescriptor.Depth.WindowLayer;
                    float tempLayer = currentWindowLayer;
                    Predicate<ISpriteObject> windowCheck = iSpriteControl => ((BaseControl) iSpriteControl).Depth.WindowLayer ==
                                                                             tempLayer;
                    labelCount = spriteControls.FindIndex(baseLabelIndex, windowCheck);
                    //windowLayers++;
                    if (vertexCount != 0)
                        renderInfoList.Add(new RenderStep
                                               {
                                                   BaseIndex = baseIndex,
                                                   BaseVertex = baseVertex,
                                                   VertexCount = vertexCount,
                                                   PrimitiveCount = primitiveCount,
                                                   BaseLabelIndex = baseLabelIndex,
                                                   LabelCount = labelCount
                                               });

                    baseIndex += indexCount;
                    baseVertex += vertexCount;
                    vertexCount = indexCount = primitiveCount = 0;
                    baseLabelIndex = (labelCount != -1) ? labelCount : 0;
                }
                if (shapeDescriptor.Depth.ComponentLayer > currentComponentLayer)
                {
                    currentComponentLayer = shapeDescriptor.Depth.ComponentLayer;
                    float tempLayer = currentComponentLayer;
                    Predicate<ISpriteObject> layerCheck = iSpriteControl => ((BaseControl) iSpriteControl).Depth.ComponentLayer ==
                                                                            tempLayer;


                    labelCount = spriteControls.FindIndex(baseLabelIndex, layerCheck);

                    if (vertexCount != 0)
                        renderInfoList.Add(new RenderStep
                                               {
                                                   BaseIndex = baseIndex,
                                                   BaseVertex = baseVertex,
                                                   VertexCount = vertexCount,
                                                   PrimitiveCount = primitiveCount,
                                                   BaseLabelIndex = baseLabelIndex,
                                                   LabelCount = labelCount
                                               });
                    baseIndex += indexCount;
                    baseVertex += vertexCount;
                    vertexCount = indexCount = primitiveCount = 0;
                    baseLabelIndex = (labelCount != -1) ? labelCount : 0;
                }

                indexCount += shapeDescriptor.Indices.Length;
                vertexCount += shapeDescriptor.Vertices.Length;
                primitiveCount += shapeDescriptor.Primitives;
            }

            renderInfoList.Add(new RenderStep
                                   {
                                       BaseIndex = baseIndex,
                                       BaseVertex = baseVertex,
                                       VertexCount = vertexCount,
                                       PrimitiveCount = primitiveCount,
                                       BaseLabelIndex = baseLabelIndex,
                                       LabelCount = spriteControls.Count
                                   });

            RenderSteps = renderInfoList.ToArray();
        }

        public void Update()
        {
            while (updateQueue.Count > 0)
            {
                UpdateElement element = updateQueue.Dequeue();

                switch (element.Action)
                {
                    case UpdateAction.Move:
                    case UpdateAction.UpdateShape:
                        UpdateShapes(element.Control);
                        break;
                    case UpdateAction.Add:
                        AddControl(element.Control);
                        if (!updateQueue.Contains(recomputeAction))
                            updateQueue.Enqueue(recomputeAction);
                        break;
                    case UpdateAction.Remove:
                        RemoveControl(element.Control);
                        if (!updateQueue.Contains(recomputeAction))
                            updateQueue.Enqueue(recomputeAction);
                        break;

                    case UpdateAction.Recompute:
                        AssembleInterface();
                        break;
                    default:
                        throw Error.WrongCase("element.Action", "Update", element.Action);
                }
                element.Control.IsBeingUpdated = false;
            }

            if (HudDescription.Multithreaded)
            {
                if (!uiUCommand.TaskQueue.Contains(UpdateBuffers))
                    uiUCommand.TaskQueue.Enqueue(UpdateBuffers);
            }
            else
            {
                AssembleInterface();
                UpdateBuffers();
            }
        }

        private void AddControl(BaseControl control)
        {
            hudShapes.AddRange(control.Shapes.Array);
            IContainer containerControl = control as IContainer;

            if (containerControl != null)
                ComputeShapes(containerControl);

            if (HudDescription.Multithreaded)
            {
                if (!uiUCommand.TaskQueue.Contains(UpdateSprites))
                {
                    if (control is ISpriteObject || (containerControl != null && containerControl.ContainsSprites))
                    {
                        uiUCommand.TaskQueue.Enqueue(UpdateSprites);
                        uiUCommand.TaskQueue.Enqueue(uiRCommand.UpdateItems);
                    }
                }
            }
            else
                UpdateSprites();
        }

        private void RemoveControl(BaseControl control)
        {
            foreach (ShapeDescription sDesc in control.Shapes)
                hudShapes.Remove(sDesc);

            ISpriteObject sCtl = control as ISpriteObject;
            if (sCtl != null) spriteControls.Remove(sCtl);

            IContainer containerControl = control as IContainer;

            if (containerControl != null)
                foreach (BaseControl childControl in TreeTraversal.PreOrderControlVisit(containerControl))
                    RemoveControl(childControl);
        }


        public void Init()
        {
            // If there were still some controls to update, their update request will
            // be canceled and the queue will be emptied. In this way controls that
            // requested an update before the UI was recomputed, will still be updated.
            // Otherwise they'll be stuck in the 'IsBeingUpdated' state.
            //if (updateQueue.Count > 0)
            //{
            //    foreach (UpdateElement element in updateQueue)
            //        element.Control.IsBeingUpdated = false;
            //}

            updateQueue.Clear();

            CreateShapes();
            AssembleInterface();
            BuildInterfaceMesh();
            UpdateSprites();

            DesignMode = false;
        }

        internal void UpdateBuffers()
        {
            Shapes[0] = hudInterface;
            InterfaceMesh.UpdateBuffers(Shapes[0].Vertices, Shapes[0].Indices);
            //Console.WriteLine(string.Format("Buffer Update"));
        }

        internal void AssembleInterface()
        {
            hudShapes.Sort();
            hudInterface = ShapeDescription.Join(hudShapes.ToArray());
            GenerateRenderSteps(hudShapes);
        }

        internal void CreateShapes()
        {
            Controls.Sort();
            hudShapes.Clear();
            spriteControls.Clear();
            ComputeAbsolutePosition();

            foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(this).Where(ctl => ctl.Description.Shape != Shape.None))
                ctl.CreateShape();

            ComputeShapes(this);
        }

        private void UpdateShapes(BaseControl control)
        {
            control.UpdateShape();

            foreach (ShapeDescription sDesc in control.Shapes)
            {
                if (!sDesc.IsDirty) continue;

                Array.Copy(sDesc.Vertices, 0, hudInterface.Vertices, sDesc.ArrayOffset,
                           sDesc.Vertices.Length);
                sDesc.IsDirty = false;
            }
        }

        internal void UpdateSprites()
        {
            foreach (ISpriteObject spriteControl in spriteControls.Where(sCtl => !sCtl.Inited))
            {
                spriteControl.CreateResource();
                spriteControl.ComputeAbsolutePosition();
                spriteControl.CreateShape();
            }

            SpriteControls = spriteControls.ToArray();
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
        public void EnqueueForUpdate(BaseControl control, UpdateAction updateAction)
        {
            if (control.IsBeingUpdated)
                return;

            //Console.WriteLine(string.Format("Enqueing {0} for {1}",control.Id, updateAction));
            UpdateElement updateElement = new UpdateElement(control, updateAction);
            updateQueue.Enqueue(updateElement);
            control.IsBeingUpdated = true;
        }

        //void DebugQueue(List<ISpriteControl> spriteList)
        //{
        //    foreach (BaseControl ctl in spriteList)
        //        Console.WriteLine(ctl.Id);
        //}


        public override bool IntersectTest(Point cursorLocation)
        {
            return Geometry.Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        public void SetBinding(KeyBinding binding)
        {
            if (!keyBindings.ContainsKey(binding.Key))
                keyBindings.Add(binding.Key, binding);
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
            OdysseyUI.CurrentHud = this;
        }

        /// <summary>
        /// Signals the UI to end the design process.
        /// </summary>
        /// <para>Adding a new control at runtime requires you to enclose the call
        /// between <c>OdysseyUI.CurrentHud.BeginDesign()</c> and 
        /// <c>OdysseyUI.CurrentHud.EndDesign()</c> calls.</para></remarks>
        public void EndDesign()
        {
            if (!HudDescription.Multithreaded)
                Init();
            else if (uiUCommand != null)
            {
                uiUCommand.TaskQueue.Enqueue(Init);
                uiUCommand.TaskQueue.Enqueue(uiRCommand.UpdateItems);
            }
        }

        #region Overriden events

        protected override void OnDisposing(EventArgs e)
        {
            base.OnDisposing(e);
            // dispose managed components

            if (!InterfaceMesh.Disposed)
            {
                InterfaceMesh.Dispose();
            }

            foreach (BaseControl control in TreeTraversal.PreOrderControlVisit(this))
            {
                control.Dispose();
            }

            foreach (ISpriteObject spriteControl in spriteControls.Where(sCtl => !sCtl.Disposed))
                spriteControl.Dispose();

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(true);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(false);
        }

        public void ProcessKeyEvents()
        {
            foreach (KeyValuePair<Keys, KeyBinding> kvp in keyBindings)
            {
                KeyBinding kb = kvp.Value;

                if (kb.State) kb.Operation();
            }
        }

        #endregion

        #region Exposed Events

        private static readonly object EventLoad;


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
        private void OnLoad(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventLoad];
            if (handler != null)
                handler(this, e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            HudDescription = new HudDescription(
                cameraEnabled: HudDescription.CameraEnabled,
                width: Size.Width,
                height: Size.Height,
                zFar: HudDescription.ZFar,
                zNear: HudDescription.ZNear,
                multithreaded: true
                );
        }

        #endregion
    }
}