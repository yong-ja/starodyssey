using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public abstract class BaseCommand : IRenderCommand
    {
        CommandType renderCommandType;
        SceneNodeCollection nodeCollection;

        public SceneNodeCollection Items
        {
            get { return nodeCollection; }
            internal set { nodeCollection = value; }
        }

        /// <summary>
        /// Executes this <see cref="BaseCommand"/>.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Performs the particular render operation required by this <see cref="BaseCommand"/>.
        /// </summary>
        public abstract void PerformRender();

        protected BaseCommand(CommandType renderCommandType, SceneNodeCollection nodeCollection)
        {
            this.renderCommandType = renderCommandType;
            this.nodeCollection = nodeCollection;
        }
    }
}