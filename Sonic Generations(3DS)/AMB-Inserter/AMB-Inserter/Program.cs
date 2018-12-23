using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AMB_Inserter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Digite o nome do arquivo que será inserido:");
            string arquivoSerLido = Console.ReadLine();
            string maiuscula = arquivoSerLido.ToUpper();
            List<string> arquivos = new List<string>(Directory.GetFiles(maiuscula));
            if (File.Exists("new"+maiuscula + ".amb"))
            {

                File.Delete("new" + maiuscula + ".amb");
            }

            CriadarTabela(maiuscula, arquivos.Count);

            FileInfo fileInfos = new FileInfo("new" + maiuscula + ".amb");
            long t1 = fileInfos.Length;
            int t3 = Convert.ToInt32(t1);
            string nomess = "new" + maiuscula + ".amb";
            int posicaotabela= 32;
            List<string> Nomearquivos = new List<string>(Directory.GetFiles(maiuscula));
            foreach (string arquivo  in arquivos)
            {
                
                FileInfo fileInfo = new FileInfo(arquivo);
                long t = fileInfo.Length;
                int t2 = Convert.ToInt32(t);
                byte[] arq = File.ReadAllBytes(arquivo);
                EscreverPonteiros(nomess, t3, t2, posicaotabela);
                posicaotabela += 16;
                using (BinaryWriter w = new BinaryWriter(File.Open("new" + maiuscula + ".amb", FileMode.Open)))
                {
                    
                    w.BaseStream.Seek(t3, SeekOrigin.Begin);
                    w.Write(arq);

                    string aas = Path.GetExtension(arquivo);

                   if (aas == ".ARC")
                    {

                        for (int i = 0; i < 88; i++)
                        {
                            byte single = 0x00;

                            w.Write(single);
                            
                        }

                        t3 += t2 + 88;

                    }
                  else if(aas == ".MG")
                  {
                    for (int i = 0; i < 32; i++)
                    {
                           byte single = 0x00;
  
                            w.Write(single);
                    }

                        t3 += t2 + 32;
                    }
                }

                

            }



            NomesArquivos(nomess, Nomearquivos, t3);
           

        }


        static void CriadarTabela(string nome, int mult)
        {

            string header = "#AMB";
            byte[] head = Encoding.ASCII.GetBytes(header);
            byte[] restoHeader = new byte[] { 0x20, 0x00, 0x00, 0x00 };
            byte[] restoHeader2 = new byte[] { 0x00, 0x00, 0x04, 0x00 };
            byte[] restoHeader3 = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            byte[] restoHeader4 = new byte[] { 0x20, 0x00, 0x00, 0x00 };
            File.Create("new"+nome + ".amb").Close();
            using (BinaryWriter w = new BinaryWriter(File.Open("new"+nome + ".amb", FileMode.Open)))
            {
                
                int result = mult * 16;
                w.Write(head);
                w.Write(restoHeader);
                w.Write(restoHeader2);
                w.Write(restoHeader3);
                w.Write(mult);
                w.Write(restoHeader4);
                w.Write(restoHeader3);
                w.Write(restoHeader3);
                byte single = 0x00;
                for (int i = 0; i < result; i++)
                {
                    w.Write(single);

                }

                for (int i = 0; i < 32; i++)
                {
                    w.Write(single);

                }

                w.Close();
                Coretor(nome);
                


                



            }



        }

        static void Coretor(string nome) {

            using (BinaryWriter w = new BinaryWriter(File.Open("new" + nome + ".amb", FileMode.Open)))
            {

                FileInfo fileInfo = new FileInfo("new" + nome + ".amb");
                long t = fileInfo.Length;
                int t2 = Convert.ToInt32(t);


                w.BaseStream.Seek(24, SeekOrigin.Begin);
                w.Write(t2);
                w.Close();
            }
        }

        static void EscreverPonteiros(string arquivo, int ponteiro, int tamanhoArq, int posSeek)
        {

            using (BinaryWriter w = new BinaryWriter(File.Open(arquivo, FileMode.Open)))
            {
                byte[] pad = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                byte[] pad2 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                byte[] pad3 = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                w.BaseStream.Seek(posSeek, SeekOrigin.Begin);
                w.Write(ponteiro);
                w.Write(tamanhoArq);
                w.Write(pad2);
                w.Write(pad3);
                w.Close();
            }

        }


        static void NomesArquivos(string nomea,List<string>arquivos, int tamanhoArqseek)
        {

            using (BinaryWriter w = new BinaryWriter(File.Open(nomea, FileMode.Open)))
            {
                w.BaseStream.Seek(28, SeekOrigin.Begin);
                w.Write(tamanhoArqseek+32);
                w.BaseStream.Seek(tamanhoArqseek, SeekOrigin.Begin);
                for (int i = 0; i < 16; i++)
                {
                    byte single = 0x00;
                    w.Write(single);
                }

                w.BaseStream.Seek(tamanhoArqseek+32, SeekOrigin.Begin);

                foreach (string arquivo in arquivos)
                {
                    string[] arquic = arquivo.Split('\\');
                    string arquic2 = ".\\" + arquic[1];
                    int tamanhoSt = arquic2.Length;
                    int loop = 32 - tamanhoSt;
                    byte[] arqui = Encoding.ASCII.GetBytes(arquic2);
                    w.Write(arqui);
                    for (int i = 0; i < loop; i++)
                    {
                        byte single = 0x00;
                        w.Write(single);
                    }
                }

                w.Close();
            }

        }

    }

}

       
