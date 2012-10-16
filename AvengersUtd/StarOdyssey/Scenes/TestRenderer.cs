using System.Drawing;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface.Text;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.Network;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.Settings;
using System;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TestRenderer : StereoRenderer
    {
       
       
        public TestRenderer(AvengersUtd.Odyssey.IDeviceContext deviceContext) : base(deviceContext)
        {
        }

        protected override void OnInit(object sender, EventArgs e)
        {
            //AvengersUtd.Odyssey.Text.TextManager.DrawText("prova");

            Camera.LookAt(Vector3.Zero, new Vector3(0,5,-10f));

            Box box = new Box(1, 1, 1);
            Sphere sphere = new Sphere(1, 16);
            Arrow arrow = new Arrow(1, 1, 3, 0.5f) { PositionV3 = new Vector3(-2.5f, 0, 0) };
            AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox bbox = new AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox(4);
            sphere.PositionV3 = new Vector3(0f, 3f, 0);
            RenderableNode rNodeBox = new RenderableNode(box) { Label = "RBox" };
            RenderableNode rNodeSPhere = new RenderableNode(sphere);
            RenderableNode rNodeBBox = new RenderableNode(bbox);
            FixedNode fNodeGrid = new FixedNode { Label = "fGrid", Position = Vector3.Zero };

            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNodeGrid);
            Scene.Tree.RootNode.AppendChild(coNode);
            fNodeGrid.AppendChild(rNodeSPhere);
            fNodeGrid.AppendChildren(arrow.ToNodes());
            fNodeGrid.AppendChildren(bbox.ToNodes());

            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            Hud = Hud.FromDescription(Game.Context.Device,
                new HudDescription(
                    width: Game.Context.Settings.ScreenWidth,
                    height: Game.Context.Settings.ScreenHeight,
                    zNear: Game.CurrentRenderer.Camera.NearClip,
                    zFar: Game.CurrentRenderer.Camera.FarClip,
                    cameraEnabled: true,
                    multithreaded: true
                    ));
            OdysseyUI.CurrentHud = Hud;
            Hud.BeginDesign();

            Game.Logger.Log("Prova1");
            Game.Logger.Log("Prova2");
            Game.Logger.Log("Prova3");
            Game.Logger.Log("Prova4");
            Game.Logger.Log("Prova5");
            //Game.Logger.Log("Prova1");
            //Game.Logger.Log("Prova2");
            //Game.Logger.Log("Prova3");
            //Game.Logger.Log("Prova4");
            //Game.Logger.Log("Prova5");
            //Game.Logger.Log("U MAD?");
            //Game.Logger.Log("PROBLEM?");
            LogEvent.UserInterface.Write("U MAD?");
            LogEvent.Engine.Write("YO DAWG");

            TexturedIcon crosshair = new TexturedIcon
            {
                Position = new Vector2(512f, 512f),
                Size = new Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")

            };

            Hud.Add(crosshair);

            //Hud.Add(new Panel
            //{
            //    Position = new Vector2(500f, 175f),
            //    Size = new Size(200, 200)
            //});

            Hud.Add(new DecoratorButton
                        {
                            Position = new Vector2(550f, 300f),
                        });


            Hud.Add(new RayPickingPanel { Size = Hud.Size, Camera = this.Camera });
            Game.Logger.Activate();
            Hud.Init();
            Hud.EndDesign();

            //Scene.BuildRenderScene();
            //Hud.AddToScene(this, Scene);
            //
            //lightSphere.SetBehaviour(new FreeMovementGamepadBehaviour(50));
            
            IsInited = true;
        }


        public override void ProcessInput()
        {
            //Camera.UpdateStates();
            Hud.ProcessKeyEvents();
        }

       
    }
}
