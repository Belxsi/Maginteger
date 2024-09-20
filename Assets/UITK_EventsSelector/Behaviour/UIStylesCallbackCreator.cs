using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIEvents
{
    [RequireComponent(typeof(UIDocument))]
    [AddComponentMenu("UI Toolkit/UI Styles Callback Creator")]
    public class UIStylesCallbackCreator : MonoBehaviour
    {
        [SerializeField] internal List<DynamicUICallback> callbacks = new List<DynamicUICallback>();
        private UIDocument document;

        private Dictionary<string, Action> readyCallbacks;

        private void Awake()
        {
            document = GetComponent<UIDocument>();

            readyCallbacks = new Dictionary<string, Action>();
            foreach(var callback in callbacks)
            {
                Action action = null;
                foreach(var element in callback.CallbackElements)
                {
                    foreach(var c in element.Callbacks)
                    {
                        var method = this.GetType().GetMethod(c.MethodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                        var parameterType = Type.GetType(c.ParamType);

                        if (parameterType == typeof(string))
                        {
                            var del = method.CreateDelegate(typeof(Action<string, VisualElementInfo>), this);
                            action += () => del.DynamicInvoke(c.StringValue, element.Info);
                        }
                        else if (parameterType == typeof(int))
                        {
                            var del = method.CreateDelegate(typeof(Action<int, VisualElementInfo>), this);
                            action += () => del.DynamicInvoke(c.IntValue, element.Info);
                        }
                        else if (parameterType == typeof(float))
                        {
                            var del = method.CreateDelegate(typeof(Action<float, VisualElementInfo>), this);
                            action += () => del.DynamicInvoke(c.FloatValue, element.Info);
                        }
                        else if (parameterType == typeof(Color))
                        {
                            var del = method.CreateDelegate(typeof(Action<Color, VisualElementInfo>), this);
                            action += () => del.DynamicInvoke(c.ColorValue, element.Info);
                        }
                        else if (parameterType.IsEnum)
                        {
                            Array enums = Enum.GetValues(parameterType);
                            Enum value = enums.GetValue(Mathf.Clamp(c.EnumValue, 0, enums.Length - 1)) as Enum;
                            var deltype = typeof(Action<,>).MakeGenericType(parameterType, typeof(VisualElementInfo));
                            var del = method.CreateDelegate(deltype, this);
                            action += () => del.DynamicInvoke(value, element.Info);
                        }
                        else
                        {
                            Debug.LogWarning($"Could not add \"{c.MethodName}\" to \"{callback.CallbackName}\" callback");
                        }
                    }
                }
                readyCallbacks.Add(callback.CallbackName, action);
            }
        }
        public void ExecuteCallback(string callbackName)
        {
            readyCallbacks[callbackName]?.Invoke();
        }


        #region methods
        private void ToggleInClass(string style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.ToggleInClassList(style);
            }
        }
        private void SetAlignContent(Align style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.alignContent = style;
            }
        }
        private void SetAlignItems(Align style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.alignItems = style;
            }
        }
        private void SetAlignSelf(Align style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.alignSelf = style;
            }
        }
        private void SetBackgroundColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.backgroundColor = style;
            }
        }
        private void SetBorderColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomColor = style;
                element.style.borderTopColor = style;
                element.style.borderRightColor = style;
                element.style.borderLeftColor = style;
            }
        }
        private void SetBorderLeftColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderLeftColor = style;
            }
        }
        private void SetBorderRightColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderRightColor = style;
            }
        }
        private void SetBorderTopColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderTopColor = style;
            }
        }
        private void SetBorderBottomColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomColor = style;
            }
        }
        private void SetBorderWidth(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomWidth = style;
                element.style.borderTopWidth = style;
                element.style.borderLeftWidth = style;
                element.style.borderRightWidth = style;
            }
        }
        private void SetBorderWidthLeft(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderLeftWidth = style;
            }
        }
        private void SetBorderWidthRight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderRightWidth = style;
            }
        }
        private void SetBorderWidthTop(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderTopWidth = style;
            }
        }
        private void SetBorderWidthBottom(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomWidth = style;
            }
        }
        private void SetBorderRadius(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomLeftRadius = style;
                element.style.borderBottomRightRadius = style;
                element.style.borderTopLeftRadius = style;
                element.style.borderTopRightRadius = style;
            }
        }
        private void SetBorderRadiusBottomLeft(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomLeftRadius = style;
            }
        }
        private void SetBorderRadiusBottomRight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderBottomRightRadius = style;
            }
        }
        private void SetBorderRadiusTopLeft(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderTopLeftRadius = style;
            }
        }
        private void SetBorderRadiusTopRight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.borderTopRightRadius = style;
            }
        }
        private void SetColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.color = style;
            }
        }
        private void SetFlexBasis(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.flexBasis = style;
            }
        }
        private void SetFlexDirection(FlexDirection style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.flexDirection = style;
            }
        }
        private void SetFlexGrow(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.flexGrow = style;
            }
        }
        private void SetFlexShrink(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.flexShrink = style;
            }
        }
        private void SetFlexWrap(Wrap style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.flexWrap = style;
            }
        }
        private void SetFontSize(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.fontSize = style;
            }
        }
        private void SetHeight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.height = style;
            }
        }
        private void SetHeightPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.height = Length.Percent(style);
            }
        }
        private void SetJustifyContent(Justify style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.justifyContent = style;
            }
        }
        private void SetLeft(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.left = style;
            }
        }
        private void SetLeftPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.left = Length.Percent(style);
            }
        }
        private void SetLetterSpacing(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.letterSpacing = style;
            }
        }
        private void SetMargin(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.marginBottom = style;
                element.style.marginTop = style;
                element.style.marginLeft = style;
                element.style.marginRight = style;
            }
        }
        private void SetMarginLeft(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.marginLeft = style;
            }
        }
        private void SetMarginRight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.marginRight = style;
            }
        }
        private void SetMarginTop(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.marginTop = style;
            }
        }
        private void SetMarginBottom(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.marginBottom = style;
            }
        }
        private void SetMaxHeight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.maxHeight = style;
            }
        }
        private void SetMaxHeightPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.maxHeight = Length.Percent(style);
            }
        }
        private void SetMaxWidth(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.maxWidth = style;
            }
        }
        private void SetMaxWidthPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.maxWidth = Length.Percent(style);
            }
        }
        private void SetMinHeight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.minHeight = style;
            }
        }
        private void SetMinHeightPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.minHeight = Length.Percent(style);
            }
        }
        private void SetMinWidth(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.minWidth = style;
            }
        }
        private void SetMinWidthPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.minWidth = Length.Percent(style);
            }
        }
        private void SetOpacity(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.opacity = style;
            }
        }
        private void SetOverflow(Overflow style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.overflow = style;
            }
        }
        private void SetPadding(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.paddingBottom = style;
                element.style.paddingTop = style;
                element.style.paddingLeft = style;
                element.style.paddingRight = style;
            }
        }
        private void SetPaddingLeft(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.paddingLeft = style;
            }
        }
        private void SetPaddingRight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.paddingRight = style;
            }
        }
        private void SetPaddingTop(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.paddingTop = style;
            }
        }
        private void SetPaddingBottom(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.paddingBottom = style;
            }
        }
        private void SetPosition(Position style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.position = style;
            }
        }
        private void SetRight(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.right = style;
            }
        }
        private void SetRightPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.right = Length.Percent(style);
            }
        }
        private void SetTextOverflow(TextOverflow style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.textOverflow = style;
            }
        }
        private void SetTop(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.top = style;
            }
        }
        private void SetTopPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.top = Length.Percent(style);
            }
        }
        private void SetBackgroundImageTintColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityBackgroundImageTintColor = style;
            }
        }
        private void SetFontStyle(FontStyle style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityFontStyleAndWeight = style;
            }
        }
        private void SetOverflowClipbox(OverflowClipBox style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityOverflowClipBox = style;
            }
        }
        private void SetParagraphSpacing(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityParagraphSpacing = style;
            }
        }
        private void SetBackgroundImageSlice(int style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unitySliceLeft = style;
                element.style.unitySliceRight = style;
                element.style.unitySliceTop = style;
                element.style.unitySliceBottom = style;
            }
        }
        private void SetBackgroundImageSliceLeft(int style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unitySliceLeft = style;
            }
        }
        private void SetBackgroundImageSliceRight(int style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unitySliceRight = style;
            }
        }
        private void SetBackgroundImageSliceTop(int style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unitySliceTop = style;
            }
        }
        private void SetBackgroundImageSliceBottom(int style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unitySliceBottom = style;
            }
        }
        private void SetBackgroundImageSliceScale(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unitySliceScale = style;
            }
        }
        private void SetTextAlign(TextAnchor style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityTextAlign = style;
            }
        }
        private void SetTextOutlineColor(Color style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityTextOutlineColor = style;
            }
        }
        private void SetTextOutlineWidth(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityTextOutlineWidth = style;
            }
        }
        private void SetTextOverflowPosition(TextOverflowPosition style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.unityTextOverflowPosition = style;
            }
        }
        private void SetVisibility(Visibility style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.visibility = style;
            }
        }
        private void SetWhiteSpace(WhiteSpace style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.whiteSpace = style;
            }
        }
        private void SetWidth(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.width = style;
            }
        }
        private void SetWidthPercent(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.width = Length.Percent(style);
            }
        }
        private void SetWordSpacing(float style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.wordSpacing = style;
            }
        }
        private void SetName(string style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.name = style;
            }
        }
        private void AddToClassList(string className, VisualElementInfo info)
        {
            foreach(var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.AddToClassList(className);
            }
        }
        private void RemoveFromClassList(string className, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.RemoveFromClassList(className);
            }
        }
        private void SetDisplayStyle(DisplayStyle style, VisualElementInfo info)
        {
            foreach (var element in UIEventsSelector.QueryElement(info, document.rootVisualElement))
            {
                element.style.display = style;
            }
        }
        #endregion
    }

    [Serializable]
    internal class DynamicUICallback
    {
        public string CallbackName;
        public List<UICallbackElement> CallbackElements = new() { new()};
        public DynamicUICallback() { }
        public DynamicUICallback(string name) { CallbackName = name; }
    }
    [Serializable]
    internal class UICallbackElement
    {
        public VisualElementInfo Info;
        public List<UICallback> Callbacks = new() { new()};
    }
    [Serializable]
    internal class UICallback
    {
        public string MethodName = "AddToClassList";
        public string ParamType = typeof(string).AssemblyQualifiedName;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public int EnumValue;
        public Color ColorValue;
    }
}
