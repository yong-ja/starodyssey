
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Graphics;
using System;


namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public abstract class BaseCommand : ICommand, IEquatable<BaseCommand>
    {
        bool disposed;

        public CommandType CommandType { get; private set; }

        public bool Disposed
        {
            get { return disposed; }
        }
        

        /// <summary>
        /// Executes this <see cref="BaseCommand"/>.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Performs the particular render operation required by this <see cref="BaseCommand"/>.
        /// </summary>
        protected abstract void OnDispose();

        protected BaseCommand(CommandType commandType)
        {
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


        #region IEquatable<BaseCommand> Members

        public virtual bool Equals(BaseCommand other)
        {
            return CommandType == other.CommandType;
        }

        public static bool operator ==(BaseCommand cmd1, BaseCommand cmd2)
        {
            return cmd1.Equals(cmd2);
        }

        public static bool operator !=(BaseCommand cmd1, BaseCommand cmd2)
        {
            return !(cmd1 == cmd2);
        }

        #endregion
    }
}