using System;
using System.Collections.Generic;

namespace ScEngineNet.Events
{
    // Этот класс нужен для поддержания безопасности типа
    // и кода при использовании EventSet
    internal sealed class EventKey : Object
    {
    }

    internal sealed class EventSet
    {
        // Закрытый словарь служит для отображения EventKey -> Delegate
        private readonly Dictionary<EventKey, Delegate> mEvents = new Dictionary<EventKey, Delegate>();

        // Добавление отображения EventKey -> Delegate, если его не существует
        // И компоновка делегата с существующим ключом EventKey
        public void Add(EventKey eventKey, Delegate handler)
        {
            lock (mEvents)
            {
                Delegate d;
                mEvents.TryGetValue(eventKey, out d);
                mEvents[eventKey] = Delegate.Combine(d, handler);
            }
        }

        // Удаление делегата из EventKey (если он существует)
        // и разрыв связи EventKey -> Delegate при удалении
        // последнего делегата
        public void Remove(EventKey eventKey, Delegate handler)
        {
            lock (mEvents)
            {
                // Вызов TryGetValue предотвращает выдачу исключения
                // при попытке удаления делегата с отсутствующим ключом EventKey.
                Delegate d;
                if (mEvents.TryGetValue(eventKey, out d))
                {
                    d = Delegate.Remove(d, handler);
                    // Если делегат остается, то установить новый ключ EventKey, 
                    // иначе – удалить EventKey
                    if (d != null)
                    {
                        mEvents[eventKey] = d;
                    }
                    else
                    {
                        mEvents.Remove(eventKey);
                    }
                }
            }
        }

        // Информирование о событии для обозначенного ключа EventKey
        public void Raise(EventKey eventKey, Object sender, EventArgs e)
        {
            // Не выдавать исключение при отсутствии ключа EventKey
            Delegate d;
            lock (mEvents)
            {
                mEvents.TryGetValue(eventKey, out d);
            }
            if (d != null)
            {
                // Из-за того что словарь может содержать несколько разных типов 
                // делегатов, невозможно создать вызов делегата, безопасный по 
                // отношению к типу, во время компиляции. Я вызываю метод
                // DynamicInvoke типа System.Delegate, передавая ему параметры метода
                // обратного вызова в виде массива объектов. DynamicInvoke будет
                // контролировать безопасность типов параметров для вызываемого
                // метода обратного вызова. Если будет найдено несоответствие типов,
                // выдается исключение.
                d.DynamicInvoke(new Object[] { sender, e });
            }
        }
    }
}