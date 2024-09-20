using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public class UIEventsSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public Action<Type> callback;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("Event Types"), 0));

            List<Type> eventTypes = TypeCache.GetTypesDerivedFrom<EventBase>()
                .Where(t => !t.IsGenericType && !t.IsAbstract)
                .ToList();

            eventTypes.AddRange(TypeCache.GetTypesDerivedFrom(typeof(CustomUIEventBase<>)).ToList());
            eventTypes.Sort((a, b) => a.Name.CompareTo(b.Name));

            Dictionary<string, List<Type>> groupedEventTypes = new Dictionary<string, List<Type>>();

            foreach (Type eventType in eventTypes)
            {
                string groupName = GetFirstCapitalizedWord(eventType.Name);

                if (!groupedEventTypes.ContainsKey(groupName))
                {
                    groupedEventTypes[groupName] = new List<Type>();
                }

                groupedEventTypes[groupName].Add(eventType);
            }

            List<Type> otherGroup = new List<Type>();
            foreach (var kvp in groupedEventTypes)
            {
                if (kvp.Value.Count == 1)
                {
                    otherGroup.AddRange(kvp.Value);
                }
                else
                {
                    searchList.Add(new SearchTreeGroupEntry(new GUIContent(kvp.Key), 1));
                    foreach (Type eventType in kvp.Value)
                    {
                        SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(eventType.Name));
                        entry.level = 2;
                        entry.userData = eventType;
                        searchList.Add(entry);
                    }
                }
            }

            if (otherGroup.Count > 0)
            {
                searchList.Add(new SearchTreeGroupEntry(new GUIContent("Other"), 1));
                foreach (Type eventType in otherGroup)
                {
                    SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(eventType.Name));
                    entry.level = 2;
                    entry.userData = eventType;
                    searchList.Add(entry);
                }
            }

            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            callback?.Invoke((Type)SearchTreeEntry.userData);
            return true;
        }

        private string GetFirstCapitalizedWord(string input)
        {
            StringBuilder result = new StringBuilder();

            result.Append(input[0]);
            int i = 1;
            while(i<input.Length && char.IsLower(input[i]))
            {
                result.Append(input[i]);
                i++;
            }

            return result.ToString();
        }
    }
}
