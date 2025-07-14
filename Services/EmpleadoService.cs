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
using Microsoft.Extensions.Caching.Memory;

namespace LavacarSeLoChaneoADomicilio.Services
{
    public class EmpleadoService
    {// Servicio para manejar la lógica de negocio relacionada con los empleados
        private readonly IMemoryCache _cache;
        private const string CacheKey = "empleados";

        public EmpleadoService(IMemoryCache cache)
        {
            _cache = cache;
            if (!_cache.TryGetValue(CacheKey, out List<Empleado> _))
            {
                _cache.Set(CacheKey, new List<Empleado>());
            }
        }
        // Obtiene todos los empleados almacenados en caché
        public List<Empleado> GetAll() => _cache.Get<List<Empleado>>(CacheKey)!;

        public void Add(Empleado empleado)
        {
            var empleados = GetAll();
            empleados.Add(empleado);
            _cache.Set(CacheKey, empleados);
        }
        // Obtiene un empleado por su cédula
        public Empleado? GetByCedula(string cedula) =>
            GetAll().FirstOrDefault(e => e.Cedula == cedula);
        // Actualiza un empleado existente por su cédula original
        public void Update(string cedulaOriginal, Empleado empleadoActualizado)
        {
            var empleados = GetAll();
            var index = empleados.FindIndex(e => e.Cedula == cedulaOriginal);
            if (index >= 0)
            {
                empleados[index] = empleadoActualizado;
                _cache.Set(CacheKey, empleados);
            }
        }

        // Elimina un empleado por su cédula
        public void Delete(string cedula)
        {
            var empleados = GetAll();
            var empleado = empleados.FirstOrDefault(e => e.Cedula == cedula);
            if (empleado != null)
            {
                empleados.Remove(empleado);
                _cache.Set(CacheKey, empleados);
            }
        }
    }
}
