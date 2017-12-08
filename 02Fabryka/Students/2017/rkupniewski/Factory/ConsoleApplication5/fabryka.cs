using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tworzenie instancji klasy w której znajduje się metoda do dokonania wypłaty
            PaymentProcessor pre = new PaymentProcessor();
            // Definiujemy operacje
            Operacja operacja = new Operacja();
            //operacja wypłaty 1
            operacja.Wyplata = 5000;
            // Dokonujemy wypłaty pierwszym sposobem.
            pre.MakePayment(EPaymentMethod.EURONET, operacja);
            Console.ReadKey();
            //operacja wypłaty 2
            operacja.Wyplata = 4000;
            pre.MakePayment(EPaymentMethod.OTHER, operacja);
            Console.ReadKey();
        }
    }
    public enum EPaymentMethod
    {
        EURONET,
        OTHER,
    }
    public class Operacja
    {
        public decimal Wyplata { get; set; }
    }
    public interface IPaymentGateway
    {
        void MakePayment(Operacja product);
    }
    public class OperatorOne : IPaymentGateway
    {
        public void MakePayment(Operacja product)
        {
            double prowizja = (float)product.Wyplata/100;
            // Metoda to pozwala na dokonanie wypłaty za pomocą pierwszego sposobu
            Console.WriteLine("Pierwszy rodzaj wypłaty kwota {0}, prowizja {1}", product.Wyplata, prowizja);
        }
    }
    public class OperatorTwo : IPaymentGateway
    {
        public void MakePayment(Operacja product)
        {
            float prowizja = (float)product.Wyplata/20;
            // Metoda to pozwala na dokonanie wypłaty za pomocą drugiego sposobu
            Console.WriteLine("Drugi rodzaj wypłaty kwota {0}, prowizja {1}", product.Wyplata, prowizja);
        }
    }
    public class PaymentGatewayFactory
    {
        public virtual IPaymentGateway CreatePaymentGateway(EPaymentMethod method, Operacja prod)
        {
            IPaymentGateway gateway = null;
            switch (method)
            {
                case EPaymentMethod.EURONET:
                    gateway = new OperatorOne();
                    break;
                case EPaymentMethod.OTHER:              
                    gateway = new OperatorTwo();
                    break;
                default:
                    break;
            }
            return gateway;
        }
    }
    public class PaymentProcessor
    {
        IPaymentGateway gateway = null;
        // Dokonywanie wypłaty
        // Wywołanie metody CreatePaymentGateway(...) zwraca nam obiekt utworzony
        // w zależności od wyboru rodzaju wypłaty przez klienta
        public void MakePayment(EPaymentMethod method, Operacja product)
        {
            PaymentGatewayFactory factory = new PaymentGatewayFactory();
            this.gateway = factory.CreatePaymentGateway(method, product);
            this.gateway.MakePayment(product);
        }
    }
}
