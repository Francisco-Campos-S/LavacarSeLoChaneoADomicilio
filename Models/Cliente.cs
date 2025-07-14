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
    public class Cliente
    {
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Canton { get; set; } = string.Empty;
        public string Distrito { get; set; } = string.Empty;
        public string DireccionExacta { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string PreferenciaLavado { get; set; } = string.Empty; // semanal, quincenal, etc.
    }
}
