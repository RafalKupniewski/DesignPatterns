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
    public Vin numer;

    public Auto ShallowCopy()
    {
        return (Auto)this.MemberwiseClone();
    }

    public Auto DeepCopy()
    {
        Auto nowy = (Auto)this.MemberwiseClone();
        nowy.numer = new Vin(numer.vin);
        nowy.Marka = String.Copy(Marka);
        nowy.Model = String.Copy(Model);
        return nowy;
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
        a1.numer = new Vin("asdsejdhs");

        // kopia plytka
        Auto a2 = a1.ShallowCopy();

        // wyswietlamy
        Console.WriteLine("Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.numer.vin);

        Console.WriteLine("Kopia plytka Auto1:");
        Console.WriteLine(a2.Model);
        Console.WriteLine(a2.Marka);
        Console.WriteLine(a2.numer.vin);

        Console.ReadKey();
        // zmiana wartosci
        a1.Model = "Ford";
        a1.Marka = "Mondeo";
        a1.numer.vin = "wewwsddfrt";
        Console.WriteLine("Zmiana wartosci w zrodlowej instancji");
        Console.WriteLine("Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.numer.vin);

        Console.WriteLine("Kopia plytka Auto1:");
        Console.WriteLine(a2.Model);
        Console.WriteLine(a2.Marka);
        Console.WriteLine(a2.numer.vin);

        Console.ReadKey();

        // kopia gleboka
        Auto a3 = a1.DeepCopy();

        Console.WriteLine("Kopia gleboka Auto3:");
        Console.WriteLine(a3.Model);
        Console.WriteLine(a3.Marka);
        Console.WriteLine(a3.numer.vin);

        Console.ReadKey();

        a1.Model = "Fiat";
        a1.Marka = "Bravo";
        a1.numer.vin = "ookkdhbrtjg";
        Console.WriteLine("Zmiana wartosci w zrodlowej instancji");

        Console.WriteLine("Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.numer.vin);

        Console.WriteLine("Kopia gleboka Auto3:");
        Console.WriteLine(a3.Model);
        Console.WriteLine(a3.Marka);
        Console.WriteLine(a3.numer.vin);

        Console.ReadKey();
    }


}
