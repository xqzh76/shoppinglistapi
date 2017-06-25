namespace ShoppingListApi.Data
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShoppingListApi.Model;

    public sealed class InMemoryDrinkRepository : IRepository<Drink>
    {
        private readonly ConcurrentDictionary<string, Drink> drinks = new ConcurrentDictionary<string, Drink>();

        public Task<IEnumerable<Drink>> GetAll()
        {
            return Task.FromResult(this.drinks.Values.AsEnumerable());
        }

        public Task<Drink> GetByName(string name)
        {
            Drink drink;
            if (this.drinks.TryGetValue(Normalize(name), out drink))
            {
                return Task.FromResult(drink);
            }

            return Task.FromResult<Drink>(null);
        }

        public Task Save(Drink drink)
        {
            this.drinks.AddOrUpdate(Normalize(drink.Name), drink, (s, d) => d);

            return Task.FromResult(0);
        }

        public Task Delete(string name)
        {
            Drink drink;
            if (this.drinks.TryRemove(Normalize(name), out drink))
            {
                return Task.FromResult(0);
            }

            throw new InvalidOperationException($"{name} cannot be found");
        }

        private static string Normalize(string name)
        {
            return name.ToUpperInvariant();
        }
    }
}