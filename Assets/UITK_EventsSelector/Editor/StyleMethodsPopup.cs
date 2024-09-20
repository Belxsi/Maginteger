using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public class StyleMethodsPopup : PopupWindowContent
    {
        public Action<object> onSelect;
        float width;
        List<MethodInfo> MethodInfos;

        public StyleMethodsPopup()
        {
            MethodInfos = typeof(UIStylesCallbackCreator)
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType == typeof(VisualElementInfo) && m.ReturnType == typeof(void)).ToList();
            MethodInfos.Sort((m,n)=>m.Name.CompareTo(n.Name));
            System.Type inspectorType = System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor");
            width = EditorWindow.GetWindow(inspectorType).position.width;
        }

        public override void OnGUI(Rect rect)
        {
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(width, 250);
        }

        public override void OnOpen()
        {
            System.Type inspectorType = System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor");
            width = EditorWindow.GetWindow(inspectorType).position.width;
            VisualElement rootElement = editorWindow.rootVisualElement;
            rootElement.name = "root-element";


            StyleSheet styles = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                UITK_EditorHelperMethods.GetPathRelativeTo($"t:Script {this.GetType().Name}", "EditorStyles/PopupStyles.uss")
                );
            rootElement.styleSheets.Add(styles);

            VisualElement topBar = new VisualElement() { name = "search-bar" };
            TextField search = new TextField() { name = "search-field" };
            SetPlaceholderText(search, "search...");
            topBar.Add(search);
            rootElement.Add(topBar);

            ScrollView scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            scrollView.style.paddingBottom = 5;
            scrollView.style.paddingTop = 2;
            rootElement.Add(scrollView);
            FillScrollView(scrollView, MethodInfos);

            search.RegisterValueChangedCallback(e =>
            {
                string placeholderClass = TextField.ussClassName + "__placeholder";
                if (string.IsNullOrEmpty(e.newValue) || search.ClassListContains(placeholderClass))
                {
                    FillScrollView(scrollView, MethodInfos);
                }
                else
                {
                    FillScrollView(scrollView, MethodInfos.Where(n => n.Name.ToLower().Contains(e.newValue.ToLower())).ToList());
                }
            });
            rootElement.schedule.Execute(() =>
            {
                search.Q<VisualElement>(name: "unity-text-input").Focus();
            });
        }

        private void FillScrollView(ScrollView scrollView, List<MethodInfo> infos)
        {
            scrollView.Clear();
            foreach (MethodInfo i in infos)
            {
                VisualElement row = new VisualElement() { name = "title-container", focusable = true };
                row.style.flexDirection = FlexDirection.Row;
                row.userData = i;

                row.Add(new Label(SplitWords(i.Name)));

                scrollView.Add(row);

                row.RegisterCallback<PointerDownEvent>(e =>
                {
                    if (e.clickCount > 1)
                    {
                        onSelect?.Invoke(row.userData);
                        editorWindow.Close();
                    }
                });
            }
        }

        private void SetPlaceholderText(TextField textField, string placeholder)
        {
            string placeholderClass = TextField.ussClassName + "__placeholder";

            onFocusOut();
            textField.RegisterCallback<FocusInEvent>(evt => onFocusIn());
            textField.RegisterCallback<FocusOutEvent>(evt => onFocusOut());

            void onFocusIn()
            {
                if (textField.ClassListContains(placeholderClass))
                {
                    textField.value = string.Empty;
                    textField.RemoveFromClassList(placeholderClass);
                }
            }

            void onFocusOut()
            {
                if (string.IsNullOrEmpty(textField.text))
                {
                    textField.SetValueWithoutNotify(placeholder);
                    textField.AddToClassList(placeholderClass);
                }
            }
        }

        public string SplitWords(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder result = new StringBuilder();
            result.Append(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                char currentChar = input[i];

                if (char.IsUpper(currentChar) && !char.IsWhiteSpace(input[i - 1]))
                {
                    result.Append(' ');
                }

                result.Append(currentChar);
            }

            return result.ToString();
        }
    }
}
