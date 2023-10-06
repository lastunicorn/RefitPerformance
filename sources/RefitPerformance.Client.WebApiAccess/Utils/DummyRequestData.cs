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

using DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess.Models;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess.Utils;

internal class DummyRequestData
{
    public static async Task<MemoryStream> CreateDummyBlobStream()
    {
        MemoryStream stream = new();

        StreamWriter streamWriter = new(stream);
        await streamWriter.WriteAsync("something here");
        await streamWriter.FlushAsync();

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public static AuditEntryRequestDto CreateDummyAudit()
    {
        return new AuditEntryRequestDto
        {
            Date = DateTime.UtcNow,
            Message = "My message",
            Details = new[]
            {
                new DetailsRequestDto
                {
                    Code = 1,
                    DetailMessage = "Some details here."
                },
                new DetailsRequestDto
                {
                    Code = 2,
                    DetailMessage = "Some other details here. Ha ha ha."
                }
            }
        };
    }
}