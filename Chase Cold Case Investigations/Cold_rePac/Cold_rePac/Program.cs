using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cold_rePac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------Cold_rePACker v1.0.0-----------");
            Console.WriteLine("(EN)");
            Console.WriteLine("Put the original .pac file in the same program folder.");
            Console.WriteLine("Enter the name of the extracted .pac folder:\n");
            Console.WriteLine("(BR)");
            Console.WriteLine("Coloque o arquivo .pac original na mesma pasta do programa.");
            Console.WriteLine("Entre com o nome da pasta do .pac extraido:\n");
            string nomePasta = Console.ReadLine();
            string[] pasta = Directory.GetFiles(nomePasta);
            int tamanhoNome = 64;
            int posTabela = 60;

            Console.WriteLine("Inserindo...");
            Console.WriteLine("Inserting...\n");

            foreach (string arquivo in pasta)
            {
                // NovoPac(arquivo, nomeNomePasta);

                

                using (BinaryReader b = new BinaryReader(File.Open(nomePasta + ".pac", FileMode.Open)))
                {
                    
                    b.BaseStream.Seek(posTabela, SeekOrigin.Begin);
                    int offsetArq = b.ReadInt32();
                    byte[] nome = b.ReadBytes(tamanhoNome);
                    string nomeAsc = Encoding.ASCII.GetString(nome).TrimEnd('\0');
                    byte[] arquivos = File.ReadAllBytes(nomePasta +@"\\"+ nomeAsc);
                    b.Close();


                    using (BinaryWriter w = new BinaryWriter(File.Open(nomePasta + ".pac", FileMode.Open)))
                    {
                        w.BaseStream.Seek(offsetArq, SeekOrigin.Begin);
                        w.Write(arquivos);
                        w.Close();

                    }

                    posTabela += 144;

                }
            }

            Console.WriteLine("\nArquivos inseridos!");
            Console.WriteLine("Files inserted!");
        }

        
    }
}
