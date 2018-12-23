using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Amb_dumper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Extrator de Arquivos AMB - Sonic Generations 3DS\n");
            Console.WriteLine("Digite o nome do arquivo a ser extraido:");
            string arquivoSerLido = Console.ReadLine();
            string maiuscula = arquivoSerLido.ToUpper();

           

            if (Directory.Exists(maiuscula))
            {
                Directory.Delete(maiuscula, true);
            }

            

            using (BinaryReader b = new BinaryReader(File.Open(maiuscula + ".amb", FileMode.Open)))
            {
                
                int posNumDeArquivos = 0x10;
                int posTabela = 0x20;
                int pulonome = 0x08;
                b.BaseStream.Seek(posNumDeArquivos, SeekOrigin.Current);
                int numeroPonteiros = b.ReadInt32();
                b.BaseStream.Seek(pulonome, SeekOrigin.Current);
                int enderecoNome = b.ReadInt32() + 2;
                int posNomeArquivos = 0x1C;
                int tamanhoNomeArquivos = 0x1E;
                

                int reset = 0;

                for (int i = 0; i < numeroPonteiros; i++)
                {
                    Directory.CreateDirectory(@"Extraido");
                   b.BaseStream.Seek(reset, SeekOrigin.Begin);
                   b.BaseStream.Seek(posTabela, SeekOrigin.Current);
                   int ponteiros = b.ReadInt32();
                   int tamanhoDoArquivo = b.ReadInt32();
                    
                   b.BaseStream.Seek(reset, SeekOrigin.Begin);

                   b.BaseStream.Seek(ponteiros, SeekOrigin.Current);

                   byte[] darc = b.ReadBytes(tamanhoDoArquivo);
                      b.BaseStream.Seek(reset, SeekOrigin.Begin);

                       b.BaseStream.Seek(posNomeArquivos, SeekOrigin.Current);
                      
                       b.BaseStream.Seek(reset, SeekOrigin.Begin);
                     b.BaseStream.Seek(enderecoNome, SeekOrigin.Current);
                     byte[] nomeDoarc = b.ReadBytes(tamanhoNomeArquivos);
                     string resultado = Encoding.ASCII.GetString(nomeDoarc).TrimEnd('\0');
                    enderecoNome += 0x20;


                        File.WriteAllBytes(resultado, (darc));
                        b.BaseStream.Seek(reset, SeekOrigin.Begin);

                                      
                    if (posTabela>=0x20)
                    {
                        posTabela += 0x10;
                        
                        ponteiros = 0;
                        tamanhoDoArquivo = 0;
                        

                        nomeDoarc = null;

                    }


                }

                var MG = Directory.GetFiles(Directory.GetCurrentDirectory(),"*.MG", SearchOption.TopDirectoryOnly);
                foreach (String filePath in MG)
                {
                    var newFile = Path.Combine(@"extraido", Path.GetFileName(filePath));
                    File.Copy(filePath, newFile, true);
                }

                var ARC = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.ARC", SearchOption.TopDirectoryOnly);
                foreach (String filePath in ARC)
                {
                    var newFile = Path.Combine(@"extraido", Path.GetFileName(filePath));
                    File.Copy(filePath, newFile, true);
                }

                var BCRES = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.BCRES", SearchOption.TopDirectoryOnly);
                foreach (String filePath in BCRES)
                {
                    var newFile = Path.Combine(@"extraido", Path.GetFileName(filePath));
                    File.Copy(filePath, newFile, true);
                }

                var BCENV = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.BCENV", SearchOption.TopDirectoryOnly);
                foreach (String filePath in BCENV)
                {
                    var newFile = Path.Combine(@"extraido", Path.GetFileName(filePath));
                    File.Copy(filePath, newFile, true);
                }

                Directory.Move("extraido", maiuscula);



                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".MG") || s.EndsWith(".ARC") || s.EndsWith(".BCRES") || s.EndsWith(".BCENV"));

                foreach (string file in files)
                {
                    File.Delete(file);
                }

                b.Close();

                Console.Write("Extração completa!");

                Console.ReadLine();


            }

        }
    }
}
