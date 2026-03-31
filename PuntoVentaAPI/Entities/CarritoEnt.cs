using System.Collections.Generic;

namespace PuntoVentaAPI.Entities
{
    public class Carrito
    {
        public List<CarritoItemEnt> Items { get; set; } = new List<CarritoItemEnt>();

        public decimal Subtotal => Items.Sum(item => item.Producto.Precio * item.Cantidad);

        public decimal IVA => Subtotal * 0.13m;  // 13% IVA

        public decimal Total => Subtotal + IVA;
    }
}
