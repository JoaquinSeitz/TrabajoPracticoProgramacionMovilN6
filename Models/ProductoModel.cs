using System;
using System.Collections.Generic;
using System.Text;

namespace TrabajoPracticoProgramacionMovilN6.Models
{
    public class ProductoModel
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}