using System;

namespace hellohuman
{
class program
{
    static void Main()
    {
       
       String hello = "Hello, World";
       Console.WriteLine(hello);

       //String hello2 = null;
        #nullable enable
        String? hello2 = null; //null safe 
    }
}

class Humanid
{
   public Humanid()  //constructor
   {
     Console.WriteLine("first , a live ");
   }

    ~Humanid()  //destructor
    {
     Console.WriteLine("final");
    }

    public String Name  //Auto-implemented properties 
    {get; set;}

    //methods
    private void sayHello()
    {
       String hello = "Hello, World";
       Console.WriteLine(hello);
    }

    private void fun() //LAMDDA expressions and functional programming patterns 
    {
        Func<int, int> doubleIt= x=> x*2;
        doubleIt(23); // ==46

    }

    async void myJob()
    {
        var a = await SlowTask();
    }

}

class SuperHuman:Humanid //inheritance 
{
    public override void walk(){   //polymorphism
        Console.WriteLine("walk faster!");
    }
}

}
