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
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoService _service;

        // Constructor principal para inicializar el servicio
        public EmpleadoController(EmpleadoService service) => _service = service;

        // Mostrar lista de empleados con filtro opcional de búsqueda
        public IActionResult Index(string? busqueda)
        {
            var empleados = _service.GetAll();

            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.ToLower();
                empleados = empleados
                    .Where(e => e.Cedula.ToLower().Contains(busqueda) ||
                                e.SalarioPorDia.ToString().Contains(busqueda) ||
                                e.DiasVacacionesAcumulados.ToString().Contains(busqueda))
                    .ToList();
            }

            ViewBag.Busqueda = busqueda;
            return View(empleados);
        }

        // Mostrar formulario para crear empleado
        public IActionResult Create() => View();

        [HttpPost]
        // Crear un nuevo empleado
        public IActionResult Create(Empleado emp)
        {
            if (!ModelState.IsValid)
                return View(emp);

            _service.Add(emp);
            return RedirectToAction("Index");
        }

        // Mostrar formulario para editar empleado
        public IActionResult Edit(string cedula)
        {
            var emp = _service.GetByCedula(cedula);
            if (emp == null)
                return NotFound();

            return View(emp);
        }

        [HttpPost]
        // Actualizar empleado existente
        public IActionResult Edit(string cedulaOriginal, Empleado emp)
        {
            if (!ModelState.IsValid)
                return View(emp);

            _service.Update(cedulaOriginal, emp);
            return RedirectToAction("Index");
        }

        // Eliminar empleado
        public IActionResult Delete(string cedula)
        {
            _service.Delete(cedula);
            return RedirectToAction("Index");
        }

        // Mostrar detalles de un empleado
        public IActionResult Details(string cedula)
        {
            var emp = _service.GetByCedula(cedula);
            if (emp == null)
                return NotFound();

            return View(emp);
        }
    }
}
