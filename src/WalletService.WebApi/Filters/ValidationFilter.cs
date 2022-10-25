using FluentValidation;
using WalletService.WebApi.Models;
using WalletService.WebApi.Models.Inputs;

namespace WalletService.WebApi.Filters;

public class ValidationFilter : IEndpointFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
        => this._serviceProvider = serviceProvider;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var input = context.Arguments.FirstOrDefault(x => x is IInput);
        if (input is null)
            return await next(context);

        var validationContext = new ValidationContext<object>(input);
        var validator = this._serviceProvider
            .GetService(typeof(IValidator<>).MakeGenericType(input.GetType())) as IValidator;

        var result = await validator?.ValidateAsync(validationContext)!;

        return result.IsValid
            ? await next(context)
            : Results.BadRequest(result.Errors.Select(_ => new ErrorApplication
            {
                ErrorMessage = $"{_.PropertyName} - {_.ErrorMessage}"
            }));
    }
}