using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class ItemFactura
    {
        public int? Id { get; private set; }
        public Consumo Consumo { get; set; }
        public double Precio { get; set; }
        public string Descripción { get; set; }
        public int Cantidad { get; set; }

        public ItemFactura(double Precio, string Descripción, int Cantidad)
        {
            this.Precio = Precio;
            this.Descripción = Descripción;
            this.Cantidad = Cantidad;
        }

        public ItemFactura(Consumo Consumo)
        {
            this.Consumo = Consumo;
            this.Precio = Consumo.Consumible.Precio;
            this.Descripción = Consumo.Consumible.Descripción;
            this.Cantidad = Consumo.Cantidad;
        }
    }
}
