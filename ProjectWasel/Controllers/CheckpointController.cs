using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/checkpoint")]
    public class CheckpointController : ControllerBase
    {
        private readonly ICheckpointRepository _checkpointRepo;

        public CheckpointController(ICheckpointRepository checkpointRepo)
        {
            _checkpointRepo = checkpointRepo;
        }

        // GET: api/checkpoint
        [HttpGet]
        public async Task<ActionResult<List<Checkpoint>>> GetAll()
        {
            var checkpoints = await _checkpointRepo.GetAllAsync();
            return Ok(checkpoints);
        }

        // GET: api/checkpoint/active
        [HttpGet("active")]
        public async Task<ActionResult<List<Checkpoint>>> GetActive()
        {
            var activeCheckpoints = await _checkpointRepo.GetActiveCheckpointsAsync();
            return Ok(activeCheckpoints);
        }

        // GET: api/checkpoint/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Checkpoint>> GetById(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdAsync(id);
            if (checkpoint == null) return NotFound();
            return Ok(checkpoint);
        }

        // POST: api/checkpoint
        [HttpPost]
        public async Task<ActionResult<Checkpoint>> Create(Checkpoint checkpoint)
        {
            var created = await _checkpointRepo.AddAsync(checkpoint);
            return CreatedAtAction(nameof(GetById), new { id = created.CheckpointId }, created);
        }

        // PUT: api/checkpoint/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Checkpoint checkpoint)
        {
            if (id != checkpoint.CheckpointId) return BadRequest();

            await _checkpointRepo.UpdateAsync(checkpoint);
            return NoContent();
        }

        // GET: api/checkpoint/active-raw
        [HttpGet("active-raw")]
        public async Task<ActionResult<List<Checkpoint>>> GetActiveRaw()
        {
            var active = await _checkpointRepo.GetActiveCheckpointsRawAsync();
            return Ok(active);
        }

        // GET: api/checkpoint/raw/{id}
        [HttpGet("raw/{id}")]
        public async Task<ActionResult<Checkpoint>> GetByIdRaw(int id)
        {
            var checkpoint = await _checkpointRepo.GetByIdRawAsync(id);

            if (checkpoint == null)
                return NotFound();

            return Ok(checkpoint);
        } 
        // DELETE: api/checkpoint/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _checkpointRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}