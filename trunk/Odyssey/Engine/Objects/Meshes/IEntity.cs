using System;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Meshes
{
    public interface IEntity : IDisposable
    {
        Vector3 Position { get; set; }
        EntityDescriptor Descriptor { get; }
        IMaterialContainer[] Materials { get; }
        void Render();
        void UpdatePosition();
        void Init();
    }
}