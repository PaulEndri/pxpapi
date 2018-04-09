#region

using System;
using System.Threading.Tasks;
using PixelPubApi.Interfaces;
using PixelPubApi.Repository.Api;
using PixelPubApi.MySQL;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace PixelPubApi.Controllers.Resources
{
    public abstract class ResourceController<T> : Controller where T : class, IModel
    {
        protected WrathIncarnateContext _context;
        protected ApiRepository<T> _repository;

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual ActionResult _get(DateTime? dateCreatedBefore, DateTime? dateCreatedAfter, int pageSize = 50, int pageNumber = 1)
        {
            try
            {
                return Ok(
                    _repository.GetAll(dateCreatedAfter, dateCreatedBefore, pageNumber, pageSize)
                );
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<IActionResult> _getOne([FromRoute] int id)
        {
            try
            {
                return Ok(
                    await _repository.GetById(id)
                );
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<IActionResult> _delete([FromRoute] int id)
        {
            try
            {
                await _repository.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> _create([FromBody] T obj)
        {
            try
            {
                var results = await _repository.Create(obj);

                return Ok(results);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<ActionResult> _update([FromRoute] int id, [FromBody] T obj)
        {
            try
            {
                await _repository.Update(id, obj);

                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}