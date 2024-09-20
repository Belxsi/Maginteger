using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public class ElementTypesPopup : PopupWindowContent
    {
        public Action<object> onSelect;
        float width;
        List<Type> VisualElementTypes;

        public ElementTypesPopup()
        {
            VisualElementTypes = TypeCache.GetTypesDerivedFrom<VisualElement>().Where(t=>!t.IsAbstract && !t.IsGenericType).ToList();
            VisualElementTypes.Add(typeof(VisualElement));
            VisualElementTypes.Sort((a, b) => a.Name.CompareTo(b.Name));
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
            TextField search = new TextField() { name = "search-field"};
            SetPlaceholderText(search, "search...");
            topBar.Add(search);
            rootElement.Add(topBar);

            ScrollView scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            scrollView.style.paddingBottom = 5;
            scrollView.style.paddingTop = 2;
            rootElement.Add(scrollView);
            FillScrollView(scrollView, VisualElementTypes);

            search.RegisterValueChangedCallback(e => 
            {
                string placeholderClass = TextField.ussClassName + "__placeholder";
                if (string.IsNullOrEmpty(e.newValue) || search.ClassListContains(placeholderClass))
                {
                    FillScrollView(scrollView, VisualElementTypes);
                }
                else
                {
                    FillScrollView(scrollView, VisualElementTypes.Where(t => t.Name.ToLower().Contains(e.newValue.ToLower())).ToList());
                }
            });
            rootElement.schedule.Execute(() =>
            {
                search.Q<VisualElement>(name: "unity-text-input").Focus();
            });
        }

        private void FillScrollView(ScrollView scrollView, List<Type> types)
        {
            scrollView.Clear();
            foreach (Type t in types)
            {
                VisualElement row = new VisualElement() { name = "title-container", focusable = true };
                row.style.flexDirection = FlexDirection.Row;
                row.userData = t;

                var elementTexture = EditorGUIUtility.Load(t.Name + "@4x");
                if (elementTexture == null) elementTexture = EditorGUIUtility.Load("VisualElement@4x");
                Image image = new Image() { image = elementTexture as Texture, name = "visual-element-image" };
                row.Add(image);

                row.Add(new Label(t.Name));

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
    }

}
