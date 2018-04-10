#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PixelPubApi.Interfaces;
using PixelPubApi.Repository.Api;
using PixelPubApi.MySQL;
using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelPubApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace PixelPubApi.Controllers.Resources
{
    [Route("api/[controller]")]
    public class KeyController : ResourceController<ApiAccess>
    {

        public KeyController(WrathIncarnateContext context)
        {
            _repository        = new ApiRepository<ApiAccess>(context);
            _context           = context;
        }


        [HttpPost]
        [ProducesResponseType(typeof(List<ApiAccess>), 200)]
        public async Task<IActionResult> post([FromBody] ApiAccess record)
        {
            await Task.Delay(1);
            return NotFound();
            //return await _create(record);
        }
    }
}