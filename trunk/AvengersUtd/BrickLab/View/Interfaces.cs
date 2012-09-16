using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab.View
{
    public interface IView
    {
        ViewModelBase ViewModel { get; }
        ViewType ViewType { get; }
    }
}