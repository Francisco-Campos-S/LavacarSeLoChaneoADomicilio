using LavacarSeLoChaneoADomicilio.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LavacarSeLoChaneoADomicilio.Services
{
    public class LavadoService
    {// Servicio para manejar la lógica de negocio relacionada con los lavados
        private readonly IMemoryCache _cache;
        private const string CacheKey = "lavados";

        public LavadoService(IMemoryCache cache)
        {
            _cache = cache;
            if (!_cache.TryGetValue(CacheKey, out List<Lavado> _))
            {
                _cache.Set(CacheKey, new List<Lavado>());
            }
        }
        // Obtiene el siguiente ID disponible para un nuevo lavado
        public int GetNextId()
        {
            var lista = GetAll();
            return lista.Count == 0 ? 1 : lista.Max(l => l.Id) + 1;
        }
        // Obtiene todos los lavados almacenados en caché
        public List<Lavado> GetAll() => _cache.Get<List<Lavado>>(CacheKey)!;

        public void Add(Lavado lavado)
        {
            lavado.Id = GetNextId();

            if (lavado.Tipo != TipoLavado.LaJoya)
            {
                lavado.Precio = ObtenerPrecioPorTipo(lavado.Tipo);
                lavado.Detalle = ObtenerDetallePorTipo(lavado.Tipo);
            }

            var lista = GetAll();
            lista.Add(lavado);
            _cache.Set(CacheKey, lista);
        }
        // Obtiene un lavado por su ID
        public Lavado? GetById(int id) =>
            GetAll().FirstOrDefault(l => l.Id == id);
        // Actualiza un lavado existente por su ID
        public void Update(int id, Lavado actualizado)
        {
            var lista = GetAll();
            var index = lista.FindIndex(l => l.Id == id);
            if (index >= 0)
            {
                actualizado.Id = id;

                if (actualizado.Tipo != TipoLavado.LaJoya)
                {
                    actualizado.Precio = ObtenerPrecioPorTipo(actualizado.Tipo);
                    actualizado.Detalle = ObtenerDetallePorTipo(actualizado.Tipo);
                }

                lista[index] = actualizado;
                _cache.Set(CacheKey, lista);
            }
        }
        // Elimina un lavado por su ID
        public void Delete(int id)
        {
            var lista = GetAll();
            var l = lista.FirstOrDefault(x => x.Id == id);
            if (l != null)
            {
                lista.Remove(l);
                _cache.Set(CacheKey, lista);
            }
        }
        // Valida un lavado para asegurarse de que cumple con las reglas de negocio
        public decimal ObtenerPrecioPorTipo(TipoLavado tipo)
        {
            return tipo switch
            {
                TipoLavado.Basico => 8000,
                TipoLavado.Premium => 12000,
                TipoLavado.Deluxe => 20000,
                TipoLavado.LaJoya => 0, // Precio a convenir
                _ => 0
            };
        }
        // Obtiene el detalle del lavado según su tipo
        public string ObtenerDetallePorTipo(TipoLavado tipo)
        {
            return tipo switch
            {
                TipoLavado.Basico => "Lavado, aspirado y encerado",
                TipoLavado.Premium => "Lavado, aspirado y encerado y limpieza profunda de asientos",
                TipoLavado.Deluxe => "Lavado, aspirado y encerado, limpieza profunda de asientos, corrección de pintura. Opción productos para lavado con tratamiento nanocerámico",
                TipoLavado.LaJoya => "Incluye todo más detalles a convenir, pulidos, tratamientos hidrofóbicos, entre otros",
                _ => string.Empty
            };
        }

    }
}
