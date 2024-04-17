using System;
using Newtonsoft.Json;

namespace z5.ms.common.abstractions
{
    /// <summary>Generic result interface</summary>
    /// <remarks>Used in railway-oriented scenario, where we want to return a generic result object with either error or result value.</remarks>
    public interface IErrorResult
    {
        /// <summary>Success</summary>
        bool Success { get; set; }

        /// <summary>Contains error information if operation has failed</summary>
        Error Error { get; set; }

        /// <summary>HTTP status code</summary>
        int StatusCode { get; set; }

        /// <summary>Type of the result value type</summary>
        Type ResultType { get; }
    }

    /// <summary>Operation result wrapper</summary>
    /// <typeparam name="T">Type of returned object</typeparam>
    public class Result<T> : IErrorResult // where T : class, new()
    {
        /// <summary>Success</summary>
        public bool Success { get; set; } = true;

        /// <summary>Contains error information if operation has failed</summary>
        public Error Error { get; set; }

        /// <summary>HTTP status code</summary>
        public int StatusCode { get; set; } = 200;

        /// <summary>Returned object, in case Success equals true</summary>
        public T Value { get; set; }

        /// <summary>Create new Result with success=false and set error value</summary>
        public static Result<T> FromError<T2>(Result<T2> result) =>
            new Result<T> {Success = false, Error = result.Error, StatusCode = result.StatusCode};

        /// <summary>Create new Result with success=false and set error value</summary>
        public static Result<T> FromError(Error error, int statusCode = 400) =>
            new Result<T> {Success = false, Error = error, StatusCode = statusCode};
        
        /// <summary>Create new Result with success=false and set error value</summary>
        public static Result<T> FromError(int errorCode, string errorMessage, int statusCode = 400) =>
            new Result<T> {Success = false, Error = new Error{ Code = errorCode, Message = errorMessage }, StatusCode = statusCode};

        /// <summary>Create new Result with success=true and set value</summary>
        public static Result<T> FromValue(T value) => new Result<T> {Success = true, Value = value};

        /// <summary>Create new Result with success=true and deserialize value from json</summary>
        public static Result<T> FromJson(string json) => 
            new Result<T> {Success = true, Value = JsonConvert.DeserializeObject<T>(json) };

        /// <inheritdoc />
        public Type ResultType => typeof(T);
    }
}