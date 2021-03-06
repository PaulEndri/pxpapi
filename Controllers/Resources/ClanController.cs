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
    public class ClanController : ResourceController<Clan>
    {

        public ClanController(WrathIncarnateContext context)
        {
            _repository        = new ApiRepository<Clan>(context);
            _context           = context;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Clan>), 200)]
        public async Task<ActionResult> get([FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1) {
            try {
                return Ok(await _repository.GetAll(pageNumber, pageSize));
            } catch(Exception e) {
                return BadRequest(e);
            }
        }

        [HttpGet("members")]
        [ProducesResponseType(typeof(List<Clan>), 200)]
        public async Task<ActionResult> getWithMembers()
        {
            try {
                return Ok(await _repository.GetAllLoaded(0, 0));
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Clan), 200)]
        public async Task<IActionResult> getOne([FromRoute] int id)
        {
            return await _getOne(id);
        }
    }
}