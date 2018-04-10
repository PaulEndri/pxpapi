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
        public ActionResult _get(DateTime? dateCreatedBefore, DateTime? dateCreatedAfter, int pageSize = 50, int pageNumber = 1)
        {
            try
            {
                return Ok(
                    _repository.GetAll(pageNumber, pageSize)
                );
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> _getOne([FromRoute] int id)
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
        public async Task<IActionResult> _create(T record)
        {
            try {
                return Ok(
                    await _repository.Create(record)
                );
            } catch (Exception e) {
                return BadRequest(e);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> _put(long id, T record)
        {
            try {
                return Ok(
                    await _repository.Replace(id, record)
                );
            } catch  (Exception e) {
                return BadRequest(e);
            }
        }
    }
}