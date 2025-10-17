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
        public  string? ErrorMessage { get; }
        public bool IsSuccess => ErrorMessage == null;


        private Result(T? value, string? errorMessage)
        {
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new Result<T>(value, null);
        public static Result<T> Failure(string errorMessage) => new Result<T>(default, errorMessage);
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
