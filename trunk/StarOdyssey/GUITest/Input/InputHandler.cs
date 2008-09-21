using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Devices;

namespace AvengersUtd.Odyssey.Input
{
    public class InputHandler
    {
        Form owner;
        bool isCameraEnabled;
        bool isInputEnabled;

        static Mouse mouse;
        static Keyboard keyboard;

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

            OdysseyUI.SetupHooks(owner);


        }
    }
}