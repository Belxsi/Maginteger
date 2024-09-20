This tool is meant to be used with Unitys new UI Toolkit system in order to simplify process of attaching functionality to UI Elements.

MonoBehaviours in this package:
-UIEventsSelector (located at UI Toolkit -> UI Events Selector)
-UIStylesCallbackCreator (located at UI Toolkit -> UI Styles Callback Creator)

All scripts require UI Document component on the same game object.

UIEventsSelector usage:
- Click "Add element" to create entry representing target UI element or query with a set of Unity Events
- Chose between "Select element" or "Build query" to target specific element in a hierarchy or every element matching the requirements specified in a query
- Specify type of event to register a callback in a dropdown menu
- Add listeners to that event
- By right clicking foldout header or event type dropdown you can open contextual menu allowing you to move, delete or add elements
- In order to register callbacks at runtime call UIEventsSelector.ReapplyCallbacks()

UIStylesCallbackCreator usage:
- Click "Add element" to create entry representing sets of targeted UI elements or queries with methods to call
- Provide name for that callback in foldouts header
- Chose between "Select element" or "Build query" to target specific element in a hierarchy or every element matching the requirements specified in a query
- Add methods to call and provide a parameter for them, like number, color or some Enum value
- By right clicking foldout header or "Select Element/Build query" box you can open contextual menu allowing you to move, delete or add elements
- Execute given callback by calling UIStylesCallbackCreator.ExecuteCallback() and passing in callbacks name as a string parameter

Custom pseudo-events:
Some events in UIEventsSelector may not work due to being filtered based on its parameters which this tool discards, like for example ContextClickEvent being a MouseDownEvent
with a button value of 1 (right mouse button). To create your own variant of an event filtered based on its parameters that UIEventsSelector can actually use, follow these steps:
- Create a class extending CustomUIEventBase<T> (located in UIEvents namespace) where T is any type of UI event, anywhere in your project
- Provide a constructor that will call base constructor of CustomUIEventBase
- Provide an implementation for an abstract method ExtractInfo() that will return bool value indicating if that event should execute associated callbacks

Here is an example of creating custom pseudo-event that will work just like ContextualClickEvent (already implemented):

    public class MouseRightClickEvent : CustomUIEventBase<MouseDownEvent>
    {
        public MouseRightClickEvent(Action callback, VisualElement element) : base(callback, element) { }

        protected override bool ExtractInfo(MouseDownEvent eventType)
        {
            return eventType.button == 1;
        }
    }

For more detailed manual visit gogzydev.mynotice.io