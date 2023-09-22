using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria_2._0
{
    internal class DetallePresupuesto
    {
        public DetallePresupuesto(Producto producto, int cantidad)
        {
            Producto = producto;
            Cantidad = cantidad;
        }

        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public double CalcularSubtotal()
        {           
            return Producto.Precio * Cantidad;
        }
    }
}
