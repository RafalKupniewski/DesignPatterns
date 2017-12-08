using System;

public class Id
{
    public int Ida;

    public Id(int Id)
    {
        this.Ida = Id;
    }
}

public class Auto
{
    public string Marka;
    public string Model;
    public Id Id;

    public Auto ShallowCopy()
    {
        return (Auto)this.MemberwiseClone();
    }

    public Auto DeepCopy()
    {
        Auto other = (Auto)this.MemberwiseClone();
        other.Id = new Id(Id.Ida);
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
        a1.Id = new Id(1);

        // kopia plytka
        Auto a2 = a1.ShallowCopy();

        // wyswietlamy
        Console.WriteLine("   Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.Id.Ida);

        Console.WriteLine("   Kopia plytka Auto1:");
        Console.WriteLine(a2.Model);
        Console.WriteLine(a2.Marka);
        Console.WriteLine(a2.Id.Ida);

        Console.ReadKey();
        // zmiana wartosci
        a1.Model = "Ford";
        a1.Marka = "Mondeo";
        a1.Id.Ida = 2;
        Console.WriteLine("Zmiana wartosci w zrodlowej instancji");
        Console.WriteLine("   Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.Id.Ida);

        Console.WriteLine("   Kopia plytka Auto1:");
        Console.WriteLine(a2.Model);
        Console.WriteLine(a2.Marka);
        Console.WriteLine(a2.Id.Ida);

        Console.ReadKey();

        // kopia gleboka
        Auto a3 = a1.DeepCopy();

        Console.WriteLine("   Kopia gleboka Auto3:");
        Console.WriteLine(a3.Model);
        Console.WriteLine(a3.Marka);
        Console.WriteLine(a3.Id.Ida);

        Console.ReadKey();

        a1.Model = "Fiat";
        a1.Marka = "Bravo";
        a1.Id.Ida = 3;
        Console.WriteLine("Zmiana wartosci w zrodlowej instancji");

        Console.WriteLine("   Auto1: ");

        Console.WriteLine(a1.Model);
        Console.WriteLine(a1.Marka);
        Console.WriteLine(a1.Id.Ida);

        Console.WriteLine("   Kopia gleboka Auto3:");
        Console.WriteLine(a3.Model);
        Console.WriteLine(a3.Marka);
        Console.WriteLine(a3.Id.Ida);

        Console.ReadKey();
    }


}
