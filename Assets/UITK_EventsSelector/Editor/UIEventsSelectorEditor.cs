using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;
using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
using UnityEditor.Events;

namespace UIEvents
{
    [CustomEditor(typeof(UIEventsSelector))]
    public class UIEventsSelectorEditor : Editor
    {
        VisualElement foldoutsContainer;
        VisualTreePopup treePopup;
        ElementTypesPopup elementsPopup;
        ElementNamesPopup namesPopup;
        ElementClassesPopup classesPopup;
        UIEventsSearchProvider eventsPopup;
        private VisualElement root;

        private UIDocument document;
        private SerializedProperty visualTreeProperty;
        private VisualTreeAsset visualTreeAsset;
        private string visualTreePath;
        private string lastModified;


        private void OnEnable()
        {
            document = (target as MonoBehaviour).GetComponent<UIDocument>();
            visualTreeProperty = new SerializedObject(document).FindProperty("sourceAsset");
            visualTreeAsset = visualTreeProperty.objectReferenceValue as VisualTreeAsset;
            if (visualTreeAsset != null)
            {
                visualTreePath = AssetDatabase.GetAssetPath(visualTreeAsset);
                lastModified = File.GetLastWriteTime(visualTreePath).ToString();
            }
            eventsPopup = ScriptableObject.CreateInstance(typeof(UIEventsSearchProvider)) as UIEventsSearchProvider;
            EditorApplication.update += CheckVisualTreeAssetChange;
            Undo.undoRedoPerformed += HandleSourceVisualTreeChange;
        }
        private void OnDisable()
        {
            EditorApplication.update -= CheckVisualTreeAssetChange;
            Undo.undoRedoPerformed -= HandleSourceVisualTreeChange;
        }

        private void CheckVisualTreeAssetChange()
        {
            if (visualTreeAsset == null) return;
            if (File.GetLastWriteTime(visualTreePath).ToString() != lastModified)
            {
                HandleSourceVisualTreeChange();
                lastModified = File.GetLastWriteTime(visualTreePath).ToString();
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement() { name = "root-visual-element" };
            root.TrackPropertyValue(visualTreeProperty, s => HandleSourceVisualTreeChange());
            root.Add(new Button(() =>
            {
                (target as UIEventsSelector).events.Add(new UIEventsPair());
                serializedObject.Update();
                var events = serializedObject.FindProperty("events");
                var element = events.GetArrayElementAtIndex(events.arraySize - 1);
                foldoutsContainer.Add(CreateElementFoldout(element.FindPropertyRelative("ElementInfo"), element.FindPropertyRelative("UIEvents"), true));
            })
            { text = "Add element", name = "add-button" });
            root[0].style.height = 20;
            root.Add(GenerateInspectorContent());
            return root;
        }

        private VisualElement GenerateInspectorContent(params int[] openIndices)
        {
            foldoutsContainer = new VisualElement() { name = "inspector-content" };
            treePopup = new VisualTreePopup(document.rootVisualElement);
            elementsPopup = new ElementTypesPopup();
            namesPopup = new ElementNamesPopup(document.rootVisualElement);
            classesPopup = new ElementClassesPopup(document.rootVisualElement);

            SerializedProperty mainListProperty = serializedObject.FindProperty("events");
            for (int i = 0; i < mainListProperty.arraySize; i++)
            {
                var currEl = mainListProperty.GetArrayElementAtIndex(i);
                foldoutsContainer.Add(CreateElementFoldout(
                    currEl.FindPropertyRelative("ElementInfo"), 
                    currEl.FindPropertyRelative("UIEvents"),
                    openIndices.Contains(i)
                ));
            }


            StyleSheet styles = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UITK_EditorHelperMethods.GetPathRelativeTo($"t:Script {this.GetType().Name}", "EditorStyles/UIEventsSelectorStyles.uss")
                );
            foldoutsContainer.styleSheets.Add(styles);

            return foldoutsContainer;
        }


        private void HandleSourceVisualTreeChange()
        {
            if(visualTreeAsset == null)
            {
                visualTreeAsset = visualTreeProperty.objectReferenceValue as VisualTreeAsset;
                if (visualTreeAsset != null)
                {
                    visualTreePath = AssetDatabase.GetAssetPath(visualTreeAsset);
                    lastModified = File.GetLastWriteTime(visualTreePath).ToString();
                }
            }
            serializedObject.Update();
            int[] openFoldouts = foldoutsContainer.Children().OfType<Foldout>().Where(f => f.value).Select(f => foldoutsContainer.IndexOf(f)).ToArray();
            root.Remove(foldoutsContainer);
            root.Add(GenerateInspectorContent(openFoldouts));
        }

        private Foldout CreateElementFoldout(SerializedProperty visualInfoProperty, SerializedProperty uiEventsProperty, bool foldValue = false)
        {
            var foldout = new Foldout() { name = "element-foldout" };
            foldout.Q<Toggle>().focusable = false;
            foldout.Q<Toggle>().Q<VisualElement>(className: "unity-foldout__input").Insert(1, new Image() { name = "foldout-element-image" });

            var infoPropertyField = new VisualElementInfoProperty(visualInfoProperty, document.rootVisualElement,
                treePopup, elementsPopup, namesPopup, classesPopup, foldout.Q<Toggle>(), () => SetFoldoutError(foldout), () => SetFoldoutTitle(foldout, visualInfoProperty));

            foldout.Add(infoPropertyField);
            foldoutsContainer.Add(foldout);

            var eventsContainer = new VisualElement() { name = "events-container"};
            for (int j = 0; j < uiEventsProperty.arraySize; j++)
            {
                eventsContainer.Add(CreateEventElement(uiEventsProperty, j, visualInfoProperty, foldout));
            }
            foldout.Add(eventsContainer);
            var menuButton = new Button(() => FoldoutContextMenu(foldout)) { name = "menu-button" };
            foldout.Q<Toggle>().Add(menuButton);
            foldout.Q<Toggle>().RegisterCallback<PointerDownEvent>(e => { if (e.button == 1) FoldoutContextMenu(foldout); });

            foldout.value = foldValue;
            return foldout;
        }

        private VisualElement CreateEventElement(SerializedProperty uiEventsProperty, int index, SerializedProperty visualInfoProperty, Foldout foldout)
        {
            var currEvent = uiEventsProperty.GetArrayElementAtIndex(index);
            var eventTypeProperty = currEvent.FindPropertyRelative("EventType");

            if (string.IsNullOrEmpty(eventTypeProperty.stringValue))
            {
                eventTypeProperty.stringValue = typeof(EventBase).AssemblyQualifiedName;
            }

            var eventType = new DropdownField() { name = "event-dropdown" };
            eventType.value = Type.GetType(eventTypeProperty.stringValue).Name;

            var eventField = new PropertyField() { name = "event-field" };
            eventField.BindProperty(currEvent.FindPropertyRelative("Event"));

            var eventContainer = new VisualElement() { name = "uievent-container" };
            eventContainer.Add(eventField);
            eventContainer.Add(eventType);

            eventType.RegisterCallback<PointerDownEvent>(e =>
            {
                if (e.button == 0)
                {
                    eventsPopup.callback = s =>
                    {
                        eventType.value = s.Name;
                        eventTypeProperty.stringValue = s.AssemblyQualifiedName;
                        serializedObject.ApplyModifiedProperties();
                    };
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), eventsPopup);
                }
                else if (e.button == 1)
                {
                    var menu = new GenericMenu();
                    if (uiEventsProperty.arraySize > 1) menu.AddItem(new GUIContent("Delete"), false, () =>
                    {
                        uiEventsProperty.DeleteArrayElementAtIndex(index);
                        serializedObject.ApplyModifiedProperties();
                        int outerIndex = foldoutsContainer.IndexOf(foldout);
                        foldoutsContainer.RemoveAt(outerIndex);
                        foldoutsContainer.Insert(outerIndex, CreateElementFoldout(visualInfoProperty, uiEventsProperty, true));
                    });
                    if (uiEventsProperty.arraySize > 1 && index != 0) menu.AddItem(new GUIContent("Move up"), false, () =>
                    {
                        uiEventsProperty.MoveArrayElement(index, index - 1);
                        serializedObject.ApplyModifiedProperties();
                        int outerIndex = foldoutsContainer.IndexOf(foldout);
                        foldoutsContainer.RemoveAt(outerIndex);
                        foldoutsContainer.Insert(outerIndex, CreateElementFoldout(visualInfoProperty, uiEventsProperty, true));
                    });
                    if (uiEventsProperty.arraySize > 1 && index != uiEventsProperty.arraySize - 1) menu.AddItem(new GUIContent("Move down"), false, () =>
                    {
                        uiEventsProperty.MoveArrayElement(index, index + 1);
                        serializedObject.ApplyModifiedProperties();
                        int outerIndex = foldoutsContainer.IndexOf(foldout);
                        foldoutsContainer.RemoveAt(outerIndex);
                        foldoutsContainer.Insert(outerIndex, CreateElementFoldout(visualInfoProperty, uiEventsProperty, true));
                    });
                    try
                    {
                        var copy = JsonUtility.FromJson<CallbackCopy>(EditorGUIUtility.systemCopyBuffer);
                        menu.AddItem(new GUIContent("Paste"), false, () =>
                        {
                            Undo.RecordObject(target, "Add event");
                            UnityAction<string> action = copy.target.GetType().GetMethod(copy.methodName).CreateDelegate(typeof(UnityAction<string>), copy.target) as UnityAction<string>;
                            UnityEventTools.AddStringPersistentListener(
                                (target as UIEventsSelector).events[foldoutsContainer.IndexOf(eventContainer.parent.parent)].UIEvents[index].Event, action, copy.param);
                            serializedObject.Update();
                        });
                    }
                    catch (ArgumentException) { }

                    menu.ShowAsContext();
                }
            });

            return eventContainer;
        }

        private void FoldoutContextMenu(Foldout foldout)
        {
            int index = foldoutsContainer.IndexOf(foldout);
            var eventsProperty = serializedObject.FindProperty("events");

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => {
                eventsProperty.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();
                List<int> openFoldouts = foldoutsContainer.Children().OfType<Foldout>().Where(f => f.value).Select(f => foldoutsContainer.IndexOf(f)).ToList();
                openFoldouts.Remove(index);
                root.Remove(foldoutsContainer);
                root.Add(GenerateInspectorContent(openFoldouts.Select(s=>s>index?s-1:s).ToArray()));
            });
            if (eventsProperty.arraySize > 1 && index != 0) menu.AddItem(new GUIContent("Move up"), false, () =>
            {
                eventsProperty.MoveArrayElement(index, index - 1);
                serializedObject.ApplyModifiedProperties();
                var fold1 = CreateElementFoldout(eventsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("ElementInfo"),
                    eventsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("UIEvents"), (foldoutsContainer[index - 1] as Foldout).value);
                var fold2 = CreateElementFoldout(eventsProperty.GetArrayElementAtIndex(index - 1).FindPropertyRelative("ElementInfo"),
                    eventsProperty.GetArrayElementAtIndex(index - 1).FindPropertyRelative("UIEvents"), (foldoutsContainer[index] as Foldout).value);
                foldoutsContainer.RemoveAt(index - 1);
                foldoutsContainer.RemoveAt(index - 1);
                foldoutsContainer.Insert(index - 1, fold1);
                foldoutsContainer.Insert(index - 1, fold2);
            });
            if (eventsProperty.arraySize > 1 && index != eventsProperty.arraySize - 1) menu.AddItem(new GUIContent("Move down"), false, () =>
            {
                eventsProperty.MoveArrayElement(index, index + 1);
                serializedObject.ApplyModifiedProperties();
                var fold1 = CreateElementFoldout(eventsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("ElementInfo"),
                    eventsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("UIEvents"), (foldoutsContainer[index + 1] as Foldout).value);
                var fold2 = CreateElementFoldout(eventsProperty.GetArrayElementAtIndex(index+1).FindPropertyRelative("ElementInfo"),
                    eventsProperty.GetArrayElementAtIndex(index + 1).FindPropertyRelative("UIEvents"), (foldoutsContainer[index] as Foldout).value);
                foldoutsContainer.RemoveAt(index);
                foldoutsContainer.RemoveAt(index);
                foldoutsContainer.Insert(index, fold2);
                foldoutsContainer.Insert(index, fold1);
            });
            menu.AddItem(new GUIContent("Add Event"), false, () => {
                var uiEvents = eventsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("UIEvents");
                uiEvents.arraySize++;
                serializedObject.ApplyModifiedProperties();
                foldout.Q<VisualElement>(name: "events-container").Add(CreateEventElement(uiEvents, uiEvents.arraySize - 1,
                    eventsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("ElementInfo"), foldout));
            });
            menu.ShowAsContext();
        }

        private void SetFoldoutError(Foldout foldout)
        {
            foldout.Q<Image>(name: "foldout-element-image").image = EditorGUIUtility.Load("console.erroricon") as Texture;
            foldout.text = "Matching element not found";
            foldout.AddToClassList("foldout-error");
            foldout.Q<Toggle>().RemoveFromClassList("with_name");
            foldout.tooltip = "This happens when source Visual Tree Asset is changed, please select element again. If error persists, consider giving elements in your hierarchy unique names.";
        }

        private void SetFoldoutTitle(Foldout foldout, SerializedProperty elementInfo)
        {
            foldout.RemoveFromClassList("foldout-error");
            foldout.Q<Toggle>().RemoveFromClassList("with_name");
            foldout.tooltip = "";
            if (elementInfo.FindPropertyRelative("FindDirect").boolValue)
            {
                var hierarchy = elementInfo.FindPropertyRelative("Hierarchy");
                UnityEngine.Object elementTexture = null;
                if (hierarchy.arraySize > 0 && !string.IsNullOrEmpty(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue))
                    elementTexture = EditorGUIUtility.Load(Type.GetType(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue).Name + "@4x");
                if (elementTexture == null) 
                    elementTexture = EditorGUIUtility.Load("VisualElement@4x");
                foldout.Q<Image>(name: "foldout-element-image").image = elementTexture as Texture;
                if(!EditorGUIUtility.isProSkin) foldout.Q<Image>(name: "foldout-element-image").tintColor = Color.gray;
                if (hierarchy.arraySize > 0 && !string.IsNullOrEmpty(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue))
                {
                    foldout.Q<Toggle>().AddToClassList("with_name");
                    foldout.text = "#" + hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue;
                }
                else if (hierarchy.arraySize > 0 && !string.IsNullOrEmpty(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue))
                    foldout.text = Type.GetType(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue).Name;
                else
                    foldout.text = "Element";
            }
            else
            {
                UnityEngine.Object elementTexture = null;
                if (!string.IsNullOrEmpty(elementInfo.FindPropertyRelative("Type").stringValue))
                    elementTexture = EditorGUIUtility.Load(Type.GetType(elementInfo.FindPropertyRelative("Type").stringValue).Name + "@4x");
                if (elementTexture == null)
                    elementTexture = EditorGUIUtility.Load("VisualElement@4x");
                foldout.Q<Image>(name: "foldout-element-image").image = elementTexture as Texture;
                if (!EditorGUIUtility.isProSkin) foldout.Q<Image>(name: "foldout-element-image").tintColor = Color.gray;
                if (!string.IsNullOrEmpty(elementInfo.FindPropertyRelative("Name").stringValue))
                {
                    foldout.Q<Toggle>().AddToClassList("with_name");
                    foldout.text = "#" + elementInfo.FindPropertyRelative("Name").stringValue;
                }
                else if (!string.IsNullOrEmpty(elementInfo.FindPropertyRelative("Classes").stringValue))
                {
                    string[] classes = elementInfo.FindPropertyRelative("Classes").stringValue.Replace(" ", "").Split(",").Where(s=>!string.IsNullOrEmpty(s)).ToArray();
                    string title = "";
                    foreach (string className in classes)
                        title += "." + className + ", ";
                    foldout.text = title.Remove(title.Length - 2, 2);
                }
                else if (!string.IsNullOrEmpty(elementInfo.FindPropertyRelative("Type").stringValue))
                    foldout.text = Type.GetType(elementInfo.FindPropertyRelative("Type").stringValue).Name;
                else
                    foldout.text = "Element";
            }
        }

    }
}