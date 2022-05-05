using System;
using System.Collections.Generic;
using System.Text;

namespace OS_Version_1
{
    class Directory_Entry
    {
        
        public string dir_name ;
        //attr -> 0x0(file),,,  0x10(directory)
        public byte attr;
        public int[] dir_empty = new int[3];
        public int dir_firstcluster;
        public int dir_fileSize;



        public Directory_Entry()
        {
        }
        // file (kero.txt)   directory (kerollos)
        public Directory_Entry(string name , byte type , int[] empty,int firstcluster , int size)
        {
            this.attr = type;
            if (this.attr == 0x10)
            {
                dir_name = writeDirName(name);
            }
            else
            {
                dir_name = writeFileName(name);
            }
            dir_empty = empty;
            dir_firstcluster = firstcluster;
            dir_fileSize = size;
        }


        public String writeDirName(String fullName)
        {
            String name = "";
            for (int i = 0; i < fullName.Length; i++)
            {
                if (i == 11)
                {
                    break;
                }
                name += fullName[i];
            }
            return name;
        }
        public String writeFileName(String fullName)
        {
            String name = "";
            if(fullName.Length > 11)
            {
                for (int i = 0; i < 7; i++)
                {
                    name += fullName[i];
                }
                for (int i = fullName.Length - 4; i < fullName.Length; i++)
                {
                    name += fullName[i];
                }

            }
            else
            {
               name = fullName;
                
            }

            return name;
        }
    }
}