using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScEngineNet
{
    [Serializable]
    public class ScContextInvalidException:Exception
    {
        public ScContextInvalidException(string message)
            :base(message)
        { 
        }
    }
}
