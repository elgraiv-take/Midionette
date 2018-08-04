using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MidionetteSampleGui.Utility
{
    class ActionEventBehavior: Behavior<UIElement>
    {
        private static readonly MethodInfo s_invokeMethod= typeof(ActionEventBehavior).GetMethod(nameof(InvokeCommand), BindingFlags.NonPublic | BindingFlags.Instance);

        public ActionEventBehavior()
        {
        }
        public object Argument
        {
            get { return GetValue(s_argumentProperty); }
            set { SetValue(s_argumentProperty, value); }
        }
        public string EventName
        {
            get { return GetValue(s_eventNameProperty) as string ?? string.Empty; }
            set { SetValue(s_eventNameProperty, value); }
        }
        public ICommand Command
        {
            get { return GetValue(s_commandProperty) as ICommand; }
            set { SetValue(s_commandProperty, value); }
        }


        private static readonly DependencyProperty s_argumentProperty =
            DependencyProperty.Register("Argument", typeof(object), typeof(ActionEventBehavior), new UIPropertyMetadata(null));

        private static readonly DependencyProperty s_eventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(ActionEventBehavior), new UIPropertyMetadata(null));

        private static readonly DependencyProperty s_commandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ActionEventBehavior), new UIPropertyMetadata(null));

        private EventInfo _targetEvent;
        private Delegate _attachedAction;
        protected override void OnAttached()
        {
            base.OnAttached();
            _targetEvent = AssociatedObject.GetType().GetEvent(EventName);
            _attachedAction = s_invokeMethod.CreateDelegate(_targetEvent.EventHandlerType, this);
            _targetEvent?.AddEventHandler(AssociatedObject, _attachedAction);
        }

        private void InvokeCommand(object sender, object e)
        {
            Command?.Execute(Argument);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _targetEvent?.RemoveEventHandler(AssociatedObject, _attachedAction);
        }
    }
}
