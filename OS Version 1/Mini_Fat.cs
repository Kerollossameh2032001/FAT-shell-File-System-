using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;


namespace OS_Version_1
{
    class Mini_Fat
    {
        // array to refer The Fat table
        static int[] Fat = new int[1024];
        
        //1- Method to prepare the Fat in intialize  
        public static void prepare_Fat()
        {
            for(int i = 0; i < Fat.Length; i++)
            {
                if(i <= 4)
                {
                    Fat[i] = -1;
                }
                else
                {
                    Fat[i] = 0;
                }
            }
        }

        //2-Method for print Fat fro debug
        public static void print_Fat_array()
        {
            for(int i=0; i<Fat.Length; i++)
            {
                Console.WriteLine(Fat[i]);
            }
        }

        //3-Method to return available cluster 
        public static int getAvailableCluster()
        {
            int index = -1;
            for(int i=0; i<Fat.Length; i++)
            {
                if (Fat[i] == 0)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //4-Method to get Cluster state
        public static int getClusterStatus(int clusterIndex)
        {
            return Fat[clusterIndex];
        }

        //5-Method to setting cluster value
        public static void setClusterStatus(int clusterindex, int status)
        {
            Fat[clusterindex] = status;
        }

        //6-Method to set the value of Fat Array
        public static void setFatArray(int[] arr)
        {
            Fat = arr;
        }

        //7-Method to write Fat array in virtual disk
        public static void writeFat()
        {
            //Buffer.BlockCopy(fat, 0, result, 0, result.Length);
            byte[] data = new byte[1024];
            
            //start from 1 bec superBlock is from index zero
            for (int i=1; i<5; i++)
            {
                Buffer.BlockCopy(Fat, i*256, data, 0, data.Length);
                virtualDisk.writeCluster(i,data);
            }
        }
        
        //8-Method to read Fat
        public static int[] readFat()
        {
            int[] readed = new int[1024];
            byte[] data = new byte[4096];

            //intialize List of byte to add the data of each cluster in one list    
            List<byte> list = new List<byte>();

            for(int i = 0; i<4; i++)
            {
                list.AddRange(virtualDisk.readCluster(i));
            }
            
            //store the list to data after converted it to array of byte
             data = list.ToArray();

            //convert the array of byte to array of int and return it 
             Buffer.BlockCopy(data, 0, readed, 0, data.Length);
            
            return readed;
        }


        //9-Method return number of available cluster
        public static int CountAvailableClusters()
        {
            int count = 0;
            for (int i = 0; i < Fat.Length; i++)
            {
                if (Fat[i] == 0)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
