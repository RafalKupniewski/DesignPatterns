using System;

class MainApp
{
    public static void Main()
    {
        // Abstract factory #1 
        AbstractFactory factory1 = new ConcreteFactory1();
        Client c1 = new Client(factory1);
        c1.Machine1();
     
        Console.ReadKey();
        // Abstract factory #2 
        AbstractFactory factory2 = new ConcreteFactory2();
        Client c2 = new Client(factory2);
        c2.Machine1();
        Console.ReadKey();

        AbstractFactory factory3 = new ConcreteFactory3();
        Client c3 = new Client(factory3);
        c3.Machine1();
       

        // Wait for user input 
        Console.ReadKey();
    }
}

// "AbstractFactory" 
abstract class AbstractFactory
{
    public abstract AbstractProductA CreateProductA();
    public abstract AbstractProductB CreateProductB();
}

// "ConcreteFactory1" 
class ConcreteFactory1 : AbstractFactory
{
    public override AbstractProductA CreateProductA()
    {
        return new Workstation();
    }
    public override AbstractProductB CreateProductB()
    {
        return new OS_Linux();
    }
}

// "ConcreteFactory2" 
class ConcreteFactory2 : AbstractFactory
{
    public override AbstractProductA CreateProductA()
    {
        return new Server();
    }
    public override AbstractProductB CreateProductB()
    {
        return new OS_Windows();
    }
}

// "ConcreteFactory3"
class ConcreteFactory3 : AbstractFactory
{
    public override AbstractProductA CreateProductA()
    {
        return new Workstation();
    }
    public override AbstractProductB CreateProductB()
    {
        return new OS_Windows();
    }
}


// "AbstractProductA" 
abstract class AbstractProductA
{
    public abstract void Wytworz(AbstractProductB typ);
}

// "AbstractProductB" 
abstract class AbstractProductB
{
    public abstract void Wytworz(AbstractProductA typ);
}

// "ProductA1" 
class Workstation : AbstractProductA
{
    public override void Wytworz(AbstractProductB typ )
    {
        Console.WriteLine(this.GetType().Name);
    }
}

// "ProductB1" 
class OS_Linux : AbstractProductB
{
    public override void Wytworz(AbstractProductA typ)
    {
        Console.WriteLine(this.GetType().Name + " jest wytwarzany w wersji " + typ.GetType().Name);
    }
}

// "ProductA2" 
class Server : AbstractProductA
{
    public override void Wytworz(AbstractProductB typ)
    {
        Console.WriteLine(this.GetType().Name + typ.GetType().Name);
    }
}

// "ProductB2" 
class OS_Windows : AbstractProductB
{
    public override void Wytworz(AbstractProductA typ)
    {
        Console.WriteLine(this.GetType().Name + " jest wytwarzany w wersji " + typ.GetType().Name);
    }
}

// "Client request product" 
class Client
{
    private AbstractProductA TypeOS;
    private AbstractProductB OS;
    
 
    // Constructor 
    public Client(AbstractFactory factory)
    {
        TypeOS = factory.CreateProductA();
        OS = factory.CreateProductB();
          
    }

    public void Machine1()
    {
        OS.Wytworz(TypeOS);
    }
   
}