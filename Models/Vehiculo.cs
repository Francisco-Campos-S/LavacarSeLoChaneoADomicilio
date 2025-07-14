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
namespace LavacarSeLoChaneoADomicilio.Models
{
    public class Vehiculo
    {
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Traccion { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public DateTime? UltimaFechaAtencion { get; set; }
        public bool TratamientoNanoCeramico { get; set; }
    }
}
