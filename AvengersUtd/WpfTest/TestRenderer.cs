﻿using AvengersUtd.Odyssey;
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
using System.Windows;
using System.Drawing;
using SlimDX.DXGI;
using AvengersUtd.Odyssey.UserInterface;
using WpfTest;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TestRenderer : Renderer
    {

        public Texture2D SharedTexture { get; set; }
       
        public TestRenderer(AvengersUtd.Odyssey.IDeviceContext deviceContext) : base(deviceContext)
        {}

        public override void Init()
        {

            Camera.LookAt(Vector3.Zero, new Vector3(0, 5, -10f));

            //triangle = PolyMesh.CreateTexturedQuad(new Vector4(0f, 0.5f, 0.5f, 1f), 0.5f, 0.5f);
            //triangle = PolyMesh.CreateTexturedQuad(new Vector4(0, 50, 0f, 1f), 105f, 105f);



            //PhongMaterial phong = new PhongMaterial();
            //WireframeMaterial wireframe = new WireframeMaterial();

            //MaterialNode mNodePhong = new MaterialNode(phong);
            //MaterialNode mNodeWire = new MaterialNode(wireframe);
            //MaterialNode mNodeSkybox = new MaterialNode(new SkyBoxMaterial());
            //SkyBox skybox = new SkyBox();
            Sphere lightSphere = Primitive.CreateSphere(1f, 8);
            lightSphere.Name = "LightSphere";
            lightSphere.SetBehaviour(new MouseDraggingBehaviour(Camera));
            //lightSphere.SetBehaviour(new KeyboardBehaviour());

            //lightSphere.PositionV3 = new Vector3(5, 0, 0);
            AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox box = AvengersUtd.Odyssey.Graphics.Meshes.BoundingBox.FromSphere(lightSphere);

            AvengersUtd.Odyssey.Graphics.Meshes.Grid grid = new AvengersUtd.Odyssey.Graphics.Meshes.Grid(50, 50, 8, 8);
            //Box box = new Box(grid.PositionV3, 5f, 20f, 5f);

            //lightSphere.DiffuseColor = new Color4(Color.Yellow);

            Sphere sphere = Primitive.CreateSphere(6f, 10);
            sphere.Name = "BigSphere";
            //sphere.DiffuseColor = new Color4(1.0f, 0.867f,0.737f,1.0f);
            sphere.PositionV3 = new Vector3(0f, 0, 20f);
            RenderableNode rNodeSphere = new RenderableNode(sphere);
            RenderableNode rNodeLightSphere = new RenderableNode(lightSphere);
            RenderableNode rNodeBox = new RenderableNode(box);
            //RenderableNode rNodeSky = new RenderableNode(skybox);
            RenderableNode rNodeGrid = new RenderableNode(grid);
            FixedNode fNodeSphere = new FixedNode { Position = new Vector3(5, 0, 0) };
            FixedNode fNodeGrid = new FixedNode { Position = grid.PositionV3 };
            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNodeSphere);

            Scene.Tree.RootNode.AppendChild(fNodeGrid);
            Scene.Tree.RootNode.AppendChild(coNode);


            fNodeSphere.AppendChild(rNodeSphere);
            fNodeSphere.AppendChild(rNodeLightSphere);
            fNodeSphere.AppendChild(rNodeBox);
            fNodeGrid.AppendChild(rNodeGrid);


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
            Game.Logger.Init();
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

            


            //Hud.Add(new Panel
            //{
            //    Position = new Vector2(500f, 175f),
            //    Size = new Size(200, 200)
            //});

            Hud.Add(new DecoratorButton
            {
                Position = new Vector2(550f, 300f),
            });

            //Hud.Add(new Button
            //{
            //    Position = new Vector2(300f, 650f),
            //    Size = new Size(400, 100)
            //});
            //DropDownList d = new DropDownList
            //                     {
            //                         Position = new Vector2(500f, 100f),
            //                         Items = new[]{"Prova1", "Prova2", "Prova3"}
            //                     };
            //hud.Add(d);
            TouchRayPanel rp = new TouchRayPanel { Size = Hud.Size, Camera = this.Camera };
            Hud.Add(rp);

            

            Game.Logger.Activate();
            Hud.Init();
            Hud.EndDesign();

            
            //ghtSphere.SetBehaviour(new FreeMovementGamepadBehaviour(50));
            
            Scene.BuildRenderScene();
            Hud.AddToScene(this, Scene);
            IsInited = true;
            //EyeTrackerServer server = new Odyssey.Network.EyeTrackerServer();
            //server.Start();
        }

        public override void Render()
        {
            Game.Logger.Update();
            Scene.Display();
        }

        public override void ProcessInput()
        {
            //Camera.UpdateStates();
            Hud.ProcessKeyEvents();
        }

       
    }
}