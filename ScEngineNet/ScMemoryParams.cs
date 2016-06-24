using ScEngineNet.NativeElements;

namespace ScMemoryNet
{
   public class ScMemoryParams
    {
       internal WScMemoryParams scParams;
       private readonly string netExtensionsPath;

       public string ExtensionsPath
       {
           get { return scParams.ExtensionsPath; }
       }

       public string ConfigFile
       {
           get { return scParams.ConfigFile; }
       }

       public string RepoPath
       {
           get { return scParams.RepoPath; }
       }

       public string NetExtensionsPath
       {
           get { return netExtensionsPath; }
       }

       public ScMemoryParams(bool clearBeforeInit, string configFile, string repoPath, string extensionsPath, string netExtensionsPath)
       {
           this.scParams = new WScMemoryParams() { Clear = clearBeforeInit, ConfigFile = configFile, RepoPath = repoPath, ExtensionsPath = extensionsPath };
           this.netExtensionsPath = netExtensionsPath;
       }
    }
}
