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
using Refit;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.Client.WebApiAccess;

public interface IDummyApi
{
    [Post("/Dummy/{Id}/test1")]
    Task<HttpResponseMessage> Test1(string id, AuditEntryRequestDto auditEntry);
    
    [Multipart]
    [Post("/Dummy/{Id}/test2")]
    Task<HttpResponseMessage> Test2(
        string id,
        [AliasAs("Binary")] StreamPart binary,
        [AliasAs("AuditEntry.Date")] string date,
        [AliasAs("AuditEntry.Message")] string message = null,
        [AliasAs("AuditEntry.Details")] IEnumerable<DetailsRequestDto> details = null);
}