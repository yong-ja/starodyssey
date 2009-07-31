namespace AvengersUtd.Odyssey.UserInterface
{
    public interface ICell : IContainer

    {
        Border Borders { get; set; }
        BaseControl HostedControl { get; }
    }
}