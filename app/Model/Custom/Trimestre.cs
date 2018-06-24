using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.Custom
{
    public class Trimestre
    {
        public int Año { get; set; }
        public int Qn { get; set; }
        public DateTime Inicio_Trimestre { get; set; }
        public DateTime Fin_Trimestre { get; set; }

        public Trimestre(int Año, int Qn)
        {
            this.Año = Año;
            this.Qn = Qn;
            onYearChanged();
        }

        public void onYearChanged()
        {
            if (Qn == -1) return;

            switch (Qn)
            {
                case 1:
                    Inicio_Trimestre = new DateTime(Año, 1, 1);
                    Fin_Trimestre = new DateTime(Año, 3, 31);
                break;
                case 2:
                    Inicio_Trimestre = new DateTime(Año, 4, 1);
                    Fin_Trimestre = new DateTime(Año, 6, 30);
                break;
                case 3:
                    Inicio_Trimestre = new DateTime(Año, 7, 1);
                    Fin_Trimestre = new DateTime(Año, 9, 30);
                break;
                case 4:
                    Inicio_Trimestre = new DateTime(Año, 10, 1);
                    Fin_Trimestre = new DateTime(Año, 12, 31);
                break;
                default:
                throw new Exception("Trimestre inválido");
            }
        }

        public override string ToString()
        {
            if (Qn == -1)
                return "- Seleccione un trimestre - ";

            string name;
            switch (Qn)
            {
                case 1: name = "Primer"; break;
                case 2: name = "Segundo"; break;
                case 3: name = "Tercer"; break;
                case 4: name = "Cuarto"; break;
                default: throw new Exception("Trimestre inválido");
            }

            StringBuilder sb = new StringBuilder(name).Append(" trimestre de ")
                .Append(Año).Append(" - ").Append(Inicio_Trimestre.ToString("dd/MM/yyyy"))
                .Append(" al ").Append(Fin_Trimestre.ToString("dd/MM/yyyy"));

            return sb.ToString();
        }
    }
}
