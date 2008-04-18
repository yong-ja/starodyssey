namespace AvengersUtd.Odyssey.Input
{
    internal interface IActionStates
    {
        void ProcessEvent(bool[] keystate);
        void Reset();
    }
}