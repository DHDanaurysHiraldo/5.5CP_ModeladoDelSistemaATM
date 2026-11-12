using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_5CP_ModeladoDelSistemaATM
{
    internal class Transaccion
    {
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }


        public string MostrarDetalles()
        {
            return $"{Fecha:dd/MM/yyyy HH:mm} | {Tipo,-10} | RD$ {Monto,10:N2}";
        }
    }
}
