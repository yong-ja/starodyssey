using System.Windows.Forms;
using AvengersUtd.Odyssey.Engine.Input.Devices;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.Engine.Input
{
    public class InputHandler
    {
        Form owner;
        bool isCameraEnabled;
        bool isInputEnabled;

        static Mouse mouse;
        static Keyboard keyboard;

        public bool IsCameraEnabled
        {
            get { return isCameraEnabled; }
            set { isCameraEnabled = true; }
        }

        public bool IsInputEnabled
        {
            get { return isInputEnabled; }
            set { isInputEnabled = value; }
        }

        public InputHandler(Form ownerForm)
        {
            owner = ownerForm;
            mouse = Mouse.Instance;
            keyboard = Keyboard.Instance;

            OdysseyUI.SetupHooks(ownerForm);

        }
    }
}