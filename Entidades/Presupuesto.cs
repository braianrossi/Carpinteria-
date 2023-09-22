using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria_2._0
{
    internal class Presupuesto
    {
        public int PresupuestoNro { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public double CostoMO { get; set; }
        public double Descuento { get; set; }
        public DateTime FechaBaja { get; set; }
        public List<DetallePresupuesto> Detalles { get; set; }

        public Presupuesto() 
        {
            Detalles = new List<DetallePresupuesto>();
        }

        public void AgregarDetalle(DetallePresupuesto det)
        {
            Detalles.Add(det);
        }
        public void QuitarDetalle(int det)
        {
            Detalles.RemoveAt(det);
        }
        public double CalcularTotal()
        {
            double total = 0;

            foreach (DetallePresupuesto item in Detalles)
            {
                total += item.CalcularSubtotal();
            }
            return total;
        }
    }
}
