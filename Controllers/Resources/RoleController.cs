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
    public class RoleController : ResourceController<DiscordRole>
    {

        public RoleController(WrathIncarnateContext context)
        {
            _repository        = new ApiRepository<DiscordRole>(context);
            _context           = context;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<DiscordRole>), 200)]
        public async Task<ActionResult> get([FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1) {
            try {
                return Ok(await _repository.GetAll(pageNumber, pageSize));
            } catch(Exception e) {
                return BadRequest(e);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DiscordRole), 200)]
        public async Task<IActionResult> getOne([FromRoute] int id)
        {
            return await _getOne(id);
        }
    }
}