using AvengersUtd.Odyssey.Graphics.Rendering.Management;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class SceneTreeUpdateCommand : BaseCommand, IUpdateCommand
    {
        private readonly SceneTree sceneTree;

        public bool IsThreaded { get { return false; } }

        public SceneTreeUpdateCommand(SceneTree sceneTree)
            : base(CommandType.Update)
        {
            this.sceneTree = sceneTree;
        }
        
        public void StartThread()
        {
            return;
        }

        public void TerminateThread()
        {
            return;
        }

        public void Activate()
        {
            return;
        }

        public override void Execute()
        {
           sceneTree.UpdateAllNodes();
        }

        public void Resume()
        {
            return;
        }
    
        protected override void OnDispose()
        {
            return;
        }
    }
}
