using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using z5.ms.common.extensions;

namespace z5.ms.common.attributes
{
    /// <inheritdoc />
    /// <summary>Enum property is bound to a query string parameter. Enum value will be looked up by EnumMemberValue</summary>
    public class FromQueryEnumMemberAttribute : ModelBinderAttribute
    {
        /// <inheritdoc />
        public FromQueryEnumMemberAttribute() : base(typeof(EnumModelBinder))
        { }

        /// <inheritdoc />
        /// <summary>Enum value will be looked up by EnumMemberValue</summary>
        public class EnumModelBinder : IModelBinder
        {
            /// <inheritdoc />
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var inputValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
                if (inputValue == null) return Task.CompletedTask;

                var enumType = bindingContext.ModelType;
                var value = EnumExtensions.LookupEnumMember(enumType, inputValue);
                if (value == null)
                {
                    try
                    {
                        value = Enum.Parse(enumType, inputValue, true);
                    }
                    catch
                    {
                        return Task.CompletedTask;
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(value);
                return Task.CompletedTask;
            }
        }
    }
}