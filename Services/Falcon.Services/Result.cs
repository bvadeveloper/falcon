using System;
using System.Threading.Tasks;

namespace Falcon.Services
{
    public interface IResult
    {
        bool Succeed { get; set; }

        Exception ProcessedException { get; set; }
    }

    public interface IResult<TResult> : IResult
    {
        TResult Value { get; set; }
    }

    public class Result : IResult
    {
        public bool Succeed { get; set; }

        public Exception ProcessedException { get; set; }
    }

    public class Result<TValue> : Result, IResult<TValue>
    {
        public TValue Value { get; set; }

        public Result<TValue> SetResult(TValue value)
        {
            Value = value;
            return this;
        }

        public Result<TValue> Ok()
        {
            Succeed = true;
            return this;
        }
    }

    public static class ResultExtensions
    {
        public static async Task<T> Extract<T>(this Task<Result<T>> task)
        {
            return (await task).Value;
        }

        public static Result<T> SetResult1<T>(this Result<T> @this, T value)
        {
            @this.Value = value;
            return @this;
        }
    }
}