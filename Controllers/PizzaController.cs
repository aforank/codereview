using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Controllers
{
    /// <summary>
    /// Pizza Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class PizzaController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaController"/> class.
        /// </summary>
        public PizzaController()
        {
        }

        /// <summary>
        /// Gets all pizzas.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Pizza>> GetAll() =>
            PizzaService.GetAll();

        /// <summary>
        /// Gets the specified pizza by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Pizza> Get(int id)
        {
            var pizza = PizzaService.Get(id);

            if (pizza == null)
                return null;

            return pizza;
        }

        /// <summary>
        /// Creates the specified pizza.
        /// </summary>
        /// <param name="pizza">The pizza.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(Pizza pizza)
        {
            PizzaService.Add(pizza);
            return CreatedAtAction(nameof(Create), new { id = pizza.Id }, pizza);
        }

        /// <summary>
        /// Updates the specified pizza by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="pizza">The pizza.</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public IActionResult Update(int id, Pizza pizza)
        {
            if (id != pizza.Id)
                return BadRequest();

            var existingPizza = PizzaService.Get(id);
            if (existingPizza is null)
                return BadRequest();

            try
            {
                PizzaService.Update(pizza);
            }
            catch (Exception ex)
            {
                return this.BadRequest("Exception occurred while processing the request.");
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes the specified pizza by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pizza = PizzaService.Get(id);

            if (pizza is null)
                return NotFound();

            try
            {
                PizzaService.Delete(id);
            }
            catch (Exception ex)
            {
                return this.BadRequest("Exception occurred while processing the request.");
            }

            return NoContent();
        }
    }
}