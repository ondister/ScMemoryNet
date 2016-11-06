using System;
using System.Collections.Generic;
using System.Threading;

namespace ScEngineNet.Events
{

    // Этот класс нужен для поддержания безопасности типа
    // и кода при использовании EventSet
    internal sealed class EventKey : Object { }
    internal sealed class EventSet
    {
        // Закрытый словарь служит для отображения EventKey -> Delegate

        private readonly Dictionary<EventKey, Delegate> m_events = new Dictionary<EventKey, Delegate>();
        // Добавление отображения EventKey -> Delegate, если его не существует
        // И компоновка делегата с существующим ключом EventKey
        public void Add(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handler);
            Monitor.Exit(m_events);
        }
        // Удаление делегата из EventKey (если он существует)
        // и разрыв связи EventKey -> Delegate при удалении
        // последнего делегата
        public void Remove(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            // Вызов TryGetValue предотвращает выдачу исключения
            // при попытке удаления делегата с отсутствующим ключом EventKey.
            Delegate d;
            if (m_events.TryGetValue(eventKey, out d))
            {
                d = Delegate.Remove(d, handler);
                // Если делегат остается, то установить новый ключ EventKey, 
                // иначе – удалить EventKey
                if (d != null) m_events[eventKey] = d;
                else m_events.Remove(eventKey);
            }
            Monitor.Exit(m_events);
        }
        // Информирование о событии для обозначенного ключа EventKey
        public void Raise(EventKey eventKey, Object sender, EventArgs e)
        {
            // Не выдавать исключение при отсутствии ключа EventKey
            Delegate d;
            Monitor.Enter(m_events);
            m_events.TryGetValue(eventKey, out d);
            Monitor.Exit(m_events);
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