using System;

class MainApp
{
    static void Main()
    {
        
        // tworzymy adapter i wywolujemy request
        Target target = new Adapter();
        target.Request();

        // guzik 
        Console.Read();
    }
}

// "pierwotna klasa target" 
class Target
{
    public virtual void Request()
    {
        int a = 3;
        string b = "alamakota";
        Console.WriteLine("{0} x {1}", a, b);
    }
}

// "rozszerzamy klase adapter o klase target i nadpisujemy metode request z klasy adaptee" 
class Adapter : Target
{
    private Adaptee adaptee = new Adaptee();

    public override void Request()
    {
        // wywolujemy request ale z klasy adapter ;-)
        adaptee.SpecificRequest();
    }
}

// "klasa adaptee" 
class Adaptee
{
    public void SpecificRequest()
    {
        string a = "dzien dobry";
        int b = 8;
        Console.WriteLine("{0} x {1}",  a ,b);
    }
}