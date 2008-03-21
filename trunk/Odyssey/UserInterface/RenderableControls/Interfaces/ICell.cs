namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public interface ICell : IContainer

    {
        Border Borders { get; set; }
        BaseControl HostedControl { get; }
    }
}