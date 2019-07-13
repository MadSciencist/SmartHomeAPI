using System;
using System.Linq;
using System.Reflection;

namespace SmartHome.Core.Infrastructure.AssemblyScanning
{
    public static class AssemblyUtils
    {
        public static T GetAttribute<T>(this Assembly asm) where T : Attribute
            => GetAttributes<T>(asm).SingleOrDefault();

        public static T GetAttribute<T>(this Type type) where T : Attribute 
            => GetAttributes<T>(type).SingleOrDefault();

        public static T[] GetAttributes<T>(this Type type) where T : Attribute
        {
            object[] attributes = type.GetCustomAttributes(typeof(T), false);
            if (attributes == null || attributes.Length == 0)
                return null;
            return attributes.OfType<T>().ToArray();
        }

        public static T[] GetAttributes<T>(this Assembly asm) where T : Attribute
        {
            object[] attributes = asm.GetCustomAttributes(typeof(T), false);
            if (attributes == null || attributes.Length == 0)
                return null;
            return attributes.OfType<T>().ToArray();
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

        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
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
