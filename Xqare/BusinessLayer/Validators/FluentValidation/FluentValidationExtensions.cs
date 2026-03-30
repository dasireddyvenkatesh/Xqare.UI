using FluentValidation;

namespace Xqare.BusinessLayer.Validators.FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static Func<object, string, Task<IEnumerable<string>>>
            ToMudValidator<T>(this IValidator<T> validator)
        {
            return async (model, propertyName) =>
            {
                var result = await validator.ValidateAsync(
                    ValidationContext<T>.CreateWithOptions(
                        (T)model,
                        x => x.IncludeProperties(propertyName)));

                if (result.IsValid)
                    return Array.Empty<string>();

                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
