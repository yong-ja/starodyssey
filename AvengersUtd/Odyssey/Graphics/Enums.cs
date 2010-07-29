using System;
namespace AvengersUtd.Odyssey.Graphics
{
    public enum RenderOp
    {
        /// <summary>
        /// Renders the object without applying effects
        /// </summary>
        Default,
        UseShader
    }

    public enum SceneNodeType
    {
        Transform,
        Renderable,
        Material,
        Dummy
    }

    public enum CommandType
    {
        RenderScene,
        ComputeShadows
    }

    public enum FXVectorOperationType
    {
        PointLightDirection,
        EntityPosition
    }

    public enum LightParameter
    {
        Position,
        SpotlightTarget,
        SpotlightDirection,
        SpotlightInnerConeCosine,
        SpotlightOuterConeCosine,
        SpotlightFalloff,
        Radius,

        LightWorldViewProjection
    }


    public enum LightingAlgorithm
    {
        Depth,
        Phong,
        Wireframe,
        Uniform,
        None
    }

    [Flags]
    public enum LightingTechnique
    {
        None = 0,
        /// <summary>
        /// Uniform color
        /// </summary>
        Diffuse = 1,
        /// <summary>
        /// Texture map
        /// </summary>
        DiffuseMap = 2,
        Specular = 4,
        Normal = 8,
        Shadows = 16,
    }

    public enum FXType
    {
        None,
        AtmosphericScattering,
        SurfaceFromSpaceWithAtmosphere,
        AtmosphereFromSpace,
        DepthMap,
        Diffuse,
        Specular,
        SelfAlign,
        SpecularBump,
        Textured,
        SurfaceFromSpace,
        ShadowMapping
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