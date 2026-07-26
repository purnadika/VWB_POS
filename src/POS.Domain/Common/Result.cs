using System;

namespace POS.Domain.Common;

public class Result
{
    protected Result(bool isSuccess, string error, string? message = null)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException();
        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
        Message = message ?? error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    public string Message { get; }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value, string? message = null) 
        => new(value, true, string.Empty, message);

    public static Result<TValue> Failure<TValue>(string error) 
        => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, string error, string? message = null)
        : base(isSuccess, error, message)
    {
        _value = value;
    }

    public TValue? Value => IsSuccess ? _value : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    // Add non-generic method for compatibility
    public new static Result<TValue> Failure(string error) => new(default, false, error);
}
