namespace AvengersUtd.Odyssey.Engine.Input
{
    internal interface IActionStates
    {
        void ProcessEvent(bool[] keystate);
        void Reset();
    }
}