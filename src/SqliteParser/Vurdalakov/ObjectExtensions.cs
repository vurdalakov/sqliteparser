namespace Vurdalakov
{
    using System;
    using System.Reflection;

    public static class ObjectExtensions
    {
        public static void ClearAllEvents(this Object obj)
        {
            foreach (var field in obj.GetType().GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                ClearEvent(obj, field.Name);
            }
        }

        public static void ClearEvent(this Object obj, String eventName)
        {
            var field = obj.GetType().GetEventField(eventName);
            field?.SetValue(obj, null);
        }

        private static FieldInfo GetEventField(this Type type, String eventName)
        {
            while (type != null)
            {
                // events defined as field
                var field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if ((field != null) && ((typeof(MulticastDelegate) == field.FieldType) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                {
                    return field;
                }

                // events defined as property { add; remove; }
                field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (field != null)
                {
                    return field;
                }

                type = type.BaseType;
            }

            return null;
        }
    }
}
