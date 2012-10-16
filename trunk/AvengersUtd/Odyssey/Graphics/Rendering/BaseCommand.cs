
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Graphics;
using System;


namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public abstract class BaseCommand : ICommand, IDisposable, IEquatable<BaseCommand>
    {
        bool disposed;

        public CommandAttributes CommandAttributes { get; protected set; }
        public CommandType CommandType { get; internal set; }

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
            CommandType = commandType;
        }

        public bool Supports(CommandAttributes commandAttributes)
        {
            return (CommandAttributes & commandAttributes) == commandAttributes;
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

        public static bool operator ==(BaseCommand left, BaseCommand right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseCommand left, BaseCommand right)
        {
            return !Equals(left, right);
        }

        public virtual bool Equals(BaseCommand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.CommandType, CommandType) && other.disposed.Equals(disposed);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (BaseCommand)) return false;
            return Equals((BaseCommand) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (disposed.GetHashCode()*397) ^ CommandType.GetHashCode();
            }
        }

        #endregion
    }
}