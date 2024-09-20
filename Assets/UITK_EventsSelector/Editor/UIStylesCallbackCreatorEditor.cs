using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UIEvents
{
    [CustomEditor(typeof(UIStylesCallbackCreator))]
    public class UIStylesCallbackCreatorEditor : Editor
    {
        VisualElement foldoutsContainer;
        VisualTreePopup treePopup;
        ElementTypesPopup elementsPopup;
        ElementNamesPopup namesPopup;
        ElementClassesPopup classesPopup;
        StyleMethodsPopup methodsPopup;
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

        private void HandleSourceVisualTreeChange()
        {
            if (visualTreeAsset == null)
            {
                visualTreeAsset = visualTreeProperty.objectReferenceValue as VisualTreeAsset;
                if (visualTreeAsset != null)
                {
                    visualTreePath = AssetDatabase.GetAssetPath(visualTreeAsset);
                    lastModified = File.GetLastWriteTime(visualTreePath).ToString();
                }
            }
            serializedObject.Update();
            int[] openFoldouts = foldoutsContainer.Children().OfType<CustomFoldout>().Where(f => f.value).Select(f => foldoutsContainer.IndexOf(f)).ToArray();
            root.Remove(foldoutsContainer);
            root.Add(GenerateInspectorContent(openFoldouts));
        }

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement() { name = "root-visual-element" };
            root.TrackPropertyValue(visualTreeProperty, s => HandleSourceVisualTreeChange());
            root.Add(new Button(() =>
            {
                (target as UIStylesCallbackCreator).callbacks.Add(new ("Callback "+ (target as UIStylesCallbackCreator).callbacks.Count));
                serializedObject.Update();
                var callbacks = serializedObject.FindProperty("callbacks");
                var element = callbacks.GetArrayElementAtIndex(callbacks.arraySize - 1);
                var newFold = CreateElementFoldout(element.FindPropertyRelative("CallbackName"), element.FindPropertyRelative("CallbackElements"), true);
                foldoutsContainer.Add(newFold);
                newFold.titleContainer.Q<TextField>().Focus();
            })
            { text = "Add callback", name = "add-button" });
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
            methodsPopup = new StyleMethodsPopup();

            SerializedProperty mainListProperty = serializedObject.FindProperty("callbacks");
            for (int i = 0; i < mainListProperty.arraySize; i++)
            {
                var currEl = mainListProperty.GetArrayElementAtIndex(i);
                foldoutsContainer.Add(CreateElementFoldout(
                    currEl.FindPropertyRelative("CallbackName"),
                    currEl.FindPropertyRelative("CallbackElements"),
                    openIndices.Contains(i)
                ));
            }


            StyleSheet styles = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UITK_EditorHelperMethods.GetPathRelativeTo($"t:Script {this.GetType().Name}", "EditorStyles/UIStylesCallbackCreatorStyles.uss")
                );
            foldoutsContainer.styleSheets.Add(styles);

            return foldoutsContainer;
        }

        private CustomFoldout CreateElementFoldout(SerializedProperty callbackNameProperty, SerializedProperty callbackElementsProperty, bool foldValue = false)
        {
            var foldout = new CustomFoldout(foldValue, 0) { name = "element-foldout" };
            foldout.titleContainer.Add(new Image() { name = "foldout-element-image", image = (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "EventTrigger Icon") as Texture) });
            var callbackNameField = new TextField() { name="callback-name-field"};
            callbackNameField.BindProperty(callbackNameProperty);
            foldout.titleContainer.Add(callbackNameField);
            callbackNameField.RegisterCallback<PointerDownEvent>(e => e.StopImmediatePropagation());
            callbackNameField.RegisterCallback<PointerUpEvent>(e => e.StopImmediatePropagation());
            var errorLabel = new Label("Matching element not found") { name = "foldout-error-label" };
            errorLabel.style.display = DisplayStyle.None;
            foldout.titleContainer.Add(errorLabel);
            foldout.titleContainer.RegisterCallback<PointerUpEvent>(e => foldout.value = !foldout.value);

            for(int i = 0; i < callbackElementsProperty.arraySize; i++)
            {
                foldout.Add(CreateCallbackElement(callbackElementsProperty, foldout, i, callbackNameProperty));
            }

            foldoutsContainer.Add(foldout);

            var menuButton = new Button(() => FoldoutContextMenu(foldout)) { name = "menu-button" };
            foldout.titleContainer.Add(menuButton);
            foldout.titleContainer.RegisterCallback<PointerDownEvent>(e => { if (e.button == 1) FoldoutContextMenu(foldout); });

            return foldout;
        }

        private VisualElement CreateCallbackElement(SerializedProperty callbackElementsProperty, CustomFoldout foldout, int index, SerializedProperty callbackNameProperty)
        {
            var callbackContainer = new VisualElement();
            SerializedProperty callbackElementProperty = callbackElementsProperty.GetArrayElementAtIndex(index);
            var infoPropertyField = new VisualElementInfoProperty(callbackElementProperty.FindPropertyRelative("Info"), document.rootVisualElement,
                treePopup, elementsPopup, namesPopup, classesPopup, foldout.titleContainer, () => SetFoldoutError(foldout), () => SetFoldoutTitle(foldout))
                {name = "visual-element-info" };
            callbackContainer.Add(infoPropertyField);

            infoPropertyField.RegisterCallback<PointerDownEvent>(e =>
            {
                if(e.button == 1)
                {
                    var menu = new GenericMenu();
                    if (callbackElementsProperty.arraySize > 1) menu.AddItem(new GUIContent("Delete"), false, () =>
                    {
                        callbackElementsProperty.DeleteArrayElementAtIndex(index);
                        serializedObject.ApplyModifiedProperties();
                        int outerIndex = foldoutsContainer.IndexOf(foldout);
                        foldoutsContainer.RemoveAt(outerIndex);
                        foldoutsContainer.Insert(outerIndex, CreateElementFoldout(callbackNameProperty, callbackElementsProperty, true));
                    });
                    if (callbackElementsProperty.arraySize > 1 && index != 0) menu.AddItem(new GUIContent("Move up"), false, () =>
                    {
                        callbackElementsProperty.MoveArrayElement(index, index - 1);
                        serializedObject.ApplyModifiedProperties();
                        int outerIndex = foldoutsContainer.IndexOf(foldout);
                        foldoutsContainer.RemoveAt(outerIndex);
                        foldoutsContainer.Insert(outerIndex, CreateElementFoldout(callbackNameProperty, callbackElementsProperty, true));
                    });
                    if (callbackElementsProperty.arraySize > 1 && index != callbackElementsProperty.arraySize - 1) menu.AddItem(new GUIContent("Move down"), false, () =>
                    {
                        callbackElementsProperty.MoveArrayElement(index, index + 1);
                        serializedObject.ApplyModifiedProperties();
                        int outerIndex = foldoutsContainer.IndexOf(foldout);
                        foldoutsContainer.RemoveAt(outerIndex);
                        foldoutsContainer.Insert(outerIndex, CreateElementFoldout(callbackNameProperty, callbackElementsProperty, true));
                    });

                    menu.ShowAsContext();
                }
            });

            var indexes = ExtractNumbers(callbackElementProperty.propertyPath);
            var callbacksList = (target as UIStylesCallbackCreator).callbacks[indexes[0]].CallbackElements[indexes[1]].Callbacks;
            var callsList = new ListView(callbacksList, -1, 
                () => new VisualElement() { name = "callback-list-element"},
                (element, i) =>
                {
                    serializedObject.Update();
                    var property = callbackElementProperty.FindPropertyRelative("Callbacks").GetArrayElementAtIndex(i);

                    element.Clear();
                    element.style.flexDirection = FlexDirection.Row;
                    var methodNameField = new DropdownField();
                    methodNameField.BindProperty(property.FindPropertyRelative("MethodName"));
                    element.Add(methodNameField);
                    var parameterContainer = new VisualElement() { name = "callback-parameter-container" };
                    methodNameField.RegisterCallback<PointerDownEvent>(e =>
                    {
                        methodsPopup.onSelect = obj =>
                        {
                            methodNameField.value = (obj as MethodInfo).Name;
                            property.FindPropertyRelative("ParamType").stringValue = (obj as MethodInfo).GetParameters()[0].ParameterType.AssemblyQualifiedName;
                            SetupParameter();
                        };
                        UnityEditor.PopupWindow.Show(element.worldBound, methodsPopup);
                    });
                    element.Add(parameterContainer);
                    SetupParameter();

                    void SetupParameter()
                    {
                        parameterContainer.Clear();
                        if (string.IsNullOrEmpty(property.FindPropertyRelative("ParamType").stringValue)) return;
                        Type parameterType = Type.GetType(property.FindPropertyRelative("ParamType").stringValue);
                        if(parameterType == typeof(string))
                        {
                            var stringParam = new TextField("");
                            stringParam.BindProperty(property.FindPropertyRelative("StringValue"));
                            parameterContainer.Add(stringParam);
                        }
                        else if(parameterType == typeof(int))
                        {
                            var intParam = new IntegerField("");
                            intParam.BindProperty(property.FindPropertyRelative("IntValue"));
                            parameterContainer.Add(intParam);
                        }
                        else if(parameterType == typeof(float))
                        {
                            var floatParam = new FloatField("");
                            floatParam.BindProperty(property.FindPropertyRelative("FloatValue"));
                            parameterContainer.Add(floatParam);
                        }
                        else if(parameterType == typeof(Color))
                        {
                            var colorParam = new ColorField("");
                            colorParam.BindProperty(property.FindPropertyRelative("ColorValue"));
                            parameterContainer.Add(colorParam);
                        }
                        else if(parameterType.IsEnum)
                        {
                            Array enums = Enum.GetValues(parameterType);
                            var enumParam = new EnumField("", Enum.GetValues(parameterType).GetValue(0) as Enum);
                            enumParam.TrackPropertyValue(property.FindPropertyRelative("EnumValue"), c => { enumParam.value = enums.GetValue(Mathf.Clamp(c.intValue, 0, enums.Length-1)) as Enum; serializedObject.ApplyModifiedProperties(); });
                            enumParam.RegisterValueChangedCallback(c=> { property.FindPropertyRelative("EnumValue").intValue = Array.IndexOf(enums, c.newValue); serializedObject.ApplyModifiedProperties(); });
                            enumParam.schedule.Execute(()=> enumParam.value = enums.GetValue(Mathf.Clamp(property.FindPropertyRelative("EnumValue").intValue, 0, enums.Length - 1)) as Enum);
                            parameterContainer.Add(enumParam);
                        }
                    }
                }
            )
            {
                reorderable = true,
                showFoldoutHeader = false,
                showBoundCollectionSize = false,
                showAddRemoveFooter = true,
                showBorder = true,
                reorderMode = ListViewReorderMode.Animated,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All
            };
            callbackContainer.Add(callsList);
            
            return callbackContainer;
        }



        private void FoldoutContextMenu(CustomFoldout foldout)
        {
            int index = foldoutsContainer.IndexOf(foldout);
            var callbacksProperty = serializedObject.FindProperty("callbacks");

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => {
                callbacksProperty.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();
                List<int> openFoldouts = foldoutsContainer.Children().OfType<CustomFoldout>().Where(f => f.value).Select(f => foldoutsContainer.IndexOf(f)).ToList();
                openFoldouts.Remove(index);
                root.Remove(foldoutsContainer);
                root.Add(GenerateInspectorContent(openFoldouts.Select(s => s > index ? s - 1 : s).ToArray()));
            });
            if (callbacksProperty.arraySize > 1 && index != 0) menu.AddItem(new GUIContent("Move up"), false, () =>
            {
                callbacksProperty.MoveArrayElement(index, index - 1);
                serializedObject.ApplyModifiedProperties();
                var fold1 = CreateElementFoldout(callbacksProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CallbackName"),
                    callbacksProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CallbackElements"), (foldoutsContainer[index - 1] as CustomFoldout).value);
                var fold2 = CreateElementFoldout(callbacksProperty.GetArrayElementAtIndex(index - 1).FindPropertyRelative("CallbackName"),
                    callbacksProperty.GetArrayElementAtIndex(index - 1).FindPropertyRelative("CallbackElements"), (foldoutsContainer[index] as CustomFoldout).value);
                foldoutsContainer.RemoveAt(index - 1);
                foldoutsContainer.RemoveAt(index - 1);
                foldoutsContainer.Insert(index - 1, fold1);
                foldoutsContainer.Insert(index - 1, fold2);
            });
            if (callbacksProperty.arraySize > 1 && index != callbacksProperty.arraySize - 1) menu.AddItem(new GUIContent("Move down"), false, () =>
            {
                callbacksProperty.MoveArrayElement(index, index + 1);
                serializedObject.ApplyModifiedProperties();
                var fold1 = CreateElementFoldout(callbacksProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CallbackName"),
                    callbacksProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CallbackElements"), (foldoutsContainer[index + 1] as CustomFoldout).value);
                var fold2 = CreateElementFoldout(callbacksProperty.GetArrayElementAtIndex(index + 1).FindPropertyRelative("CallbackName"),
                    callbacksProperty.GetArrayElementAtIndex(index + 1).FindPropertyRelative("CallbackElements"), (foldoutsContainer[index] as CustomFoldout).value);
                foldoutsContainer.RemoveAt(index);
                foldoutsContainer.RemoveAt(index);
                foldoutsContainer.Insert(index, fold2);
                foldoutsContainer.Insert(index, fold1);
            });
            menu.AddItem(new GUIContent("Add Element"), false, () => {
                var callbackElements = callbacksProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CallbackElements");
                callbackElements.arraySize++;
                serializedObject.ApplyModifiedProperties();
                foldout.Add(CreateCallbackElement(callbackElements, foldout, callbackElements.arraySize - 1, callbacksProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CallbackName")));
            });
            menu.AddItem(new GUIContent("Copy"), false, () => {
                UnityAction<string> action = (target as UIStylesCallbackCreator).ExecuteCallback;
                var copy = new CallbackCopy() { target = (target as UIStylesCallbackCreator), methodName = "ExecuteCallback", param = foldout.titleContainer.Q<TextField>().value };
                string json = JsonUtility.ToJson(copy);
                EditorGUIUtility.systemCopyBuffer = json;
            });
            menu.ShowAsContext();
        }

        private void SetFoldoutError(CustomFoldout foldout)
        {
            foldout.titleContainer.Q<Image>(name: "foldout-element-image").image = EditorGUIUtility.Load("console.erroricon") as Texture;
            foldout.titleContainer.Q<TextField>(name: "callback-name-field").style.display = DisplayStyle.None;
            foldout.titleContainer.Q<Label>(name: "foldout-error-label").style.display = DisplayStyle.Flex;
            foldout.AddToClassList("foldout-error");
            foldout.tooltip = "This happens when source Visual Tree Asset is changed, please select element again. If error persists, consider giving elements in your hierarchy unique names.";
        }

        private void SetFoldoutTitle(CustomFoldout foldout)
        {
            foldout.titleContainer.Q<Image>(name: "foldout-element-image").image = (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "EventTrigger Icon") as Texture);
            foldout.titleContainer.Q<TextField>(name: "callback-name-field").style.display = DisplayStyle.Flex;
            foldout.titleContainer.Q<Label>(name: "foldout-error-label").style.display = DisplayStyle.None;
            foldout.RemoveFromClassList("foldout-error");
            foldout.tooltip = "";
        }

        public int[] ExtractNumbers(string input)
        {
            List<int> numbers = new List<int>();
            string pattern = @"\[(\d+)\]";

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string numberStr = match.Groups[1].Value;

                    if (int.TryParse(numberStr, out int number))
                    {
                        numbers.Add(number);
                    }
                }
            }

            return numbers.ToArray();
        }
    }
    internal class CallbackCopy
    {
        public MonoBehaviour target;
        public string methodName;
        public string param;
    }
}
