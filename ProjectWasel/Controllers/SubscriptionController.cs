using Microsoft.AspNetCore.Mvc;
using ProjectWasel.Models;
using ProjectWasel.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/subscription")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepo;

        public SubscriptionController(ISubscriptionRepository subscriptionRepo)
        {
            _subscriptionRepo = subscriptionRepo;
        }

        // GET: api/subscription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetAll()
        {
            var subs = await _subscriptionRepo.GetAllAsync();
            return Ok(subs);
        }

        // GET: api/subscription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetById(int id)
        {
            var sub = await _subscriptionRepo.GetByIdAsync(id);

            if (sub == null)
                return NotFound();

            return Ok(sub);
        }

        // GET: api/subscription/user/3
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetByUser(int userId)
        {
            var subs = await _subscriptionRepo.GetByUserAsync(userId);
            return Ok(subs);
        }

        // POST: api/subscription
        [HttpPost]
        public async Task<ActionResult<Subscription>> Create(Subscription subscription)
        {
            var created = await _subscriptionRepo.AddAsync(subscription);
            return Ok(created);
        }

        // PUT: api/subscription/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
                return BadRequest();

            await _subscriptionRepo.UpdateAsync(subscription);
            return NoContent();
        }
        // GET: api/subscription/raw/user/{userId}
        [HttpGet("raw/user/{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetByUserRaw(int userId)
        {
            var subs = await _subscriptionRepo.GetByUserRawAsync(userId);
            return Ok(subs);
        }

        // GET: api/subscription/raw/area/{area}
        [HttpGet("raw/area/{area}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetByAreaRaw(string area)
        {
            var subs = await _subscriptionRepo.GetByAreaRawAsync(area);
            return Ok(subs);
        }

        // DELETE: api/subscription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _subscriptionRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}