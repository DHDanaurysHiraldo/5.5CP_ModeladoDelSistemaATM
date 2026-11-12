namespace _5_5CP_ModeladoDelSistemaATM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("  CASO 5.5 — MODELADO ATM: CLASES Y OBJETOS");
            Console.WriteLine("=================================================");
            Console.WriteLine();

            Cuenta cuentaJuan = new Cuenta();
            Cuenta cuentaMaria = new Cuenta();

            // ===== Asignar propiedades =====
            cuentaJuan.NumeroCuenta = "10024589";
            cuentaJuan.Saldo = 15500.50m;
            cuentaJuan.Activa = true;

            cuentaMaria.NumeroCuenta = "20031177";
            cuentaMaria.Saldo = 3000.00m;
            cuentaMaria.Activa = false;

            Console.WriteLine("1) ESTADO INICIAL DE LAS CUENTAS");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"cuentaJuan  -> No. {cuentaJuan.NumeroCuenta} | Saldo: RD$ {cuentaJuan.ConsultarSaldo():N2} | Activa: {cuentaJuan.Activa}");
            Console.WriteLine($"cuentaMaria -> No. {cuentaMaria.NumeroCuenta} | Saldo: RD$ {cuentaMaria.ConsultarSaldo():N2} | Activa: {cuentaMaria.Activa}");
            Console.WriteLine();

            Console.WriteLine("2) OPERACIONES SOBRE cuentaJuan");
            Console.WriteLine("-------------------------------------------------");

            decimal montoDeposito = 1000.00m;
            if (cuentaJuan.Depositar(montoDeposito))
                Console.WriteLine($"Depósito de RD$ {montoDeposito:N2}  -> EXITOSO   | Saldo: RD$ {cuentaJuan.ConsultarSaldo():N2}");
            else
                Console.WriteLine($"Depósito de RD$ {montoDeposito:N2}  -> RECHAZADO | Saldo: RD$ {cuentaJuan.ConsultarSaldo():N2}");

            decimal montoRetiro = 2000.00m;
            if (cuentaJuan.Retirar(montoRetiro))
                Console.WriteLine($"Retiro de RD$ {montoRetiro:N2}    -> EXITOSO   | Saldo: RD$ {cuentaJuan.ConsultarSaldo():N2}");
            else
                Console.WriteLine($"Retiro de RD$ {montoRetiro:N2}    -> RECHAZADO | Saldo: RD$ {cuentaJuan.ConsultarSaldo():N2}");

            decimal montoExcesivo = 50000.00m;
            if (!cuentaJuan.TieneFondos(montoExcesivo))
                Console.WriteLine($"Retiro de RD$ {montoExcesivo:N2}   -> RECHAZADO: fondos insuficientes");

            decimal montoInvalido = -500.00m;
            if (!cuentaJuan.EsMontoValido(montoInvalido))
                Console.WriteLine($"Depósito de RD$ {montoInvalido:N2}   -> RECHAZADO: el monto debe ser mayor que cero");
            Console.WriteLine();

            Console.WriteLine("3) OPERACIÓN SOBRE cuentaMaria (cuenta inactiva)");
            Console.WriteLine("-------------------------------------------------");
            if (!cuentaMaria.Depositar(500.00m))
                Console.WriteLine($"Depósito de RD$ 500.00    -> RECHAZADO: la cuenta no está activa | Saldo: RD$ {cuentaMaria.ConsultarSaldo():N2}");
            Console.WriteLine();

            // ===== Objetos Transaccion =====
            Transaccion depositoHoy = new Transaccion();
            depositoHoy.Tipo = "Depósito";
            depositoHoy.Monto = montoDeposito;
            depositoHoy.Fecha = DateTime.Now;

            Transaccion retiroHoy = new Transaccion();
            retiroHoy.Tipo = "Retiro";
            retiroHoy.Monto = montoRetiro;
            retiroHoy.Fecha = DateTime.Now;

            Console.WriteLine("4) PROPIEDADES DEL OBJETO retiroHoy");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Tipo:  {retiroHoy.Tipo}");
            Console.WriteLine($"Monto: RD$ {retiroHoy.Monto:N2}");
            Console.WriteLine($"Fecha: {retiroHoy.Fecha:dd/MM/yyyy}");
            Console.WriteLine();

            Console.WriteLine("5) COMPROBANTES CON MostrarDetalle()");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(depositoHoy.MostrarDetalle());
            Console.WriteLine(retiroHoy.MostrarDetalle());
            Console.WriteLine();

            // ===== Clase vs. objeto =====
            Console.WriteLine("6) CLASE vs. OBJETO");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Ambas cuentas salen de la misma clase Cuenta,");
            Console.WriteLine("pero cada objeto mantiene su propio estado:");
            Console.WriteLine($"Saldo final cuentaJuan:  RD$ {cuentaJuan.ConsultarSaldo():N2}");
            Console.WriteLine($"Saldo final cuentaMaria: RD$ {cuentaMaria.ConsultarSaldo():N2}");
            Console.WriteLine();
            Console.WriteLine("Fin de la demostración...");
        }
    }
}
