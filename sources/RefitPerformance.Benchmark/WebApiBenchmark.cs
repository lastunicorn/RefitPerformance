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

using BenchmarkDotNet.Attributes;
using DustInTheWind.AspNetCorePills.RefitPerformance.Client;
using DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Benchmark;

[SimpleJob(launchCount: 3, iterationCount: 200)]
public class WebApiBenchmark
{
    private readonly DummyWebApiClient dummyWebApiClient;

    public WebApiBenchmark()
    {
        Config config = new();
        dummyWebApiClient = new DummyWebApiClient(config.WebServiceUrl);
    }

    [Benchmark]
    public async Task NormalEndpoint_HttpClient()
    {
        await dummyWebApiClient.NormalEndpoint_HttpClient();
    }

    [Benchmark]
    public async Task NormalEndpoint_Refit()
    {
        await dummyWebApiClient.NormalEndpoint_Refit();
    }

    [Benchmark]
    public async Task MultipartEndpoint_HttpClient()
    {
        await dummyWebApiClient.MultipartEndpoint_HttpClient();
    }

    [Benchmark]
    public async Task MultipartEndpoint_Refit()
    {
        await dummyWebApiClient.MultipartEndpoint_Refit();
    }
}