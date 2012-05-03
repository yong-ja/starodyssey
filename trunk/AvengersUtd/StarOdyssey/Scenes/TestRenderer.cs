﻿using System.Drawing;
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

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TestRenderer : Renderer
    {
       
        RenderableNode rNode;
        private RenderableNode rNode1;


        public TestRenderer(DeviceContext11 deviceContext11) : base(deviceContext11)
        {}

        public override void Init()
        {
            //AvengersUtd.Odyssey.Text.TextManager.DrawText("prova");

            Camera.LookAt(Vector3.Zero, new Vector3(0,0,-10f));
            
            //triangle = PolyMesh.CreateTexturedQuad(new Vector4(0f, 0.5f, 0.5f, 1f), 0.5f, 0.5f);
            //triangle = PolyMesh.CreateTexturedQuad(new Vector4(0, 50, 0f, 1f), 105f, 105f);

            StyleManager.LoadControlDescription(Global.XmlPath + "ControlDescriptions.ocd");
            StyleManager.LoadTextDescription(Global.XmlPath + "TextDescriptions.otd");

            PhongMaterial phong = new PhongMaterial();
            IMaterial wireframe = new WireframeMaterial();

            MaterialNode mNodePhong = new MaterialNode(phong);
            MaterialNode mNodeWire = new MaterialNode(wireframe);
            MaterialNode mNodeSkybox = new MaterialNode(new SkyBoxMaterial());
            SkyBox skybox = new SkyBox();
            Sphere lightSphere = Primitive.CreateSphere(1f, 8);
            AABB3D box = AABB3D.FromSphere(lightSphere);
            lightSphere.PositionV3 = new Vector3(2,0,0);

            AvengersUtd.Odyssey.Graphics.Meshes.Grid grid = new AvengersUtd.Odyssey.Graphics.Meshes.Grid(50, 50, 8, 8);
            //Box box = new Box(grid.PositionV3, 5f, 20f, 5f);
            
            //lightSphere.DiffuseColor = new Color4(Color.Yellow);
            
            Sphere sphere = Primitive.CreateSphere(6f, 10);
            //sphere.DiffuseColor = new Color4(1.0f, 0.867f,0.737f,1.0f);
            sphere.PositionV3 = new Vector3(0f,0, 20f);
            rNode = new RenderableNode(sphere);
            rNode1 = new RenderableNode(lightSphere);
            RenderableNode rNodeBox = new RenderableNode(box);
            RenderableNode rNodeSky = new RenderableNode(skybox);
            RenderableNode rNodeGrid = new RenderableNode(grid);
            FixedNode fNodeSphere = new FixedNode {Position = box.PositionV3};
            FixedNode fNodeGrid = new FixedNode {Position = grid.PositionV3};
            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNodeSphere);

            Scene.Tree.RootNode.AppendChild(fNodeGrid);
            Scene.Tree.RootNode.AppendChild(coNode);
            fNodeSphere.AppendChild(mNodePhong);
            fNodeGrid.AppendChild(mNodeWire);
            coNode.AppendChild(mNodeSkybox);
    
            mNodePhong.AppendChild(rNode);
            mNodePhong.AppendChild(rNode1);
            mNodePhong.AppendChild(rNodeBox);
            mNodeWire.AppendChild(rNodeGrid);
            //mNode3.AppendChild(rNode3);

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
            Hud.BeginDesign();

            //hud.Add(new Button
            //{
            //    Position = new Vector2(200f, 50f),
            //    Size = new Size(200, 200),
            //    Content = "Adal & Nadia :)",
            //});


            Hud.Add(new Panel
            {
                Position = new Vector2(300f, 175f),
                Size = new Size(200, 200)
            });

            Hud.Add(new DecoratorButton
                        {
                            Position = new Vector2(550f, 300f),
                        });

            Hud.Add(new Button
            {
                Position = new Vector2(300f, 650f),
                Size = new Size(400, 100)
            });
            //DropDownList d = new DropDownList
            //                     {
            //                         Position = new Vector2(500f, 100f),
            //                         Items = new[]{"Prova1", "Prova2", "Prova3"}
            //                     };
            //hud.Add(d);
            Game.Logger.Activate();
            Hud.Init();
            Hud.EndDesign();

            Scene.BuildRenderScene();
            //hud.AddToScene(this, Scene);
        }
        
        public override void Render()
        {
            Game.Logger.Update();
            Scene.Display();
        }

        public override void ProcessInput()
        {
            Camera.UpdateStates();
        }

       
    }
}
