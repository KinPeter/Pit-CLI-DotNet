using Pit.Logs;

namespace Pit.Types
{
    public abstract class PitAction
    {
        private readonly Logger log;
        private readonly string[] args;
        public abstract void Run();
    }
}