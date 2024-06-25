using MailKit.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tvshow.Application.Features.Tvshow.Commands.CreateTvShow;
using Tvshow.Application.Features.Tvshow.Commands.DeleteTvShow;
using Tvshow.Application.Features.Tvshow.Commands.UpdateTvShow;
using Tvshow.Application.Features.Tvshow.Queries.GetTvShowById;
using Tvshow.Application.Features.Tvshow.Queries.GetTvShowList;
using Tvshow.Application.Features.Tvshow.Queries.PaginationTvShow;
using Tvshow.Application.Features.Tvshow.Vms;
using Tvshow.Application.Shared.Queries;

namespace Tvshow.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TvShowController : ControllerBase
    {
        private IMediator _mediator;

        public TvShowController(IMediator mediator)
        {
            _mediator = mediator;
        }

      
        // Define la ruta de acceso y el nombre de la ruta para generar enlaces
        [HttpGet("list", Name = "GetTvShowList")]
        // Especifica el tipo de respuesta y el código de estado HTTP esperado
        [ProducesResponseType(typeof(IReadOnlyList<TvShowVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<TvShowVm>>> GetCategoryList()
        {
            // Crea una instancia de la consulta para obtener la lista de programas de televisión
            var query = new GetTvShowListQuery();
            // Envia la consulta al mediador y devuelve una respuesta HTTP OK con los resultados
            return Ok(await _mediator.Send(query));
        }


        [HttpGet("{id}", Name = "GetTvShowById")]
        [ProducesResponseType(typeof(TvShowVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TvShowVm>> GetOrderById(int id)
        {
            var query = new GetTvShowByIdQuery(id);
            return Ok(await _mediator.Send(query));

        }

        // Define la ruta de acceso y el nombre de la ruta para generar enlaces
        [HttpPost(Name = "CreateTvShow")]
        // Especifica el código de estado HTTP esperado
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<TvShowVm>> CreateReview([FromBody] CreateTvShowCommand request)
        {
            // Envia el comando para crear un nuevo programa de televisión al mediador
            return await _mediator.Send(request);
        }


        // Define la ruta de acceso y el nombre de la ruta para generar enlaces
        [HttpPut("update", Name = "UpdateTvShow")]
        // Especifica el código de estado HTTP esperado
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<TvShowVm>> UpdateTvShow([FromForm] UpdateTvShowCommand request)
        {
            // Envia el comando para actualizar un programa de televisión al mediador
            return await _mediator.Send(request);
        }

        // Define la ruta de acceso con un parámetro de ruta y el nombre de la ruta para generar enlaces
        [HttpDelete("status/{id}", Name = "UpdateStatusTvShow")]
        // Especifica el código de estado HTTP esperado
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<TvShowVm>> UpdateStatusTvShow(int id)
        {
            // Crea una instancia del comando para eliminar un programa de televisión por su ID
            var request = new DeleteTvShowCommand(id);
            // Envia el comando al mediador y devuelve una respuesta HTTP OK con el resultado
            return await _mediator.Send(request);
        }

        // Define la ruta de acceso y el nombre de la ruta para generar enlaces
        [HttpGet("paginationAdmin", Name = "PaginationTvShow")]
        // Especifica el tipo de respuesta y el código de estado HTTP esperado
        [ProducesResponseType(typeof(PaginationVm<TvShowVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<TvShowVm>>> PaginationProducAdmin(
            [FromQuery] PaginationTvShowQuery paginationTvshowQuery
        )
        {
            // Envia la consulta de paginación al mediador y obtiene una respuesta paginada
            var paginationTv = await _mediator.Send(paginationTvshowQuery);
            // Devuelve una respuesta HTTP OK con la respuesta paginada
            return Ok(paginationTv);
        }
    }
}
