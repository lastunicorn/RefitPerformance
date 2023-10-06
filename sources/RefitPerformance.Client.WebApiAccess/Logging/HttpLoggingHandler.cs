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

using System.Diagnostics;
using System.Net.Http.Headers;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess.Logging;

public class HttpLoggingHandler : DelegatingHandler
{
    private readonly string[] types = { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

    public HttpLoggingHandler(HttpMessageHandler innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpRequestLog log = new();

        log.WriteRequestMessage("========Start==========");

        await DisplayRequest(request, cancellationToken, log);

        Stopwatch stopwatch = Stopwatch.StartNew();
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        stopwatch.Stop();

        log.WriteRequestMessage($"Call Duration: {stopwatch.Elapsed}");
        log.WriteRequestMessage("==========End==========");

        log.WriteResponseMessage("=========Start=========");

        await DisplayResponse(cancellationToken, log, response);

        log.WriteResponseMessage("==========End==========");
        return response;
    }

    private async Task DisplayRequest(HttpRequestMessage request, CancellationToken cancellationToken, HttpRequestLog log)
    {
        log.WriteRequestMessage($"URI: {request.RequestUri}");

        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
        {
            string valuesAsString = string.Join(", ", header.Value);
            log.WriteRequestMessage($"Header: {header.Key} = {valuesAsString}");
        }

        if (request.Content != null)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Content.Headers)
            {
                string valuesAsString = string.Join(", ", header.Value);
                log.WriteRequestMessage($"Content Header: {header.Key} = {valuesAsString}");
            }

            bool isTextContent = request.Content is StringContent || IsTextBasedContentType(request.Headers) || IsTextBasedContentType(request.Content.Headers);

            if (isTextContent)
            {
                string result = await request.Content.ReadAsStringAsync(cancellationToken);

                log.WriteRequestMessage($"Content:\r\n{result}");
            }
            else
            {
                Stream stream = await request.Content.ReadAsStreamAsync(cancellationToken);
                StreamReader streamReader = new(stream);
                string result = await streamReader.ReadToEndAsync();

                log.WriteRequestMessage($"Content:\r\n{result}");
            }
        }
    }

    private async Task DisplayResponse(CancellationToken cancellationToken, HttpRequestLog log, HttpResponseMessage response)
    {
        log.WriteResponseMessage($"Version: {response.Version}");
        log.WriteResponseMessage($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");

        foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
        {
            string valuesAsString = string.Join(", ", header.Value);
            log.WriteResponseMessage($"Response Header: {header.Key} = {valuesAsString}");
        }

        if (response.Content != null)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in response.Content.Headers)
            {
                string valuesAsString = string.Join(", ", header.Value);
                log.WriteResponseMessage($"Content Header: {header.Key} = {valuesAsString}");
            }

            bool isTextContent = response.Content is StringContent || IsTextBasedContentType(response.Headers) || IsTextBasedContentType(response.Content.Headers);

            if (isTextContent)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                stopwatch.Stop();

                log.WriteResponseMessage($"Content\r\n{responseContent}");
                log.WriteResponseMessage($"Duration: {stopwatch.Elapsed}");
            }
        }
    }

    private bool IsTextBasedContentType(HttpHeaders headers)
    {
        if (!headers.TryGetValues("Content-Type", out IEnumerable<string> values))
            return false;

        string header = string.Join(" ", values).ToLowerInvariant();

        return types.Any(x => header.Contains(x));
    }
}