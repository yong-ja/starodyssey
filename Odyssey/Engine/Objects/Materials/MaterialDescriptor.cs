using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AvengersUtd.Odyssey.Objects.Materials
{
    public enum TextureType : int
    {
        Diffuse,
        Normal,
        Texture1,
        Texture2
    }

    public struct MaterialDescriptor
    {
        Type effectType;
        TextureDescriptor[] textureDescriptors;
        Dictionary<TextureType, int> links;

        public MaterialDescriptor(Type effectType, params TextureDescriptor[] textureDescriptors) : this()
        {
            this.effectType = effectType;
            TextureDescriptors = textureDescriptors;
        }


        public TextureDescriptor[] TextureDescriptors
        {
            get { return textureDescriptors; }
            set
            {
                textureDescriptors = value;
                links = new Dictionary<TextureType, int>();
                for (int i = 0; i < textureDescriptors.Length; i++)
                {
                    TextureDescriptor tDesc = textureDescriptors[i];
                    links.Add(tDesc.Type, i);
                }
                
            }
        }
        
        [XmlIgnore]
        public Type EffectType
        {
            get { return effectType; }
            set { effectType = value; }
        }

        [XmlAttribute(AttributeName="EffecType")]
        public string XmlEffectType
        {
            get
            {
                return effectType.ToString();
            }
            set { effectType = Type.GetType(value); }
        }

        public TextureDescriptor this[TextureType type]
        {
            get
            {
                return textureDescriptors[links[type]];
            }
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

        [XmlAttribute]
        public string TextureFilename
        {
            get { return textureFilename; }
            set { textureFilename = value; }
        }

    }
}