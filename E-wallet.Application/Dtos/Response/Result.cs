using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Dtos.Response
{
    public class Result<T> //where T : class
    {
        public  T? Value { get; }
        public List<T?> Values { get; }
        public  string? ErrorMessage { get; }
        public bool IsSuccess => ErrorMessage == null;


        private Result(T? value, string? errorMessage,List<T?>? values)
        {
            Value = value;
            ErrorMessage = errorMessage;
            Values = values;
        }

        public static Result<T> Success(T value) => new Result<T>(value, null,default);
        public static Result<T> Success(List <T?> values) => new Result<T>(default, null,values);

        public static Result<T> Failure(string errorMessage) => new Result<T>(default, errorMessage,default);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        private Result(string? errorMessage)
        {
            ErrorMessage = errorMessage;
            IsSuccess = errorMessage == null;
        }

        public static Result Success() => new(null);
        public static Result Failure(string message) => new(message);
    }
}
