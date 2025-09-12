using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_commerceApplication.Validation
{
    public class ValidateGameListParamsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string actionParameterName = "gameFilterAndSortRequestDto";
            int emptyGenreCount = 0;

            List<string> errors = new();

            if (context.ActionArguments.TryGetValue(actionParameterName, out var arg) 
                && arg is GameFilterAndSortRequestDto dto)
            {
                if (dto.Genres.Any(string.IsNullOrWhiteSpace) || dto.Genres.Count == emptyGenreCount)
                {
                    errors.Add(GamesDtoValidationMessages.InvalidGenres);
                }

                if (dto.Age == null || !Enum.IsDefined(typeof(Rating), dto.Age))
                {
                    errors.Add(GamesDtoValidationMessages.InvalidAgeFilter);
                }

                if (dto.SortBy == null || !Enum.IsDefined(typeof(SortByField), dto.SortBy))
                {
                    errors.Add(GamesDtoValidationMessages.InvalidSort);
                }

                if (dto.SortOrder == null || !Enum.IsDefined(typeof(SortOrder), dto.SortOrder))
                {
                    errors.Add(GamesDtoValidationMessages.InvalidSortOrder);
                }

                if (errors.Any())
                {
                    context.Result = new BadRequestObjectResult(new { Errors = errors });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}