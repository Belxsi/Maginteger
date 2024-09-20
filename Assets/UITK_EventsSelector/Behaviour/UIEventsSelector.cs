using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Reflection;

[assembly: InternalsVisibleTo("UIEventsEditor")]
namespace UIEvents
{
    [RequireComponent(typeof(UIDocument))]
    [AddComponentMenu("UI Toolkit/UI Events Selector")]
    public class UIEventsSelector : MonoBehaviour
    {
        [SerializeField] internal List<UIEventsPair> events = new();
        private UIDocument document;
        private Dictionary<VisualElement, List<UIEvent>> registeredCallbacks;

        private void Awake()
        {
            document = GetComponent<UIDocument>();
            registeredCallbacks = new Dictionary<VisualElement, List<UIEvent>>();
            ReapplyCallbacks();
        }

        /// <summary>
        /// Call this function if visual tree changed at runtime.
        /// If there are new elements matching a query that haven't got their
        /// callbacks registered yet, this will apply them.
        /// </summary>
        public void ReapplyCallbacks()
        {
            foreach (UIEventsPair pair in events)
            {
                foreach (VisualElement element in QueryElement(pair.ElementInfo, document.rootVisualElement))
                {
                    if (!registeredCallbacks.ContainsKey(element)) registeredCallbacks.Add(element, new List<UIEvent>());
                    foreach (UIEvent uIEvent in pair.UIEvents)
                    {
                        if (!registeredCallbacks[element].Contains(uIEvent))
                        {
                            RegisterCallback(element, uIEvent);
                            registeredCallbacks[element].Add(uIEvent);
                        }
                    }
                }
            }
        }

        private void Update()
        {
            
        }

        internal static List<VisualElement> QueryElement(VisualElementInfo info, VisualElement rootVisualElement)
        {
            if(info == null)
            {
                return new List<VisualElement>();
            }

            if (info.FindDirect)
            {
                if(info.Hierarchy == null || info.Hierarchy.Count == 0)
                {
                    return new List<VisualElement>();
                }
                var current = rootVisualElement;
                for(int i = info.Hierarchy.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(info.Hierarchy[i].Name))
                    {
                        if (info.Hierarchy[i].index < 0 || info.Hierarchy[i].index >= current.hierarchy.childCount)
                        {
                            return new List<VisualElement>();
                        }
                        current = current.hierarchy[info.Hierarchy[i].index];
                    }
                    else
                    {
                        var tmp = current.hierarchy.Children().Where(e => e.name == info.Hierarchy[i].Name);
                        if (tmp.Count() > 1)
                        {
                            if (info.Hierarchy[i].index < 0 || info.Hierarchy[i].index >= current.hierarchy.childCount)
                            {
                                return new List<VisualElement>();
                            }
                            current = current.hierarchy[info.Hierarchy[i].index];
                        }
                        else if (tmp.Count() < 1)
                        {
                            return new List<VisualElement>();
                        }
                        else
                            current = tmp.First();
                    }
                }
                return new List<VisualElement>() { current };
            }
            else
            {
                UQueryBuilder<VisualElement> query = new UQueryBuilder<VisualElement>(rootVisualElement);
                if (!string.IsNullOrEmpty(info.Name))
                {
                    query.Name(info.Name);
                }
                if (!string.IsNullOrEmpty(info.Classes))
                {
                    string[] classes = info.Classes.Replace(" ","").Split(",").Where(s=>!string.IsNullOrEmpty(s)).ToArray();
                    foreach(string c in classes)
                    {
                        query.Class(c);
                    }
                }
                if (!string.IsNullOrEmpty(info.Type))
                {
                    return query.ToList().Where(v => v.GetType() == Type.GetType(info.Type)).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
        }

        private void RegisterCallback(VisualElement target, UIEvent eventInfo)
        {
            if(target == null || eventInfo == null || string.IsNullOrEmpty(eventInfo.EventType))
            {
                return;
            }
            Type eventType = Type.GetType(eventInfo.EventType);
            Action callback = () => {
                if (enabled) eventInfo.Event?.Invoke();
            };
            if (eventType.BaseType.GetGenericTypeDefinition() == typeof(CustomUIEventBase<>))
            {
                Activator.CreateInstance(eventType, new object[] { callback, target });
            }
            else
            {
                Type eventCallbackType = typeof(EventCallback<>).MakeGenericType(eventType);
                Type genericExecuterType = typeof(GenericEventExecuter<>).MakeGenericType(eventType);
                object executer = Activator.CreateInstance(genericExecuterType, new object[] {callback});
                var callbackDelegate = genericExecuterType.GetMethod("HandleExecution", BindingFlags.Public | BindingFlags.Instance)
                    .CreateDelegate(eventCallbackType, executer);


                MethodInfo registerCallbackMethod = typeof(VisualElement).GetMethods()
                    .Where(x => x.Name == "RegisterCallback")
                    .FirstOrDefault(x => x.IsGenericMethod)
                    .MakeGenericMethod(eventType);
                registerCallbackMethod.Invoke(target, new object[] { callbackDelegate, TrickleDown.TrickleDown });
            }
        }
    }

    internal class GenericEventExecuter<T> where T : EventBase
    {
        private Action callback;

        public GenericEventExecuter(Action callback)
        {
            this.callback = callback;
        }

        public void HandleExecution(T type)
        {
            callback.Invoke();
        }
    }

    [Serializable]
    internal class UIEventsPair
    {
        public VisualElementInfo ElementInfo;
        public List<UIEvent> UIEvents = new() { new UIEvent()};
    }
    [Serializable]
    internal class UIEvent
    {
        public string EventType = typeof(EventBase).AssemblyQualifiedName;
        public UnityEvent Event = new();
    }
    [Serializable]
    internal class VisualElementInfo
    {
        public string Type;
        public string Name;
        public string Classes;
        public bool FindDirect = true;
        public List<VisualParentInfo> Hierarchy = new();
    }
    [Serializable]
    internal class VisualParentInfo
    {
        public string Type;
        public string Name;
        public int index;
    }
}

