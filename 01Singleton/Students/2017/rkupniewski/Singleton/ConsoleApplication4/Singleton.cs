using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Singleton1
{
    [Serializable]
    public sealed class Singleton1 : ISerializable
    {
        private static Singleton1 sInstancja = null;
        private static readonly object sLock = new object();
        private int licznik = 0;
        //private int licznik2 = 0;
        private static string bufor;

        public static Singleton1 Instancja
        {
           get
            {
                lock (sLock)
                {
                    if (sInstancja == null)
                    {
                        sInstancja = new Singleton1();
                    }
                    return sInstancja;
                }
            }
        }

        public void Start()
        {
            Console.WriteLine("Zainicjalizowany");
        }

        public void Test()
        {
            Console.WriteLine("Start {0}!", licznik++);
            string textToSave = "Start" + (licznik).ToString();
            string fileName = "test2.txt";
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(textToSave);
            }
        }

        public void Zapisz(string textToSave)
        {
             string fileName = "test.txt";
             using (StreamWriter writer = new StreamWriter(fileName, true))
             {
                writer.WriteLine(textToSave);
             }
        }

        public void Zmienna1(string textA)
        {
            bufor = textA;
        }

        public string Zmienna2()
        {
            string textB;
            textB = bufor;
            return textB;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //throw new NotImplementedException();
            info.SetType(typeof(SingletonHelper));
        }

        [Serializable]
        private class SingletonHelper : IObjectReference
        {
            public object GetRealObject(StreamingContext context)
            {
                return Singleton1.Instancja;
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("DataFile.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            for (int i=0; i<10; ++i)
            {
                Console.WriteLine("iteracja {0}", i+1);
                Singleton1.Instancja.Test();
            }
            Console.ReadKey();           
            Parallel.For(0, 10, i => {Singleton1.Instancja.Start(); Console.WriteLine(i); }); 
            Console.ReadKey();
           

            Singleton1.Instancja.Start();
            Singleton1 ob1 = Singleton1.Instancja;
            Singleton1 ob2 = Singleton1.Instancja;
            formatter.Serialize(fs, ob1);

            fs.Position = 0;
            Singleton1 ob3 = (Singleton1)formatter.Deserialize(fs);

            fs.Position = 0;
            Singleton1 ob4 = (Singleton1)formatter.Deserialize(fs);

            if (ob3 == ob2) { Console.WriteLine("ob2 i ob3 Identyczne") ; }
            if (ob4 == ob2) { Console.WriteLine("ob2 i ob4 Identyczne"); }
            Console.ReadKey();
            ob1.Zapisz("Ala ma kota");
            ob2.Zapisz("Kot ma Ale");
            ob3.Zapisz("Cokolwiek");
            Console.WriteLine("ob1 " + ob1.GetType().ToString());
            Console.WriteLine("ob2 " + ob2.GetType().ToString());
            Console.WriteLine("ob3 " + ob3.GetType().ToString());
            Console.ReadKey();
            //Console.WriteLine("Pobieramy tekst do ob1");
            //ob1.Zmienna1(Console.ReadLine());
            //Console.WriteLine("Wyswietlamy tekst z ob2");
            //Console.WriteLine(ob2.Zmienna2());
            //Console.ReadKey();
        }
    }

}