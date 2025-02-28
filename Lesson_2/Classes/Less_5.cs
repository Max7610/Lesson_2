using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_2.Classes
{
    internal class Less_5
    {
    }
    public class OrderService
    {
        OrderRepository order = new OrderRepository();// класс отвечающий операции над ордером 
        List<PaymentMethod> paymentMethods = new List<PaymentMethod> { new PaymentPayPal(), new PaymentCart() };//методы оплаты
        public void OrderMeneger()//метод отвечающий за выбор действия 
        {
            WriteList();
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (key)
            {
                case 'A':
                    order.Save();
                    break;
                case 'B':
                    order.Load();
                    break;
                case 'C':
                    paymentMethods[0].ProcessPayment();
                    break;
                case 'D':
                    paymentMethods[1].ProcessPayment();
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }

        }
        void WriteList()
        {
            Console.WriteLine("Выбор действия:\n" +
                "A. Save\n" + "B. Load\n" + "C. PayPal\n" + "D. Cart\n");
        }

    }
    public class OrderRepository
    {
        public void Save()
        {
            Console.WriteLine("Save");
        }
        public void Load()
        {
            Console.WriteLine("Load");
        }
    }

    interface PaymentMethod
    {
        void ProcessPayment();
    }
    class PaymentPayPal : PaymentMethod
    {
        public void ProcessPayment()
        {
            Console.WriteLine("Введите номер телефона для оплаты");
        }
    }
    class PaymentCart : PaymentMethod
    {
        public void ProcessPayment()
        {
            Console.WriteLine("Введите номер карты");
        }
    }

    interface IUser
    {
        void Read();
    }
    class User : IUser
    {
        public void Read()
        {
            Console.WriteLine("Прочитано");
        }
    }
    class AdminUser : IUser
    {
        public void Read()
        {
            Console.WriteLine("Прочитано");
        }
        public void Write()
        {
            Console.WriteLine("Записано");
        }
    }
}
