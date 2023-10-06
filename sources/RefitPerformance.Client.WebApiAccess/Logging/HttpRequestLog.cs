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

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess.Logging;

internal class HttpRequestLog
{
    private readonly string requestMessageHeader;
    private readonly string responseMessageHeader;

    public HttpRequestLog()
    {
        string id = Guid.NewGuid().ToString();
        requestMessageHeader = $"[{id} -   Request]";
        responseMessageHeader = $"[{id} -   Response]";

        Debug.WriteLine("...Debug...");
        Console.WriteLine("...Console...");
        Trace.WriteLine("...Trace...");
    }

    public void WriteRequestMessage(string message)
    {
        Console.WriteLine($"{requestMessageHeader} {message}");
    }

    public void WriteResponseMessage(string message)
    {
        Console.WriteLine($"{responseMessageHeader} {message}");
    }
}