using System;
using System.Collections.Generic;
using System.Text;

namespace OS_Version_1
{
    class Views
    {
        static String currentPath;
        static directory current;
        
        //Method to return current path
        public static String current_path()
        {
            return currentPath;
        }
       
        //Method to return current directory
        public static directory current_directory()
        {
            return current;
        }

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
                    case "help":
                        Console.WriteLine("provides help information to the command");
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




        /// <summary>
        ///  command (cd) method :change directory command
        /// Change the current default directory to the directory given in the argument.
        /// make optional parameter called argument  if it has not 
        /// value print the current path , if not we have to cases argument 
        /// a) is a name or b) fullpath as the following.
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        public static void command(String current_Path, directory current_directory,String argument = "")
        {
            current_directory.readDirectory();

            if (argument == "" || argument == ".")
            {
                Console.WriteLine("\n"+current_Path);
            }
            else if (argument == "..")
            {
                int index = current_Path.LastIndexOf('\\');
                if(index != -1 || current_directory.parent != null)
                {
                    currentPath = current_Path.Remove(index);
                    current = Program.moveToDir(currentPath);
                }
            }
            else if (argument.Contains(".\\."))
            {
                var depth = argument.Split('\\');
                for(int i =0; i < depth.Length; i++)
                {
                    int index = current_Path.LastIndexOf('\\');
                    if (index == -1 || current_directory.parent == null)
                    {
                        break;
                    }
                    current_Path = current_Path.Remove(index);
                    currentPath = current_Path;
                    current = Program.moveToDir(currentPath);
                }
            }
            else
            {
                //if the argument is name 
                if (!argument.Contains('\\'))
                {
                    current_directory.readDirectory();
                    int index = current_directory.search(argument);
                    if(index == -1)
                    {
                        Console.WriteLine("Error: directory is not exist");
                        current = current_directory;
                        currentPath = current_Path;
                    }
                    else
                    {
                        directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                        current = obj;
                        currentPath = current_Path + "\\"+ obj.dir_name;
                    }
                }
                else   //if the argument is fullPath
                {
                    directory obj = Program.moveToDir(argument);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: directory is not exist");
                        current = current_directory;
                        currentPath = current_Path;
                    }
                    else
                    {
                        current = obj;
                        currentPath = argument;
                    }
                }
            }
        }






        public static void printContent(directory current_directory, bool root)
        {
            if(root == true)
            {
                int fileNumber = 0, dirNumber = 0;
                int filesSize = 0;
                current_directory.readDirectory();
                

                Console.WriteLine($"Directory of {current_directory.dir_name}");
                for (int i = 0; i < current_directory.dirsOrfiles.Count; i++)
                {
                    if (current_directory.dirsOrfiles[i].attr == 0x0)
                    {
                        fileNumber++;
                        Console.WriteLine($"\t\t {current_directory.dirsOrfiles[i].dir_fileSize}\t " +
                            $"{current_directory.dirsOrfiles[i].dir_name}");
                        filesSize += current_directory.dirsOrfiles[i].dir_fileSize;
                    }
                    else
                    {
                        dirNumber++;
                        Console.WriteLine($"\t\t <DIR> \t " +
                            $"{current_directory.dirsOrfiles[i].dir_name}");
                    }

                }
                Console.WriteLine($"\t\t\t ({fileNumber}) File(s) \t {filesSize} bytes");
                Console.WriteLine($"\t\t\t ({dirNumber}) Dir(s) \t {virtualDisk.getEmptySize()} bytes free");
            }
            else
            {
                int fileNumber = 0, dirNumber = 0;
                int filesSize = 0;
                Console.WriteLine($"Directory of {current_directory.dir_name}");

                Console.WriteLine("\t\t <DIR> \t .");
                Console.WriteLine("\t\t <DIR> \t ..");
                current_directory.readDirectory();
                for (int i = 0; i < current_directory.dirsOrfiles.Count; i++)
                {
                    if (current_directory.dirsOrfiles[i].attr == 0x0)
                    {
                        fileNumber++;
                        Console.WriteLine($"\t\t {current_directory.dirsOrfiles[i].dir_fileSize}\t " +
                            $"{current_directory.dirsOrfiles[i].dir_name}");
                        filesSize += current_directory.dirsOrfiles[i].dir_fileSize;
                    }
                    else
                    {
                        dirNumber++;
                        Console.WriteLine($"\t\t <DIR> \t " +
                            $"{current_directory.dirsOrfiles[i].dir_name}");
                    }

                }
                Console.WriteLine($"\t\t\t ({fileNumber}) File(s) \t {filesSize} bytes");
                Console.WriteLine($"\t\t\t ({dirNumber}) Dir(s) \t {virtualDisk.getEmptySize()} bytes free");

            }

        }
       



        /// <summary>
        /// List the contents of directory given in the argument.
        /// If the argument is not present, list the content of the current directory.
        /// If the directory does not exist an appropriate error should be reported.
        /// dir command syntax is 'dir' or 'dir[directory]'
        /// where [directory] can be directory name or fullpath of a directory or 
        /// file name or full path of a file.
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        public static void directoryContent(String current_Path, directory current_directory,String argument = "")
        {
            if (argument == "" || argument == ".")
            {
                //if the current  is the root directory print 
                printContent(current_directory, current_directory.parent == null ? true : false);
            }
            else if(argument == "..")
            {
                int index = current_Path.LastIndexOf('\\');
                if(index != -1)
                {
                    directory obj = Program.moveToDir(current_Path.Remove(index));
                    obj.readDirectory();
                    printContent(obj, obj.parent == null ? true : false);
                }
                else
                {
                    printContent(current_directory, current_directory.parent == null ? true : false);
                }
                
            }
            else
            {
                //if the argument is name of directory
                if (!argument.Contains('\\') && !argument.Contains(".txt"))
                {
                    int index = current_directory.search(argument);
                    if (index == -1)
                    {
                        Console.WriteLine("Error: directory is not exist");
                    }
                    else
                    {
                        directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                        obj.readDirectory();
                        printContent(obj, obj.parent == null ? true : false);
                    }
                }
                else if(argument.Contains('\\') && !argument.Contains(".txt"))// if the argument is full path directory
                {
                    directory obj = Program.moveToDir(argument);
                    if (obj == null)
                    {
                        Console.WriteLine("Error: the directory is not  exist");
                    }
                    else
                    {
                        obj.readDirectory();
                        //to print the content 
                        printContent(obj, obj.parent == null ? true : false);
                    }
                }
                else if (!argument.Contains('\\') && argument.Contains(".txt"))//name of file
                {
                    int index = current_directory.search(argument);
                    if(index == -1)
                    {
                        Console.WriteLine("Error: The file is not exist ");
                    }
                    else
                    {
                        Console.WriteLine($"Directory of {current_directory.dir_name}");
                        Console.WriteLine($"\t\t {current_directory.dirsOrfiles[index].dir_fileSize}\t " +
                          $"{current_directory.dirsOrfiles[index].dir_name}");
                        Console.WriteLine($"\t\t\t (1) File(s) \t {current_directory.dirsOrfiles[index].dir_fileSize} bytes");
                        Console.WriteLine($"\t\t\t (0) Dir(s) \t {virtualDisk.getEmptySize()} bytes free");
                    }
                }
                else if(argument.Contains('\\') && argument.Contains(".txt"))//fullPath of file
                {
                    File_Entry obj = Program.moveToFile(argument);
                    if(obj == null)
                    {
                        Console.WriteLine("Error:  the file is not exist");
                    }
                    else
                    {
                        int index = argument.LastIndexOf('\\');
                        argument = argument.Remove(index);
                        
                        
                        Console.WriteLine($"Directory of {argument}");
                        Console.WriteLine($"\t\t {obj.dir_fileSize}\t " +
                          $"{obj.dir_name}");
                        Console.WriteLine($"\t\t\t (1) File(s) \t {obj.dir_fileSize} bytes");
                        Console.WriteLine($"\t\t\t (0) Dir(s) \t {virtualDisk.getEmptySize()} bytes free");
                    }
                }
            }
        }





        /// <summary>
        /// Creates a directory.
        /// md command syntax is
        /// md[directory]
        /// [directory] can be a new directory name or fullpath of a new directory
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        public static void makeDirectory(String current_Path, directory current_directory, String argument = "")
        {
            if (argument == "")
            {
                Console.WriteLine("md command syntax is " +
                    "\n md[directory]" +
                    "\n where [directory] can be a new directory name or fullpath of a new directory");
            }
            else
            {
                //if the argument is the name of directory
                if (!argument.Contains('\\'))
                {
                    //Console.WriteLine(Mini_Fat.getClusterStatus(current_directory.dir_firstcluster));
                    int index = current_directory.search(argument);
                    //Console.WriteLine(index);
                    if(index != -1)
                    {
                        Console.WriteLine($"This directory\" {argument}\" is already exist!");
                    }
                    else
                    {
                        Directory_Entry obj = new Directory_Entry(argument, 0x10, 0, 0);
                        //Console.WriteLine(current_directory.canAddEntry(obj));
                        if (current_directory.canAddEntry(obj) == false)
                        {
                            Console.WriteLine("There is no enough space to add this directory");
                        }
                        else
                        {
                            current_directory.addEntry(obj);
                        }
                    }
                }
                else // if the argument is full path
                {
                    int count = argument.LastIndexOf('\\');
                    String Newdir = argument.Substring(count+1 , (argument.Length - count-1));
                    argument = argument.Remove((count));

                    directory obj = Program.moveToDir(argument);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: path is not exist!");
                    }
                    else
                    {
                        obj.readDirectory();
                        if(obj.search(Newdir) != -1)
                        {
                            Console.WriteLine($"This directory\" {Newdir}\" is already exist!");
                        }
                        else
                        {
                            Directory_Entry newObj = new Directory_Entry(Newdir, 0x10, 0, 0);
                            if (obj.canAddEntry(newObj) == false)
                            {
                                Console.WriteLine("There is no enough space to add this directory");
                            }
                            else
                            {
                                obj.addEntry(newObj);
                            }
                        }
                    }
                }
            }
        }



        /// <summary>
        /// rd: Removes a directory.
        /// it confirms the user choice to delete the directory before deleting
        /// rd command syntax is
        /// rd[directory]+
        /// [directory] can be a directory name or fullpath of a directory
        /// + after[directory] represent that you can pass more than directory name(or full path of directory)
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        public static void removeDirectory(String current_Path, directory current_directory , String argument = "")
        {
            if(argument == "")
            {
                Console.WriteLine("rd command syntax is \n rd[directory] + \n [directory] can be a directory name or fullpath of a directory" +
                    "\n + after[directory] represent that you can pass more than directory name(or full path of directory)");
            }
            else
            {
                //if the argument is a name 
                if (!argument.Contains('\\'))
                {
                    int index = current_directory.search(argument);
                    if (index == -1)
                    {
                        Console.WriteLine($"This directory\" {argument}\" is not  exist!");
                    }
                    else
                    {
                        directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);

                        if(obj.dir_firstcluster == 0)
                        {
                            Console.WriteLine($"Are you sure that you want to delete the {obj.dir_name} directory ?" +
                                $"if yes press Y, if not press N");
                            string s = Console.ReadLine().ToLower();
                            if (s.Equals("y")) { 
                                obj.deleteDirectory();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: directory is not empty");
                        }
                    }
                }
                else // if the argument is full path
                {
                    directory obj = Program.moveToDir(argument);

                    if(obj == null)
                    {
                        Console.WriteLine("Error : the path is not esixt");
                    }
                    else
                    {
                        if(obj.dir_firstcluster == 0)
                        {
                            Console.WriteLine($"Are you sure that you want to delete the {obj.dir_name} directory ?" +
                                $"if yes press Y, if not press N");
                            string s = Console.ReadLine().ToLower();
                            if (s.Equals("y"))
                            {
                                obj.deleteDirectory();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: directory is not empty");
                        }
                    }
                }
            }
        }



        /// <summary>
        /// type: Displays the contents of a text file.
        /// type command syntax is
        /// type[file]+
        /// NOTE: it displays the filename before its content for every file
        /// [file] can be file Name(or fullpath of file) of text file
        /// + after[file] represent that you can pass more than file Name(or fullpath of file).
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        public static void type(String current_Path, directory current_directory, String argument = "")
        {
            if (argument == "")
            {
                Console.WriteLine("type command syntax is " +
                    "\n type[file] + [file] can be file Name(or fullpath of file) of text file" +
                    "\n + after[file] represent that you can pass more than file Name(or fullpath of file)." +
                    "\n So, you expect from user to input type and arguments represent file names or a full paths to file names.");
            }
            else
            {
                //if the file is name 
                if (!argument.Contains('\\'))
                {
                    int index = current_directory.search(argument);
                    if(index == -1)
                    {
                        Console.WriteLine("the file is not exist");
                    }
                    else
                    {
                        File_Entry obj = new File_Entry(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory, 
                            current_directory.dirsOrfiles[index].dir_fileSize);
                        obj.readFile();
                        obj.print();
                    }
                }
                else //if the file is full path
                {
                    File_Entry obj = Program.moveToFile(argument);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: file is not exist");
                    }
                    else
                    {
                        obj.readFile();
                        obj.print();
                    }
                }
            }
        }



        /// <summary>
        /// Renames a file.
        /// rename command syntax is
        /// rename [fileName][new fileName]
        /// [fileName] can be a file name or fullpath of a filename
        /// [new fileName] can be a new file name not fullpath
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        /// <param name="NewFileName"></param>
        public static void rename(String current_Path, directory current_directory,String argument = "", String NewFileName = "")
        {
            if(argument == "" || NewFileName == "")
            {
                Console.WriteLine("Renames a file. \n rename command syntax is" +
                    "\n rename [fileName][new fileName] => [fileName] can be a file name or fullpath of a filename +" +
                    "\n [new fileName]  => can be a new file name not fullpath");
            }
            else
            {
                //if the argument is the name 
                if (!argument.Contains('\\'))
                {
                    int index = current_directory.search(argument);
                    if (index == -1)
                    {
                        Console.WriteLine("the file is not exist");
                    }
                    else
                    {
                        Directory_Entry Old = new Directory_Entry(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, 
                            current_directory.dirsOrfiles[index].dir_fileSize);
                        
                        if (current_directory.search(NewFileName) != -1)
                        {
                            Console.WriteLine("Error: the file is already exist");
                        }
                        else
                        {
                            Directory_Entry New = new Directory_Entry(NewFileName, Old.attr,
                                Old.dir_firstcluster, Old.dir_fileSize);
                            current_directory.update(Old, New);
                            current_directory.writeDirectory();
                        }
                    }
                }
                else
                {
                    File_Entry obj = Program.moveToFile(argument);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: path is not exist");
                    }
                    else
                    {
                        Directory_Entry Old = new Directory_Entry(obj.dir_name, obj.attr, obj.dir_firstcluster,
                            obj.dir_fileSize);

                        argument = argument.Remove(argument.LastIndexOf('\\'));
                        directory parent = Program.moveToDir(argument);
                        if (parent.search(NewFileName) != -1)
                        {
                            Console.WriteLine("Error: The file is already exist");
                        }
                        else
                        {
                            Directory_Entry New = new Directory_Entry(NewFileName, Old.attr,
                                Old.dir_firstcluster, Old.dir_fileSize);
                            parent.update(Old, New);
                            parent.writeDirectory();
                        }
                    }
                }
            }
        }



        /// <summary>
        /// del: Deletes one or more files.
        /// NOTE: it confirms the user choice to delete the file before deleting
        /// del command syntax is
        /// del[dirFile]+
        /// + after[dirfile] represent that you can pass more than file Name(or fullpath of file) or directory name(or fullpath of directory)
        /// [dirfile] can be file Name(or fullpath of file) or directory name(or fullpath of directory).
        /// </summary>
        /// <param name="current_Path"></param>
        /// <param name="current_directory"></param>
        /// <param name="argument"></param>
        public static void delete(String current_Path, directory current_directory, String argument = "")
        {
            if(argument == "")
            {
                Console.WriteLine("del: Deletes one or more files. \n del command syntax is" +
                    "\n del[dirFile] +" +
                    "\n+after[dirfile] represent that you can pass more than file Name(or fullpath of file) or directory name(or fullpath of directory)" +
                    "\n [dirfile] can be file Name(or fullpath of file) or directory name(or fullpath of directory)");
            }
            else
            {
                //a - if the argument is file name 
                if(!argument.Contains('\\')&& argument.Contains(".txt"))
                {
                    int index = current_directory.search(argument);
                    if(index == -1)
                    {
                        Console.WriteLine("Error: the file is not exist");
                    }
                    else
                    {
                        File_Entry obj = new File_Entry(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory, 
                            current_directory.dirsOrfiles[index].dir_fileSize);
                       
                        Console.WriteLine($"Are you sure that you want to delete the {obj.dir_name} file ?" +
                                $"if yes press Y, if not press N");
                        string s = Console.ReadLine().ToLower();
                        if (s.Equals("y"))
                        {
                            obj.deleteFile();
                        }
                    }
                }
                else if(argument.Contains('\\') && argument.Contains(".txt"))//b- full path of file
                {
                    File_Entry obj = Program.moveToFile(argument);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: file is not exist");
                    }
                    else
                    {
                        Console.WriteLine($"Are you sure that you want to delete the {obj.dir_name} file ?" +
                                $"if yes press Y, if not press N");
                        string s = Console.ReadLine().ToLower();
                        if (s.Equals("y"))
                        {
                            obj.deleteFile();
                        }
                    }
                }
                else if(!argument.Contains('\\') && !argument.Contains(".txt"))//c- directory name
                {
                    int index = current_directory.search(argument);
                    if(index == -1)
                    {
                        Console.WriteLine("Error: the directory is not exist");
                    }
                    else
                    {
                        directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                        obj.readDirectory();
                        Console.WriteLine($"Are you sure that you want to delete the {obj.dir_name} all files ?" +
                               $"if yes press Y, if not press N");
                        string s = Console.ReadLine().ToLower();
                        if(s == "y")
                        {
                            for(int i = 0; i<obj.dirsOrfiles.Count; i++)
                            {
                                if(obj.dirsOrfiles[i].attr == 0x0)
                                {
                                    File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                                   obj.dirsOrfiles[i].attr,
                                   obj.dirsOrfiles[i].dir_firstcluster, obj, 
                                   obj.dirsOrfiles[index].dir_fileSize);
                                    file.deleteFile();
                                }
                            }
                        }
                    }
                }
                else if(!argument.Contains('\\') && argument.Contains(".txt"))//d- full path of directory
                {
                    directory obj = Program.moveToDir(argument);
                    if(obj == null)
                    {
                        Console.Write("Error: the directory is not exist");
                    }
                    else
                    {
                        obj.readDirectory();
                        Console.WriteLine($"Are you sure that you want to delete the {obj.dir_name} all files ?" +
                              $"if yes press Y, if not press N");
                        string s = Console.ReadLine().ToLower();
                        if (s == "y")
                        {
                            for (int i = 0; i < obj.dirsOrfiles.Count; i++)
                            {
                                if (obj.dirsOrfiles[i].attr == 0x0)
                                {
                                    File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                                   obj.dirsOrfiles[i].attr,
                                   obj.dirsOrfiles[i].dir_firstcluster, obj,
                                   obj.dirsOrfiles[i].dir_fileSize);
                                    file.deleteFile();
                                }
                            }
                        }
                    }
                }
            }
        }



        public static void createFile(directory obj , String fileName , String fileContent)
        {
            File_Entry file = new File_Entry(fileName, 0x0, 0, obj , fileContent.Length);
            Directory_Entry dirEntry = file.getMyDirectoryEntry();
            if (obj.canAddEntry(dirEntry) == false)
            {
                Console.WriteLine("Error: there is no spcae ");
            }
            else
            {
                obj.addEntry(dirEntry);
                file.writeFile();
            }
        }

        public static void overWriteFile()
        {

        }
        public static void copy(String current_Path, directory current_directory, String argument1 = "", String argument2 = "")
        {
            if (argument1 == "")
            {
                Console.WriteLine("Copies one or more files to another location. " +
                    "\n\n copy command syntax is \n copy[source] \n or \n copy[source][destination] " +
                    "\n[source] can be file Name(or fullpath of file) or directory Name(or fullpath of directory)" +
                    "\n[destination] can be file Name(or fullpath of file) or directory name or fullpath of a directory");
            }
            else if (argument2 == "")
            {
                //a- if argument is file name
                if(!argument1.Contains('\\') && argument1.Contains(".txt")) 
                {
                    if (current_directory.search(argument1) == -1)
                    {
                        Console.WriteLine("Error : file is not exist");
                    }
                    else
                    {
                        Console.WriteLine("Error : the file is already exist , " +
                            "As the file is can not be copied on it self");
                    }

                }
                else if(argument1.Contains('\\') && argument1.Contains(".txt"))//b- full path of file
                {
                    File_Entry obj = Program.moveToFile(argument1);
                    if (obj == null)
                    {
                        Console.WriteLine("Error: The file is not exist");
                    }
                    else
                    {
                        obj.readFile();
                        if(current_directory.search(obj.dir_name) == -1)
                        {
                            //createFile
                        }
                        else
                        {
                            Console.WriteLine("Note , there is the file with same name, " +
                                "do you want to replace it ? \n if yes press Y , if not press N");
                            string s = Console.ReadLine().ToLower();
                            if(s == "y")
                            {
                                //overwrite file
                            }
                        }
                    }
                }
                else if(!argument1.Contains('\\') && !argument1.Contains(".txt"))//c- directory name
                {
                    int index = current_directory.search(argument1);
                    if(index == -1)
                    {
                        Console.WriteLine("Error: The directory is not found");
                    }
                    else
                    {
                        directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                        obj.readDirectory();

                        for(int i = 0; i < obj.dirsOrfiles.Count; i++)
                        {
                            if (obj.dirsOrfiles[i].attr == 0x0)
                            {
                                File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                               obj.dirsOrfiles[i].attr,
                               obj.dirsOrfiles[i].dir_firstcluster, obj,
                               obj.dirsOrfiles[i].dir_fileSize);
                                file.readFile();
                                if (current_directory.search(file.dir_name) == -1)
                                {
                                    //createFile
                                }
                                else
                                {
                                    Console.WriteLine("Note , there is the file with same name, " +
                                        "do you want to replace it ? \n if yes press Y , if not press N");
                                    string s = Console.ReadLine().ToLower();
                                    if (s == "y")
                                    {
                                        //overwrite file
                                    }
                                }
                            }
                        }
                    }
                }
                else if(argument1.Contains('\\') && !argument1.Contains(".txt"))//d- directory fullPath
                {
                    directory obj = Program.moveToDir(argument1);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: the directory is not exist");
                    }
                    else
                    {
                        obj.readDirectory();
                        for (int i = 0; i < obj.dirsOrfiles.Count; i++)
                        {
                            if (obj.dirsOrfiles[i].attr == 0x0)
                            {
                                File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                               obj.dirsOrfiles[i].attr,
                               obj.dirsOrfiles[i].dir_firstcluster, obj,
                               obj.dirsOrfiles[i].dir_fileSize);
                                file.readFile();
                                if (current_directory.search(file.dir_name) == -1)
                                {
                                    //createFile
                                }
                                else
                                {
                                    Console.WriteLine("Note , there is the file with same name, " +
                                        "do you want to replace it ? \n if yes press Y , if not press N");
                                    string s = Console.ReadLine().ToLower();
                                    if (s == "y")
                                    {
                                        //overwrite file
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //a- if argument is file name
                if (!argument1.Contains('\\') && argument1.Contains(".txt"))
                {
                    int index = current_directory.search(argument1);
                    if(index == -1)
                    {
                        Console.WriteLine("Error: the file is not exist");
                    }
                    else
                    {
                        File_Entry obj = new File_Entry(current_directory.dirsOrfiles[index].dir_name,
                               current_directory.dirsOrfiles[index].attr,
                               current_directory.dirsOrfiles[index].dir_firstcluster, current_directory,
                               current_directory.dirsOrfiles[index].dir_fileSize);
                        obj.readFile();
                        destinationCasesfile(current_directory, argument2);
                    }
                }
                else if (argument1.Contains('\\') && argument1.Contains(".txt"))//b- full path of file
                {
                    File_Entry obj = Program.moveToFile(argument1);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: The file is not exist");
                    }
                    else
                    {
                        obj.readFile();
                        destinationCasesfile(current_directory, argument2);
                    }
                }
                else if (!argument1.Contains('\\') && !argument1.Contains(".txt"))//c- directory name
                {
                    int index = current_directory.search(argument1);
                    if(index == -1)
                    {
                        Console.Write("Error : The directory in not exist");
                    }
                    else
                    {
                        directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                            current_directory.dirsOrfiles[index].attr,
                            current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                        obj.readDirectory();
                        destinationCasesDir(current_directory, obj, argument2);
                    }
                }
                else if (argument1.Contains('\\') && !argument1.Contains(".txt"))//d- directory fullPath
                {
                    directory obj = Program.moveToDir(argument1);
                    if(obj == null)
                    {
                        Console.WriteLine("Error: The directory is not exist");
                    }
                    else
                    {
                        obj.readDirectory();
                        destinationCasesDir(current_directory, obj, argument2);
                    }
                }
            }
        }

        public static void destinationCasesfile(directory current_directory, String argument)
        {
            //a- if argument is file name
            if (!argument.Contains('\\') && argument.Contains(".txt"))
            {
                int index = current_directory.search(argument);
                if(index == -1)
                {
                    //create file
                }
                else
                {
                    Console.WriteLine("Note , there is the file with same name, " +
                                    "do you want to replace it ? \n if yes press Y , if not press N");
                    string s = Console.ReadLine().ToLower();
                    if (s == "y")
                    {
                        //overwrite file
                    }
                }
            }
            else if (argument.Contains('\\') && argument.Contains(".txt"))//b- full path of file
            {
                File_Entry obj = Program.moveToFile(argument);
                directory parent = Program.moveToDir(argument.Remove(argument.LastIndexOf('\\')));
                if (obj == null)
                {
                    //create file
                }
                else
                {
                    Console.WriteLine("Note , there is the file with same name, " +
                                   "do you want to replace it ? \n if yes press Y , if not press N");
                    string s = Console.ReadLine().ToLower();
                    if (s == "y")
                    {
                        //overwrite file
                    }
                }
            }
            else if (!argument.Contains('\\') && !argument.Contains(".txt"))//c- directory name
            {
                int index = current_directory.search(argument);
                if (index == -1)
                {
                    Console.Write("Error : The directory in not exist");
                }
                else
                {
                    directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                        current_directory.dirsOrfiles[index].attr,
                        current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                    obj.readDirectory();
                    for (int i = 0; i < obj.dirsOrfiles.Count; i++)
                    {
                        if (obj.dirsOrfiles[i].attr == 0x0)
                        {
                            File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                           obj.dirsOrfiles[i].attr,
                           obj.dirsOrfiles[i].dir_firstcluster, obj,
                           obj.dirsOrfiles[i].dir_fileSize);
                            file.readFile();
                            if (current_directory.search(file.dir_name) == -1)
                            {
                                //createFile
                            }
                            else
                            {
                                Console.WriteLine("Note , there is the file with same name, " +
                                    "do you want to replace it ? \n if yes press Y , if not press N");
                                string s = Console.ReadLine().ToLower();
                                if (s == "y")
                                {
                                    //overwrite file
                                }
                            }
                        }
                    }
                }
            }
            else if (argument.Contains('\\') && !argument.Contains(".txt"))//d- directory fullPath
            {
                directory obj = Program.moveToDir(argument);
                if (obj == null)
                {
                    Console.WriteLine("Error: the directory is not exist");
                }
                else
                {
                    obj.readDirectory();
                    for (int i = 0; i < obj.dirsOrfiles.Count; i++)
                    {
                        if (obj.dirsOrfiles[i].attr == 0x0)
                        {
                            File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                           obj.dirsOrfiles[i].attr,
                           obj.dirsOrfiles[i].dir_firstcluster, obj,
                           obj.dirsOrfiles[i].dir_fileSize);
                            file.readFile();
                            if (current_directory.search(file.dir_name) == -1)
                            {
                                //createFile
                            }
                            else
                            {
                                Console.WriteLine("Note , there is the file with same name, " +
                                    "do you want to replace it ? \n if yes press Y , if not press N");
                                string s = Console.ReadLine().ToLower();
                                if (s == "y")
                                {
                                    //overwrite file
                                }
                            }
                        }
                    }
                }
            }
        }


        public static void destinationCasesDir(directory current_directory,directory sourceObj, String argument)
        {
            //a- if argument is file name
            if (!argument.Contains('\\') && argument.Contains(".txt"))
            {
                String content = string.Empty;

                for (int i = 0; i < sourceObj.dirsOrfiles.Count; i++)
                {
                    if (sourceObj.dirsOrfiles[i].attr == 0x0)
                    {
                        File_Entry file = new File_Entry(sourceObj.dirsOrfiles[i].dir_name,
                       sourceObj.dirsOrfiles[i].attr,
                       sourceObj.dirsOrfiles[i].dir_firstcluster, sourceObj, sourceObj.dirsOrfiles[i].dir_fileSize);
                        file.readFile();
                        content += file.content;
                    }
                }
                int index = current_directory.search(argument);
                if(index == -1)
                {
                    //creat file
                }
                else
                {
                    Console.WriteLine("Note , there is the file with same name, " +
                                        "do you want to replace it ? \n if yes press Y , if not press N");
                    string s = Console.ReadLine().ToLower();
                    if (s == "y")
                    {
                        //overwrite file
                    }
                }
            }
            else if (argument.Contains('\\') && argument.Contains(".txt"))//b- full path of file
            {
                String content = string.Empty;
                for (int i = 0; i < sourceObj.dirsOrfiles.Count; i++)
                {
                    if (sourceObj.dirsOrfiles[i].attr == 0x0)
                    {
                        File_Entry file = new File_Entry(sourceObj.dirsOrfiles[i].dir_name,
                       sourceObj.dirsOrfiles[i].attr,
                       sourceObj.dirsOrfiles[i].dir_firstcluster, sourceObj,
                       sourceObj.dirsOrfiles[i].dir_fileSize);
                        file.readFile();
                        content += file.content;
                    }
                }
                File_Entry obj = Program.moveToFile(argument);
                directory parent = Program.moveToDir(argument.Remove(argument.LastIndexOf('\\')));
                if (obj == null)
                {
                    
                    //create file
                }
                else
                {
                    Console.WriteLine("Note , there is the file with same name, " +
                                       "do you want to replace it ? \n if yes press Y , if not press N");
                    string s = Console.ReadLine().ToLower();
                    if (s == "y")
                    {
                        //overwrite file
                    }
                }

            }
            else if (!argument.Contains('\\') && !argument.Contains(".txt"))//c- directory name
            {
                int index = current_directory.search(argument);
                if (index == -1)
                {
                    Console.Write("Error : The directory in not exist");
                }
                else
                {
                    directory obj = new directory(current_directory.dirsOrfiles[index].dir_name,
                        current_directory.dirsOrfiles[index].attr,
                        current_directory.dirsOrfiles[index].dir_firstcluster, current_directory);
                    obj.readDirectory();
                    for (int i = 0; i < obj.dirsOrfiles.Count; i++)
                    {
                        if (obj.dirsOrfiles[i].attr == 0x0)
                        {
                            File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                           obj.dirsOrfiles[i].attr,
                           obj.dirsOrfiles[i].dir_firstcluster, obj,
                           obj.dirsOrfiles[i].dir_fileSize);
                            file.readFile();
                            if (current_directory.search(file.dir_name) == -1)
                            {
                                //createFile
                            }
                            else
                            {
                                Console.WriteLine("Note , there is the file with same name, " +
                                    "do you want to replace it ? \n if yes press Y , if not press N");
                                string s = Console.ReadLine().ToLower();
                                if (s == "y")
                                {
                                    //overwrite file
                                }
                            }
                        }
                    }
                }
            }
            else if (argument.Contains('\\') && !argument.Contains(".txt"))//d- directory fullPath
            {
                directory obj = Program.moveToDir(argument);
                if (obj == null)
                {
                    Console.WriteLine("Error: the directory is not exist");
                }
                else
                {
                    obj.readDirectory();
                    for (int i = 0; i < obj.dirsOrfiles.Count; i++)
                    {
                        if (obj.dirsOrfiles[i].attr == 0x0)
                        {
                            File_Entry file = new File_Entry(obj.dirsOrfiles[i].dir_name,
                           obj.dirsOrfiles[i].attr,
                           obj.dirsOrfiles[i].dir_firstcluster, obj,
                           obj.dirsOrfiles[i].dir_fileSize);
                            file.readFile();
                            if (current_directory.search(file.dir_name) == -1)
                            {
                                //createFile
                            }
                            else
                            {
                                Console.WriteLine("Note , there is the file with same name, " +
                                    "do you want to replace it ? \n if yes press Y , if not press N");
                                string s = Console.ReadLine().ToLower();
                                if (s == "y")
                                {
                                    //overwrite file
                                }
                            }
                        }
                    }
                }
            }
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
