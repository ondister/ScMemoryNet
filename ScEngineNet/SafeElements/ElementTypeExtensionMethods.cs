using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Методы расширения для <see cref="ElementType"/>
    /// </summary>
    public static class ElementTypeExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementTypes"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="elementTypes"></param>
        /// <param name="testElementTypes"></param>
        /// <returns></returns>
        public static Boolean HasAnyType(this ElementType elementTypes, ElementType testElementTypes)
        {
            return ((elementTypes & testElementTypes) != 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementTypes"></param>
        /// <param name="addElementTypes"></param>
        /// <returns></returns>
        public static ElementType AddType(this ElementType elementTypes, ElementType addElementTypes)
        {
            return elementTypes | addElementTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementTypes"></param>
        /// <param name="removeElementTypes"></param>
        /// <returns></returns>
        public static ElementType RemoveType(this ElementType elementTypes, ElementType removeElementTypes)
        {
            return elementTypes & ~removeElementTypes;
        }

    }

}
