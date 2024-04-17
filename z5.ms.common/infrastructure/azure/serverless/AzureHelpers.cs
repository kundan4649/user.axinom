using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace z5.ms.common.infrastructure.azure.serverless
{
    /// <summary>Methods for reading azure function app settings</summary>
    public static class AzureHelpers
    {
        private static readonly ConcurrentDictionary<Type, object> Configurations = new ConcurrentDictionary<Type, object>();

        /// <summary>Configure an object with app settings (uses reflection)</summary>
        public static T Configure<T>(T result, bool recursive = false) where T:class
            => Configure(typeof(T), result, recursive) as T;

        /// <summary>Configure an object with app settings (uses reflection)</summary>
        public static T Configure<T>(bool recursive = false) where T : class, new() 
            => Configurations.GetOrAdd(typeof(T), t => Configure(typeof(T), new T(), recursive)) as T;

        /// <summary>Read a single app setting value by name</summary>
        public static string ReadAppSetting(string name, bool required = false)
        {
            var v = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (required && string.IsNullOrEmpty(v)) throw new ArgumentException($"Required appsetting '{name}' missing.");
            return v;
        }

        private static object Configure(Type type, object result, bool recursive = false)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsClass && (!property.PropertyType.FullName?.StartsWith("System.") ?? false))
                {
                    if (recursive && property.GetValue(result) == null)
                        property.SetValue(result, Configure(property.PropertyType, Activator.CreateInstance(property.PropertyType), true));

                    continue;
                }

                var required = property.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute));
                var value = ReadAppSetting(property.Name, required);
                if (value == null)
                    continue;

                property.SetValue(result, property.PropertyType.IsEnum
                    ? Enum.Parse(property.PropertyType, value, true)
                    : Convert.ChangeType(value, property.PropertyType));
            }
            return result;
        }

        /// <summary>Create a json http response</summary>
        /// <param name="statusCode"></param>
        /// <param name="responseObject"></param>
        /// <returns></returns>
        public static HttpResponseMessage JsonResponse(object responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            new HttpResponseMessage(statusCode) { Content = new StringContent(JsonConvert.SerializeObject(responseObject), Encoding.UTF8, "application/json") };
    }
}