namespace ShoppingListApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Serilog;
    using ShoppingListApi.Data;
    using ShoppingListApi.Infrastructure;
    using ShoppingListApi.Model;

    [Route("v1/shoppinglist/[controller]")]
    [Authorize]
    public class DrinksController : Controller
    {
        private readonly ILogger logger = Log.ForContext<DrinksController>();

        private readonly IRepository<Drink> repository;

        public DrinksController(IRepository<Drink> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var drinks = (await this.repository.GetAll()).ToList();

            return this.Ok(new DrinkList
                            {
                                Count = drinks.Count, Data = drinks.ToList()
                            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var drink = await this.repository.GetByName(name);
            if (drink == null)
            {
                var httpError = new HttpError("drink_not_found", $"Cannot get drink {name} because it cannot be found", string.Empty);
                this.logger.Error(httpError.ToString());
                return this.NotFound(httpError);
            }

            return this.Ok(drink);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Drink drink)
        {
            var existingDrink = await this.repository.GetByName(drink.Name);
            if (existingDrink != null)
            {
                var httpError = new HttpError("drink_exists", $"Cannot add drink {drink.Name} because it already exists", string.Empty);
                this.logger.Error(httpError.ToString());
                return this.BadRequest(httpError);
            }

            await this.repository.Save(drink);

            return this.Ok(drink);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody]Drink drink)
        {
            var existingDrink = await this.repository.GetByName(name);
            if (existingDrink == null)
            {
                var httpError = new HttpError("drink_not_found", $"You cannot update drink {name} because it cannot be found", string.Empty);
                this.logger.Error(httpError.ToString());
                return this.NotFound(httpError);
            }

            existingDrink.Quantity = drink.Quantity;
            await this.repository.Save(existingDrink);

            return this.Ok(drink);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var drink = await this.repository.GetByName(name);
            if (drink == null)
            {
                var httpError = new HttpError("drink_not_found", $"Cannot delete drink {name} because it cannot be found", string.Empty);
                this.logger.Error(httpError.ToString());
                return this.NotFound(httpError);
            }

            await this.repository.Delete(name);

            return this.Ok(new OkResponse {Message = "Ok"});
        }
    }
}
