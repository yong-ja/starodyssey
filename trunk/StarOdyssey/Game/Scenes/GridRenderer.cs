using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;
using AvengersUtd.Odyssey.Engine.Meshes;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
using System.Windows.Forms;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class GridRenderer : Renderer
    {
        Mesh sphere;
        Mesh plane;
        Mesh grid;
        SimpleMesh<SpecularMaterial> simpleSphere;
        SimpleMesh<SpecularMaterial> simpleGrid;
        Hud hud;
        CameraHostControl cameraHost;
        float zDepth = -30f;

        public override void Init()
        {
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");

            Game.Input.IsInputEnabled = true;

            sphere = Mesh.FromFile(Game.Device, "Meshes\\Planets\\Large.x", MeshFlags.Managed);
            plane = Mesh.FromFile(Game.Device, "Meshes\\plane.x", MeshFlags.Managed);
            grid = Mesh.CreateBox(Game.Device, 10f, 10f, 10f);
            simpleSphere = new SimpleMesh<SpecularMaterial>(sphere);
            simpleGrid = new SimpleMesh<SpecularMaterial>(plane);
            qCam.LookAt(new Vector3(), new Vector3(0, -5, zDepth));
            hud = new Hud();
            OdysseyUI.CurrentHud = hud;

            //cameraHost = new CameraHostControl();
            
            //hud.BeginDesign();
            //hud.Add(cameraHost);
            //cameraHost.Focus();
            //hud.EndDesign();
            
        }

        public void SetupCamera()
        {
            cameraHost.SetBinding(new CameraBinding(Keys.A, CameraAction.StrafeLeft, qCam.Strafe, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.D, CameraAction.StrafeLeft, qCam.Strafe, QuaternionCam.DefaultSpeed);
        }

        public override void Render()
        {
            //hud.Render();
            DebugManager.Instance.DisplayStats();

            device.SetTransform(TransformState.World, Matrix.Translation(0,0,0)*Matrix.RotationZ(0));
                        device.SetRenderState<FillMode>(RenderState.FillMode, FillMode.Wireframe);
            simpleGrid.DrawWithEffect();
            //grid.DrawSubset(0);
            device.SetRenderState<FillMode>(RenderState.FillMode, FillMode.Solid);
            device.SetTransform(TransformState.World, Matrix.Translation(0, 0, 50));
            simpleSphere.DrawWithEffect();
            qCam.Update();
            
            
        }

        public override void ProcessInput()
        {

        }

        public override void Dispose()
        {
            sphere.Dispose();
        }
    }
}
