using System.Reflection;

namespace Elgraiv.Midionette
{
    public class BoolPropertyKeyReceiver : IKeyReceiver
    {
        public string Name { get; }

        private MethodInfo _setter;
        private object _target;

        public BoolPropertyKeyReceiver(string name, object targetObject, string propertyName)
        {
            Name = name;
            var property = targetObject.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new TargetException($"Property \"{property}\" NOT Found in Target Object");
            }
            _setter = property.SetMethod;
            _target = targetObject;
        }

        public void OnKeyDown(byte velocity)
        {
            _setter.Invoke(_target, new object[] { true });
        }

        public void OnKeyUp(byte velocity)
        {
            _setter.Invoke(_target, new object[] { false });
        }
    }
}
