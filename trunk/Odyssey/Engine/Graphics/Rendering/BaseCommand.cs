using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public abstract class BaseCommand : IRenderCommand
    {
        bool disposed;
        CommandType renderCommandType;
        SceneNodeCollection nodeCollection;

        public bool Disposed
        {
            get { return disposed; }
        }

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

        protected abstract void OnDispose();

        protected BaseCommand(CommandType renderCommandType, SceneNodeCollection nodeCollection)
        {
            this.renderCommandType = renderCommandType;
            this.nodeCollection = nodeCollection;
        }

         #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    OnDispose();
                }
            }
            disposed = true;
        }

        ~BaseCommand()
        {
            Dispose(false);
        }

        #endregion
    }
}