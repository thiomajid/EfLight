using LanguageExt;

namespace EfLight.Extensions;

public static class Object
{
    /// <summary>
    /// Converts a nullable value into an <see cref="T:LanguageExt.Option`1"/> 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? value) where T : class
    {
        return value is null ? Option<T>.None : Option<T>.Some(value);
    }
}