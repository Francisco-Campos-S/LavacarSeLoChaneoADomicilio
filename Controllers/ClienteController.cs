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
    public class ClienteController : Controller
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }

        // Acción para mostrar la lista de clientes con opción de búsqueda
        public IActionResult Index(string? busqueda)
        {
            var clientes = _service.GetAll();

            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.ToLower();
                clientes = clientes
                    .Where(c => c.Identificacion.ToLower().Contains(busqueda) ||
                                c.NombreCompleto.ToLower().Contains(busqueda) ||
                                c.Provincia.ToLower().Contains(busqueda) ||
                                c.Telefono.Contains(busqueda))
                    .ToList();
            }

            ViewBag.Busqueda = busqueda;
            return View(clientes);
        }

        // Mostrar formulario para crear cliente
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Cliente c)
        {
            if (!ModelState.IsValid)
                return View(c);

            // Validar que no exista otro cliente con la misma identificación
            if (_service.GetById(c.Identificacion) != null)
            {
                ModelState.AddModelError(nameof(c.Identificacion), "Ya existe un cliente con esta identificación.");
                return View(c);
            }

            _service.Add(c);
            return RedirectToAction("Index");
        }

        // Mostrar formulario para editar cliente
        public IActionResult Edit(string identificacion)
        {
            var cliente = _service.GetById(identificacion);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        public IActionResult Edit(string idOriginal, Cliente c)
        {
            if (!ModelState.IsValid)
                return View(c);

            var clienteExistente = _service.GetById(idOriginal);
            if (clienteExistente == null)
            {
                ModelState.AddModelError("", "El cliente que intenta modificar no existe.");
                return View(c);
            }

            // Verifica si se cambió la identificación y ya existe otro con esa nueva identificación
            if (idOriginal != c.Identificacion && _service.GetById(c.Identificacion) != null)
            {
                ModelState.AddModelError(nameof(c.Identificacion), "Ya existe otro cliente con esta nueva identificación.");
                return View(c);
            }

            _service.Update(idOriginal, c);
            return RedirectToAction("Index");
        }

        // Mostrar detalles del cliente
        public IActionResult Details(string identificacion)
        {
            var cliente = _service.GetById(identificacion);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // Eliminar cliente
        public IActionResult Delete(string identificacion)
        {
            var cliente = _service.GetById(identificacion);
            if (cliente == null)
                return NotFound();

            _service.Delete(identificacion);
            TempData["Mensaje"] = "Cliente eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
