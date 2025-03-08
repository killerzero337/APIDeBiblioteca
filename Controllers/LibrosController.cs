using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDeBiblioteca.Models;

namespace APIDeBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly Contexto _context;

        public LibrosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        [HttpGet("titulo/{titulo}")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorTitulo(string titulo)
        {
            var libros = await _context.Libros
                                       .Where(l => l.Titulo.Contains(titulo))
                                       .ToListAsync();
            if (!libros.Any())
            {
                return NotFound();
            }
            return libros;
        }

        // GET: api/Libros/autor/{autor}
        [HttpGet("autor/{autor}")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorAutor(string autor)
        {
            var libros = await _context.Libros
                                       .Where(l => l.Autor.Contains(autor))
                                       .ToListAsync();

            if (!libros.Any())
            {
                return NotFound();
            }

            return libros;
        }

        // GET: api/Libros/genero/{genero}
        [HttpGet("genero/{genero}")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorGenero(string genero)
        {
            var libros = await _context.Libros
                                       .Where(l => l.Genero.Contains(genero))
                                       .ToListAsync();

            if (!libros.Any())
            {
                return NotFound();
            }

            return libros;
        }

        // GET: api/Libros/disponible/{disponible}
        [HttpGet("disponible/{disponible}")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorDisponibilidad(bool disponible)
        {
            var libros = await _context.Libros
                                       .Where(l => l.Disponible == disponible)
                                       .OrderBy(l => l.Titulo)
                                       .ToListAsync();

            if (!libros.Any())
            {
                return NotFound();
            }

            return libros;
        }

        // Get: api/Libros/fechaPublicacion/{fechaPublicacion}
        [HttpGet("fechaPublicacion/{fechaPublicacion}")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorFechaPublicacion(DateTime fechaPublicacion)
        {
            var libros = await _context.Libros
                                       .Where(l => l.FechaPublicacion == DateOnly.FromDateTime(fechaPublicacion))
                                       .ToListAsync();

            if (!libros.Any())
            {
                return NotFound();
            }

            return libros;
        }

        // Get: api/Libros/fechaPublicacion/?desde={fechaPublicacionDesde}&hasta={fechaPublicacionHasta}
        [HttpGet("fechaPublicacion")]
        public async Task<IActionResult> GetLibrosPorRangoDeFechas([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            if (desde == null || hasta == null)
            {
                return BadRequest("Debes proporcionar ambas fechas (desde y hasta).");
            }

            DateOnly desdeDateOnly = DateOnly.FromDateTime(desde.Value);
            DateOnly hastaDateOnly = DateOnly.FromDateTime(hasta.Value);

            var libros = await _context.Libros
                .Where(l => l.FechaPublicacion >= desdeDateOnly && l.FechaPublicacion <= hastaDateOnly)
                .ToListAsync();

            return libros.Any() ? Ok(libros) : NotFound("No se encontraron libros en el rango de fechas.");
        }




        // PUT: api/Libros/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }

            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
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

        // POST: api/Libros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLibro", new { id = libro.Id }, libro);
        }

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}
