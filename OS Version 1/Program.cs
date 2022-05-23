using System;
using System.IO;

namespace OS_Version_1
{
    class Program
    {
         static void printException()
        {
            Console.WriteLine("Error: The path is not exist ");
        }


        public static directory moveToDir(String fullPath)
        {
            var path = fullPath.Split('\\');
            if(path.Length == 1)
            {
                if(path[0] == "K:")
                {
                    directory root = new directory("K:", 0x10, 5, null);
                    return root;
                }
                else
                {
                    return null;
                }
            }
            else if(path[0] == "K:")
            {
                directory root = new directory("K:", 0x10, 5, null);
                root.readDirectory();
                directory parent = root;
                int index = root.search(path[1]);
                directory obj;
                
                if (index == -1)
                {
                    return null;
                }
                else
                {
                     obj = new directory(root.dirsOrfiles[index].dir_name,
                            root.dirsOrfiles[index].attr,
                            root.dirsOrfiles[index].dir_firstcluster, parent);
                    obj.readDirectory();
                    parent = obj;
                }


                for (int i=2; i<path.Length; i++)
                {
                     index = obj.search(path[i]);
                    if (index == -1)
                    {
                        return null;
                    }
                    else
                    {
                         obj = new directory(obj.dirsOrfiles[index].dir_name,
                            obj.dirsOrfiles[index].attr,
                            obj.dirsOrfiles[index].dir_firstcluster, parent);
                        obj.readDirectory();
                        parent = obj;
                    }
                }
                return obj;
            }
            else
            {
                return null;
            }
        }


        public static File_Entry moveToFile(String fullPath)
        {
            int count = 0;
            for (int i = fullPath.Length - 1; i >= 0; i--)
            {
                if (fullPath[i] == '\\')
                {
                    break;
                }
                count++;
            }
            String fileName = fullPath.Substring((fullPath.Length - count));
            fullPath= fullPath.Remove((fullPath.Length - count));
            directory obj = moveToDir(fullPath);

            if(obj == null)
            {
                return null;
            }
            else
            {
                obj.readDirectory();
                int index = obj.search(fileName);
                if(index == -1)
                {
                    return null;
                }
                else
                {
                    File_Entry fileObj = new File_Entry(obj.dirsOrfiles[index].dir_name,
                            obj.dirsOrfiles[index].attr,
                            obj.dirsOrfiles[index].dir_firstcluster, obj, obj.dirsOrfiles[index].dir_fileSize);
                    return fileObj;
                }
            }

        }
        //public static String currentk;

        static void Main(string[] args)
        {
            //Set two variable to take the input and the current path 
            String currentPath;
            directory current;
            virtualDisk.intialize(@"fat.txt");
            directory root = new directory("K:", 0x10, 5, null);
            current = root;
            current.readDirectory();
            currentPath = current.dir_name.Remove(current.dir_name.IndexOf(' '));
            
           

            //print the welcome message
            Console.WriteLine("Shell System  \t\t [Version 2 ]");
            Console.WriteLine("Developed by \t\t [Kerollos Sameh Fouad(CS) -  khaled Mohamed hamza (CS) - Abanoub youssef Naem (IS)] \n\n");

            //create infinity loop to make user Enter command continuosly
            while (true)
            {

                //initialize the current path,  print it  and
                //wait user to Enter the command

                var path = currentPath.Split(' ');
                currentPath = string.Empty;
                for(int i = 0; i<path.Length; i++)
                {
                    currentPath += path[i];
                }
                Console.Write(currentPath + " \\>");
                //take input as a list of size 3 splited by sapace 
                //to Extract command name 
                var inPut = (Console.ReadLine()).Split(' ',3);


                //check on the command name  
                if (inPut[0].ToLower() == "quit")
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
                }
                else if (inPut[0].ToLower() == "help")
                {
                    // to check if the input has argument or not 
                    // if has sent it to the functoin as parameter if not send nothing
                    if (inPut.Length == 2)
                    {
                        Views.Help(inPut[1]);
                    }
                    else
                    {
                        Views.Help();
                    }
                }
                else if (inPut[0].ToLower() == "cd")
                {
                    // to check if the input has argument or not 
                    // if has sent it to the functoin as parameter if not send nothing
                    if (inPut.Length == 2)
                    {
                        Views.command(currentPath, current, inPut[1]);
                        current = (Views.current_directory() != null ? Views.current_directory() : current);
                        currentPath = (Views.current_path() != null ? Views.current_path() : currentPath);
                    }
                    else
                    {
                        Views.command(currentPath, current);
                    }

                }
                else if (inPut[0].ToLower() == "dir")
                {
                    // to check if the input has argument or not 
                    // if has sent it to the functoin as parameter if not send nothing
                    if (inPut.Length == 2)
                    {
                        Views.directoryContent(currentPath, current, inPut[1]);
                    }
                    else
                    {
                        Views.directoryContent(currentPath, current);
                    }
                }
                else if (inPut[0].ToLower() == "md")
                {
                    if (inPut.Length == 2)
                    {
                        Views.makeDirectory(currentPath, current, inPut[1]);
                    }
                    else
                    {
                        Views.makeDirectory(currentPath, current);
                    }
                }
                else if (inPut[0].ToLower() == "rd")
                {
                    if (inPut.Length == 2)
                    {
                        Views.removeDirectory(currentPath, current, inPut[1]);
                    }
                    else
                    {
                        Views.removeDirectory(currentPath, current);
                    }
                }
                else if (inPut[0].ToLower() == "type")
                {
                    if (inPut.Length == 2)
                    {
                        Views.type(currentPath, current, inPut[1]);
                    }
                    else
                    {
                        Views.type(currentPath, current);
                    }
                }
                else if (inPut[0].ToLower() == "rename")
                {
                    if (inPut.Length == 3)
                    {
                        Views.rename(currentPath, current, inPut[1], inPut[2]);
                    }
                    else
                    {
                        Views.rename(currentPath, current);
                    }
                }
                else if (inPut[0].ToLower() == "del")
                {
                    if (inPut.Length == 2)
                    {
                        Views.delete(currentPath, current, inPut[1]);
                    }
                    else
                    {
                        Views.delete(currentPath, current);

                    }
                }
                else if (inPut[0] == "" +
                    "")
                {
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    Views.PrintExeption(inPut[0]);
                }
                Console.WriteLine();
            }

        }
    }
}
