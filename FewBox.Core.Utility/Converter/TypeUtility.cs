using System;

namespace FewBox.Core.Utility.Converter
{
    public static class TypeUtility
    {
        public static T Converte<T>(object input)
        {
            T value = default(T);
            Type inputType = input.GetType();
            Type outputType = typeof(T);
            if(inputType == typeof(Guid) && outputType == typeof(Guid))
            {
                value = (T)Convert.ChangeType(input, typeof(T));
            }
            else if(inputType == typeof(string) && outputType == typeof(Guid))
            {
                value = (T)Convert.ChangeType(new Guid(input as string), typeof(T));
            }
            else if(outputType == typeof(string))
            {
                value = (T)Convert.ChangeType(input.ToString(), typeof(T));
            }
            else
            {
                value = (T)Convert.ChangeType(input, typeof(T));
            }
            return value;
        }
    }
}