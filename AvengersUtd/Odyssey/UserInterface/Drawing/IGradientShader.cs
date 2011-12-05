using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public interface IGradientShader
    {
        string Name { get; set; }
        GradientType GradientType { get; set; }
        Shader Method { get; set; }
        GradientStop[] Gradient { get; set; }
    }

    public interface IBorderShader : IGradientShader
    {
        Borders Borders { get; set; }
    }

    public interface IRadialShader : IGradientShader
    {
        Vector2 Center { get; set; }
        float RadiusX { get; set; }
        float RadiusY { get; set; }
    }
}