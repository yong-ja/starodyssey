using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class PlanetRenderer : Renderer
    {
        CameraHostControl cameraHost;
        Hud hud;
        TestPlanet testPlanet;
        Skybox skybox;
        Billboard sun;

        public override void Init()
        {
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");

            EntityManager.LoadDescriptors("Descriptor.xml");

            Game.Input.IsInputEnabled = true;
            qCam.LookAt(new Vector3(0,0,0), new Vector3(30, 5, -30f));
            qCam.Update();
            hud = new Hud();
            OdysseyUI.CurrentHud = hud;
            cameraHost = new CameraHostControl();
            hud.KeyDown += new KeyEventHandler(hud_KeyDown);

            testPlanet = new TestPlanet(EntityManager.GetEntityDescriptor("OceanPlanet"));
            skybox = new Skybox(EntityManager.GetEntityDescriptor("Skybox"));
            sun = new Billboard(EntityManager.GetEntityDescriptor("YellowMainSequence"),8f);

            testPlanet.Init();
            skybox.Init();
            sun.Init();


            sun.PositionV3 = new Vector3(0,0,0);
            testPlanet.PositionV3 = new Vector3(30,0,0);

           
            //device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            //device.SetRenderState(RenderState.SourceBlend, Blend.One);
            //device.SetRenderState(RenderState.DestinationBlend, Blend.One);
            //device.SetRenderState(RenderState.AlphaBlendEnable, true);
            //device.SetRenderState(RenderState.AlphaTestEnable, false);

            SetupCamera();
        }

        public void SetupCamera()
        {
            cameraHost.SetBinding(new CameraBinding(Keys.A, CameraAction.StrafeLeft, qCam.SetState, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.D, CameraAction.StrafeRight, qCam.SetState, -QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.Q, CameraAction.HoverUp, qCam.SetState, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.E, CameraAction.HoverDown, qCam.SetState, -QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.Z, CameraAction.RotateLeft, qCam.SetState, QuaternionCam.DefaultRotationSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.C, CameraAction.RotateRight, qCam.SetState, -QuaternionCam.DefaultRotationSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.W, CameraAction.MoveForward, qCam.SetState, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(Keys.S, CameraAction.MoveBackward, qCam.SetState, -QuaternionCam.DefaultSpeed));
        }

        public void hud_KeyDown(object sender, KeyEventArgs e)
        {
            cameraHost.Focus();
        }

        public override void Render()
        {
            //hud.Render();
            //
            qCam.Update();



            skybox.Render();

            testPlanet.Render();
            device.SetTransform(TransformState.World, Matrix.Identity);
            sun.Render();
            device.SetRenderState(RenderState.AlphaBlendEnable, false);
            DebugManager.Instance.DisplayStats();   

                //device.SetTransform(TransformState.World, Matrix.Identity);
        }



        public override void ProcessInput()
        {
            qCam.UpdateStates();
        }

        public override void Dispose()
        {
            
        }
    }
}
