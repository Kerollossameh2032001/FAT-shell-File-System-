using System;
using System.Collections.Generic;
using System.Text;

namespace OS_Version_1
{
    class Views
    {
        //Help Method
        //make optional parameter if it has value done details of argument 
        //and if not print short note
        public static void Help(String argument = "")
        {
            if (argument == "")
            {
                Console.WriteLine("cls \t\t\t Clear the screen");
                Console.WriteLine("dir \t\t\t List the contents of directory");
                Console.WriteLine("quit \t\t\t Quit the shell");
                Console.WriteLine("cd \t\t\t Change the current default directory");
                Console.WriteLine("copy \t\t\t Copies one or more files to another location");
                Console.WriteLine("del \t\t\t Deletes one or more files");
                Console.WriteLine("help \t\t\t Provides Help information for commands");
                Console.WriteLine("md \t\t\t Creates a directory");
                Console.WriteLine("rd \t\t\t Removes a directory");
                Console.WriteLine("rename \t\t\t Renames a file.");
                Console.WriteLine("type \t\t\t Displays the contents of a text file");
                Console.WriteLine("import \t\t\t import text file(s) from your computer");
                Console.WriteLine("export \t\t\t export text file(s) to your computer");

            }
            else
            {
                switch (argument)
                {
                    case "cd":
                        Console.WriteLine("Change the current default directory to ." +
                            " If the argument is not present, report the current directory." +
                            " If the directory does not exist an appropriate error should be reported");
                        break;
                    case "dir":
                        Console.WriteLine("Displays a list of files and subdirectories in a directory.");
                        break;
                    case "quit":
                        Console.WriteLine("Quit the shell");
                        break;
                    case "copy":
                        Console.WriteLine("Copies one or more files to another location.");
                        break;
                    case "del":
                        Console.WriteLine("Deletes one or more files.");
                        break;
                    case "md":
                        Console.WriteLine("Creates a directory.");
                        break;
                    case "rd":
                        Console.WriteLine("Removes a directory");
                        break;
                    case "rename":
                        Console.WriteLine("Renames a file.");
                        break;
                    case "cls":
                        Console.WriteLine("Clear the screen");
                        break;
                    case "type":
                        Console.WriteLine("Displays the contents of a text file");
                        break;
                    case "import":
                        Console.WriteLine("import text file(s) from your computer");
                        break;
                    case "export":
                        Console.WriteLine("export text file(s) to your computer");
                        break;
                    default:
                        PrintExeption(argument);
                        break;
                }
            }
        }
        //Method to quit the program


        public static void Quit(String argument = "")
        {
            if (argument == "")
            {
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("the quit command does not take argument");
            }

        }


        //Clear Method
        public static void Clear()
        {
            Console.Clear();
        }

        //Method to print Exptions 
        public static void PrintExeption(String inPut)
        {
            Console.WriteLine($"\'{inPut}\'is not recognized as " +
                        $"internal or external command,");
            Console.WriteLine("operable program or batch file.");

        }
        public static void message(String text)
        {
            Console.WriteLine(text);
        }
    }
}
