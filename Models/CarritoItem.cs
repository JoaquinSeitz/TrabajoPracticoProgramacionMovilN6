using System;
using System.Collections.Generic;
using System.Text;

namespace TrabajoPracticoProgramacionMovilN6.Models
{
    public class CarritoItem
    {
        public ProductoModel Producto { get; set; } = new();
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.Precio * Cantidad;
    }
}
