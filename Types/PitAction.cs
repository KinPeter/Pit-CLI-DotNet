using Pit.Logs;

namespace Pit.Types
{
    public abstract class PitAction
    {
        protected Logger Log { get; }
        protected string[] Args { get; }
        protected PitAction(string module, string[] args)
        {
            Log = new Logger(module);
            Args = args;
        }
        public abstract void Run();
    }
}