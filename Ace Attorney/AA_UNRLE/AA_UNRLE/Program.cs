using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AA_UNRLE
{

    class Program
    {
       
        static void Main(string[] args)
        {
            string[] OAMs = Directory.GetFiles("OAMS_Comprimidos");

            foreach (string OAM in OAMs)
            {
                File.Create(OAM.Replace(".bin", ".new")).Close();
                Chunck(OAM);

            }
        }

        static void Chunck(string oam)
        {
            byte[] chunck = null;
            int numberPointers = 0, tablePosition = 0, chunckSize = 0, pointer1 = 0, pointer2 = 0, fileSizeInt = 0, lastPointer = 0;
            long fileSize = new FileInfo(oam).Length;
            fileSizeInt = (int)fileSize;
            List<int> compesadores = new List<int>();
            using (BinaryReader r = new BinaryReader(File.Open(oam, FileMode.Open)))
            {
                numberPointers = r.ReadInt32() / 4;
                r.BaseStream.Seek(tablePosition, SeekOrigin.Begin);
                pointer1 = r.ReadInt32();
                r.BaseStream.Seek(pointer1 - 4, SeekOrigin.Begin);
                lastPointer = r.ReadInt32();
                int tableSz = pointer1;
                r.BaseStream.Seek(0, SeekOrigin.Begin);
                byte[] table = r.ReadBytes(tableSz);
                File.WriteAllBytes(oam.Replace(".bin", ".new"), table);

                for (int i = 0; i < numberPointers; i++)
                {
                    PointerRecalc(tablePosition, oam);
                    r.BaseStream.Seek(tablePosition, SeekOrigin.Begin);
                    pointer1 = r.ReadInt32();
                    pointer2 = r.ReadInt32();
                    chunckSize = pointer2 - pointer1;


                    if (pointer1 == lastPointer)
                    {
                        chunckSize = fileSizeInt - lastPointer;
                    }

                    r.BaseStream.Seek(pointer1, SeekOrigin.Begin);
                    chunck = r.ReadBytes(chunckSize);
                    File.Create("temp").Close();
                    File.WriteAllBytes("temp", chunck);
                    tablePosition += 4;                                                           
                    Decomp(tablePosition, oam);


                }
            }

            ArrumandoAporraToda(oam.Replace(".bin", ".new"));
        }

        static void Decomp(int tablePosition, string fileName )
        {

            long fileSize = new FileInfo("temp").Length;
            byte[] bytes = new byte[] { 0x00, 0x00 };
            //int fileSizeInt = (int)fileSize;
            


            byte verify1 = 0;
            byte verify2 = 0;
            byte verify3 = 0;
            byte[] data;
            File.Create("tempUncomp").Close();
            int actualPosition = 0;

            using (BinaryReader b = new BinaryReader(File.Open("temp", FileMode.Open)))
            {


                using (BinaryWriter w = new BinaryWriter(File.Open("tempUncomp", FileMode.Append)))
                {

                    for (int i = 0; i < fileSize;)
                    {
                        
                            b.BaseStream.Seek(actualPosition, SeekOrigin.Begin);
                        
                        


                        verify1 = b.ReadByte();
                        verify2 = b.ReadByte();

                        if (verify2 == 0x80 || verify3 == 0x81)
                        {
                            int combined = verify2 << 8 | verify1;
                            int numTimes = (combined - 0x8000);
                            data = b.ReadBytes(0x02);


                            for (int j = 0; j < numTimes; j++)
                            {
                                w.Write(data);
                            }

                            if (actualPosition < fileSize - 4)
                            {
                                b.BaseStream.Seek(actualPosition + 5, SeekOrigin.Begin);
                                verify3 = b.ReadByte();
                                if (verify3 == 0x80 || verify3 == 0x81)
                                {
                                    actualPosition += 4;
                                    i += 4;
                                }

                                else
                                {
                                    actualPosition += 6;
                                    i += 6;
                                }
                            }
                            else
                            {
                                actualPosition += 4;
                                i += 4;
                            }






                        }
                        else
                        {
                            // b.BaseStream.Seek(actualPosition, SeekOrigin.Begin);
                            w.Write(verify1);
                            w.Write(verify2);
                            actualPosition += 2;
                            i += 2;
                        }


                    }


                    w.Close();

                }


                b.Close();
                Join(tablePosition, fileName);
            }


        }

        static void Join(int tablePosition, string fname) {

            byte[] uncomp = File.ReadAllBytes("tempUncomp");
            long fileSize = new FileInfo("tempUncomp").Length;
            short tamainho = (short)fileSize;
            //int fileSizeInt = (int)fileSize;
            using (BinaryWriter w = new BinaryWriter(File.Open(fname.Replace(".bin", ".new"), FileMode.Append)))
            {
                w.Write(tamainho);
                w.Write(uncomp);
                w.Close();
            }



        }

        static void PointerRecalc(int tablePosition, string fname)
        {
            long tm = new FileInfo(fname.Replace(".bin", ".new")).Length;
            int ponteiro = (int)tm;
            using (BinaryWriter w = new BinaryWriter(File.Open(fname.Replace(".bin", ".new"), FileMode.Open)))
            {
                w.Seek(tablePosition, SeekOrigin.Begin);
                w.Write(ponteiro);
                w.Close();
            }

        }

        static void ArrumandoAporraToda(string nomeAk)
        {
            int numberPointers=0,tamanhoTbl =0, tablePosition =0, ponteiro=0, tamanho = 0, posTabela = 0;
            byte[] chunckBackup;
           

            using (BinaryReader b = new BinaryReader(File.Open(nomeAk, FileMode.Open)))
            {
                b.BaseStream.Seek(tablePosition, SeekOrigin.Begin);
                tamanhoTbl = b.ReadInt32();
                int tamanhoTbl2 = tamanhoTbl;
                numberPointers = tamanhoTbl / 4;
                byte[] copia = new byte[tamanhoTbl];
                File.Create(nomeAk.Replace(".new", ".fixed.new")).Close();
                File.WriteAllBytes(nomeAk.Replace(".new", ".fixed.new"), copia);
                 
                    
                for (int i = 0; i < numberPointers; i++)
                 {
                    using (BinaryWriter w = new BinaryWriter(File.Open(nomeAk.Replace(".new", ".fixed.new"), FileMode.Open)))
                    {
                        b.BaseStream.Seek(tablePosition, SeekOrigin.Begin);
                    ponteiro = b.ReadInt32();
                    tablePosition += 4;
                    b.BaseStream.Seek(ponteiro, SeekOrigin.Begin);
                    tamanho = b.ReadInt16();
                    if (tamanho % 8 != 0)
                    {
                        b.BaseStream.Seek(ponteiro + 4, SeekOrigin.Begin);
                        chunckBackup = b.ReadBytes(tamanho - 2);
                    }
                    else
                    {
                        b.BaseStream.Seek(ponteiro + 2, SeekOrigin.Begin);
                        chunckBackup = b.ReadBytes(tamanho);

                    }

                       // int tamanhoNew = chunckBackup.Length;
                        short tamanhoS =(short)chunckBackup.Length;
                        w.BaseStream.Seek(posTabela, SeekOrigin.Begin);
                        w.Write(tamanhoTbl2);
                        w.BaseStream.Seek(tamanhoTbl2, SeekOrigin.Begin);
                        w.Write(tamanhoS);
                        w.Write(chunckBackup);
                        tamanhoTbl2 += tamanhoS + 2;
                         posTabela += 4;
                        

                        w.Close();
                 }

                }
            }

                
           
        }
       

        
    }
}
