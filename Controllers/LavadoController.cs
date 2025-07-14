/*
UNIVERSIDAD ESTATAL A DISTANCIA
Curso: FUNDAMENTOS DE PROGRAMACION WEB
Código: 03075 
Proyecto #1: Lavacar se lo chaneo a domicilio
Tutor: Sigifredo Leitón Luna
Grupo: 04
Estudiante: Francisco Campos Sandi
Cédula: 114750560
II Cuatrimestre 2025
*/

using LavacarSeLoChaneoADomicilio.Models;
using LavacarSeLoChaneoADomicilio.Services;
using Microsoft.AspNetCore.Mvc;

namespace LavacarSeLoChaneoADomicilio.Controllers
{
    public class LavadoController : Controller
    {
        private readonly LavadoService _service;
        // Constructor que recibe el servicio de Lavado
        public LavadoController(LavadoService service)
        {
            _service = service;
        }

        public IActionResult Index(string? busqueda)
        {// Acción para mostrar la lista de lavados con opción de búsqueda
            var lavados = _service.GetAll();

            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.ToLower();
                lavados = lavados
                    .Where(l => l.PlacaVehiculo.ToLower().Contains(busqueda) ||
                                l.IdCliente.ToLower().Contains(busqueda) ||
                                l.IdEmpleado.ToLower().Contains(busqueda) ||
                                l.Tipo.ToString().ToLower().Contains(busqueda) ||
                                l.Estado.ToString().ToLower().Contains(busqueda))
                    .ToList();
            }

            ViewBag.Busqueda = busqueda;
            return View(lavados);
        }
        // Mostrar formulario para crear un nuevo lavado
        [HttpGet]
        public IActionResult Create()
        {
            var proximoId = _service.GetNextId();
            var nuevo = new Lavado
            {
                Id = proximoId,
                Tipo = TipoLavado.Basico,
                Precio = 8000,
                Detalle = "Lavado, aspirado y encerado"
            };
            return View(nuevo);
        }
        // Acción para crear un nuevo lavado
        [HttpPost]
        public IActionResult Create(Lavado l, bool actualizar = false)
        {
            if (actualizar)
            {
                l.Detalle = _service.ObtenerDetallePorTipo(l.Tipo);
                l.Precio = _service.ObtenerPrecioPorTipo(l.Tipo);

                ModelState.Clear();
                return View(l);
            }

            ValidarLavado(l);

            if (!ModelState.IsValid)
                return View(l);

            _service.Add(l);
            return RedirectToAction("Index");
        }
        // Mostrar formulario para editar un lavado existente
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var l = _service.GetById(id);
            if (l == null) return NotFound();
            return View(l);
        }

        [HttpPost]
        public IActionResult Edit(int id, Lavado l, string? actualizar)
        {// Acción para editar un lavado existente
            if (id != l.Id)
                return BadRequest("ID no coincide");

            var lavadoOriginal = _service.GetById(id);
            if (lavadoOriginal == null)
                return NotFound();

            if (!string.IsNullOrEmpty(actualizar) && actualizar == "true")
            {
                l.Detalle = _service.ObtenerDetallePorTipo(l.Tipo);
                l.Precio = _service.ObtenerPrecioPorTipo(l.Tipo);

                ModelState.Clear();
                return View(l);
            }

            ValidarLavado(l);

            if (!ModelState.IsValid)
                return View(l);

            lavadoOriginal.PlacaVehiculo = l.PlacaVehiculo;
            lavadoOriginal.IdCliente = l.IdCliente;
            lavadoOriginal.IdEmpleado = l.IdEmpleado;
            lavadoOriginal.Tipo = l.Tipo;
            lavadoOriginal.Estado = l.Estado;
            lavadoOriginal.Precio = l.Precio;
            lavadoOriginal.Detalle = l.Detalle;

            _service.Update(id, lavadoOriginal);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var l = _service.GetById(id);
            if (l == null) return NotFound();
            return View(l);
        }

        public IActionResult Delete(int id) 
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }


        // Método privado para validar campos manualmente
        private void ValidarLavado(Lavado l)
        {
            // Validar PlacaVehiculo (no vacío, longitud mínima 3)
            if (string.IsNullOrWhiteSpace(l.PlacaVehiculo) || l.PlacaVehiculo.Length < 3)
            {
                ModelState.AddModelError(nameof(l.PlacaVehiculo), "La placa del vehículo es obligatoria y debe tener al menos 3 caracteres.");
            }

            // Validar IdCliente (no vacío)
            if (string.IsNullOrWhiteSpace(l.IdCliente))
            {
                ModelState.AddModelError(nameof(l.IdCliente), "La identificación del cliente es obligatoria.");
            }

            // Validar IdEmpleado (no vacío)
            if (string.IsNullOrWhiteSpace(l.IdEmpleado))
            {
                ModelState.AddModelError(nameof(l.IdEmpleado), "La identificación del empleado es obligatoria.");
            }

            // Validar Tipo válido
            if (!Enum.IsDefined(typeof(TipoLavado), l.Tipo))
            {
                ModelState.AddModelError(nameof(l.Tipo), "Seleccione un tipo de lavado válido.");
            }

            // Validar Estado válido
            if (!Enum.IsDefined(typeof(EstadoLavado), l.Estado))
            {
                ModelState.AddModelError(nameof(l.Estado), "Seleccione un estado válido.");
            }

            // Validar Precio positivo
            if (l.Precio <= 0)
            {
                ModelState.AddModelError(nameof(l.Precio), "El precio debe ser mayor que cero.");
            }

            // Validar Detalle no vacío
            if (string.IsNullOrWhiteSpace(l.Detalle))
            {
                ModelState.AddModelError(nameof(l.Detalle), "El detalle es obligatorio.");
            }
        }
    }
}
