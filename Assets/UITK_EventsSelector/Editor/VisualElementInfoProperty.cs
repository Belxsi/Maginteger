using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public class VisualElementInfoProperty : VisualElement
    {
        private VisualElement selectElementContent;
        private VisualElement buildQueryContent;

        public VisualElementInfoProperty(SerializedProperty property, VisualElement rootToRefer, 
            VisualTreePopup treePopup, ElementTypesPopup elementsPopup, ElementNamesPopup namesPopup, ElementClassesPopup classesPopup, VisualElement popupParent,
            Action onSelectionError, Action onSelectionChange)
        {

            // ========================  Prepare toggleable container ==========================

            VisualElement buttonsContainer = new VisualElement() { name = "toggleable-buttons-container" };
            buttonsContainer.style.flexDirection = FlexDirection.Row;
            Add(buttonsContainer);

            Button selectElementToggle = new Button() { text = "Select element", name = "select-element-toggle" };
            selectElementToggle.AddToClassList("toggleable-button");
            selectElementContent = new VisualElement();
            selectElementContent.AddToClassList("toggleable-content");
            buttonsContainer.Add(selectElementToggle);
            Add(selectElementContent);

            Button buildQueryToggle = new Button() { text = "Build query", name = "build-query-toggle" };
            buildQueryToggle.AddToClassList("toggleable-button");
            buildQueryContent = new VisualElement();
            buildQueryContent.AddToClassList("toggleable-content");
            buttonsContainer.Add(buildQueryToggle);
            Add(buildQueryContent);

            selectElementToggle.clicked += () =>
            {
                property.FindPropertyRelative("FindDirect").boolValue = true;
                property.serializedObject.ApplyModifiedProperties();

                buildQueryToggle.RemoveFromClassList("toggled-button");
                buildQueryContent.style.display = DisplayStyle.None;

                selectElementToggle.AddToClassList("toggled-button");
                selectElementContent.style.display = DisplayStyle.Flex;

                if (!VerifyElementSelection(property, rootToRefer))
                    onSelectionError?.Invoke();
                else
                    onSelectionChange?.Invoke();
            };
            buildQueryToggle.clicked += () =>
            {
                property.FindPropertyRelative("FindDirect").boolValue = false;
                property.serializedObject.ApplyModifiedProperties();

                selectElementToggle.RemoveFromClassList("toggled-button");
                selectElementContent.style.display = DisplayStyle.None;

                buildQueryToggle.AddToClassList("toggled-button");
                buildQueryContent.style.display = DisplayStyle.Flex;

                onSelectionChange?.Invoke();
            };

            // ========================  Fill Select element content ==========================

            var selectorButton = new Button();
            selectorButton.clicked += () =>
            {
                List<int> parents = new();
                for(int i = 0; i < property.FindPropertyRelative("Hierarchy").arraySize; i++)
                {
                    parents.Add(property.FindPropertyRelative("Hierarchy").GetArrayElementAtIndex(i).FindPropertyRelative("index").intValue);
                }
                treePopup.hierarchyInfo = parents;
                treePopup.onSelect = obj =>
                {
                    BuildElementInfo(property, obj as VisualElement, rootToRefer);
                    if (!VerifyElementSelection(property, rootToRefer))
                    {
                        onSelectionError?.Invoke();
                        selectorButton.text = "Select element";
                        selectorButton.RemoveFromClassList("with_name");
                    }
                    else
                    {
                        onSelectionChange?.Invoke();
                        SetButtonContent(selectorButton, property);
                    }
                };
                UnityEditor.PopupWindow.Show(popupParent.worldBound, treePopup);
            };
            selectorButton.Add(new Image() { name = "foldout-element-image" });
            SetButtonContent(selectorButton, property);
            selectElementContent.Add(selectorButton);

            // ========================= Fill build query content ======================

            VisualElement typeContainer = new VisualElement() { name = "type-container" };
            typeContainer.AddToClassList("input-container");
            VisualElement nameContainer = new VisualElement() { name = "name-container" };
            nameContainer.AddToClassList("input-container");
            VisualElement classesContainer = new VisualElement() { name = "classes-container", tooltip = "Multiple classes mean targeting elemenets containing all specified classes, not any of them. Make sure your classes do not have whitespaces." };
            classesContainer.AddToClassList("input-container");

            typeContainer.Add(new Label("Type"));
            nameContainer.Add(new Label("Name"));
            classesContainer.Add(new Label("Classes"));

            TextField typeField = new TextField() { isReadOnly = true, name = "text-type-field" };
            if (!string.IsNullOrEmpty(property.FindPropertyRelative("Type").stringValue))
            {
                typeField.SetValueWithoutNotify(Type.GetType(property.FindPropertyRelative("Type").stringValue).Name);
            }
            Image pickType = new Image() { image = (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "pick_uielements") as Texture), focusable = true, name = "pick-button" };
            typeField[0].Add(pickType);

            TextField nameField = new TextField() { name = "text-name-field" };
            nameField.BindProperty(property.FindPropertyRelative("Name"));
            Image pickName = new Image(){ image = (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "pick_uielements") as Texture), focusable = true, name = "pick-button" };
            nameField[0].Add(pickName);
            nameField.RegisterCallback<PointerDownEvent>(e => e.StopImmediatePropagation());

            TextField classesField = new TextField() { name = "text-classes-field" };
            classesField.BindProperty(property.FindPropertyRelative("Classes"));
            Image pickClasses = new Image() { image = (EditorGUIUtility.Load((EditorGUIUtility.isProSkin ? "d_" : "") + "pick_uielements") as Texture), focusable = true, name = "pick-button" };
            classesField[0].Add(pickClasses);
            classesField.RegisterCallback<PointerDownEvent>(e => e.StopImmediatePropagation());

            typeContainer.Add(typeField);
            nameContainer.Add(nameField);
            classesContainer.Add(classesField);

            typeField.RegisterCallback<PointerDownEvent>(e => {
                e.StopImmediatePropagation();
                if (e.button != 0) return;
                elementsPopup.onSelect = obj =>
                {
                    property.FindPropertyRelative("Type").stringValue = (obj as Type).AssemblyQualifiedName;
                    typeField.value = (obj as Type).Name;
                    property.serializedObject.ApplyModifiedProperties();
                };
                UnityEditor.PopupWindow.Show(popupParent.worldBound, elementsPopup);
            });
            pickName.RegisterCallback<PointerDownEvent>(e =>
            {
                e.StopImmediatePropagation();
                if (e.button != 0) return;
                namesPopup.onSelect = obj =>
                {
                    nameField.value = (obj as string);
                    nameField.Blur();
                };
                UnityEditor.PopupWindow.Show(popupParent.worldBound, namesPopup);
            });
            pickClasses.RegisterCallback<PointerDownEvent>(e =>
            {
                e.StopImmediatePropagation();
                if (e.button != 0) return;
                classesPopup.originValue = classesField.value;
                classesPopup.onSelect = obj =>
                {
                    classesField.value = (obj as string);
                    classesField.Blur();
                };
                UnityEditor.PopupWindow.Show(popupParent.worldBound, classesPopup);
            });

            typeField.RegisterValueChangedCallback(e => { if (buildQueryContent.style.display == DisplayStyle.Flex) onSelectionChange?.Invoke(); });
            nameField.RegisterValueChangedCallback(e => { if (buildQueryContent.style.display == DisplayStyle.Flex) onSelectionChange?.Invoke(); });
            classesField.RegisterValueChangedCallback(e => { if (buildQueryContent.style.display == DisplayStyle.Flex) onSelectionChange?.Invoke(); });

            buildQueryContent.style.flexDirection = FlexDirection.Row;
            buildQueryContent.Add(typeContainer);
            buildQueryContent.Add(nameContainer);
            buildQueryContent.Add(classesContainer);


            if (property.FindPropertyRelative("FindDirect").boolValue)
            {
                selectElementToggle.AddToClassList("toggled-button");
                selectElementContent.style.display = DisplayStyle.Flex;
                buildQueryContent.style.display = DisplayStyle.None;

                if (!VerifyElementSelection(property, rootToRefer))
                {
                    onSelectionError?.Invoke();
                    selectorButton.text = "Select element";
                    selectorButton.RemoveFromClassList("with_name");
                }
                else
                {
                    onSelectionChange?.Invoke();
                    SetButtonContent(selectorButton, property);
                }
            }
            else
            {
                buildQueryToggle.AddToClassList("toggled-button");
                buildQueryContent.style.display = DisplayStyle.Flex;
                selectElementContent.style.display = DisplayStyle.None;
                onSelectionChange?.Invoke();
            }

            // ========================= Add Styles ======================

            StyleSheet styles = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UITK_EditorHelperMethods.GetPathRelativeTo($"t:Script {this.GetType().Name}", "EditorStyles/VisualElementInfoStyles.uss")
                );
            styleSheets.Add(styles);
        }

        public void BuildElementInfo(SerializedProperty target, VisualElement source, VisualElement root)
        {
            var hierarchy = target.FindPropertyRelative("Hierarchy");
            hierarchy.ClearArray();
            var tmp = source;
            while (tmp != root)
            {
                hierarchy.arraySize++;
                var visualParent = hierarchy.GetArrayElementAtIndex(hierarchy.arraySize - 1);
                visualParent.FindPropertyRelative("Type").stringValue = tmp.GetType().AssemblyQualifiedName;
                visualParent.FindPropertyRelative("Name").stringValue = tmp.name;
                visualParent.FindPropertyRelative("index").intValue = tmp.hierarchy.parent.hierarchy.IndexOf(tmp);
                tmp = tmp.hierarchy.parent;
            }
            target.serializedObject.ApplyModifiedProperties();
        }

        private bool VerifyElementSelection(SerializedProperty info, VisualElement root)
        {
            VisualElement current = root;
            var hierarchy = info.FindPropertyRelative("Hierarchy");

            for (int i = hierarchy.arraySize - 1; i >= 0; i--)
            {
                var nameProp = hierarchy.GetArrayElementAtIndex(i).FindPropertyRelative("Name");
                var indexProp = hierarchy.GetArrayElementAtIndex(i).FindPropertyRelative("index");
                if (!string.IsNullOrEmpty(nameProp.stringValue))
                {
                    if (indexProp.intValue < 0 || indexProp.intValue >= current.hierarchy.childCount)
                    {
                        return false;
                    }
                    current = current.hierarchy[indexProp.intValue];
                }
                else
                {
                    var tmp = current.hierarchy.Children().Where(e => e.name == nameProp.stringValue);
                    if (tmp.Count() > 1)
                    {
                        if (indexProp.intValue < 0 || indexProp.intValue >= current.hierarchy.childCount)
                        {
                            return false;
                        }
                        current = current.hierarchy[indexProp.intValue];
                    }
                    else if (tmp.Count() < 1)
                    {
                        return false;
                    }
                    else
                        current = tmp.First();
                }
            }
            return true;
        }

        private void SetButtonContent(Button button, SerializedProperty elementInfo)
        {
            var hierarchy = elementInfo.FindPropertyRelative("Hierarchy");
            button.RemoveFromClassList("with_name");
            UnityEngine.Object elementTexture = null;

            if (hierarchy.arraySize > 0 && !string.IsNullOrEmpty(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue))
                elementTexture = EditorGUIUtility.Load(Type.GetType(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue).Name + "@4x");
            if (elementTexture == null)
                elementTexture = EditorGUIUtility.Load("VisualElement@4x");
            button.Q<Image>().image = elementTexture as Texture;
            if (!EditorGUIUtility.isProSkin) button.Q<Image>(name: "foldout-element-image").tintColor = Color.gray;

            if (hierarchy.arraySize > 0 && !string.IsNullOrEmpty(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue))
            {
                button.AddToClassList("with_name");
                button.text = "#" + hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue;
            }
            else if (hierarchy.arraySize > 0 && !string.IsNullOrEmpty(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue))
                button.text = Type.GetType(hierarchy.GetArrayElementAtIndex(0).FindPropertyRelative("Type").stringValue).Name;
            else
            {
                button.text = "Select element";
            }
        }
    }
}
