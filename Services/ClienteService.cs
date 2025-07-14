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
{// Servicio para manejar la lógica de negocio relacionada con los clientes
    public class ClienteService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "clientes";

        public ClienteService(IMemoryCache cache)
        {
            _cache = cache;
            if (!_cache.TryGetValue(CacheKey, out List<Cliente> _))
            {
                _cache.Set(CacheKey, new List<Cliente>());
            }
        }
        // Obtiene todos los clientes almacenados en caché
        public List<Cliente> GetAll() => _cache.Get<List<Cliente>>(CacheKey)!;

        public void Add(Cliente c)
        {
            var lista = GetAll();
            lista.Add(c);
            _cache.Set(CacheKey, lista);
        }
        // Obtiene un cliente por su identificación
        public Cliente? GetById(string identificacion) =>
            GetAll().FirstOrDefault(c => c.Identificacion == identificacion);

        public void Update(string idOriginal, Cliente actualizado)
        {
            var lista = GetAll();
            var index = lista.FindIndex(c => c.Identificacion == idOriginal);
            if (index >= 0)
            {
                lista[index] = actualizado;
                _cache.Set(CacheKey, lista);
            }
        }
        // Elimina un cliente por su identificación
        public void Delete(string identificacion)
        {
            var lista = GetAll();
            var cliente = lista.FirstOrDefault(c => c.Identificacion == identificacion);
            if (cliente != null)
            {
                lista.Remove(cliente);
                _cache.Set(CacheKey, lista);
            }
        }
    }
}
