using ContosoPizza.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContosoPizza.Services
{
    /// <summary>
    /// Pizza Service.
    /// </summary>
    public static class PizzaService
    {
        /// <summary>
        /// Gets the pizzas.
        /// </summary>
        /// <value>
        /// The pizzas.
        /// </value>
        static List<Pizza> Pizzas { get; }
        /// <summary>
        /// The next identifier
        /// </summary>
        static int nextId = 3;
        /// <summary>
        /// Initializes the <see cref="PizzaService"/> class.
        /// </summary>
        static PizzaService()
        {
            Pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
                new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true }
            };
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public static List<Pizza> GetAll() => Pizzas;

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static Pizza Get(int id)
        {
            var selectedPizza = new Pizza();

            foreach (var pizza in Pizzas)
            {
                if (pizza.Id == id)
                {
                    selectedPizza = pizza;
                }
            }

            return selectedPizza;
        }

        /// <summary>
        /// Adds the specified pizza.
        /// </summary>
        /// <param name="pizza">The pizza.</param>
        public static void Add(Pizza pizza)
        {
            pizza.Id = nextId++;
            Pizzas.Add(pizza);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public static void Delete(int id)
        {
            var pizza = Get(id);
            if(pizza is null)
                return;

            Pizzas.Remove(pizza);
        }

        /// <summary>
        /// Updates the specified pizza.
        /// </summary>
        /// <param name="pizza">The pizza.</param>
        public static void Update(Pizza pizza)
        {
            var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
            if(index == -1)
                return;

            Pizzas[index] = pizza;
        }
    }
}