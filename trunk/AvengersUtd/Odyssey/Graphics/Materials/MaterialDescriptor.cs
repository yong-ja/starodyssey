using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public enum TextureType : int
    {
        Diffuse,
        Normal,
        Texture1,
        Texture2,
        Specular,
        Shadows
    }

    public struct MaterialDescriptor
    {
        Type materialType;
        TextureDescriptor[] textureDescriptors;
        Dictionary<TextureType, int> textureMaps;

        public MaterialDescriptor(Type materialType, params TextureDescriptor[] textureDescriptors)
            : this()
        {
            this.materialType = materialType;
            TextureDescriptors = textureDescriptors;
        }


        public TextureDescriptor[] TextureDescriptors
        {
            get { return textureDescriptors; }
            set
            {
                textureDescriptors = value;
                textureMaps = new Dictionary<TextureType, int>();
                for (int i = 0; i < textureDescriptors.Length; i++)
                {
                    TextureDescriptor tDesc = textureDescriptors[i];
                    textureMaps.Add(tDesc.Type, i);
                }
            }
        }

        [XmlIgnore]
        public Type MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }

        [XmlAttribute(AttributeName = "Type")]
        public string XmlEffectType
        {
            get { return materialType.ToString(); }
            set { materialType = Type.GetType(value); }
        }

        public TextureDescriptor this[TextureType type]
        {
            get { return textureDescriptors[textureMaps[type]]; }
        }
    }


    public struct TextureDescriptor
    {
        TextureType type;
        string textureFilename;

        public TextureDescriptor(TextureType type, string texFilename)
        {
            textureFilename = texFilename;
            this.type = type;
        }

        [XmlAttribute]
        public TextureType Type
        {
            get { return type; }
            set { type = value; }
        }

        [XmlAttribute("Filename")]
        public string TextureFilename
        {
            get { return textureFilename; }
            set { textureFilename = value; }
        }
    }
}