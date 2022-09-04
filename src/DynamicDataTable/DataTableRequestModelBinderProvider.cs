using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace DynamicDataTable
{
    /// <summary>
    /// Custom <see cref="DataTableRequest"/> model binder provider.
    /// </summary>
    public class DataTableRequestModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(DataTableRequest))
            {
                return new BinderTypeModelBinder(typeof(DataTableRequestModelBinder));
            }

            return null;
        }
    }
}