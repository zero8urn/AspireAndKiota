using Asp.Versioning;
using AspireApi.Common.Contracts;
using AspireApi.Common.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AspireApi.Controllers.V1
{
    [ApiController]
    [Authorize]
    [ApiVersion(1)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoDataAccess todoDataAccess;

        public TodosController(ITodoDataAccess todoDataAccess)
        {
            this.todoDataAccess = todoDataAccess;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TodoItem>> Get([Required] Guid id)
        {
            var resp = await todoDataAccess.GetById<TodoItem>(id);
            return resp;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TodoItem>> Add(TodoItem todo)
        {
            var resp = await todoDataAccess.Add(todo);
            return resp;
        }
    }
}
