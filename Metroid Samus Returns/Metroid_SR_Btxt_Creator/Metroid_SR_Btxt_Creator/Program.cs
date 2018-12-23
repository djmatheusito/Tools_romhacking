using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Metroid_SR_Btxt_Creator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Digite o nome do arquivo .txt(sem a extensão) que foi dumpado e pressione enter para iniciar.");
            string nomeArq = Console.ReadLine();
            string nomeArqCerto = nomeArq.ToLower();
            Console.WriteLine("Criando novo txt...");
            using (StreamReader sr = new StreamReader(nomeArqCerto + ".txt"))
            {

                string texto = sr.ReadToEnd().Replace("\n", string.Empty)
                                             .Replace("\r", string.Empty)
                                             .Replace("ã", "ä")
                                             .Replace("õ", "ö")
                                             .Replace("Ã", "Ä")
                                             .Replace("Õ", "Ö")
                                             .Replace("(A)", "☯")
                                             .Replace("(R)", "⚭")
                                             .Replace("(Y)", "☵")
                                             .Replace("(L)", "☮")
                                             .Replace("(Circle Pad)", "☷")
                                             .Replace("(X)", "☶")
                                             .Replace("(UP)", "☱")
                                             .Replace("(RIGHT)", "☲")
                                             .Replace("(DOWN)", "☳")
                                             .Replace("(LEFT)", "☴")
                                             .Replace("(B)", "☰");

                string[] splitString = texto.Split('<', '>');
                sr.Close();

                byte[] separador = new byte[] { 0x00 };
                byte[] header = new byte[] { 0x42, 0x54, 0x58, 0x54, 0x01, 0x00, 0x08, 0x00 };

                if (File.Exists("Novo_" + nomeArqCerto + ".txt"))
                {
                    File.Delete("Novo_" + nomeArqCerto + ".txt");

                }

                File.Create("Novo_" + nomeArqCerto + ".txt").Close();
                using (BinaryWriter w2 = new BinaryWriter(File.Open("Novo_" + nomeArqCerto + ".txt", FileMode.Open)))
                {
                    w2.Write(header);
                    w2.Close();
                }

                int inde = 8;


                for (int i = 1; i < 985;)
                {

                    byte[] asc = Encoding.ASCII.GetBytes(splitString[i++]);
                    byte[] unicode = Encoding.Unicode.GetBytes(splitString[i++].Replace("[Q]", "|"));

                    using (BinaryWriter w = new BinaryWriter(File.Open("Novo_" + nomeArqCerto + ".txt", FileMode.Open)))
                    {
                        w.BaseStream.Seek(inde, SeekOrigin.Begin);
                        w.Write(asc);
                        w.Write(separador);
                        w.Write(unicode);
                        w.Write(separador);
                        w.Write(separador);


                        long tamanhoAsc = asc.Length + 1;
                        int tamanhoStAsc = ((int)tamanhoAsc);

                        long tamanhoUni = unicode.Length + 2;
                        int tamanhoStUni = ((int)tamanhoUni);

                        inde += tamanhoStAsc;
                        inde += tamanhoStUni;

                        w.Close();
                    }



                }

                Console.WriteLine("Novo .txt criado, pressione qualquer tecla para sair.");
                Console.ReadLine();

            }

        }
    }
}
