using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Metroid_SR_Dumper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dumper de textos - Metroid Samus Returns 3DS");
            Console.WriteLine("Digite o nome do arquivo .txt(btxt) (sem a entensão) que deseja extrair e pressione enter para inicar a extração");
            string nomeArquivo = Console.ReadLine();
            string nomeCerto = nomeArquivo.ToLower();
            int posicaoTexto = 8;
            long t = new FileInfo(nomeCerto + ".txt").Length - 8;
            int tamanhoDoarquivo = ((int)t);
            List<string> dialogos = new List<string>();

            Console.WriteLine("Extraindo...");
            while (posicaoTexto < tamanhoDoarquivo)
                {

                    using (var b = new BinaryReader(File.OpenRead(nomeCerto + ".txt")))
                    {
                        b.BaseStream.Seek(posicaoTexto, SeekOrigin.Begin);

                        string StringAsc = "";
                        char ch;
                        while ((ch = b.ReadChar()) != 0)
                        StringAsc = StringAsc + ch;

                        dialogos.Add("<"+StringAsc+">");

                        int separador = 1;
                        long tamanhoAsc = StringAsc.Length + separador;
                        int tamanhoStAsc = ((int)tamanhoAsc);

                        posicaoTexto += tamanhoStAsc;

                        b.Close();

                            using (StreamReader sr = new StreamReader(nomeCerto + ".txt", Encoding.Unicode))
                            {
                                sr.BaseStream.Seek(posicaoTexto, SeekOrigin.Begin);

                                string uni = sr.ReadToEnd();
                                string[] sep = uni.Split('\0');
                                dialogos.Add(sep[0].Replace("☯", "(A)")
                                                   .Replace("⚭", "(R)")
                                                   .Replace("☵", "(Y)")
                                                   .Replace("☮", "(L)")
                                                   .Replace("☷", "(Circle Pad)")
                                                   .Replace("☶", "(X)")
                                                   .Replace("☱", "(UP)")
                                                   .Replace("☲", "(RIGHT)")
                                                   .Replace("☳", "(DOWN)")
                                                   .Replace("☴", "(LEFT)")
                                                   .Replace("☰", "(B)")
                                                   .Replace("|", "[Q]\n") + "\n");

                                long tamanhoUniC = 2 + (sep[0].Length *2);
                                int tamanhoStUniC = ((int)tamanhoUniC);
                                posicaoTexto += tamanhoStUniC;
                                sr.Close();
                            }

                                StringAsc = string.Empty;
                    }

                
            }
                        File.WriteAllLines(nomeCerto + "D" + ".txt", dialogos);
                        Console.WriteLine("Extração completa, pressione qualquer tecla para sair.");
                        Console.ReadLine();
        }
    }
}
