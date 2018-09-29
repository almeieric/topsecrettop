using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacaPalavra
{
    class Program
    {
        static void Main(string[] args)
        {
            string letras = "LOREMIPSUMDOLORSITAMETCONSECTETURADIPISICINGELITCUMACCUSANTIUMEXPLICABOARCHITECTOSUNTCONSECTETURINSO";

            char[,] grid = new char[10, 10];

            int proxLetra = 0;

            Console.WriteLine("   0 1 2 3 4 5 6 7 8 9");

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                Console.Write(i + ": ");

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(letras[proxLetra] + " ");

                    grid[i, j] = letras[proxLetra];

                    proxLetra++;
                }
                Console.Write("\n");
            }

            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("Digite a palavra a ser buscada: ");
                string busca = Console.ReadLine();
                var posicoes = encontrarPalavra(busca, grid);

                imprimirResultado(posicoes, grid);

                Console.Write("Finalizar?: ");
                var finalizar = Console.ReadKey();
                sair = finalizar.KeyChar == 'S' || finalizar.KeyChar == 's';
                Console.WriteLine();
            }
        }

        static Dictionary<string, List<int[]>> encontrarPalavra(string busca, char[,] grid)
        {
            Dictionary<string, List<int[]>> resultado = new Dictionary<string, List<int[]>>()
            {
                { "H", new List<int[]>() },
                { "V", new List<int[]>() }
            };

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                string linhaHorizontal = "";
                string linhaVertical = "";

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    linhaHorizontal += grid[i, j];
                    linhaVertical += grid[j, i];
                }

                int[] indicesH = pesquisarIndicesNaPalavra(linhaHorizontal, busca);
                int[] indicesV = pesquisarIndicesNaPalavra(linhaVertical, busca);
                
                foreach (int indice in indicesH)
                {
                    for (int k = 0; k < busca.Length; k++)
                    {
                        resultado["H"].Add(new int[] { i, indice + k });
                    }
                }

                foreach (int indice in indicesV)
                {
                    for (int k = 0; k < busca.Length; k++)
                    {
                        resultado["V"].Add(new int[] { i, indice + k });
                    }
                }
            }

            return resultado;
        }

        static int[] pesquisarIndicesNaPalavra(string palavra, string busca)
        {
            List<int> indices = new List<int>();

            int index = palavra.ToLower().IndexOf(busca.ToLower());
            
            while (index > -1)
            {
                indices.Add(index);

                string linhaCortada = palavra.Substring(index + busca.Length); //  homem aranha

                int novoIndex = linhaCortada.ToLower().IndexOf(busca.ToLower()); // 7

                if (novoIndex > -1)
                {
                    index += busca.Length + novoIndex;
                }
                else
                {
                    index = novoIndex;
                }
            }
            return indices.ToArray();
        }

        static void imprimirResultado(Dictionary<string, List<int[]>> pontos, char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (pontoExiste(i, j, pontos["H"]) || pontoExiste(j, i, pontos["V"]))
                    {
                        Console.Write(grid[i, j] + " ");
                    }
                    else
                    {
                        Console.Write("* ");
                    }
                }
                Console.WriteLine();
            }
        }

        static bool pontoExiste(int i, int j, List<int[]> lista)
        {
            foreach (int[] ponto in lista)
            {
                if (ponto[0] == i && ponto[1] == j)
                    return true;
            }

            return false;
        }
    }
}
