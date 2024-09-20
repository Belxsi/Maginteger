using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public class ElementClassesPopup : PopupWindowContent
    {
        public Action<object> onSelect;
        public string originValue;
        float width;
        List<string> VisualElementClasses;

        public ElementClassesPopup(VisualElement root)
        {
            VisualElementClasses = new List<string>();
            GetAllClasses(VisualElementClasses, root);
            VisualElementClasses.Sort();
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

            var infoLabel = new Label("To append class hold Ctrl key");
            infoLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            rootElement.Add(infoLabel);

            VisualElement topBar = new VisualElement() { name = "search-bar" };
            TextField search = new TextField() { name = "search-field" };
            SetPlaceholderText(search, "search...");
            topBar.Add(search);
            rootElement.Add(topBar);

            ScrollView scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            scrollView.style.paddingBottom = 5;
            scrollView.style.paddingTop = 2;
            rootElement.Add(scrollView);
            FillScrollView(scrollView, VisualElementClasses);

            search.RegisterValueChangedCallback(e =>
            {
                string placeholderClass = TextField.ussClassName + "__placeholder";
                if (string.IsNullOrEmpty(e.newValue) || search.ClassListContains(placeholderClass))
                {
                    FillScrollView(scrollView, VisualElementClasses);
                }
                else
                {
                    FillScrollView(scrollView, VisualElementClasses.Where(n => n.ToLower().Contains(e.newValue.ToLower())).ToList());
                }
            });
            rootElement.schedule.Execute(() =>
            {
                search.Q<VisualElement>(name: "unity-text-input").Focus();
            });
        }

        private void FillScrollView(ScrollView scrollView, List<string> classes)
        {
            scrollView.Clear();
            foreach (string c in classes)
            {
                VisualElement row = new VisualElement() { name = "title-container", focusable = true };
                row.style.flexDirection = FlexDirection.Row;
                row.userData = c;

                row.Add(new Label("." + c));

                scrollView.Add(row);

                row.RegisterCallback<PointerDownEvent>(e =>
                {
                    if (e.clickCount > 1)
                    {
                        onSelect?.Invoke(e.ctrlKey ? (originValue.Trim(' ').Trim(',').Trim(' ')+", "+row.userData) : row.userData);
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

        private void GetAllClasses(List<string> classNames, VisualElement root)
        {
            foreach (string className in root.GetClasses())
            {
                if (!classNames.Contains(className))
                {
                    classNames.Add(className);
                }
            }

            foreach (VisualElement child in root.hierarchy.Children())
            {
                GetAllClasses(classNames, child);
            }
        }
    }
}
