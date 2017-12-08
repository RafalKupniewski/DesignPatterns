using System;

public class Vin
{
    public string vin;

    public Vin(string Vin)
    {
        this.vin = Vin;
    }
}

public class Auto
{
    public string Marka;
    public string Model;
    public Vin vin;

    public Auto ShallowCopy()
    {
        return (Auto)this.MemberwiseClone();
    }

    public Auto DeepCopy()
    {
        Auto other = (Auto)this.MemberwiseClone();
        other.vin = new Vin(vin.vin);
        other.Marka = String.Copy(Marka);
        other.Model = String.Copy(Model);
        return other;
    }
}

public class Example
{
    public static void Main()
    {
        // tworzenie instancji
        Auto a1 = new Auto();
        a1.Model = "Ford";
        a1.Marka = "SMax";
        a1.vin = new Vin("asdsejdhs");

        // kopia plytka
        Auto a2 = a1.ShallowCopy();

        // wyswietlamy
        Console.WriteLine("Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.vin.vin);

        Console.WriteLine("Kopia plytka Auto1:");
        Console.WriteLine(a2.Model);
        Console.WriteLine(a2.Marka);
        Console.WriteLine(a2.vin.vin);

        Console.ReadKey();
        // zmiana wartosci
        a1.Model = "Ford";
        a1.Marka = "Mondeo";
        a1.vin.vin = "wewwsddfrt";
        Console.WriteLine("Zmiana wartosci w zrodlowej instancji");
        Console.WriteLine("Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.vin.vin);

        Console.WriteLine("Kopia plytka Auto1:");
        Console.WriteLine(a2.Model);
        Console.WriteLine(a2.Marka);
        Console.WriteLine(a2.vin.vin);

        Console.ReadKey();

        // kopia gleboka
        Auto a3 = a1.DeepCopy();

        Console.WriteLine("Kopia gleboka Auto3:");
        Console.WriteLine(a3.Model);
        Console.WriteLine(a3.Marka);
        Console.WriteLine(a3.vin.vin);

        Console.ReadKey();

        a1.Model = "Fiat";
        a1.Marka = "Bravo";
        a1.vin.vin = "ookkdhbrtjg";
        Console.WriteLine("Zmiana wartosci w zrodlowej instancji");

        Console.WriteLine("Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.vin.vin);

        Console.WriteLine("Kopia gleboka Auto3:");
        Console.WriteLine(a3.Model);
        Console.WriteLine(a3.Marka);
        Console.WriteLine(a3.vin.vin);

        Console.ReadKey();
    }


}
