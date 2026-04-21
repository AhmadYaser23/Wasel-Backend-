using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWasel21.Models;
using ProjectWasel21.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWasel21.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/subscription")]
    [Authorize] // 🔥 حماية
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepo;

        public SubscriptionController(ISubscriptionRepository subscriptionRepo)
        {
            _subscriptionRepo = subscriptionRepo;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetAll()
        {
            var subs = await _subscriptionRepo.GetAllAsync();

            return Ok(new
            {
                count = subs.Count(),
                data = subs
            });
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetById(int id)
        {
            var sub = await _subscriptionRepo.GetByIdAsync(id);

            if (sub == null)
                return NotFound(new { message = $"Subscription {id} not found" });

            if (sub.User != null)
                sub.User.Subscriptions = null;

            return Ok(new
            {
                data = sub
            });
        }

        // ================= GET BY USER =================
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetByUser(int userId)
        {
            var subs = await _subscriptionRepo.GetByUserAsync(userId);

            foreach (var s in subs)
            {
                if (s.User != null)
                    s.User.Subscriptions = null;
            }

            return Ok(new
            {
                count = subs.Count(),
                data = subs
            });
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<ActionResult<Subscription>> Create(Subscription subscription)
        {
            subscription.CreatedAt = DateTime.UtcNow;
            subscription.User = null;

            var created = await _subscriptionRepo.AddAsync(subscription);
            var result = await _subscriptionRepo.GetByIdAsync(created.SubscriptionId);

            return Ok(new
            {
                message = "Subscription created successfully",
                data = result
            });
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
                return BadRequest(new { message = "ID mismatch" });

            await _subscriptionRepo.UpdateAsync(subscription);

            return Ok(new
            {
                message = "Subscription updated successfully"
            });
        }

        // ================= GET RAW USER =================
        [HttpGet("raw/user/{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetByUserRaw(int userId)
        {
            var subs = await _subscriptionRepo.GetByUserRawAsync(userId);

            foreach (var s in subs)
            {
                if (s.User != null)
                    s.User.Subscriptions = null;
            }

            return Ok(new
            {
                count = subs.Count(),
                data = subs
            });
        }

        // ================= GET RAW AREA =================
        [HttpGet("raw/area/{area}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetByAreaRaw(string area)
        {
            var subs = await _subscriptionRepo.GetByAreaRawAsync(area);

            foreach (var s in subs)
            {
                if (s.User != null)
                    s.User.Subscriptions = null;
            }

            return Ok(new
            {
                count = subs.Count(),
                data = subs
            });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _subscriptionRepo.DeleteAsync(id);

            return Ok(new
            {
                message = "Subscription deleted successfully",
                subscriptionId = id
            });
        }
    }
}