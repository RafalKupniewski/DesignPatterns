using System;
using System.Collections;

public class MainApp
{
    public static void Main()
    {
        // Create director and builders 

        
        //Console.WriteLine(DateTime.Now.Ticks);
       

        Director director = new Director();
        Builder b1 = new ConcreteBuilder1();
         Builder b2 = new ConcreteBuilder2();
        // Construct two products 
        director.Construct(b1);
        Product p1 = b1.GetResult();
        p1.Show();

        director.Construct(b2);
        Product p2 = b2.GetResult();
        p2.Show();
        
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
        //Console.WriteLine(DateTime.Now.Ticks);
        long a = DateTime.Now.Ticks;

        for (int i = 0; i < 10000000; i++)
        {
            Builder b3 = new ConcreteBuilder1();
            director.Construct(b3);
            Product p3 = b3.GetResult();
            p3.Show2();

        }
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
        //Console.WriteLine(DateTime.Now.Ticks);
        long b = DateTime.Now.Ticks;
        Console.WriteLine((b - a)/ TimeSpan.TicksPerMillisecond );

        // Wait for user 
        Console.Read();
    }
}

// "Director" 
class Director
{
    // Builder uses a complex series of steps 
    public void Construct(Builder builder)
    {
        builder.BuildPartA();
        builder.BuildPartB();
    }
}

// "Builder" 
abstract class Builder
{
    public abstract void BuildPartA();
    public abstract void BuildPartB();
    public abstract Product GetResult();
}

// "ConcreteBuilder1" 
class ConcreteBuilder1 : Builder
{
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

// "ConcreteBuilder2" 
class ConcreteBuilder2 : Builder
{
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

// "Product" 
class Product
{
    ArrayList parts = new ArrayList();

    public void Add(string part)
    {
        parts.Add(part);
    }

    public void Show()
    {
        Console.WriteLine("\nProduct Parts -------");
        foreach (string part in parts)
            Console.WriteLine(part);
    }

    public void Show2()
    {
        //Console.WriteLine("\nProduct Parts -------");
        //foreach (string part in parts)
            //Console.WriteLine(part);
    }
}