﻿using System;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Идентификатор SC-элемента. Он может быть только текстовым.
    /// </summary>
    public class Identifier : IEquatable<Identifier>
    {
        /// <summary>
        ///     Возвращает пустой идентификатор, который не разрешен в системе
        /// </summary>
        public static readonly Identifier Invalid = String.Empty;

        /// <summary>
        ///     Инициализирует новый идентификатор SC-элемента.
        /// </summary>
        /// <param name="value">значение</param>
        public Identifier(string value)
        {
            Value = value;
        }

        /// <summary>
        ///     Значение.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Определяет равен ли заданный объект <see cref="Identifier" /> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="Identifier" /></param>
        public bool Equals(Identifier obj)
        {
            return obj != null && obj.Value == Value;
        }

        /// <summary>
        ///     Возвращает уникльный идентификатор
        /// </summary>
        /// <param name="scExtContext">Указатель на контекст</param>
        /// <param name="node">Узел для которого создается идентификатор</param>
        /// <returns>Уникальный идентификатор</returns>
        internal static Identifier GetUnique(ScMemoryContext scExtContext, ScNode node)
        {
            Identifier identifier = "idtf_" + node.ScAddress.GetHashCode();

            while (scExtContext.FindNode(identifier) != null)
            {
                var rand = new Random();
                identifier = identifier + "_" + rand.Next();
            }

            return identifier;
        }

        /// <summary>
        ///     Возвращает уникльный идентификатор
        /// </summary>
        /// <param name="scExtContext">Указатель на контекст</param>
        /// <param name="prefix">Преффикс</param>
        /// <param name="node">Узел для которого создается идентификатор</param>
        /// <returns>Уникальный идентификатор</returns>
        internal static Identifier GetUnique(ScMemoryContext scExtContext, string prefix, ScNode node)
        {
            var initialIdentifier = GetUnique(scExtContext, node);
            return prefix + "_" + initialIdentifier;
        }

        /// <summary>
        ///     Преобразование Идентификатора из строки.
        /// </summary>
        /// <param name="value">строковое значение</param>
        /// <returns>SC-идентификатор</returns>
        public static implicit operator Identifier(string value)
        {
            return new Identifier(value);
        }

        /// <summary>
        ///     Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        ///     Определяет равен ли заданный объект <see cref="T:System.Object" /> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object" /></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var identifier = obj as Identifier;
            return identifier != null && identifier.Value == Value;
        }

        /// <summary>
        ///     Возвращает хэш-код значения
        /// </summary>
        /// <returns>Хэш-код значения</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        ///     Оператор сравнения идентификаторов
        /// </summary>
        /// <param name="identifier1">Первый идентификатор</param>
        /// <param name="identifier2">Второй идентификатор</param>
        /// <returns>Возвращает True, если значения идентификаторов равны</returns>
        public static bool operator ==(Identifier identifier1, Identifier identifier2)
        {
            var isEqual = false;
            if (((object) identifier1 != null) && ((object) identifier2 != null))
            {
                isEqual = identifier1.Equals(identifier2);
            }
            return isEqual;
        }

        /// <summary>
        ///     Оператор сравнения идентификаторов
        /// </summary>
        /// <param name="identifier1">Первый идентификатор</param>
        /// <param name="identifier2">Второй идентификатор</param>
        /// <returns>Возвращает True, если значения идентификаторов не равны</returns>
        public static bool operator !=(Identifier identifier1, Identifier identifier2)
        {
            return !(identifier1 == identifier2);
        }

        /// <summary>
        ///     Получить массив байт для передачи.
        /// </summary>
        internal byte[] GetBytes()
        {
            return ScEngineNet.TextEncoding.GetBytes(Value);
        }
    }
}