using System.Threading.Tasks;
using Pit.Args;

namespace Pit
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            await new ArgParser().Parse(args);
        }
    }
}