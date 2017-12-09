using System;
using System.Collections;

public class MainApp
{
    public static void Main()
    {
        // tworzymy instancje director
        Director director = new Director();

        //tworzymy instancje b1 i b2 dla buildera 1 i 2
        Builder b1 = new ConcreteBuilder1();
        Builder b2 = new ConcreteBuilder2();
        // wytwarzamy dwa produkty

        //z instancji buildera b1 wytwarzamy produkt p1 i metoda show wyswietalmy arrayliste 
        director.Construct(b1);
        Product p1 = b1.GetResult();
        p1.Show();

        //z instancji buildera b2 wytwarzamy produkt p2 i metoda show wyswietalmy arrayliste 
        director.Construct(b2);
        Product p2 = b2.GetResult();
        p2.Show();
        
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));

        long a = DateTime.Now.Ticks;

        for (int i = 0; i < 10000000; i++)
        {
            Builder b3 = new ConcreteBuilder1();
            director.Construct(b3);
            Product p3 = b3.GetResult();
            p3.Show2();

        }
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
        
        long b = DateTime.Now.Ticks;
        Console.WriteLine((b - a)/ TimeSpan.TicksPerMillisecond );

        // nacisnij guzik
        Console.Read();
    }
}

// 
class Director
{
    //  klasa uruchamia nam obydwa buildery
    public void Construct(Builder builder)
    {
        builder.BuildPartA();
        builder.BuildPartB();
    }
}

// "abstarkcyjna klasa builder" 
abstract class Builder
{
    public abstract void BuildPartA();
    public abstract void BuildPartB();
    public abstract Product GetResult();
}

// "klasa konkretna rozszerzajaca bulidera wytwarzajaca produkt Windows workstation" 
class ConcreteBuilder1 : Builder
{
    //robimy instancje product i dodajemy do arraylisty czesci produktu    
    private Product product = new Product();

    public override void BuildPartA()
    {
        product.Add("Windows");
    }

    public override void BuildPartB()
    {
        product.Add("Workstation");
    }

    public override Product GetResult()
    {
        return product;
    }
}

// "klasa konkretna rozszerzajaca bulidera wytwarzajaca produkt Linux server" 
class ConcreteBuilder2 : Builder
{   
    //robimy instancje product i dodajemy do arraylisty czesci produktu    
    private Product product = new Product();

    public override void BuildPartA()
    {
        product.Add("Linux");
    }

    public override void BuildPartB()
    {
        product.Add("Server");
    }

    public override Product GetResult()
    {
        return product;
    }
}

// "laczenie produktow ;-)" 
class Product
{
    ArrayList parts = new ArrayList();

    public void Add(string part)
    {
        parts.Add(part);
    }

    public void Show()
    {   
        //wyswietlamy z arraylisty elementy produktu
        Console.WriteLine("\nProduct Parts -------");
        foreach (string part in parts)
            Console.WriteLine(part);
    }

    public void Show2()
    {
        //nic nie robie
    }
}