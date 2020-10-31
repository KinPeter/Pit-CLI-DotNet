using System.Threading.Tasks;

namespace Pit.Types
{
    public interface IPitActionAsync
    {
        public Task RunAsync();
    }
}