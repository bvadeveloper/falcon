using System;
using System.Threading.Tasks;
using Falcon.Profiles;

namespace Falcon.Services.RequestManagement
{
    public interface IResult
    {
        bool Succeed { get; set; }

        Guid Session { get; set; }
    }

    public interface IResult<TResult> : IResult
    {
        TResult Value { get; set; }
    }

    public class Result : IResult
    {
        public bool Succeed { get; set; }

        public Guid Session { get; set; }
    }

    public class Result<TValue> : Result, IResult<TValue>
    {
        public TValue Value { get; set; }

        public Result<TValue> UseResult(TValue value)
        {
            Value = value;
            return this;
        }

        public Result<TValue> Ok()
        {
            Succeed = true;
            return this;
        }

        public Result<TValue> Fail()
        {
            Succeed = false;
            return this;
        }

        public Result<TValue> Context(SessionContext context)
        {
            Session = context.SessionId;
            return this;
        }
    }

    public static class ResultExtensions
    {
        public static async Task<T> Extract<T>(this Task<Result<T>> task)
        {
            return (await task).Value;
        }
    }
}