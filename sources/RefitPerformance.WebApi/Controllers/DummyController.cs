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

using DustInTheWind.AspNetCorePills.RefitPerformance.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DustInTheWind.AspNetCorePills.RefitPerformance.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DummyController : ControllerBase
{
    [HttpPost("{id}/test1")]
    [Produces("application/json")]
    public async Task<IActionResult> DoSomething(string id, [FromBody] AuditEntryRequestModel auditEntryRequestModel)
    {
        ResponseViewModel responseViewModel = new()
        {
            Message = "All was ok"
        };

        return Ok(responseViewModel);
    }

    [HttpPost("{id}/test2")]
    [Produces("application/json")]
    public async Task<IActionResult> DoSomething(string id, [FromForm] DummyRequestModel dummyRequestModel)
    {
        if (dummyRequestModel.Binary != null)
        {
            await using Stream stream = dummyRequestModel.Binary.OpenReadStream();
            StreamReader streamReader = new(stream);
            string file = await streamReader.ReadToEndAsync();
        }

        ResponseViewModel responseViewModel = new()
        {
            Message = "All was ok"
        };

        return Ok(responseViewModel);
    }
}