namespace Application.Common;

public class Result<T>
{
    public bool Success { get; init; } = true;
    public T? Data { get; init; }
    public string? Message { get; init; }

    public static Result<T> Ok(T data)
        => new() { Data = data, Success = true };

    public static Result<T> Fail(string msg)
        => new() { Success = false, Message = msg };
}
