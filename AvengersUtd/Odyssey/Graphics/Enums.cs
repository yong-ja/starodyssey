using System;
namespace AvengersUtd.Odyssey.Graphics
{
    public enum SceneNodeType
    {
        Transform,
        Renderable,
        Material,
        Dummy
    }

    public enum CommandType
    {
        Render,
        Update,
        ComputeShadows,
        RasterizerStateChange,
        BlendStateChange,
        DepthStencilStateChange,
        UserInterfaceRenderCommand
    }

    public enum RenderingOrderType
    {
        OpaqueGeometry,
        MixedGeometry,
        AdditiveBlendingGeometry,
        SubtractiveBlendingGeometry,
        First,
        Last
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
        /// Uniform Color
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

    public enum CameraAction
    {
        MoveForward,
        MoveBackward,
        RotateLeft,
        RotateRight,
        StrafeLeft,
        StrafeRight,
        HoverUp,
        HoverDown,
        None
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

    public enum Color4Op
    {
        Brightness,
        Contrast,
        Color4ize,
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
        Color4Dodge,
        Color4Burn,
    }

    public enum MatrixType
    {
        Brightness,
        Contrast,
        GrayScale,
        Color4ize
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