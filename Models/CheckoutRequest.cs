using System;
using System.Collections.Generic;
using System.Text;

namespace TrabajoPracticoProgramacionMovilN6.Models
{
    public class SalidaProductoRequest
    {
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public List<DetalleMovimientoRequest> Detalles { get; set; } = new();
    }

    public class DetalleMovimientoRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
