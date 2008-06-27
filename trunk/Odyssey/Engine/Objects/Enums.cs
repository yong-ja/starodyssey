namespace AvengersUtd.Odyssey.Objects
{
    public enum RenderOp
    {
        /// <summary>
        /// Renders the object without applying effects
        /// </summary>
        Default,
        UseShader
    }

    public enum FXType
    {
        None,
        AtmosphericScattering,
        Diffuse,
        Specular,
        SelfAlign,
        SpecularBump
    }

    public enum ScaleOp
    {
        Downsample,
        Upsample
    }

    public enum BlurOp
    {
        GaussianH,
        GaussianV,
        BloomH,
        BloomV
    }

    public enum ColorOp
    {
        Brightness,
        Contrast,
        Colorize,
        Invert
    }

    public enum FilterOp
    {
        SobelEdge
    }

    public enum BlendOp
    {
        Replace,
        Screen,
        Mask,
        Multiply,
        ColorDodge,
        ColorBurn,
    }

    public enum MatrixType
    {
        Brightness,
        Contrast,
        GrayScale,
        Colorize
    } ;


    public enum MeshOp
    {
        None,
        GenerateTangents
    }

    public enum NoiseType
    {
        Simple,
        Gaussian,
        Perlin
    }

    public enum InterpolationFunction
    {
        Linear,
        Cosine
    }
}