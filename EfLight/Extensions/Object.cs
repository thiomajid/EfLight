using LanguageExt;

namespace EfLight.Extensions;

public static class Object
{
    public static Option<T> ToOption<T>(this T? value) where T : class
    {
        return value is null ? Option<T>.None : Option<T>.Some(value);
    }
}