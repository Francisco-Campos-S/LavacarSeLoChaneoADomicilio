/*
UNIVERSIDAD ESTATAL A DISTANCIA
Curso: FUNDAMENTOS DE PROGRAMACION WEB
Código: 03075 
Proyecto #1: Lavacar se lo chaneo  a domicilio
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
    public class VehiculoController : Controller
    {
        private readonly VehiculoService _service;
        // Constructor que recibe el servicio de vehículo
        public VehiculoController(VehiculoService service)
        {
            _service = service;
        }

        public IActionResult Index(string? busqueda)
        {
            var vehiculos = _service.GetAll();
            // Si se proporciona una búsqueda, filtrar la lista de vehículos
            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.ToLower();
                vehiculos = vehiculos
                    .Where(v => v.Placa.ToLower().Contains(busqueda) ||
                                v.Marca.ToLower().Contains(busqueda) ||
                                v.Color.ToLower().Contains(busqueda))
                    .ToList();
            }

            ViewBag.Busqueda = busqueda;
            return View(vehiculos);
        }
        // Acción para crear un nuevo vehículo
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Vehiculo v)
        {
            if (!ModelState.IsValid) return View(v);
            _service.Add(v);
            return RedirectToAction("Index");
        }
        // Acción para editar un vehículo existente
        public IActionResult Edit(string placa)
        {
            var v = _service.GetByPlaca(placa);
            if (v == null) return NotFound();
            return View(v);
        }
        // Acción para actualizar un vehículo existente
        [HttpPost]
        public IActionResult Edit(string placaOriginal, Vehiculo v)
        {
            if (!ModelState.IsValid) return View(v);
            _service.Update(placaOriginal, v);
            return RedirectToAction("Index");
        }
        // Acción para ver los detalles de un vehículo
        public IActionResult Details(string placa)
        {
            var v = _service.GetByPlaca(placa);
            if (v == null) return NotFound();
            return View(v);
        }
        // Acción para eliminar un vehículo existente
        public IActionResult Delete(string placa)
        {
            _service.Delete(placa);
            return RedirectToAction("Index");
        }
    }
}
