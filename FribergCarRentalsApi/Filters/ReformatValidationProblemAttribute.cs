using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FribergCarRentals.Shared.Models.Dto.Api;

namespace FribergCarRentalsApi.Filters
{
    /// <summary>
    /// Reformats validation problems details from bad requests into an <see cref="ApiErrorResponseDto"/> object.
    /// </summary>
    public class ReformatValidationProblemAttribute : ActionFilterAttribute
    {
        #region Methods

        /// <inheritdoc/>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (context.Result is BadRequestObjectResult result)
            {
                if (result.Value is ValidationProblemDetails details)
                {
                    var errorMessages = details.Errors.SelectMany(x => x.Value.Select(y => new KeyValuePair<string, string>(x.Key, y))).ToList();
                    context.Result = new BadRequestObjectResult(ApiResponseDto.CreateErrorResponse(errors: errorMessages));
                }
            }
        }

        #endregion
    }
}
