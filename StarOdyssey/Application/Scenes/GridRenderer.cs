#region Using directives
using System;
using System.Collections.Generic;
using System.Text;

using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.StarOdyssey.Objects.Entities;
using SlimDX.Direct3D9;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Devices;
using SlimDX.XInput;
using AvengersUtd.Odyssey.Objects;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics; 
#endregion

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class GridRenderer : Renderer
    {

        Airplane airplane;
        Platform platform;
        Grid grid;
        Office office;
        
        Hud hud;
        CameraHostControl cameraHost;

        Vector3 vObjPos;
        Vector3 vAngles;
        Matrix mTranslation;
        Matrix mRotation;
        SceneGraph sceneGraph;

        XBox360Controller gamepad;
        ObjectHostControl objectController;

        private DepthMaterial depthMaterial ;
        PhongMaterial phongMaterial;

        Spotlight spotlight;


        // UI data

        TimeSpan time;
        Label lTime;
        Button bStartCmd;
      
        public override void Init()
        {
            spotlight = new Spotlight(new Vector3(0, 15, -0f), new Vector3());
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");
            StyleManager.LoadTextStyles("Other.ots");
            LightManager.SetLight(0, spotlight);

            Game.Input.IsInputEnabled = true;

            platform = new Platform();
            office = new Office();
            grid = new Grid();
            
            qCam.LookAt(new Vector3(), new Vector3(0,20, -40f));
            qCam.Update();

            hud = new Hud();
            OdysseyUI.CurrentHud = hud;
            cameraHost = new CameraHostControl();
            hud.KeyDown += new System.Windows.Forms.KeyEventHandler(hud_KeyDown);
          
            hud.Add(cameraHost);
            
            SetupCamera();
            //hud.EndDesign();

            FreeTransformNode ftn1 = new FreeTransformNode("Platform");
            FreeTransformNode ftn2 = new FreeTransformNode("teapot");
            FreeTransformNode ftn3 = new FreeTransformNode("grid");
            sceneGraph = new SceneGraph(new FixedNode());
            RenderableNode rPlatformNode = new RenderableNode(platform);
            RenderableNode rOfficeNode = new RenderableNode(office);
            RenderableNode rGridNode = new RenderableNode(grid);
            ftn1.AppendChild(rPlatformNode);
            ftn3.AppendChild(rGridNode);

            ftn2.AppendChild(rOfficeNode);
            sceneGraph.RootNode.AppendChild(ftn2);
            sceneGraph.RootNode.AppendChild(ftn1);
            sceneGraph.RootNode.AppendChild(ftn3);

            Game.Scene.BuildRenderScene(sceneGraph);
            Game.Scene.AddPreprocessEffect(CommandType.ComputeShadows);

            SetupUI();

            

        }

        void SetupUI()
        {
            hud.Size = new System.Drawing.Size(1024, 768);
            hud.BeginDesign();
            lTime = new Label {Id = "TimeLabel",
                TextStyleClass = "Title",
                Position = new Vector2(0, 100)};
            bStartCmd = new Button
                            {
                                Id = "StartCommand",
                                Position = new Vector2(824, 718),
                                Text = "Start Test"
                            };
            
            
            time = new TimeSpan(0, 0, 0);
            lTime.Text = time.ToString();

            objectController = new ObjectHostControl {Id = "Airplane", Entity = office};

            hud.Add(lTime);
            hud.Add(bStartCmd);
            hud.Add(objectController);

            hud.EndDesign();

            sceneGraph.UpdateAllNodes();
        }

       
        bool value = false;
        void hud_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Space)
            {
                objectController.SwitchPlane(value);
                value = !value;
                if (value)
                {
                    grid.Rotation = -1.5f;
                    grid.PositionV3 = new Vector3(0, office.PositionV3.Y, office.PositionV3.Z);
                }

                else
                {
                    grid.Rotation = 0;
                    grid.PositionV3 = office.PositionV3;
                }

            }


            cameraHost.Focus();
        }

        public void SetupCamera()
        {
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.A, CameraAction.StrafeLeft, qCam.SetState, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.D, CameraAction.StrafeRight, qCam.SetState, -QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.Q, CameraAction.HoverUp, qCam.SetState, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.E, CameraAction.HoverDown, qCam.SetState, -QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.Z, CameraAction.RotateLeft, qCam.SetState, QuaternionCam.DefaultRotationSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.C, CameraAction.RotateRight, qCam.SetState, -QuaternionCam.DefaultRotationSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.W, CameraAction.MoveForward, qCam.SetState, QuaternionCam.DefaultSpeed));
            cameraHost.SetBinding(new CameraBinding(System.Windows.Forms.Keys.S, CameraAction.MoveBackward, qCam.SetState, -QuaternionCam.DefaultSpeed));
        }

        float t;
        public override void Render()
        {
            
            
            DebugManager.Instance.DisplayStats();
            qCam.Update();

            
            //device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            //device.SetRenderState(RenderState.SourceBlend, Blend.One);
            //device.SetRenderState(RenderState.DestinationBlend, Blend.One);
            ////device.SetRenderState(RenderState.AlphaBlendEnable, true);


            float x = (float) (5*Math.Cos(t));
            float y = (float) (5*Math.Sin(t));
            t += 0.5f*(float)Game.FrameTime;
            spotlight.Position = new Vector3(x, 15, y);
            Game.Scene.Display();
            hud.Render();
        }

        const int speedFactor = 10;
        public override void ProcessInput()
        {
            sceneGraph.UpdateAllNodes();
            //qCam.FirstPersonCameraWithGamepad(XBox360Controller.GetController(0));
            //return;

            qCam.UpdateStates();
            return;

            vObjPos.X += gamepad.LeftThumbstick.X / speedFactor;
            vObjPos.Z += gamepad.LeftThumbstick.Y / speedFactor;
            if ((gamepad.ActiveButtons & GamepadButtonFlags.LeftThumb) == 0)
            {
                vObjPos.X += gamepad.LeftThumbstick.X / speedFactor;
                vObjPos.Z += gamepad.LeftThumbstick.Y / speedFactor;
                
            }
            else
            {
                vObjPos.X += gamepad.LeftThumbstick.X / speedFactor;
                vObjPos.Y += gamepad.LeftThumbstick.Y / speedFactor;
                vAngles.X += gamepad.RightThumbstick.X / 20;
                vAngles.Z += gamepad.RightThumbstick.Y / 20;
            }

            if ((gamepad.ActiveButtons & GamepadButtonFlags.RightThumb) == 0)
            {
                vAngles.X += gamepad.RightThumbstick.X / 20;
                vAngles.Y += gamepad.RightThumbstick.Y / 20;

            }
            else
            {
                
                vAngles.Z += gamepad.RightThumbstick.X / 20;
                vAngles.Y += gamepad.RightThumbstick.Y / 20;
            }

            airplane.PositionV3 = vObjPos;
            //mTranslation = Matrix.Translation(vObjPos);

            //Quaternion qRotationX = new Quaternion(new Vector3(0, 1, 0), -vAngles.X % 360f);
            //Quaternion qRotationY = new Quaternion(new Vector3(1, 0, 0), -vAngles.Y % 360f);
            //Quaternion qRotationZ = new Quaternion(new Vector3(0, 0, 1), -vAngles.Z % 360f);
            ////qRotationX.Normalize(); qRotationY.Normalize(); qRotationZ.Normalize();
            //Quaternion qRotation = qRotationX * qRotationY;// * qRotationZ;
            ////qRotation.Normalize();

            //Matrix mRotationX = Matrix.RotationQuaternion(qRotationX);
            //Matrix mRotationY = Matrix.RotationQuaternion(qRotationY);
            //mRotation = mRotationX * mRotationY;

            Matrix mRotationX = Matrix.RotationX(-vAngles.Y % 360f);
            Matrix mRotationY = Matrix.RotationY(-vAngles.X % 360f);
            Matrix mRotationZ = Matrix.RotationZ(-vAngles.Z % 360f);

            mRotation = mRotationX * mRotationY * mRotationZ;

            mRotation = Matrix.RotationYawPitchRoll(-vAngles.X % 360f, -vAngles.Y % 360f, -vAngles.Z % 360f);

            airplane.rotation = mRotation;
        }

        public override void Dispose()
        {

        }
    }
}
