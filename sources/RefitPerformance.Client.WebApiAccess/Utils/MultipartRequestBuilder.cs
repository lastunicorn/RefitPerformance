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

using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess.Utils;

internal class MultipartRequestBuilder<TModel>
{
    private TModel model;
    private MultipartFormDataContent multipartFormDataContent;

    public MultipartRequestBuilder(TModel model)
    {
        this.model = model;
    }

    public MultipartFormDataContent Build()
    {
        if (model == null)
            return null;

        multipartFormDataContent = new MultipartFormDataContent();

        Type modelType = model.GetType();

        PropertyInfo[] modelProperties = modelType.GetProperties();

        foreach (PropertyInfo property in modelProperties)
        {
            object propertyValue = property.GetValue(model);

            if (propertyValue != null)
                TryAddValueToTheFormContent(property, propertyValue);
        }

        return multipartFormDataContent;

    }

    private void TryAddValueToTheFormContent(PropertyInfo property, object propertyValue, string parentPropertyName = null)
    {
        Type propertyType = propertyValue.GetType();

        bool isPrimitive = propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(Guid) || propertyType == typeof(DateTime);
        if (isPrimitive)
        {
            StringContent stringContent = new(propertyValue.ToString());

            string propertyName = string.IsNullOrEmpty(parentPropertyName)
                ? property.Name
                : $"{parentPropertyName}.{property.Name}";

            multipartFormDataContent.Add(stringContent, propertyName);
            return;
        }

        if (propertyValue is IFormFile formFile)
        {
            ByteArrayContent byteArrayContent = GetFormFileByteArrayContent1(formFile);
            multipartFormDataContent.Add(byteArrayContent, property.Name, formFile.FileName);

            return;
        }

        if (propertyValue is IEnumerable enumerableValue)
        {
            foreach (object value in enumerableValue)
            {
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    PropertyNamingPolicy = null
                };
                string json = JsonSerializer.Serialize(value, jsonSerializerOptions);

                StringContent stringContent = new(json, Encoding.Unicode, "application/json");
                multipartFormDataContent.Add(stringContent, $"{parentPropertyName}.{property.Name}");
            }

            return;
        }

        if (propertyType.IsClass)
        {
            PropertyInfo[] classProperties = propertyType.GetProperties();

            foreach (PropertyInfo prop in classProperties)
            {
                object propValue = prop.GetValue(propertyValue);

                if (propValue != null)
                    TryAddValueToTheFormContent(prop, propValue, property.Name);
            }

            return;
        }
    }

    private static ByteArrayContent GetFormFileByteArrayContent1(IFormFile file)
    {
        if (file == null) return null;

        byte[] payload;
        using (Stream stream = file.OpenReadStream())
        {
            payload = new byte[stream.Length];
            stream.Read(payload, 0, (int)stream.Length);
        }

        return new ByteArrayContent(payload);
    }

    private static StreamContent GetFormFileByteArrayContent2(IFormFile file)
    {
        if (file == null) return null;

        Stream stream = file.OpenReadStream();
        return new StreamContent(stream);
    }
}