using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    class Program
    {
        static void Main(String[] args)
        {

            
            //Console.WriteLine("hello world");

            Console.WriteLine("   /|");
            Console.WriteLine("  / |");
            Console.WriteLine(" /  |");
            Console.WriteLine("/___|");


        //variables
            String charName="Rana";
            int charAge;
            charAge=25;
            Console.WriteLine("Name: "+charName+", Age: "+charAge);

            charName="Maher";
            Console.WriteLine("Name: "+charName+", Age: "+charAge);

        //data types
            String phrase="some academy";
            Char grade='K';
            int age=26;
            //float, double, decimal
            double gpa=3.1;
            bool isMale= true;
            //constant
            Console.WriteLine("k");
            Console.WriteLine(30);

        //work with strings
            Console.WriteLine("Some\nAcademy"); //new line ("\n")

            String somephrase="some academy"+"is cool";
            Console.WriteLine(somephrase);

            // String methods
            Console.WriteLine(somephrase.ToUpper());
            Console.WriteLine(somephrase.Contains("academy"));

            //start with index 012...i
            Console.WriteLine(somephrase[2]); //print m

            //to return indexof 
            Console.WriteLine(somephrase.IndexOf("academy"));

            //return Substring(index)
            //Substring(index, how char to grab)
            Console.WriteLine(somephrase.Substring(8, 3));

        //numbers
            // + - * /
            Console.WriteLine(5 + 3); //return 8
            Console.WriteLine(5 - 3); //return 2
            Console.WriteLine((4 + 2) * 2); //return 12

            //decimal number
            Console.WriteLine(5 / 2.0); // return 2.5

            //Math 
            Console.WriteLine(Math.Pow(3, 2)); 
            Console.WriteLine(Math.Max(3, 2)); 

    //take values from user 
        Console.Write("Enter your name:");
        String username = Console.ReadLine();
        Console.WriteLine("hello " + username);
    
        Console.Write("Enter your age:");
        String userage = Console.ReadLine();
        Console.WriteLine("hello " + username + "your age is "+userage);


    //convert string "45" to decimal (to int ) ..
        int num = Convert.ToInt32("45");

    //calculator
        Console.Write("Enter a number:");
        double num1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter another number:");
        double num2 = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine(num1 + num2);

    Console.ReadLine();
        }
    }
}
