using System;
using System.Collections.Generic;
using System.Linq;
namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            CuentaBancaria miEntidad = new CuentaBancaria();

            int continuar = 1;
            Console.WriteLine("BIENVENIDO AL SISTEMA");
            while (continuar > 0)
            {

                Console.WriteLine("ingrese un Valor numerico: " +
                    "{0}  1 para dar de Alta una cuenta " +
                    "{0}  2 para Extraccion " +
                    "{0}  3 para Deposito  " +
                    "{0}  4 para dar de Baja una cuenta " +
                    "{0}  5 para imprimir lista de operaciones del sistema" +
                    "{0}  6 para imprimir todas las Cuentas en el sistema",
                              Environment.NewLine);
                continuar = int.Parse(Console.ReadLine());
                string cbu = "";
                double monto = 0;
                switch (continuar)
                {
                    case 0:
                        break;
                    case 1:
                        Console.WriteLine("dar de Alta una cuenta bancaria");
                        Console.WriteLine("ingrese el CBU");
                        cbu = Console.ReadLine();
                        Console.WriteLine("ingrese el Nombre del Usuario");
                        var Nombre = Console.ReadLine();
                        Console.WriteLine("ingrese el Saldo inicial");
                        double saldoInicial = Double.Parse(Console.ReadLine());

                        Console.WriteLine(miEntidad.AltaCuenta(saldoInicial, Nombre, cbu));
                        break;
                    case 2:
                        Console.WriteLine("Extraccion");
                        Console.WriteLine("Ingrese el CBU");
                        cbu = Console.ReadLine();
                        Console.WriteLine("Ingrese el Monto a retirar");
                        monto = Double.Parse(Console.ReadLine());
                        Console.WriteLine(miEntidad.Extraccion(cbu, monto));
                        break;
                    case 3:
                        Console.WriteLine("Deposito");
                        Console.WriteLine("Ingrese el CBU");
                        cbu = Console.ReadLine();
                        Console.WriteLine("Ingrese el Monto a depositar");
                        monto = Double.Parse(Console.ReadLine());
                        Console.WriteLine(miEntidad.Deposito(cbu, monto));
                        break;
                    case 4:
                        Console.WriteLine("dar de baja una cuenta bancaria");
                        Console.WriteLine("Ingrese el CBU");
                        cbu = Console.ReadLine();
                        Console.WriteLine(miEntidad.BajaCuenta(cbu));
                        break;
                    case 5:
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", "Usuario", "Operacion", "Monto", "Saldo", "Fecha");
                        foreach (var c in miEntidad.ListOperaciones.OrderByDescending(x => x.Fecha))
                        {
                            var usuario = miEntidad.ListCuentas.FirstOrDefault(x => x.CBU == c.CBU).Usuario;
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", usuario, c.TOperacion, c.Monto, c.Saldo, c.Fecha.ToString());
                        }
                        break;
                    case 6:
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", "CBU", "Usuario", "Saldo", "Saldo Inicial");
                        foreach (var c in miEntidad.ListCuentas)
                        {
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}", c.CBU, c.Usuario, c.Saldo, c.SaldoInicial);
                        }
                        break;
                }
            }
        }
    }
    class CuentaBancaria
    {
        public IList<Cuenta> ListCuentas;
        public IList<Operacion> ListOperaciones;

        public CuentaBancaria()
        {
            this.ListCuentas = new List<Cuenta>();
            this.ListOperaciones = new List<Operacion>();
        }

        public string AltaCuenta(double saldoInicial, string usuario, string cbu)
        {
            try
            {
                if (saldoInicial >= 0)
                {
                    Cuenta nuevaCuenta = new Cuenta
                    {
                        Saldo = saldoInicial,
                        SaldoInicial = saldoInicial,
                        Usuario = usuario,
                        CBU = cbu,
                        Estado = true
                    };
                    ListCuentas.Add(nuevaCuenta);
                    NuevaOperacion("Apertura", cbu, saldoInicial, saldoInicial, DateTime.Now);
                    return "OK";
                }
                else
                    throw new ArgumentException("Saldo incorrecto");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string Extraccion(string cbu, double monto)
        {
            try
            {
                Cuenta miCuenta = ListCuentas.FirstOrDefault(x => x.CBU == cbu && x.Estado == true);
                if (monto <= miCuenta.Saldo)
                {
                    miCuenta.Saldo -= monto;
                    NuevaOperacion("Extraccion", cbu, monto, miCuenta.Saldo, DateTime.Now);
                    return "OK";
                }
                else
                    throw new ArgumentException("Saldo insuficiente");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string Deposito(string cbu, double monto)
        {
            try
            {
                Cuenta miCuenta = ListCuentas.FirstOrDefault(x => x.CBU == cbu && x.Estado == true);
                if (monto > 0)
                {
                    miCuenta.Saldo += monto;
                    NuevaOperacion("Deposito", cbu, monto, miCuenta.Saldo, DateTime.Now);
                    return "OK";
                }
                else
                    throw new ArgumentException("Monto invalido");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public void NuevaOperacion(string tOperacion, string cbu, double monto, double saldo, DateTime fecha)
        {
            Operacion nuevaOperacion = new Operacion();
            nuevaOperacion.CBU = cbu;
            nuevaOperacion.TOperacion = tOperacion;
            nuevaOperacion.Monto = monto;
            nuevaOperacion.Saldo = saldo;
            nuevaOperacion.Fecha = fecha;
            ListOperaciones.Add(nuevaOperacion);
        }
        public string BajaCuenta(string cbu)
        {
            try
            {
                Cuenta miCuenta = ListCuentas.FirstOrDefault(x => x.CBU == cbu && x.Estado == true);
                miCuenta.Estado = false;
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
    public class Cuenta
    {
        public string Usuario { get; set; }
        public double Saldo { get; set; }
        public double SaldoInicial { get; set; }
        public string CBU { get; set; }
        public bool Estado { get; set; }
    }
    public class Operacion
    {
        public string CBU { get; set; }
        public string TOperacion { get; set; }
        public double Monto { get; set; }
        public double Saldo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
