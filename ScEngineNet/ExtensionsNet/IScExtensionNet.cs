using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScEngineNet.NativeElements;
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
