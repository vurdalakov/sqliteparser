// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class ObjectExtensions
    {
        public static Object GetEventHandler(this Object obj, String eventName)
        {
            var field = obj.GetType().GetEventField(eventName);
            return field?.GetValue(obj);
        }

        public static void SetEventHandler(this Object obj, String eventName, Object value)
        {
            var field = obj.GetType().GetEventField(eventName);
            field?.SetValue(obj, value);
        }

        public static void ClearEventHandle(this Object obj, String eventName)
        {
            SetEventHandler(obj, eventName, null);
        }

        public static void ClearAllEventHandlers(this Object obj)
        {
            foreach (var fieldName in GetEventHandlerNames(obj))
            {
                ClearEventHandle(obj, fieldName);
            }
        }

        public static String[] GetEventHandlerNames(this Object obj)
        {
            var handlerNames = new List<String>();

            foreach (var field in obj.GetType().GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                handlerNames.Add(field.Name);
            }

            return handlerNames.ToArray();
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
