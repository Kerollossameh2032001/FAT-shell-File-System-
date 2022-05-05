using System;
using System.IO;

namespace OS_Version_1
{
    class Program
    {
        static void Main(string[] args)
        {

            //Set two variable to take the input and the current path 
            String  currentPath;
            directory current;
            virtualDisk.intialize(@"fat.txt");
            directory root = new directory("K:", 0x10, 5, null);
            current = root;
            currentPath = current.dir_name;


            //print the welcome message
            Console.WriteLine("Shell System  \t\t [Version 2 ]");
            Console.WriteLine("Developed by \t\t [Kerollos Sameh Fouad(CS) -  khaled Mohamed hamza (CS) - Abanoub youssef Naem (IS)] \n\n");

            //create infinity loop to make user Enter command continuosly
            while (true)
            {
               
                //initialize the current path,  print it  and
                //wait user to Enter the command
                
                //currentPath = Directory.GetCurrentDirectory();
                Console.Write(currentPath +  " :/>");
                
                //take input as a list of size 3 splited by sapace 
                //to Extract command name 
                var inPut = (Console.ReadLine()).Split(' ',3);

                
                //check on the command name  
                if(inPut[0].ToLower() == "quit")
                {
                    if (inPut.Length == 2)
                    {
                        Views.Quit(inPut[1]);
                    }
                    else
                    {
                        Views.Quit();
                    }
                }
                else if (inPut[0].ToLower() == "cls")
                {
                    Views.Clear();
                }else if (inPut[0].ToLower() == "help")
                {
                    // to check if the input has argument or not 
                    // if has sent it to the functoin as parameter if not send nothing
                    if(inPut.Length == 2)
                    {
                        Views.Help(inPut[1]);
                    }
                    else
                    {
                        Views.Help();
                    }
                    
                }
                else if (inPut[0] == "" +
                    "")
                {
                    Console.WriteLine();
                    continue;
                }else
                {
                    Views.PrintExeption(inPut[0]);
                 }
                Console.WriteLine();
            }

        }
    }
}
