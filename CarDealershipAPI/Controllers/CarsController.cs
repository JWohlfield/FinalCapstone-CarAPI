using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarDealershipAPI.Models;

namespace CarDealershipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly DealershipDBContext _context;

        public CarsController(DealershipDBContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        [HttpGet("SearchColor/{color}")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchColor(string color)
        {
            int matches = _context.Cars.Count(x => x.Color.Contains(color));

            if(matches > 0)
            {
                return await _context.Cars.Where(x => x.Color.Contains(color)).ToListAsync();
            }

            return NotFound();
        }

        [HttpGet("SearchMake/{make}")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchMake(string make)
        {
            int matches = _context.Cars.Count(x => x.Make.Contains(make));

            if (matches > 0)
            {
                return await _context.Cars.Where(x => x.Make.Contains(make)).ToListAsync();
            }

            return NotFound();
        }

        [HttpGet("SearchModel/{model}")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchModel(string model)
        {
            int matches = _context.Cars.Count(x => x.Model.Contains(model));

            if (matches > 0)
            {
                return await _context.Cars.Where(x => x.Model.Contains(model)).ToListAsync();
            }

            return NotFound();
        }

        [HttpGet("SearchYear/{year}")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchYear(int year)
        {
            int matches = _context.Cars.Count(x => x.Year == year);

            if (matches > 0)
            {
                return await _context.Cars.Where(x => x.Year == year).ToListAsync();
            }

            return NotFound();
        }

        [HttpGet("SearchAll")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchAll([FromQuery] string color = null, [FromQuery] string make = null, [FromQuery] string model = null, [FromQuery] int year = 0)
        {
            var query = _context.Cars.Where(x => x.Id > 0);
            if(color != null)
            {
                query = query.Where(x => x.Color == color);
            }

            if(make != null)
            {
                query = query.Where(x => x.Make == make);
            }

            if (model != null)
            {
                query = query.Where(x => x.Model == model);
            }

            if (year != 0)
            {
                query = query.Where(x => x.Year == year);
            }

            int matches = query.Count(x => x.Id > 0);

            if (matches > 0)
            {
                return await query.ToListAsync();
            }

            return NotFound();
        }
    }
}
