using System.Runtime.InteropServices;

namespace ScEngineNet.NativeElements
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct WScMemoryParams
    {
        public string RepoPath;
        public string ConfigFile;
        public string ExtensionsPath;
        public bool Clear;
    }
}
