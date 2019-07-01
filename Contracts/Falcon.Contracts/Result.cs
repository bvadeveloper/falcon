using System;
using System.Threading.Tasks;

namespace Falcon.Contracts
{
    public interface IResult
    {
        bool Succeed();

        Exception ProcessedException { get; set; }
    }

    public interface IResult<TResult> : IResult
    {
        TResult Value { get; set; }
    }

    public class Result : IResult
    {
        public bool Succeed()
        {
            return ProcessedException == null;
        }

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
    }
    
    public static class ResultExtensions
    {
        public static async Task<T> ExtractValue<T>(this Task<Result<T>> task)
        {
            return (await task).Value;
        }
    }
}