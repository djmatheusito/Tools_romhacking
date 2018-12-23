using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cold_unPAC
{
    class Program
    {
        static void Main(string[] args)
        {
            int posNumeroArqs = 12;
            Console.WriteLine("-----------Cold_unPACker v1.0.0-----------");
            Console.WriteLine("(EN)");
            Console.WriteLine("Enter with .pac file name(without extension):\n");
            Console.WriteLine("(BR)");
            Console.WriteLine("Entre com o nome do arquivo .pac(sem a extensão):\n");
            
         //   string nomeArq = Console.ReadLine();
            Console.WriteLine("Extraindo...");
            Console.WriteLine("Extracting...");
            string[] pacs = Directory.GetFiles("pacs");

            foreach (string pac in pacs)
            {
                int numeroArqs;
                int posTabela = 56;
                int tamanhoNome = 64;

                using (BinaryReader leitor = new BinaryReader(File.Open(pac, FileMode.Open)))
                {
                    leitor.BaseStream.Seek(posNumeroArqs, SeekOrigin.Begin);
                    numeroArqs = leitor.ReadInt32();
                    string[] diretorio = pac.Split('.');
                    Directory.CreateDirectory(diretorio[0]);

                    for (int i = 0; i < numeroArqs; i++)
                    {

                        leitor.BaseStream.Seek(posTabela, SeekOrigin.Begin);
                        int tamanhoArq = leitor.ReadInt32();
                        int ponteiroArq = leitor.ReadInt32();
                        byte[] nome = leitor.ReadBytes(tamanhoNome);
                        string nomeAsc = Encoding.ASCII.GetString(nome).TrimEnd('\0');

                        leitor.BaseStream.Seek(ponteiroArq, SeekOrigin.Begin);
                        byte[] arquivo = leitor.ReadBytes(tamanhoArq);
                        File.WriteAllBytes(diretorio[0] + @"\" + nomeAsc, (arquivo));

                        nomeAsc = null;
                        posTabela += 144;
                    }

                    leitor.Close();

                    Console.WriteLine("\nExtração completa!");
                    Console.WriteLine("Extraction completed!");

                }
            }

           


        }
    }
}
