using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;


namespace OS_Version_1
{
    class File_Entry : Directory_Entry
    {
        public String content;
        public directory parent;


        public File_Entry(string name, byte type, int firstcluster, directory p, [Optional] int[] empty, [Optional] int size)
        {
            this.attr = type;
            if (this.attr == 0x10)
            {
                this.dir_name = writeDirName(name);
            }
            else
            {
                this.dir_name = writeFileName(name);
            }
            this.dir_empty = empty;
            this.dir_firstcluster = firstcluster;
            this.dir_fileSize = size;
            if(p != null)
            {
                this.parent = p;
            }
        }

        //1-Method to empty clusters
        public void emptyMyClusters()
        {
            if (this.dir_firstcluster != 0)
            {
                int clusterIndex = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(clusterIndex);

                //root 
                if (clusterIndex == 5 && next == 0)
                {
                    return;
                }

                do
                {
                    Mini_Fat.setClusterStatus(clusterIndex, 0);
                    clusterIndex = next;
                    if (next != -1)
                    {
                        next = Mini_Fat.getClusterStatus(clusterIndex);
                    }
                } while (clusterIndex != -1);
            }
        }


        //2-Method to get directory entery
        public Directory_Entry getMyDirectoryEntry()
        {

            Directory_Entry MyDirectory = new Directory_Entry(this.dir_name, this.attr,
                this.dir_empty, this.dir_firstcluster, this.dir_fileSize);
            return MyDirectory;
        }



        //3-Method to get size on disk
        public int getMySizeOnDisk()
        {
            int count = 0;
            if (this.dir_firstcluster != 0)
            {
                int clusterIndex = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(clusterIndex);
                do
                {
                    count++;
                    clusterIndex = next;
                    if (next != -1)
                    {
                        next = Mini_Fat.getClusterStatus(clusterIndex);
                    }
                } while (clusterIndex != -1);
            }
            return count;
        }


        //4_Method to delete Directory
        public void deleteDirectory()
        {
            emptyMyClusters();
            if (parent != null)
            {
                this.parent.removeEnetry(getMyDirectoryEntry());
            }

        }


        //5-Method to print The content of file
        public void print()
        {
            Console.WriteLine();
            Console.WriteLine(this.dir_name);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(content);
            Console.WriteLine();
        }

        
        //6-Method to write File
        public void writeFile()
        {
            Directory_Entry obj = getMyDirectoryEntry();
            byte[] data = new byte[content.Length * 32];
            List<byte[]> list = new List<byte[]>();

            data = Encoding.ASCII.GetBytes(content);

            //to split array of data in to arrays of size 1024
            int index1 = 0;
            int index2 = 0;
            for (int i = 0; i < data.Length; i++)
            {

                list[index1][index2] = data[i];
                index2++;
                if (i % 1024 == 0)
                {
                    index1++;
                    index2 = 0;
                }
            }

            // write the directory
            int clusterIndex;
            int lastCluster = -1;
            if (this.dir_firstcluster == 0)
            {
                clusterIndex = Mini_Fat.getAvailableCluster();
                this.dir_firstcluster = clusterIndex;
            }
            else
            {
                //empty all its cluster 
                emptyMyClusters();
                clusterIndex = Mini_Fat.getAvailableCluster();
                this.dir_firstcluster = clusterIndex;
            }

            for (int i = 0; i < list.Count; i++)
            {
                virtualDisk.writeCluster(clusterIndex, list[i]);
                Mini_Fat.setClusterStatus(clusterIndex, -1);
                if (lastCluster != -1)
                {
                    Mini_Fat.setClusterStatus(lastCluster, clusterIndex);
                }
                lastCluster = clusterIndex;
                clusterIndex = Mini_Fat.getAvailableCluster();
            }
            Directory_Entry obj2 = getMyDirectoryEntry();
            if (parent != null)
            {
                parent.update(obj, obj2);

                //q: infinet loop recursion
                parent.writeDirectory();
            }
            Mini_Fat.writeFat();
        }

        //7-Method to read the File
        public void readFile()
        {
            if (this.dir_firstcluster != 0)
            {

                content = string.Empty;

                int clusterIndex = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(clusterIndex);

                do
                {
                    byte[] data = virtualDisk.readCluster(clusterIndex);
                    content += Encoding.Default.GetString(data); 
                    clusterIndex = next;
                    if (next != -1)
                    {
                        next = Mini_Fat.getClusterStatus(clusterIndex);
                    }
                } while (clusterIndex != -1);
            }
        }
    }
}
