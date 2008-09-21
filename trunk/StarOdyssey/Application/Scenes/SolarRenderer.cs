using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey;
using AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.StarOdyssey.Objects.Entities;
using AvengersUtd.StarOdyssey.Objects.UI;
using SlimDX;
using Button=AvengersUtd.Odyssey.UserInterface.RenderableControls.Button;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using SlimDX.Direct3D9;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class SolarRenderer : Renderer
    {
        OSolarSystem systemEntity;
        Skybox skybox;
        Hud hud;

        CameraHostControl cameraHost;

        public override void Init()
        {
            EntityManager.LoadDescriptors("Descriptor.xml");
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");

            StyleManager.LoadControlStyles("StarOdyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("StarOdyssey TextStyles.ots");

            GalaxyGenerator galGen = new GalaxyGenerator();
            SolarSystem system = galGen.CreateSingleStarSystem("Alpha Centauri", 1.0);
            while (system.Primary.CelestialBodiesCount == 0)
                galGen.CreateSingleStarSystem("Alpha Centauri", 1.0); ;
            systemEntity = OSolarSystem.ConvertFromSolarSystem(system);

            skybox = new Skybox(EntityManager.GetEntityDescriptor("Skybox"));
            skybox.Init();

            qCam.LookAt(new Vector3(), new Vector3(0, 30, -100) );
            qCam.Update();

            Game.Input.IsInputEnabled = true;
            hud = GUIDesigner.SolarMenuScreen;
            OdysseyUI.CurrentHud = hud;
            DataViewer dataViewer = (DataViewer)hud.Find("Temp");

            List<PlanetaryFeatures> tempList = new List<PlanetaryFeatures>();
            foreach (Planet planet in system.Primary)
                tempList.Add(planet.PlanetaryFeatures);

            dataViewer.RowCount = tempList.Count;
            dataViewer.DataSource = tempList;
            dataViewer.RefreshValues();



            //PlanetInfoPanel pInfo = new PlanetInfoPanel() {Id = "PlanetInfoPanel"};
            //pInfo.Position = new Vector2(25, 25);
            //pInfo.DataSource = (Planet)system.Primary[0];

            //hud.Add(pInfo);

            cameraHost = new CameraHostControl();
            hud.KeyDown += new KeyEventHandler(hud_KeyDown);

            hud.EndDesign();


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
            
            qCam.Update();
            
            skybox.Render();
            systemEntity.Render();
            DebugManager.Instance.DisplayStats();
            hud.Render();
            device.SetTransform(TransformState.World, Matrix.Identity);
        }

        public override void ProcessInput()
        {
            qCam.UpdateStates();
        }

        public override void Dispose()
        {
            return;
        }
    }
}
