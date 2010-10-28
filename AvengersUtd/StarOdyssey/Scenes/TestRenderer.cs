using System.Drawing;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Materials;
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
            
            //triangle = ColoredShape.CreateTexturedQuad(new Vector4(0f, 0.5f, 0.5f, 1f), 0.5f, 0.5f);
            //triangle = ColoredShape.CreateTexturedQuad(new Vector4(0, 50, 0f, 1f), 105f, 105f);

            StyleManager.LoadControlDescription("Odyssey ControlDescriptions.ocd");
            StyleManager.LoadTextDescription("Odyssey TextDescriptions.otd");

            PhongMaterial phong = new PhongMaterial();

            MaterialNode mNode1 = new MaterialNode(phong);
            MaterialNode mNode2 = new MaterialNode(phong);
            MaterialNode mNode3 = new MaterialNode(new SkyBoxMaterial());
            SkyBox skybox = new SkyBox();
            Sphere lightSphere = Primitive.CreateSphere(0.5f,4);
            lightSphere.DiffuseColor = new Color4(Color.Yellow);
            lightSphere.PositionV3 = new Vector3(0,0,0);
            Sphere sphere = Primitive.CreateSphere(6f, 10);
            sphere.DiffuseColor = new Color4(1.0f, 0.867f,0.737f,1.0f);
            sphere.PositionV3 = new Vector3(0f,0, 10f);
            rNode = new RenderableNode(sphere);
            rNode1 = new RenderableNode(lightSphere);
            RenderableNode rNode3 = new RenderableNode(skybox);
            FixedNode fNode = new FixedNode {Position = sphere.PositionV3};
            FixedNode fNode2 = new FixedNode {Position = lightSphere.PositionV3};
            CameraAnchorNode coNode = new CameraAnchorNode();
            Scene.Tree.RootNode.AppendChild(fNode);

            Scene.Tree.RootNode.AppendChild(fNode2);
            Scene.Tree.RootNode.AppendChild(coNode);
            fNode.AppendChild(mNode1);
            fNode2.AppendChild(mNode2);
            coNode.AppendChild(mNode3);
    
            mNode1.AppendChild(rNode);
            mNode2.AppendChild(rNode1);
            mNode3.AppendChild(rNode3);

            DeviceContext.Immediate.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            Hud hud = Hud.FromDescription(Game.Context.Device,
                new HudDescription(
                    width: Game.Context.Settings.ScreenWidth,
                    height: Game.Context.Settings.ScreenHeight,
                    zNear: Game.CurrentRenderer.Camera.NearClip,
                    zFar: Game.CurrentRenderer.Camera.FarClip,
                    cameraEnabled: true,
                    multithreaded: true
                    ));
            hud.BeginDesign();
 
            hud.Add(new Button
            {
                Position = new Vector2(200f, 50f),
                Size = new Size(200, 200),
                Content ="Adal & Nadia :)",
            });


            hud.Add(new Panel
            {
                Position = new Vector2(300f, 175f),
                Size = new Size(200, 200)
            });

            hud.Add(new DecoratorButton
                        {
                            Position = new Vector2(550f, 300f),
                        });

            hud.Add(new Button
            {
                Position = new Vector2(300f, 650f),
                Size = new Size(200, 200)
            });
            DropDownList d = new DropDownList
                                 {
                                     Position = new Vector2(500f, 100f),
                                     Items = new[]{"Prova1", "Prova2", "Prova3"}
                                 };
            hud.Add(d);
            Game.Logger.Activate();
            hud.Init();
            hud.EndDesign();

            Scene.BuildRenderScene();
            hud.AddToScene(this,Scene);
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
