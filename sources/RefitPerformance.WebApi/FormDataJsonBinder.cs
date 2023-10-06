// ASP.NET Core Pills
// Copyright (C) 2022-2023 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.WebApi;

public class FormDataJsonBinder : IModelBinder
{
    private readonly ILogger<FormDataJsonBinder> logger;

    public FormDataJsonBinder(ILogger<FormDataJsonBinder> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        string modelName = bindingContext.ModelName;

        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        string value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        try
        {
            object result = JsonSerializer.Deserialize(value, bindingContext.ModelType);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}