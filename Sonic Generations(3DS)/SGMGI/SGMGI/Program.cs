using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SGMGI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sonic Generations MG Inserter");
            Console.WriteLine("Digete o nome do Arquivo TXT sem a extensão");
            string nomeDoArquivo = Console.ReadLine();
            string nomeDoArqCorreto = nomeDoArquivo.ToUpper();
            
            Insercao(nomeDoArqCorreto);

           

        }

        

        static void Insercao(string arquivo)
        {
            string textoAInser = File.ReadAllText(arquivo + ".txt").Replace("\n\n\r\n", string.Empty).Replace("\r\n", "\n").Replace(">", "~").Replace("\n<", "<");
            List<string> dialgos = new List<string>(textoAInser.Split('~'));
            dialgos.RemoveAt(dialgos.Count -1);
            if (File.Exists(arquivo + ".MG"))
            {

                File.Delete(arquivo + ".MG");
            }

            int dlg = dialgos.Count;

            CriadarTabela(dlg, arquivo);
            

            FileInfo fileInfo = new FileInfo(arquivo + ".MG");


            long tamanho = fileInfo.Length;
            int t = Convert.ToInt32(tamanho);
            int posicaoTab = 16;
            int tamanhoTab = t;
            int ponte = t + 16;
            int tamanhoUniAtualizado = 0;
            int ponteirosAsc = 0;

            foreach (string dialogo in dialgos)
            {
                string[] corte = dialogo.Split('<');
                byte[] unicode = Encoding.Unicode.GetBytes(corte[0]);
                byte[] asc = Encoding.ASCII.GetBytes(corte[1]);
                byte[] pad = {0x00};
                long tamanhoSt = corte[0].Length*2;
                long tamanhoAsc = corte[1].Length;
                EscreverPonteirosUni(arquivo, tamanhoTab, posicaoTab);
                posicaoTab += 8;
                
                int tamanhoSeekog = Convert.ToInt32(tamanhoSt);
                int tamanhoSeek = Convert.ToInt32(tamanhoSt);
                int tamanhoAscResu = Convert.ToInt32(tamanhoAsc);
              
                
                using (BinaryWriter writter = new BinaryWriter(File.Open(arquivo + ".MG", FileMode.Open)))
                {
                    byte[] pad2 = new byte[] {0x01, 0x00, 0x00, 0x00};
                    byte[] pad3 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
                    byte[] pad4 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    byte[] pad5 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    
                    writter.Seek(tamanhoTab, SeekOrigin.Begin);
                    writter.Write(pad2);
                    writter.Write(ponte);
                    writter.Write(pad3);
                    ponte += 16;
                    writter.Write(ponte);
                    writter.Write(pad4);

                    writter.Write(unicode);
                    


                    if (tamanhoSt % 16 != 0)
                    {

                        while (tamanhoSeek % 16 != 0)
                        {
                            tamanhoSeek += 1;

                            if (tamanhoSeek % 16 == 0)
                            {

                                tamanhoUniAtualizado = tamanhoSeek;
                                ponte += tamanhoSeek;
                                
                                
                                int loop = tamanhoSeek - tamanhoSeekog;
                                // tamanhoTab += loop;
                                for (int i = 0; i < loop; i++)
                                {
                                    writter.Write(pad);
                                    FileInfo poasc = new FileInfo(arquivo + ".MG");


                                    long posc2 = poasc.Length;
                                    int pontasc = Convert.ToInt32(posc2);
                                    ponteirosAsc = pontasc + tamanhoSeek +32;
                                }
                            }

                            
                        }

                        
                    }
                    else {

                        writter.Write(pad5);

                        ponte += 16;
                        ponte += tamanhoSeek;

                        FileInfo poasc = new FileInfo(arquivo + ".MG");


                        long posc2 = poasc.Length;
                        int pontasc = Convert.ToInt32(posc2);
                        ponteirosAsc = pontasc+16+32+tamanhoSeek;
                    }

                    writter.Write(asc);

                    if (tamanhoAscResu <= 16)
                    {
                        int outy = 16 - tamanhoAscResu;
                       ponte += 16+16;
                        for (int i = tamanhoAscResu; i < 16; i++)
                        {
                            writter.Write(pad);
                        }

                        writter.Close();
                        
                    }

                    EscreverPonteirosAsc(arquivo, ponteirosAsc, posicaoTab);


                    posicaoTab += 8;



                    FileInfo fileInfos = new FileInfo(arquivo + ".MG");
                  

                    long tamanho2 = fileInfos.Length;
                    int t2 = Convert.ToInt32(tamanho2);
                    tamanhoTab = t2;


                }

            }
        }

      static void CriadarTabela(int mult, string nome)
      {
            
            string header = "#MSG";
            byte[] head = Encoding.ASCII.GetBytes(header);
            byte[] restoHeader = new byte[] { 0x00, 0x01, 0x03, 0x00 };
            byte[] restoHeader2 = new byte[] { 0x10, 0x00, 0x00, 0x00 };
            File.Create(nome + ".MG").Close();
            using (BinaryWriter w = new BinaryWriter(File.Open(nome + ".MG", FileMode.Open)))
            {
                int result = 12 + (mult * 16);
                w.Write(head);
                byte single = 0x00;
                for (int i = 0; i < result; i++)
                {
                    w.Write(single);

                }


                w.BaseStream.Seek(4, SeekOrigin.Begin);
                w.Write(restoHeader);
                w.Write(mult);
                w.Write(restoHeader2);
                w.Close();






            }
          

      }

        static void EscreverPonteirosUni(string arquivo, int ponteiro, int posSeek) {

            using (BinaryWriter w = new BinaryWriter(File.Open(arquivo+".MG", FileMode.Open))) {
                byte[] pad = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                w.BaseStream.Seek(posSeek, SeekOrigin.Begin);
                w.Write(pad);
                w.Write(ponteiro);
                w.Close();
            }

        }

        static void EscreverPonteirosAsc(string arquivo, int ponteiro, int posSeek)
        {
            using (BinaryWriter w = new BinaryWriter(File.Open(arquivo + ".MG", FileMode.Open)))
            {
                byte[] pad = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                w.BaseStream.Seek(posSeek, SeekOrigin.Begin);
                w.Write(ponteiro);
                w.Write(pad);
                w.Close();
            }

        }
    }

}
