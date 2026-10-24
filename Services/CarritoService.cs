namespace TrabajoPracticoProgramacionMovilN6.Services
{
    public class CarritoService
    {
        // Evento para avisarle a los componentes (como un badge en el menú) que el carrito se actualizó
        public event Action? OnCarritoActualizado;

        // Lista interna que mantiene los productos en memoria
        private readonly List<ItemCarrito> _items = new();

        // Propiedad pública para leer los items desde las vistas
        public IReadOnlyList<ItemCarrito> Items => _items.AsReadOnly();

        public void AgregarAlCarrito(int productoId, string nombre, decimal precio, int stockDisponible, int cantidad = 1)
        {
            var itemExistente = _items.FirstOrDefault(i => i.ProductoId == productoId);

            if (itemExistente != null)
            {
                // Verificamos no pasarnos del stock
                if (itemExistente.Cantidad + cantidad <= stockDisponible)
                {
                    itemExistente.Cantidad += cantidad;
                }
                else
                {
                    throw new InvalidOperationException("No hay suficiente stock disponible.");
                }
            }
            else
            {
                if (cantidad <= stockDisponible)
                {
                    _items.Add(new ItemCarrito
                    {
                        ProductoId = productoId,
                        Nombre = nombre,
                        PrecioUnitario = precio,
                        Cantidad = cantidad,
                        StockMaximo = stockDisponible
                    });
                }
            }

            NotificarCambio();
        }

        public void RemoverDelCarrito(int productoId)
        {
            var item = _items.FirstOrDefault(i => i.ProductoId == productoId);
            if (item != null)
            {
                _items.Remove(item);
                NotificarCambio();
            }
        }

        public void VaciarCarrito()
        {
            _items.Clear();
            NotificarCambio();
        }

        public decimal ObtenerTotal() => _items.Sum(i => i.Subtotal);

        public int ObtenerCantidadItems() => _items.Sum(i => i.Cantidad);

        private void NotificarCambio() => OnCarritoActualizado?.Invoke();
    }

    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int StockMaximo { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}