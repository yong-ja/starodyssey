using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AvengersUTD.Odyssey.Engine
{
    public static class SafeNativeMethods
    {
        /// <summary>Window messages</summary>
        public enum WindowMessage : uint
        {
            // Misc messages
            Destroy = 0x0002,
            Close = 0x0010,
            Quit = 0x0012,
            Paint = 0x000F,
            SetCursor = 0x0020,
            ActivateApplication = 0x001C,
            EnterMenuLoop = 0x0211,
            ExitMenuLoop = 0x0212,
            NonClientHitTest = 0x0084,
            PowerBroadcast = 0x0218,
            SystemCommand = 0x0112,
            GetMinMax = 0x0024,

            // Keyboard messages
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            Character = 0x0102,
            SystemKeyDown = 0x0104,
            SystemKeyUp = 0x0105,
            SystemCharacter = 0x0106,

            // Mouse messages
            MouseMove = 0x0200,
            LeftButtonDown = 0x0201,
            LeftButtonUp = 0x0202,
            LeftButtonDoubleClick = 0x0203,
            RightButtonDown = 0x0204,
            RightButtonUp = 0x0205,
            RightButtonDoubleClick = 0x0206,
            MiddleButtonDown = 0x0207,
            MiddleButtonUp = 0x0208,
            MiddleButtonDoubleClick = 0x0209,
            MouseWheel = 0x020a,
            XButtonDown = 0x020B,
            XButtonUp = 0x020c,
            XButtonDoubleClick = 0x020d,
            MouseFirst = LeftButtonDown, // Skip mouse move, it happens a lot and there is another message for that
            MouseLast = XButtonDoubleClick,

            // Sizing
            EnterSizeMove = 0x0231,
            ExitSizeMove = 0x0232,
            Size = 0x0005,

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public WindowMessage msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
    }
}
