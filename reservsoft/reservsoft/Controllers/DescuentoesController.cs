using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reservsoft.DATA;
using reservsoft.Models;

namespace reservsoft.Controllers
{
    public class DescuentoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DescuentoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Descuentoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Descuentos.ToListAsync());
        }

        // Acción para actualizar el estado del descuento
        [HttpPost]
        public async Task<IActionResult> UpdateEstado(int id, bool Estado)
        {
            var descuento = await _context.Descuentos.FindAsync(id);
            if (descuento == null)
            {
                return NotFound();
            }

            descuento.Estado = Estado; // Actualizamos el estado
            _context.Update(descuento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirigir al índice después de la actualización
        }

        // GET: Descuentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var descuento = await _context.Descuentos
                .FirstOrDefaultAsync(m => m.IdDescuento == id);
            if (descuento == null)
            {
                return NotFound();
            }

            return View(descuento);
        }

        // GET: Descuentoes/Create
        public IActionResult Create()
        {
            ViewData["IdApartamento"] = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento");
            return View();
        }

        // POST: Descuentoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDescuento,IdApartamento,Descripcion,Precio,Descuentos,FechaInicio,FechaFin,Estado")] Descuento descuento)
        {
            if (ModelState.IsValid)
            {
                if (descuento.ValidarFechas())
                {
                    _context.Add(descuento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio.");
                }
            }
            ViewData["IdApartamento"] = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento", descuento.IdApartamento);
            return View(descuento);
        }

        // GET: Descuentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var descuento = await _context.Descuentos.FindAsync(id);
            if (descuento == null)
            {
                return NotFound();
            }
            ViewData["IdApartamento"] = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento", descuento.IdApartamento);
            return View(descuento);
        }

        // POST: Descuentoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDescuento,IdApartamento,Descripcion,Precio,Descuentos,FechaInicio,FechaFin,Estado")] Descuento descuento)
        {
            if (id != descuento.IdDescuento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(descuento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DescuentoExists(descuento.IdDescuento))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdApartamento"] = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento", descuento.IdApartamento);
            return View(descuento);
        }

        // POST: Descuentoes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var descuento = await _context.Descuentos.FindAsync(id);
            if (descuento == null)
            {
                return NotFound();
            }

            _context.Descuentos.Remove(descuento);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool DescuentoExists(int id)
        {
            return _context.Descuentos.Any(e => e.IdDescuento == id);
        }
    }
}