namespace WatchCat.Core.Common;

/// <summary>
/// Represents the result of an operation, which may produce data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data that the result may contain.</typeparam>
public class Result<T> : IResult<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the data produced by the operation. This can be <see langword="null"/> if the operation was not successful.
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// Gets a message that provides additional information about the result. This can be <see langword="null"/>.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="isSuccess">A value indicating whether the operation was successful.</param>
    /// <param name="data">The data produced by the operation. This can be <see langword="null"/>.</param>
    /// <param name="message">A message that provides additional information about the result. This can be <see langword="null"/>.</param>
    public Result(bool isSuccess, T data = default, string message = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Result{T}"/> class representing a successful operation with the specified data.
    /// </summary>
    /// <param name="data">The data produced by the operation.</param>
    /// <returns>A <see cref="Result{T}"/> instance representing a successful operation.</returns>
    public static Result<T> Success(T data = default)
    {
        return new Result<T>(true, data);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Result{T}"/> class representing a failed operation with the specified message.
    /// </summary>
    /// <param name="message">A message that provides additional information about the result.</param>
    /// <returns>A <see cref="Result{T}"/> instance representing a failed operation.</returns>
    public static Result<T> Failure(string message = null)
    {
        return new Result<T>(false, default, message);
    }
}

/// <summary>
/// Represents the result of an operation that can produce data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the data that the result may contain.</typeparam>
public interface IResult<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets the data produced by the operation. This can be <see langword="null"/> if the operation was not successful.
    /// </summary>
    T Data { get; }

    /// <summary>
    /// Gets a message that provides additional information about the result. This can be <see langword="null"/>.
    /// </summary>
    string Message { get; }
}
