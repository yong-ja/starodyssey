using System;

namespace AvengersUtd.Odyssey.UserInterface.Devices
{
    interface IXBox360Controller
    {
        SlimDX.XInput.GamepadButtonFlags ActiveButtons { get; }
        SlimDX.XInput.Gamepad GamepadState { get; }
        SlimDX.Vector2 LeftThumbstick { get; }
        SlimDX.Vector2 RightThumbstick { get; }
    }
}
