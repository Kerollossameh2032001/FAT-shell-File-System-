using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace OS_Version_1
{
    class virtualDisk
    {
        static  FileStream disk;
        static String path;
        

        ///1- <summary>
        /// Method that take the name of file and check if exists or not 
        /// if exists read the Fat from file
        /// if not create the file and prepare method to intialize the fat array then
        /// write it in file 
        /// </summary>
        /// <param name="fileName"></param>

        public static void intialize(String fileName)
        {
            path = fileName;
            if (!File.Exists(fileName))
            {
                disk = new FileStream(fileName, FileMode.Create);
                
                //write super block
                byte[] data = new byte[1024];
                for (int i = 0; i<1024; i++)
                {
                    data[i] = 0;
                }
                writeCluster(0,data);
                Mini_Fat.prepare_Fat();
                Mini_Fat.writeFat();

            }
            else
            {
                disk = new FileStream(fileName, FileMode.Open);
                Mini_Fat.setFatArray(Mini_Fat.readFat());
            }
            
        }

        //2-Method to write cluster in file
        public static void writeCluster(int cluster, byte[] data)
        {
            //disk = File.Open(path, FileMode.Open);
            disk.Seek(1024 * cluster, SeekOrigin.Begin);
            disk.Write(data);
            disk.Flush();
            //disk.Close();
        }



        //3-Method to read cluster from file
        public static byte[] readCluster(int index)
        {
            byte[] result = new byte[1024];

           // disk = File.Open(path, FileMode.Open);
            disk.Seek(1024 * index, SeekOrigin.Begin);

            disk.Read(result);
           // disk.Close();

            return result;
        }

        //4-Method to get empty spaces in virtualDisk
        public static int getEmptySize()
        {
            int size = Mini_Fat.CountAvailableClusters()*1024;
            return size;
        }
    }
}