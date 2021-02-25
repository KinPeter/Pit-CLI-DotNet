using System;
using System.Runtime.InteropServices;
using Pit.Logs;

namespace Pit.OS
{
    public class Os
    {
        private readonly Logger log;

        public Os()
        {
            log = new Logger("OS");
        }
        public bool IsLinux()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return true;
            log.Error("Operating system is not supported.");
            Environment.Exit(1);
            return false;
        }
    }
}