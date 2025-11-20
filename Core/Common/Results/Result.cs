using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Common.Results
{
    public static class Result
    {
        public static Success Success { get; set; }
        public static Created Created { get; set; }
        public static Deleted Deleted { get; set; }
        public static Updated Updated { get; set; }

        // it marker for non body responses --> For more Expressive instead of using Void or Unit - bool and so on .. 
        // usage : return Result.FromValue(Result.Created); 

    }

    #region IResult Interface & Implementation
    public interface IResult<TValue>
    {
        bool IsSuccess { get; }
        bool IsError { get; }
        List<Error> Errors { get; }
        TValue Value { get; }
        Error TopError { get; }
        TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<List<Error>, TNextValue> onError);
    }

    public sealed class Result<TValue> : IResult<TValue>
    {
        private readonly TValue? _value = default;
        private readonly List<Error>? _errors = null;

        public bool IsSuccess { get; }

        [JsonConstructor]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("For serializer only.", true)]
        public Result(TValue? value, List<Error>? errors, bool isSuccess)
        {
            if (isSuccess)
            {
                _value = value ?? throw new ArgumentNullException(nameof(value));
                _errors = new List<Error>();
                IsSuccess = true;
            }
            else
            {
                if (errors == null || errors.Count == 0)
                {
                    throw new ArgumentException("Provide at least one error.", nameof(errors));
                }

                _errors = errors;
                _value = default!;
                IsSuccess = false;
            }
        }

        private Result(Error error)
        {
            _errors = new List<Error> { error };
            IsSuccess = false;
        }

        private Result(List<Error> errors)
        {
            if (errors is null || errors.Count == 0)
            {
                throw new ArgumentException("Cannot create a Result<TValue> from an empty collection of errors. Provide at least one error.", nameof(errors));
            }

            _errors = errors;
            IsSuccess = false;
        }

        private Result(TValue value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _value = value;
            _errors = new List<Error>();
            IsSuccess = true;
        }

        public bool IsError => !IsSuccess;

        public List<Error> Errors => IsError ? _errors! : new List<Error>();

        [JsonIgnore]
        public TValue Value
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("Cannot access Value when IsSuccess is false.");
                }
                return _value!;
            }
        }

        public Error TopError => (_errors?.Count > 0) ? _errors[0] : default;

        public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue,
            Func<List<Error>, TNextValue> onError)
        {
            if (onValue == null) throw new ArgumentNullException(nameof(onValue));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            return IsSuccess ? onValue(_value!) : onError(_errors!);
        }

        public static implicit operator Result<TValue>(TValue value)
            => new Result<TValue>(value);

        public static implicit operator Result<TValue>(Error error)
            => new Result<TValue>(error);

        public static implicit operator Result<TValue>(List<Error> errors)
            => new Result<TValue>(errors);


        public static Result<TValue> FromValue(TValue value) => new Result<TValue>(value);
        public static Result<TValue> FromError(Error error) => new Result<TValue>(error);
        public static Result<TValue> FromErrors(List<Error> errors) => new Result<TValue>(errors);
    }
    #endregion

    #region Result Types
    /// <summary>
    ///     For Team 
    ///     equal , gethashcode .. 
    ///     it not carry any data , it is just a marker to indicate success
    ///     and use with Event Type 
    ///     
    /// </summary>
    public readonly struct Success : IEquatable<Success>
    {
        public bool Equals(Success other) => true;
        public override bool Equals(object obj) => obj is Success;
        public override int GetHashCode() => 0;
        public static bool operator ==(Success left, Success right) => true;
        public static bool operator !=(Success left, Success right) => false;
    }

    public readonly struct Created : IEquatable<Created>
    {
        public bool Equals(Created other) => true;
        public override bool Equals(object obj) => obj is Created;
        public override int GetHashCode() => 0;
        public static bool operator ==(Created left, Created right) => true;
        public static bool operator !=(Created left, Created right) => false;
    }

    public readonly struct Deleted : IEquatable<Deleted>
    {
        public bool Equals(Deleted other) => true;
        public override bool Equals(object obj) => obj is Deleted;
        public override int GetHashCode() => 0;
        public static bool operator ==(Deleted left, Deleted right) => true;
        public static bool operator !=(Deleted left, Deleted right) => false;
    }

    public readonly struct Updated : IEquatable<Updated>
    {
        public bool Equals(Updated other) => true;
        public override bool Equals(object obj) => obj is Updated;
        public override int GetHashCode() => 0;
        public static bool operator ==(Updated left, Updated right) => true;
        public static bool operator !=(Updated left, Updated right) => false;
    }
    #endregion
}
