using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using z5.ms.common.helpers;

namespace z5.ms.common.attributes
{
    /// <inheritdoc />
    /// <summary>Bind an input csv string to a list of integers</summary>
    public class FromQueryCsvIntsAttribute : ModelBinderAttribute
    {
        /// <inheritdoc />
        public FromQueryCsvIntsAttribute() : base(typeof(CsvIntsModelBinder))
        { }
    }

    /// <inheritdoc />
    /// <summary>Bind an input csv string to a list of integers</summary>
    public class CsvIntsModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var inputValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (inputValue == null) return Task.CompletedTask;

            var listItems = inputValue.SplitCsv().Select(x => Convert.ToInt32(x)).ToList();
            bindingContext.Result = ModelBindingResult.Success(listItems);
            return Task.CompletedTask;
        }
    }
}