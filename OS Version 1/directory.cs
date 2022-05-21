using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;



namespace OS_Version_1
{
    class directory : Directory_Entry
    {
        public List<Directory_Entry> dirsOrfiles = new List<Directory_Entry>();
        public directory parent;


        public directory() { }
        public directory(string name, byte type, int firstcluster, directory p ,[Optional] int[] empty, [Optional] int size)
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




        // Method to convert any object to array of byte
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }



        //to split array of data in to arrays of size 1024
        /*int index1 = 0;
        int index2 = 0;
        for(int i = 0; i < data.Length; i++)
        {
            list[index1][index2] = data[i];
            index2++;
            if(i%1024 == 0)
            {
                index1++;
                index2 = 0;
            }
        }*/

        //1-Method to write directory
        /*public void writeDirectory()
        {
            Directory_Entry obj = getMyDirectoryEntry();
            
            byte[] data = new byte[dirsOrfiles.Count * 32];
            List<byte> conv = new List<byte>();
            List<byte[]> list = new List<byte[]>();


            for(int i = 0; i < dirsOrfiles.Count; i++)
            {
                Directory_Entry objElement = dirsOrfiles[i];

                for(int j = 0; j < objElement.dir_name.Length; j++)
                {
                    conv.Add(Convert.ToByte(objElement.dir_name[j]));
                }
                conv.Add(objElement.attr);

                byte[] emptyByte = new byte[12];
                if (objElement.dir_empty != null) {
                    Buffer.BlockCopy(objElement.dir_empty, 0, emptyByte, 0, emptyByte.Length);
                }
                else
                {
                    for(int j =0; j< emptyByte.Length; j++)
                    {
                        emptyByte[j] = 0;
                    }
                }

                conv.AddRange(emptyByte);

                conv.Add(Convert.ToByte(objElement.dir_firstcluster));
                conv.Add(Convert.ToByte(objElement.dir_fileSize));
            }
            data = conv.ToArray();
            
            list = Mini_Fat.splitBytes(data);
            // write the directory
            int clusterIndex;
            int lastCluster = -1;
            if( this.dir_firstcluster == 0)
            {
                clusterIndex = Mini_Fat.getAvailableCluster();
                this.dir_firstcluster  = clusterIndex;
            }
            else
            {
                //empty all its cluster 
                emptyMyClusters();
                clusterIndex = Mini_Fat.getAvailableCluster();
                this.dir_firstcluster = clusterIndex;
            }


            for(int i=0; i<list.Count; i++)
            {
                virtualDisk.writeCluster(clusterIndex, list[i]);
                Mini_Fat.setClusterStatus(clusterIndex, -1);
                if(lastCluster != -1)
                {
                    Mini_Fat.setClusterStatus(lastCluster, clusterIndex);
                }
                lastCluster = clusterIndex;
                clusterIndex = Mini_Fat.getAvailableCluster();
            }
            Directory_Entry obj2 = getMyDirectoryEntry();
            
            if(parent != null)
            {
                parent.update(obj,obj2);
                
                //q: infinet loop recursion
                parent.writeDirectory();
            }
            Mini_Fat.writeFat();
        }*/


        public void writeDirectory()
        {
            Directory_Entry obj = getMyDirectoryEntry();

            byte[] data = new byte[dirsOrfiles.Count * 32];
            List<byte> conv = new List<byte>();
            List<byte[]> list = new List<byte[]>();


            for (int i = 0; i < dirsOrfiles.Count; i++)
            {
                Directory_Entry objElement = dirsOrfiles[i];

                for (int j = 0; j < objElement.dir_name.Length; j++)
                {
                    byte b;
                    b = Convert.ToByte(objElement.dir_name[j]);
                    conv.Add(b);
                }
                conv.Add(objElement.attr);

                byte[] emptyByte = new byte[12];
                if (objElement.dir_empty != null)
                {
                    Buffer.BlockCopy(objElement.dir_empty, 0, emptyByte, 0, emptyByte.Length);
                }
                else
                {
                    for (int j = 0; j < emptyByte.Length; j++)
                    {
                        emptyByte[j] = 0;
                    }
                }

                conv.AddRange(emptyByte);

                byte[] fc = new byte[4];
                fc = BitConverter.GetBytes(objElement.dir_firstcluster);
                conv.AddRange(fc);

                conv.AddRange(BitConverter.GetBytes(objElement.dir_fileSize));
            }
            data = conv.ToArray();

            list = Mini_Fat.splitBytes(data);
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

        public static List<byte[]> splitBytes(byte[] bytes)
        {
            List<byte[]> ls = new List<byte[]>();
            int number_of_arrays = bytes.Length / 32;
            int rem = bytes.Length % 32;
            for (int i = 0; i < number_of_arrays; i++)
            {
                byte[] b = new byte[32];
                for (int j = i * 32, k = 0; k < 32; j++, k++)
                {
                    b[k] = bytes[j];
                }
                ls.Add(b);
            }
            if (rem > 0)
            {
                byte[] b1 = new byte[32];
                for (int i = number_of_arrays * 32, k = 0; k < rem; i++, k++)
                {
                    b1[k] = bytes[i];
                }
                ls.Add(b1);
            }
            return ls;
        }
        /*//2-Method to read the directory
        public void readDirectory()
        {
            if (this.dir_firstcluster != 0)
            {
                //empty the dirOrFiles
                dirsOrfiles = new List<Directory_Entry>();
                
                int clusterIndex = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(clusterIndex);

                do
                {
                    byte[] data = virtualDisk.readCluster(clusterIndex);
                    List<byte[]> list = new List<byte[]>();
                    
                    //to split array of data in to arrays of size 32
                    int index1 = 0;
                    int index2 = 0;
                    for (int i = 0; i < data.Length; i++)
                    {

                        list[index1][index2] = data[i];
                        index2++;
                        if (i % 32 == 0)
                        {
                            index1++;
                            index2 = 0;
                        }
                    }

                    
                    for(int i=0; i < list.Count; i++)
                    {
                        //intialize object from Directory_Entry
                        Directory_Entry obj = new Directory_Entry();

                        //0->10
                       //System.Text.Encoding.UTF8.GetString(list[i]).ToCharArray();
                        obj.dir_name = Convert.ToBase64String(list[i]);
                        //11
                        obj.attr = list[i][11];
                        //start 12 end 12+12 = 23
                        Buffer.BlockCopy(list[i], 12, obj.dir_empty, 0, obj.dir_empty.Length);
                        
                        obj.dir_firstcluster= BitConverter.ToInt32(list[i], 24);
                        //24+4=28
                        //28+4=32
                        obj.dir_fileSize = BitConverter.ToInt32(list[i], 28);
                        dirsOrfiles.Add(obj);
                    }

                    clusterIndex = next;
                    if (next != -1)
                    {
                        next = Mini_Fat.getClusterStatus(clusterIndex);
                    }
                } while (clusterIndex != -1);
            }
        }*/
      
        
        public static Directory_Entry BytesToDirectoryEntry(Byte[] bytes)
        {
            char[] name = new char[11];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = (char)bytes[i];
            }
            byte attr = bytes[11];
            byte[] empty = new byte[12];
            int j = 12;
            for (int i = 0; i < empty.Length; i++)
            {
                empty[i] = bytes[j];
                j++;
            }
            byte[] fc = new byte[4];
            for (int i = 0; i < fc.Length; i++)
            {
                fc[i] = bytes[j];
                j++;
            }
            int firstcluster = BitConverter.ToInt32(fc, 0);
            byte[] sz = new byte[4];
            for (int i = 0; i < sz.Length; i++)
            {
                sz[i] = bytes[j];
                j++;
            }
            int filesize = BitConverter.ToInt32(sz, 0);


            int[] arr = new int[3];
            Buffer.BlockCopy(empty, 0, arr, 0, arr.Length);
            Directory_Entry obj = new Directory_Entry(new string(name), attr, firstcluster, filesize, arr);
            return obj;

        }



        //2-Method to read the directory
        public void readDirectory()
        {
            if (this.dir_firstcluster != 0)
            {
                //empty the dirOrFiles
                dirsOrfiles = new List<Directory_Entry>();

                int clusterIndex = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(clusterIndex);
                
                List<byte> data = new List<byte>();
                List<byte[]> list = new List<byte[]>();
                do
                {
                    data.AddRange(virtualDisk.readCluster(clusterIndex));
                    
                    clusterIndex = next;
                    if (next != -1)
                    {
                        next = Mini_Fat.getClusterStatus(clusterIndex);
                    }
                } while (clusterIndex != -1);
                list = splitBytes(data.ToArray());

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i][0] == 0)
                    {
                        break;
                    }
                    dirsOrfiles.Add(BytesToDirectoryEntry(list[i]));
                }
            }
        }
        
        /*public void readDirectory()
        {
            if (this.dir_firstcluster != 0)
            {
                dirsOrfiles = new List<Directory_Entry>();
                int cluster = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(cluster);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(virtualDisk.readCluster(cluster));
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_Fat.getClusterStatus(cluster);
                }
                while (next != -1);
                for (int i = 0; i < ls.Count; i++)
                {
                    byte[] b = new byte[32];
                    for (int k = i * 32, m = 0; m < b.Length && k < ls.Count; m++, k++)
                    {
                        b[m] = ls[k];
                    }
                    if (b[0] == 0)
                        break;
                    dirsOrfiles.Add(BytesToDirectoryEntry(b));
                }
            }
        }
        */
        
        
        //3-Method to get directory entery
        public Directory_Entry getMyDirectoryEntry()
        {
            Directory_Entry MyDirectory = new Directory_Entry(this.dir_name, this.attr,
                 this.dir_firstcluster, this.dir_fileSize, this.dir_empty);
            return MyDirectory;
        }
       


        //4-Method to search for element 
        public int search(String name)
        {
            int index = -1;
            if (name.Length < 11)
            {
                //name += "\0";
                for (int i = name.Length + 1; i < 12; i++)
                    name += " ";
            }
            else
            {
                name = name.Substring(0, 11);
            }
            
            for (int i=0; i < dirsOrfiles.Count; i++)
            {
                if(name == dirsOrfiles[i].dir_name)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }



        //5-Method to update dirsOrfiles List
        public void update(Directory_Entry oldOne , Directory_Entry newOne)
        {
            int index = search(oldOne.dir_name);
            dirsOrfiles.RemoveAt(index);
            dirsOrfiles.Insert(index, newOne);
        }



        //6- Method to remove Entry
        public void removeEnetry(Directory_Entry obj)
        {
            int index = search(obj.dir_name);
            dirsOrfiles.RemoveAt(index);
            writeDirectory();
        }



        //7-Method to add Entry
        public void addEntry(Directory_Entry obj)
        {
            dirsOrfiles.Add(obj);
            writeDirectory();
        }




        //8-Method to empty clusters
        public void emptyMyClusters()
        {
            if (this.dir_firstcluster != 0)
            {
                int clusterIndex = this.dir_firstcluster;
                int next = Mini_Fat.getClusterStatus(clusterIndex);

                //root 
                if(clusterIndex == 5 && next == 0)
                {
                    return;
                }

                do
                {
                    Mini_Fat.setClusterStatus(clusterIndex, 0);
                    clusterIndex = next;
                    if(next != -1)
                    {
                        next = Mini_Fat.getClusterStatus(clusterIndex);
                    }
                } while (clusterIndex != -1);
            }
        }


        //9_Method to delete Directory
        public void deleteDirectory()
        {
            emptyMyClusters();
            if(parent != null)
            {
                parent.removeEnetry(getMyDirectoryEntry());
            }
            
        }

        //10-Method to get size on disk
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

        //11-Method to check if we can add or not 
        public bool canAddEntry(Directory_Entry obj)
        {
           // Directory_Entry obj1 = getMyDirectoryEntry();
            int size = (dirsOrfiles.Count+1)*32;
            int clusters = size / 1024;
            if (size % 1024 > 0)
            {
                clusters++;
            }
            clusters += obj.dir_fileSize / 1024;
            if (obj.dir_fileSize % 1024>0) {
                clusters++;
            }
            /*if(Mini_Fat.getAvailableCluster()+getMySizeOnDisk() >= clusters)
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return Mini_Fat.CountAvailableClusters() + getMySizeOnDisk() >= clusters ? true : false;
        } 
    }
}
