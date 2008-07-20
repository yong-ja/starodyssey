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
using AvengersUtd.Odyssey.UserInterface.Devices;
using SlimDX.XInput;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class GridRenderer : Renderer
    {
        Mesh sphere;
        Mesh plane;
        SimpleMesh<SpecularMaterial> simpleSphere;
        SimpleMesh<SpecularMaterial> simpleGrid;

        Airplane airplane;
        Planet1 widget;
        Grid grid;
        Office office;
        LightWidget lightWidget;
        
        Hud hud;
        CameraHostControl cameraHost;

        Vector3 vObjPos;
        Vector3 vAngles;
        Matrix mTranslation;
        Matrix mRotation;

        XBox360Controller gamepad;
        ObjectHostControl objectController;

        AvengersUtd.Odyssey.UserInterface.RenderableControls.TrackBar tb;
        public override void Init()
        {
            StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
            StyleManager.LoadTextStyles("Odyssey TextStyles.ots");

            Game.Input.IsInputEnabled = true;


            airplane = new Airplane();
            widget = new Planet1();
            grid = new Grid();
            office = new Office();
            lightWidget = new LightWidget();
            //sphere = Mesh.FromFile(Game.Device, "Meshes\\Airplane.x", MeshFlags.Managed);
            //simpleSphere = new SimpleMesh<SpecularMaterial>(sphere);
            
            qCam.LookAt(new Vector3(), new Vector3(0, 15, -30f));
            qCam.Update();
            hud = new Hud();
            OdysseyUI.CurrentHud = hud;
            cameraHost = new CameraHostControl();
            cameraHost.KeyDown += new KeyEventHandler(hud_KeyDown);

            tb = new AvengersUtd.Odyssey.UserInterface.RenderableControls.TrackBar();
            tb.Position = new Vector2(15, 15);
            tb.MinimumValue = 0;
            tb.MaximumValue = 100;
            
            tb.TickFrequency = 1;
            tb.Size = new System.Drawing.Size(500, tb.Size.Height);
            tb.ValueChanged += new EventHandler(tb_ValueChanged);
            
            gamepad = XBox360Controller.GetController(0);

            objectController = new ObjectHostControl();
            objectController.Id = "Airplane";
            objectController.entity = airplane;
          
            hud.BeginDesign();
            hud.Add(cameraHost);
            hud.Add(objectController);
            hud.Add(tb);
            cameraHost.Focus();
            SetupCamera();
            hud.EndDesign();

            Texture t = AvengersUtd.Odyssey.Objects.EntityManager.RenderToTexture(512, 512, grid,office);
            Texture.ToFile(t, "prova.jpg", ImageFileFormat.Jpg);
            office.shadowMap = t;
            grid.shadowMap = t;
            
        }

        //float fFarView = 100;
        void tb_ValueChanged(object sender, EventArgs e)
        {
            //fFarView += 10;
            float fFarClip = tb.Value;
            office.Materials[0].EffectDescriptor.Effect.SetValue(new EffectHandle("fFarClip"), fFarClip);
            grid.Materials[0].EffectDescriptor.Effect.SetValue(new EffectHandle("fFarClip"), fFarClip);
            Texture t = AvengersUtd.Odyssey.Objects.EntityManager.RenderToTexture(512, 512, grid, office);
            //Texture.ToFile(t, "prova.jpg", ImageFileFormat.Jpg);
            office.shadowMap = t;
            grid.shadowMap = t;
        }
        bool value = false;
        
        void hud_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                objectController.SwitchPlane(value);
                value = !value;
                if (value)
                {
                    grid.Rotation = -1.5f;
                    grid.Position = new Vector3(0, airplane.Position.Y, airplane.Position.Z);
                }

                else
                {
                    grid.Rotation = 0;
                    grid.Position = new Vector3(0, airplane.Position.Y, 0);
                }

            }


            cameraHost.Focus();
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

        public override void Render()
        {
            hud.Render();
            
            DebugManager.Instance.DisplayStats();
            //device.SetRenderState<FillMode>(RenderState.FillMode, FillMode.Wireframe);
            device.SetTransform(TransformState.World, Matrix.Identity);
            //device.SetTransform (TransformState.World, Matrix.Identity * Matrix.RotationAxis(new Vector3(0, 1, 0), 0));
            grid.RenderWithTechnique("ShadowMap");
            //grid.RenderWithTechnique("Scene");

            office.RenderWithTechnique("ShadowMap");
          
          //office.RenderWithTechnique("Scene");
            
            //device.SetRenderState<FillMode>(RenderState.FillMode, FillMode.Solid);
            lightWidget.Render();


            
            
            //widget.Render();
            //airplane.Render();

            //widget.Position = airplane.Position;
            qCam.Update();
            
            
            
            
        }

        const int speedFactor = 10;
        public override void ProcessInput()
        {
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

            airplane.Position = vObjPos;
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
            sphere.Dispose();
        }
    }
}
