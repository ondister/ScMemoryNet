using System;

namespace ScEngineNet.Native
{
    /// <summary>
    /// Методы расширения для <see cref="ElementTypes"/>
    /// </summary>
    internal static class ElementTypeExtensionMethods
    {
        /// <summary>
        /// Аналог HadFlag. Определяет, установлен ли конкретный тип.
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="elementType">Определяемый тип</param>
        /// <returns>Возвращает True или False в зависимости от реультата</returns>
        internal static Boolean IsType(this ElementTypes elementTypes, ElementTypes elementType)
        {
            bool isset = false;
            if (elementType != 0x00)
            {
                isset = (elementTypes & elementType) == elementType;
            }
            return isset;
        }

        /// <summary>
        /// Определяет имеет ли тип элемента некоторый подтип (ы)
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="testElementTypes">Определяемый подтип</param>
        /// <returns>Возвращает True или False в зависимости от реультата</returns>
        internal static Boolean HasAnyType(this ElementTypes elementTypes, ElementTypes testElementTypes)
        {
            return ((elementTypes & testElementTypes) != 0);
        }

        /// <summary>
        /// Добавляет подтип к типу элемента
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="addElementTypes">Добавляемый подтип</param>
        /// <returns>Взвращает итоговый тип</returns>
        internal static ElementTypes AddType(this ElementTypes elementTypes, ElementTypes addElementTypes)
        {
            return  elementTypes | addElementTypes;
        }

        /// <summary>
        /// Удаляет подтип
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="removeElementTypes">удаляемый подтип</param>
        /// <returns>Взвращает итоговый тип</returns>
        internal static ElementTypes RemoveType(this ElementTypes elementTypes, ElementTypes removeElementTypes)
        {
            return elementTypes & ~removeElementTypes;
        }
    }
}
