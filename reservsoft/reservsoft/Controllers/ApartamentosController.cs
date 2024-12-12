using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservsoft.DATA;
using reservsoft.Models;

namespace reservsoft.Controllers
{
    public class ApartamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApartamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Apartamentos
        public async Task<IActionResult> Index()
        {
            var apartamentos = await _context.Apartamentos.ToListAsync();
            return View(apartamentos);
        }

        // GET: Apartamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del apartamento es nulo.");
            }

            var apartamento = await _context.Apartamentos
                .Include(a => a.Reserva)
                .FirstOrDefaultAsync(m => m.IdApartamento == id);

            if (apartamento == null)
            {
                return NotFound($"No se encontró un apartamento con el ID {id}.");
            }

            return View(apartamento);
        }

        // GET: Apartamentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdApartamento,TipoApartamento,Descripcion,Capacidad,Tamaño,Piso,Tarifa")] Apartamentos apartamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apartamento);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Apartamento creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error al crear el apartamento.";
            return View(apartamento);
        }

        // GET: Apartamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID del apartamento es nulo.");
            }

            var apartamento = await _context.Apartamentos.FindAsync(id);
            if (apartamento == null)
            {
                return NotFound($"No se encontró un apartamento con el ID {id}.");
            }

            return View(apartamento);
        }

        // POST: Apartamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdApartamento,TipoApartamento,Descripcion,Capacidad,Tamaño,Piso,Tarifa")] Apartamentos apartamento)
        {
            if (id != apartamento.IdApartamento)
            {
                return NotFound("El ID proporcionado no coincide con el del apartamento.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartamento);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Apartamento actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartamentosExists(apartamento.IdApartamento))
                    {
                        TempData["ErrorMessage"] = "El apartamento no existe.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error al actualizar el apartamento.";
            return View(apartamento);
        }

        // POST: Apartamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartamento = await _context.Apartamentos.FindAsync(id);
            if (apartamento == null)
            {
                return Json(new { success = false, message = "El apartamento no fue encontrado." });
            }

            try
            {
                _context.Apartamentos.Remove(apartamento);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Apartamento eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, message = "No se puede eliminar este apartamento debido a que está siendo utilizado en otros registros." });
            }
        }

        private bool ApartamentosExists(int id)
        {
            return _context.Apartamentos.Any(e => e.IdApartamento == id);
        }

        // Método adicional: Obtener mobiliarios asociados a un apartamento
        [HttpGet]
        public async Task<IActionResult> GetMobiliarios(int id)
        {
            try
            {
                var mobiliarios = await _context.Mobiliarios
                    .Where(m => m.IdApartamento == id)
                    .Select(m => new { m.IdMobiliario, m.Nombre, m.IdentMobiliario, m.Estado })
                    .ToListAsync();

                return Json(mobiliarios);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Método adicional: Cambiar estado de un mobiliario
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoMobiliario(int idMobiliario)
        {
            var mobiliario = await _context.Mobiliarios.FindAsync(idMobiliario);
            if (mobiliario == null)
            {
                return Json(new { success = false });
            }

            mobiliario.Estado = (EstadoMobiliario)(((int)mobiliario.Estado + 1) % Enum.GetValues(typeof(EstadoMobiliario)).Length);

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Método adicional: Transferir un mobiliario a otro apartamento
        [HttpPost]
        public async Task<IActionResult> TransferirMobiliario(int idMobiliario, int nuevoIdApartamento)
        {
            var mobiliario = await _context.Mobiliarios.FindAsync(idMobiliario);
            if (mobiliario == null)
            {
                return Json(new { success = false });
            }

            var nuevoApartamento = await _context.Apartamentos.FindAsync(nuevoIdApartamento);
            if (nuevoApartamento == null)
            {
                return Json(new { success = false });
            }

            mobiliario.IdApartamento = nuevoIdApartamento;
            mobiliario.Estado = EstadoMobiliario.Transferido;

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}