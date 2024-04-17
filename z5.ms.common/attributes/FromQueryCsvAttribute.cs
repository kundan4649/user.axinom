using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using z5.ms.common.helpers;

namespace z5.ms.common.attributes
{
    /// <inheritdoc />
    /// <summary>Bind an input csv string to a list of strings</summary>
    public class FromQueryCsvAttribute : ModelBinderAttribute
    {
        /// <inheritdoc />
        public FromQueryCsvAttribute() : base(typeof(CsvModelBinder))
        { }
    }
    
    /// <inheritdoc />
    /// <summary>Bind an input csv string to a list of strings</summary>
    public class CsvModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var inputValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (inputValue == null) return Task.CompletedTask;

            var listItems = inputValue.SplitCsv();
            bindingContext.Result = ModelBindingResult.Success(listItems);
            return Task.CompletedTask;
        }
    }
}