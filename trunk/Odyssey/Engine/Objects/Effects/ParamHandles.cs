namespace AvengersUtd.Odyssey.Objects.Effects
{
    public struct ParamHandles
    {
        public struct Colors
        {
            public const string Ambient = "cAmbient";
        }

        public struct Floats
        {
            public const string Scale = "fScale";
        }

        public struct Matrices
        {
            public const string Projection = "mProj";
            public const string View = "mView";
            public const string WorldView = "mWorldView";
            public const string World = "mWorld";
            public const string WorldViewProjection = "mWorldViewProj";
        }

        public struct Vectors
        {
            public const string EyePosition = "vEye";
            public const string LightPosition = "vLightPos";
            public const string LightDirection = "vLightDir";
        }

        public struct Techniques
        {
            public const string Default = "Default";
        }

        public struct Textures
        {
            public const string Texture1 = "t1";
            public const string Texture2 = "t2";
            public const string Texture3 = "t3";
        }
    }

    public enum FXParameterType
    {
        World,
        View,
        Projection,
        WorldViewProjection,
        EyePosition,
        LightDirection,
        AmbientColor,
        DiffuseColor,
    }
}