using ScEngineNet.SafeElements;

namespace ScEngineNet.ExtensionsNet
{
    public interface IScExtensionNet
    {
        string NetExtensionName { get; }
        string NetExtensionDescription { get; }
        ScResult Initialize();
        ScResult ShutDown();
    }
}
