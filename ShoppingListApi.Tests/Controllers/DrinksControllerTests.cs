namespace ShoppingListApi.Tests.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using NUnit.Framework;
    using ShoppingListApi.Controllers;
    using ShoppingListApi.Data;
    using ShoppingListApi.Model;
    using ShoppingListApi.Tests.Extensions;
    using Shouldly;

    [TestFixture]
    public class DrinksControllerTests
    {
        private Mock<IRepository<Drink>> repository;

        [SetUp]
        public void Setup()
        {
            this.repository = new Mock<IRepository<Drink>>();
        }

        [Test]
        public async Task GetExistingDrinkFromShoppingListReturnsOk()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => drink);

            var subject = this.CreateSubject();

            var result = await subject.Get(drink.Name);

            result.ShouldBeOfType<OkObjectResult>();
            var value = ActionResultHelper.GetOkObject<Drink>(result);
            value.Name.ShouldBe(drink.Name);
            value.Quantity.ShouldBe(drink.Quantity);
        }

        [Test]
        public async Task GetNonExistingDrinkFromShoppingListReturnsNotFound()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => null);

            var subject = this.CreateSubject();

            var result = await subject.Get(drink.Name);

            result.ShouldBeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task AddNonExistingDrinkToShoppingListReturnsOK()
        {
            var drink = new Drink {Name = "drink", Quantity = 1};

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => null);
            this.repository.Setup(r => r.Save(drink)).Returns(() =>Task.CompletedTask);

            var subject = this.CreateSubject();
            
            var result = await subject.Post(drink);

            result.ShouldBeOfType<OkObjectResult>();
            var value = ActionResultHelper.GetOkObject<Drink>(result);
            value.Name.ShouldBe(drink.Name);
            value.Quantity.ShouldBe(drink.Quantity);
        }

        [Test]
        public async Task AddDuplicateDrinkToShoppingListReturnsBadRequest()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => drink);

            var subject = this.CreateSubject();
            
            var result = await subject.Post(drink);

            result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task UpdateExistingDrinkToShoppingListReturnsOK()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => drink);
            this.repository.Setup(r => r.Save(drink)).Returns(() => Task.CompletedTask);

            var subject = this.CreateSubject();

            drink.Quantity = drink.Quantity.Value + 1;
            var result = await subject.Put(drink.Name, drink);

            result.ShouldBeOfType<OkObjectResult>();
            var value = ActionResultHelper.GetOkObject<Drink>(result);
            value.Name.ShouldBe(drink.Name);
            value.Quantity.ShouldBe(drink.Quantity);
        }

        [Test]
        public async Task UpdateNonExistingDrinkToShoppingListReturnsNotFound()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => drink);

            var subject = this.CreateSubject();

            var newDrink = new Drink { Name = drink.Name + "I am a new drink", Quantity = 1 };
            var result = await subject.Put(newDrink.Name, newDrink);

            result.ShouldBeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task DeleteExistingDrinkToShoppingListReturnsOk()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => drink);
            this.repository.Setup(r => r.Delete(drink.Name)).Returns(() => Task.CompletedTask);

            var subject = this.CreateSubject();

            var result = await subject.Delete(drink.Name);

            result.ShouldBeOfType<OkObjectResult>();
            var value = ActionResultHelper.GetOkObject<OkResponse>(result);
            value.Message.ShouldBe("Ok");
        }

        [Test]
        public async Task DeleteNonExistingDrinkToShoppingListReturnsNotFound()
        {
            var drink = new Drink { Name = "drink", Quantity = 1 };

            this.repository.Setup(r => r.GetByName(drink.Name)).ReturnsAsync(() => drink);

            var subject = this.CreateSubject();

            var result = await subject.Delete(drink.Name + "I am a new drink");

            result.ShouldBeOfType<NotFoundObjectResult>();
        }


        private DrinksController CreateSubject()
        {
            return new DrinksController(this.repository.Object);
        }
    }
}
