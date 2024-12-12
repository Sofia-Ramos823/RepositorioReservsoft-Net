using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reservsoft.DATA;
using reservsoft.Models;

namespace reservsoft.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Apartamentos)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Apartamentos)
                .FirstOrDefaultAsync(m => m.NumReserva == id);

            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewBag.Apartamentos = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento");
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservas reservas)
        {
            // Establecer la fecha de reserva como la fecha actual
            reservas.FechaReserva = DateTime.Today;

            // Validar selección de apartamentos
            if (reservas.ApartamentoIds == null || reservas.ApartamentoIds.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar al menos un apartamento.");
                ViewBag.Apartamentos = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento");
                return View(reservas);
            }

            // Validar fechas
            if (reservas.FInicio >= reservas.FFin || reservas.FInicio < DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, "Las fechas de reserva son inválidas.");
                ViewBag.Apartamentos = new SelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento");
                return View(reservas);
            }

            // Obtener los apartamentos seleccionados
            reservas.Apartamentos = await _context.Apartamentos
                .Where(a => reservas.ApartamentoIds.Contains(a.IdApartamento))
                .ToListAsync();

            // Calcular el total de la reserva
            reservas.TotalReserva = CalcularTotalReserva(reservas.Apartamentos, reservas.FInicio, reservas.FFin);

            // Configurar estado de reserva predeterminado
            reservas.Estado = EstadoReserva.Pendiente;

            // Agregar la nueva reserva al contexto y guardar
            _context.Add(reservas);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Apartamentos)
                .FirstOrDefaultAsync(r => r.NumReserva == id);

            if (reservas == null)
            {
                return NotFound();
            }

            reservas.ApartamentoIds = reservas.Apartamentos.Select(a => a.IdApartamento).ToArray();
            ViewBag.Apartamentos = new MultiSelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento", reservas.ApartamentoIds);

            return View(reservas);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservas reservas)
        {
            if (id != reservas.NumReserva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingReserva = await _context.Reservas
                        .Include(r => r.Apartamentos)
                        .FirstOrDefaultAsync(r => r.NumReserva == id);

                    if (existingReserva == null)
                    {
                        return NotFound();
                    }

                    // Actualizar propiedades
                    existingReserva.Cliente = reservas.Cliente;
                    existingReserva.Apellido = reservas.Apellido;
                    existingReserva.TipoDoc = reservas.TipoDoc;
                    existingReserva.NumDoc = reservas.NumDoc;
                    existingReserva.Acompanantes = reservas.Acompanantes;
                    existingReserva.FInicio = reservas.FInicio;
                    existingReserva.FFin = reservas.FFin;
                    existingReserva.Estado = reservas.Estado;

                    // Actualizar apartamentos
                    existingReserva.Apartamentos.Clear();
                    existingReserva.Apartamentos = await _context.Apartamentos
                        .Where(a => reservas.ApartamentoIds.Contains(a.IdApartamento))
                        .ToListAsync();

                    // Recalcular el total de la reserva
                    existingReserva.TotalReserva = CalcularTotalReserva(existingReserva.Apartamentos, existingReserva.FInicio, existingReserva.FFin);

                    _context.Update(existingReserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservasExists(reservas.NumReserva))
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

            ViewBag.Apartamentos = new MultiSelectList(_context.Apartamentos, "IdApartamento", "TipoApartamento", reservas.ApartamentoIds);
            return View(reservas);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return Json(new { success = false, message = "No se puede eliminar esta reserva porque está asociada a un apartamento." });
        }

        private bool ReservasExists(int id)
        {
            return _context.Reservas.Any(e => e.NumReserva == id);
        }

        // POST: Calcular total
        [HttpPost]
        public async Task<IActionResult> CalcularTotal(int[] apartamentoIds, DateTime fInicio, DateTime fFin)
        {
            var apartamentos = await _context.Apartamentos
                .Where(a => apartamentoIds.Contains(a.IdApartamento))
                .ToListAsync();

            var total = CalcularTotalReserva(apartamentos, fInicio, fFin);

            return Json(new
            {
                rawValue = total,
                formattedValue = string.Format("$ {0:N0}", total)
            });
        }

        private double CalcularTotalReserva(ICollection<Apartamentos> apartamentos, DateTime fInicio, DateTime fFin)
        {
            var noches = (fFin - fInicio).Days;
            return apartamentos.Sum(a => a.Tarifa * noches);
        }
    }
}
