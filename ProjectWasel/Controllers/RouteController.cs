using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Models.ModelsDTO;
using ProjectWasel.Repositories;
using ProjectWasel.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Route = ProjectWasel.Models.Route;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/routes")]
    public class RouteController : ControllerBase
    {
        private readonly RouteService _routeService;
        private readonly IRouteRepository _routeRepo;

        public RouteController(RouteService routeService, IRouteRepository routeRepo)
        {
            _routeService = routeService;
            _routeRepo = routeRepo;
        }

        // POST: api/routes/estimate
        [HttpPost("estimate")]
        public async Task<ActionResult<RouteResponseDTO>> EstimateRoute([FromBody] RouteRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body is required");

            var result = await _routeService.EstimateRoute(request);
            return Ok(result);
        }

        // GET: api/routes/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Route>>> GetAllRoutes()
        {
            var routes = await _routeRepo.GetAllAsync();
            return Ok(routes);
        }
        // GET: api/routes/raw
        [HttpGet("raw")]
        public async Task<ActionResult<IEnumerable<Route>>> GetAllRaw()
        {
            var routes = await _routeRepo.GetAllRawAsync();
            return Ok(routes);
        }

        // GET: api/routes/raw/{id}
        [HttpGet("raw/{id}")]
        public async Task<ActionResult<Route>> GetByIdRaw(int id)
        {
            var route = await _routeRepo.GetByIdRawAsync(id);

            if (route == null)
                return NotFound();

            return Ok(route);
        }

        // GET: api/routes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Route>> GetRouteById(int id)
        {
            var route = await _routeRepo.GetByIdAsync(id);
            if (route == null) return NotFound();
            return Ok(route);
        }
    }
}