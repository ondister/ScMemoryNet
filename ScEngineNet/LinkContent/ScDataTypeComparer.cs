using System;

namespace ScEngineNet.LinkContent
{
    public class ScDataTypeComparer<T>
    {
        #region Конструкторы

        private ScDataTypeComparer(ComparerTypeEnum compareType)
        {
            this.CompareType = compareType;
        }

        public ScDataTypeComparer(ComparerTypeEnum compareType, T element1, T element2)
            : this(compareType)
        {
            if (compareType != ComparerTypeEnum.Between)
                throw new Exception("При указании двух элементов должен быть указан тип сравнения " +
                                    ComparerTypeEnum.Between);
          
            Element1 = element1;
            Element2 = element2;
        }
        
        public ScDataTypeComparer(ComparerTypeEnum compareType, T element)
            : this(compareType)
        {
            if (compareType == ComparerTypeEnum.Between)
                throw new Exception("Для указанного типа сравнения должно быть два элемента");
            Console.WriteLine(typeof(T));
            Element1 = element;
            Element2 = default(T);
        }

        #endregion


        public T Element1 { get; }

        public T Element2 { get; }

        public ComparerTypeEnum CompareType { get; }
    }
}