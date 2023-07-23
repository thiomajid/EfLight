namespace EfLight.Utils;


/// <summary>
/// A wrapper around database read operations to handle cases where results may be null.
/// </summary>
public sealed class Optional<TResult>
{
    private readonly TResult _value;


    /// <summary>
    /// Indicates if the <see cref="Value"/> is not null.
    /// </summary>
    public bool HasValue { get => _value is not null; }


    /// <summary>
    /// Returns the value held in the <see cref="Optional{T}"/>
    /// </summary>
    /// <throws><see cref="NullReferenceException"/> when the <see cref="Value"/> is null.</throws>
    public TResult Value
    {
        get => HasValue ?
            _value! :
            throw new NullReferenceException($"The optional of type {nameof(TResult)} has no value");
    }

    #region Constructor
    private Optional(TResult value) => _value = value;
    #endregion


    #region Builder functions
    /// <summary>
    /// Creates an <see cref="Optional{TResult}"/> from a given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">A value used to create the <see cref="Optional{TResult}"/></param>
    public static Optional<TResult> Of(TResult value) => new(value);

#pragma warning disable CS8604 // Existence possible d'un argument de référence null.
    /// <summary>
    /// Creates an <see cref="Optional{TResult}"/> from a given <strong>nullable</strong> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">A nullable value used to create the <see cref="Optional{TResult}"/></param>
    public static Optional<TResult> OfNullable(TResult? value) => new(value);
#pragma warning restore CS8604 // Existence possible d'un argument de référence null.
    #endregion


    /// <summary>
    /// If the current value held by the <see cref="Optional{T}"/> is null
    /// then the <paramref name="fallback"/> value will be returned.
    /// </summary>
    /// <param name="fallback"></param>
    public TResult OrElse(TResult fallback) => _value ?? fallback;


    /// <summary>
    /// Returns the held <see cref="Value"/> if it is not null. Otherwise,
    /// the provided <paramref name="exceptionSupplier"/> will throw an exception.
    /// </summary>
    /// <param name="exceptionSupplier">
    /// An action that will throw an exception if the <see cref="Value"/> 
    /// is null.
    /// </param>
    public TResult OrElseThrow(Func<Exception> exceptionSupplier)
    {
        if (HasValue) return Value;
        throw exceptionSupplier.Invoke();
    }


    /// <summary>
    /// Returns the held <see cref="Value"/> if it is not null. Otherwise,
    /// the provided <paramref name="callbackAction"/> will be executed.
    /// </summary>
    /// <param name="exceptionSupplier">
    /// A callback to execute if the <see cref="Value"/> 
    /// is null.
    /// </param>
    public TResult IfNullThen(Action callbackAction)
    {
        if (HasValue) return Value;
        callbackAction.Invoke();

#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
        return default;
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
    }
}