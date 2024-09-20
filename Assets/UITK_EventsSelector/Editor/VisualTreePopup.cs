using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    public class VisualTreePopup : PopupWindowContent
    {
        VisualElement treeRoot;
        public Action<object> onSelect;
        public List<int> hierarchyInfo;
        float width;

        public VisualTreePopup(VisualElement root)
        {
            treeRoot = root;
            System.Type inspectorType = System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor");
            width = EditorWindow.GetWindow(inspectorType).position.width;
        }

        public override void OnGUI(Rect rect)
        {
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(width, Mathf.Min(editorWindow.rootVisualElement.Q<VisualElement>(name: "content-container").resolvedStyle.height + 40, 250));
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

            ScrollView scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            scrollView.StretchToParentSize();
            scrollView.style.paddingBottom = 5;
            scrollView.style.paddingTop = 2;
            rootElement.Add(scrollView);
            DrawHierarchy(treeRoot, scrollView, true, 0);

            if(hierarchyInfo != null && hierarchyInfo.Count > 0)
            {
                CustomFoldout current = scrollView[0] as CustomFoldout;
                for (int i = hierarchyInfo.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        current.value = true;
                        current = current.content[hierarchyInfo[i]] as CustomFoldout;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                scrollView.schedule.Execute(() =>
                {
                    current.titleContainer.Focus();
                });
            }
        }

        private void DrawHierarchy(VisualElement target, VisualElement parent, bool fold, float indent)
        {
            CustomFoldout foldout = new CustomFoldout(fold, indent);
            if (parent is CustomFoldout)
                (parent as CustomFoldout).Add(foldout);
            else
                parent.Add(foldout);

            var elementImage = new Image() { name = "visual-element-image" };
            var elementTexture = EditorGUIUtility.Load(target.GetType().Name + "@4x");
            if (elementTexture == null) elementTexture = EditorGUIUtility.Load("VisualElement@4x");
            elementImage.image = elementTexture as Texture;

            foldout.titleContainer.Add(elementImage);
            foldout.titleContainer.Add(new Label((string.IsNullOrEmpty(target.name)) ? target.GetType().Name : "#" + target.name));
            if (!string.IsNullOrEmpty(target.name)) foldout.titleContainer.AddToClassList("with_name");
            if (target.childCount == 0) foldout.titleContainer.Query<Image>(name: "arrow").First().visible = false;

            foldout.titleContainer.userData = target;
            foldout.titleContainer.RegisterCallback<PointerDownEvent>(e =>
            {
                if (e.clickCount > 1)
                {
                    onSelect?.Invoke(foldout.titleContainer.userData);
                    editorWindow.Close();
                }
            });

            foreach (VisualElement child in target.hierarchy.Children())
            {
                DrawHierarchy(child, foldout, false, indent + 15);
            }

        }
    }

    public class CustomFoldout : VisualElement
    {
        private Texture expandedTex;
        private Texture collapsedTex;
        private Texture expandedFocusTex;
        private Texture collapsedFocusTex;

        private bool _value = true;
        public bool value { get => _value; set { _value = value; ToggleFoldout(); } }

        public VisualElement titleContainer { get; private set; }

        private Image foldArrow;
        public VisualElement content { get; private set; }

        public CustomFoldout(bool value, float indent)
        {
            _value = value;
            expandedTex = EditorGUIUtility.Load("IN foldout on") as Texture;
            collapsedTex = EditorGUIUtility.Load("IN foldout") as Texture;
            expandedFocusTex = EditorGUIUtility.Load("IN foldout focus on") as Texture;
            collapsedFocusTex = EditorGUIUtility.Load("IN foldout focus") as Texture;

            content = new VisualElement() { name = "content-container" };
            titleContainer = new VisualElement() { name = "title-container", focusable = true };
            titleContainer.style.flexDirection = FlexDirection.Row;
            titleContainer.style.paddingLeft = indent;
            foldArrow = new Image() { image = _value ? expandedTex : collapsedTex, name = "arrow" };
            foldArrow.style.width = 13;
            foldArrow.RegisterCallback<PointerDownEvent>(e => { foldArrow.image = _value ? expandedFocusTex : collapsedFocusTex; e.StopImmediatePropagation(); });
            foldArrow.RegisterCallback<PointerUpEvent>(e => { this.value = !this.value; e.StopImmediatePropagation(); });
            titleContainer.Add(foldArrow);

            Insert(0, titleContainer);
            base.Add(content);

            content.style.display = _value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public new void Add(VisualElement element)
        {
            content.Add(element);
        }

        private void ToggleFoldout()
        {
            content.style.display = _value ? DisplayStyle.Flex : DisplayStyle.None;
            foldArrow.image = _value ? expandedTex : collapsedTex;
        }
    }
}
