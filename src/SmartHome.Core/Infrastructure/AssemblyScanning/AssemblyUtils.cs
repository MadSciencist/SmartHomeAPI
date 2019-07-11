using System;
using System.Collections.Generic;
using System.IO;

namespace SmartHome.Core.Infrastructure.AssemblyScanning
{
    public static class AssemblyUtils
    {

        public static T[] GetAttributes<T>(this Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(typeof(T), false);
            return customAttributes as T[];
        }

        public static bool IsAssignableToGenericType(Type genericType, Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
