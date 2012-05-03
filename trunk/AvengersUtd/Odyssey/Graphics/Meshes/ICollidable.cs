using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface ICollidable
    {
        bool IsCollidable { get; }
        /// <summary>
        /// Programmatically causes a collision event involving this object with
        /// another entity.
        /// </summary>
        void CollideWith(IRenderable collidedObject);
    }
}
