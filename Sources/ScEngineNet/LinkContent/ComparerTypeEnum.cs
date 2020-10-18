
using System.ComponentModel;

namespace ScEngineNet.LinkContent
{
  public enum ComparerTypeEnum
    {
        [Description("Не указано")]
        UnknownWrapper =0,

        [Description("Равно")]
        Equal = 1,

        [Description("Часть строки или похожее фото")]
        PartOfEqual =2,

        [Description("Меньше чем")]
        Less =3,

        [Description("Больше чем")]
        More =4,

        [Description("В диапазоне")]
        Between =5
    }
}
