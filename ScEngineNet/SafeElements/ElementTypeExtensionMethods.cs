using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Методы расширения для <see cref="ElementType"/>
    /// </summary>
    public static class ElementTypeExtensionMethods
    {
        /// <summary>
        /// Определяет имеется ли некоторый подтип в типе.
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="elementType">Определяемый тип</param>
        /// <returns>Возвращает True или False в зависимости от реультата</returns>
        public static Boolean IsType(this ElementType elementTypes, ElementType elementType)
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
        public static Boolean HasAnyType(this ElementType elementTypes, ElementType testElementTypes)
        {
            return ((elementTypes & testElementTypes) != 0);
        }

        /// <summary>
        /// Добавляет подтип к типу элемента
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="addElementTypes">Добавляемый подтип</param>
        /// <returns>Взвращает итоговый тип</returns>
        public static void AddType(this ElementType elementTypes, ElementType addElementTypes)
        {
            elementTypes = elementTypes | addElementTypes;
        }

        /// <summary>
        /// Удаляет подтип
        /// </summary>
        /// <param name="elementTypes">Тип элемента</param>
        /// <param name="removeElementTypes">удаляемый подтип</param>
        /// <returns>Взвращает итоговый тип</returns>
        public static ElementType RemoveType(this ElementType elementTypes, ElementType removeElementTypes)
        {
            return elementTypes & ~removeElementTypes;
        }
    }
}
