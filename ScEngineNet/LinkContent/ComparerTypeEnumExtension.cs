using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScEngineNet.LinkContent
{
   public static class ComparerTypeEnumExtension
    {
        public static string Description(this ComparerTypeEnum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(false)
                .First() as DescriptionAttribute;

            return attribute != null ? attribute.Description : string.Empty;
        }
    }

}
