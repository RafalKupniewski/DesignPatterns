using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsExamples
{
    class PrototypePattern
    {
        static void Main(string[] args)
        {
            // Instancje i klonowanie
            ConcretePrototype p1 = new ConcretePrototype("wartosc 1");
            ConcretePrototype c1 = (ConcretePrototype)p1.Clone();
            ConcretePrototype c3 = (ConcretePrototype)p1.DeepClone();
            Console.WriteLine("Cloned P1: {0}", c1.Id);
            Console.WriteLine("DeepCloned: {0}", c3.Id);
            c1.id = "Wartosc 2";
            Console.WriteLine("Cloned P1: {0}", c1.Id);
            Console.WriteLine("DeepCloned: {0}", c3.Id);
            c3.id = "Wartosc 4";
            Console.WriteLine("Cloned P1: {0}", c1.Id);
            Console.WriteLine("DeepCloned: {0}", c3.Id);
            ConcretePrototype c2 = (ConcretePrototype)p1.Clone();
            //c1.Id = "new";
            Console.WriteLine("Cloned P1: {0}", c1.Id);
            Console.WriteLine("DeepCloned: {0}", c3.Id);
            Console.WriteLine("Cloned P2: {0}", c2.Id);
            
             
            Console.Read();
        }
    }

    // Prototyp 
    abstract class Prototype
    {
        public string id;

        // Konstruktor  
        public Prototype(string id)
        {
            this.id = id;
        }

        // Wlasciwosc 
        public string Id
        {
            get { return id; }
        }

        public abstract Prototype Clone();
        public abstract Prototype DeepClone();
    }

    // Prototyp rozszerzenie
    class ConcretePrototype : Prototype
    {
        // Konstruktor "konkretny"
        public ConcretePrototype(string id)
            : base(id)
        {
        }
    
        public override Prototype Clone()
        {
            // kopia plytka 
            return (Prototype)this.MemberwiseClone();
        }

        public override Prototype DeepClone()
        {
            // kopia gleboka
            Prototype other = (Prototype)this.MemberwiseClone();
            other.id = String.Copy(id);          
            return other;
        }
    }

    public class ClientClass
    {
        private Prototype prototype = null;
        ClientClass(Prototype prototype)
        {
            this.prototype = prototype;
        }
    }
}

