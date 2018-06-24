using FrbaHotel.Model.DAO;
using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Factura
    {
        public int? Id { get; set; }
        public DateTime Fecha { get; set; }
        public double Monto_Total { get; set; }
        public Estadia Estadia { get; set; }
        public FormaDePago Forma_Pago { get; set; }
        public string Detalle_Forma_Pago { get; set; }
        public List<ItemFactura> Items_Factura { get; set; }

        public Factura(DateTime Fecha, double Monto_Total, Estadia Estadia, FormaDePago Forma_Pago,
            string Detalle_Forma_Pago, List<ItemFactura> Items_Factura)
        {
            this.Fecha = Fecha;
            this.Monto_Total = Monto_Total;
            this.Estadia = Estadia;
            this.Forma_Pago = Forma_Pago;
            this.Detalle_Forma_Pago = Detalle_Forma_Pago;
            this.Items_Factura = Items_Factura;
            this.Estadia.Habitaciones = new HabitacionDAO().ObtenerHabitacionesDeEstadia(Estadia);
        }

        public void GuardarTXT()
        {
            FileUtils.CreateDirectory("facturas");
            FileUtils.EscribirFile("facturas", Contenido(), this.Id.Value.ToString("000000000000") + ".txt");
        }

        public String Contenido()
        {
            String factura = this.Id.Value.ToString("000000000000");
            Hotel hotel = Estadia.Habitaciones[0].Hotel;
            StringBuilder sb = new StringBuilder("CADENA DE HOTELES EL MONSTRUO DEL LAGO MASER\r\n").
                Append("www.emdlm.com.ar - rovaf_rop_sonebeurpa@emdlm.com.ar\r\n\r\n").
                Append("FACTURA NRO ").Append(factura).Append("\r\n").
                Append("FECHA: ").Append(this.Fecha.ToString("dd-MM-yyyy")).Append("\r\n\r\n").
                Append("DETALLES DE FACTURACIÓN\r\n");
            
            foreach (ItemFactura i in Items_Factura)
                sb.Append("\t").Append(i.Descripción).Append(" - CANT ").Append(i.Cantidad).
                    Append(" - USD ").Append(i.Precio).Append(" - TOTAL ").
                    Append("USD ").Append(i.Precio * i.Cantidad).Append("\r\n");

            sb.Append("\r\n\r\nTOTAL FACTURA: USD ").Append(this.Monto_Total).Append("\r\n\r\n")
                .Append("FORMA DE PAGO: ").Append(this.Forma_Pago.Descripción).Append("\r\n\r\n")
                .Append("MUCHAS GRACIAS POR LA VISITA - LO EXTRAÑAREMOS\r\n");
            return sb.ToString();
        }
    }
}
