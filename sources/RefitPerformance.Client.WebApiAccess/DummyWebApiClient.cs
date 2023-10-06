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

using System.Globalization;
using System.Text;
using DustInTheWind.AspNetCorePills.RefitPerformance.Client.Models;
using DustInTheWind.AspNetCorePills.RefitPerformance.Client.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Refit;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Client;

public class DummyWebApiClient
{
    private readonly string baseUrl;

    public DummyWebApiClient(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    public async Task NormalEndpoint_HttpClient()
    {
        HttpClient httpClient = CreateHttpClient();

        AuditEntryRequestDto auditEntry = DummyRequestData.CreateDummyAudit();

        const string id = "123";
        const string uri = $"/Dummy/{id}/test1";
        string auditEntryJson = JsonConvert.SerializeObject(auditEntry);

        HttpContent httpContent = new StringContent(auditEntryJson, Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uri, httpContent);

        string body = await httpResponseMessage.Content.ReadAsStringAsync();
    }

    public async Task NormalEndpoint_Refit()
    {
        HttpClient httpClient = CreateHttpClient();

        AuditEntryRequestDto auditEntry = DummyRequestData.CreateDummyAudit();

        HttpResponseMessage httpResponseMessage = await RestService.For<IDummyApi>(httpClient)
            .Test1("123", auditEntry).ConfigureAwait(false);

        string body = await httpResponseMessage.Content.ReadAsStringAsync();
    }

    public async Task MultipartEndpoint_HttpClient()
    {
        HttpClient httpClient = CreateHttpClient();

        await using MemoryStream stream = await DummyRequestData.CreateDummyBlobStream();

        DummyRequestDto dummyRequestDto = new()
        {
            Binary = new FormFile(stream, 0, stream.Length, string.Empty, "blob.bin"),
            AuditEntry = DummyRequestData.CreateDummyAudit()
        };

        const string id = "123";
        const string uri = $"/Dummy/{id}/test2";

        MultipartRequestBuilder<DummyRequestDto> multipartRequestBuilder = new(dummyRequestDto);
        MultipartFormDataContent multipartDataContent = multipartRequestBuilder.Build();
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uri, multipartDataContent);

        string body = await httpResponseMessage.Content.ReadAsStringAsync();
    }

    public async Task MultipartEndpoint_Refit()
    {
        HttpClient httpClient = CreateHttpClient();

        await using MemoryStream stream = await DummyRequestData.CreateDummyBlobStream();
        StreamPart streamPart = new(stream, string.Empty);

        AuditEntryRequestDto auditEntry = DummyRequestData.CreateDummyAudit();

        HttpResponseMessage httpResponseMessage = await RestService.For<IDummyApi>(httpClient)
            .Test2("123", streamPart, auditEntry.Date.ToString(CultureInfo.InvariantCulture), auditEntry.Message, auditEntry.Details).ConfigureAwait(false);

        string body = await httpResponseMessage.Content.ReadAsStringAsync();
    }

    private HttpClient CreateHttpClient()
    {
        return new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }
}