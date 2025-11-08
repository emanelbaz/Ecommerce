using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repo;

        public BasketController(IBasketRepository repo)
        {
            _repo = repo;
        }

        // GET api/basket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _repo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket { Id = id });
        }

        // POST api/basket  (update whole basket)
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updated = await _repo.UpdateBasketAsync(basket);
            return Ok(updated);
        }

        // DELETE api/basket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await _repo.DeleteBasketAsync(id);
            return NoContent();
        }

        // POST api/basket/merge?userKey={userKey}  merge guestBasket into userBasket
        [HttpPost("merge")]
        public async Task<ActionResult<CustomerBasket>> MergeBaskets([FromQuery] string userKey, [FromBody] CustomerBasket guestBasket)
        {
            // userKey: e.g. user email or "user-{userid}"
            // guestBasket.Id is guest guid
            if (guestBasket == null || string.IsNullOrEmpty(guestBasket.Id))
                return BadRequest("Invalid guest basket");

            var userBasket = await _repo.GetBasketAsync(userKey) ?? new CustomerBasket { Id = userKey };
            // merge items: sum quantities for same variant
            foreach (var item in guestBasket.Items)
            {
                var existing = userBasket.Items.FirstOrDefault(i => i.VariantId == item.VariantId);
                if (existing == null)
                    userBasket.Items.Add(item);
                else
                    existing.Quantity += item.Quantity;
            }

            await _repo.UpdateBasketAsync(userBasket);
            // delete guest basket after merge
            await _repo.DeleteBasketAsync(guestBasket.Id);

            return Ok(userBasket);
        }
    }

}
