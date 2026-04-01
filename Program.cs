using System;
using System.Threading;
using System.Collections.Generic;

namespace ProblemaFilosofos
{
    static class Mesa
    {
        public static object RegiaCritica = new object();
        public static bool FacaF1 = true;
        public static bool GarfoG1 = true;
        public static bool FacaF2 = true;
        public static bool GarfoG2 = true;
    }

    abstract class Filosofos
    {
        protected string Nome;
        protected int Ciclos;
        protected bool PegaFacaPrimeiro;
        private Random rnd = new Random();
        public static List<string> OrdemDeComida = new List<string>();

        protected Filosofos(string nome, int ciclos, bool pegaFacaPrimeiro)
        {
            Nome = nome;
            Ciclos = ciclos;
            PegaFacaPrimeiro = pegaFacaPrimeiro;
        }

        protected abstract void PegarTalheres();
        protected abstract void DevolverTalheres();

        public void Executar()
        {
            for (int i = 0; i < Ciclos; i++)
            {
                Pensar();

                lock (Mesa.RegiaCritica)
                {
                    PegarTalheres();
                    Comer();
                    DevolverTalheres();
                    OrdemDeComida.Add(Nome); 
                }
            }
            Console.WriteLine(Nome + " terminou de comer.");
        }

        private void Pensar()
        {
            int tempo = rnd.Next(300, 800);
            Console.WriteLine(Nome + " esta pensando...");
            Thread.Sleep(tempo);
        }

        private void Comer()
        {
            int tempo = rnd.Next(400, 900);
            Console.WriteLine(Nome + " esta comendo...");
            Thread.Sleep(tempo);
        }
    }

    class Filosofo1 : Filosofos
    {
        public Filosofo1(int ciclos, bool pegaFacaPrimeiro) : base("F1", ciclos, pegaFacaPrimeiro) { }

        protected override void PegarTalheres()
        {
            if (PegaFacaPrimeiro)
            {
                Mesa.FacaF1 = false;
                Console.WriteLine(Nome + " pegou a faca f1");
                Mesa.GarfoG1 = false;
                Console.WriteLine(Nome + " pegou o garfo g1");
            }
            else
            {
                Mesa.GarfoG1 = false;
                Console.WriteLine(Nome + " pegou o garfo g1");
                Mesa.FacaF1 = false;
                Console.WriteLine(Nome + " pegou a faca f1");
            }
        }

        protected override void DevolverTalheres()
        {
            Mesa.FacaF1 = true;
            Mesa.GarfoG1 = true;
            Console.WriteLine(Nome + " devolveu f1 e g1");
        }
    }

    class Filosofo2 : Filosofos
    {
        public Filosofo2(int ciclos, bool pegaFacaPrimeiro) : base("F2", ciclos, pegaFacaPrimeiro) { }

        protected override void PegarTalheres()
        {
            if (PegaFacaPrimeiro)
            {
                Mesa.FacaF2 = false;
                Console.WriteLine(Nome + " pegou a faca f2");
                Mesa.GarfoG2 = false;
                Console.WriteLine(Nome + " pegou o garfo g2");
            }
            else
            {
                Mesa.GarfoG2 = false;
                Console.WriteLine(Nome + " pegou o garfo g2");
                Mesa.FacaF2 = false;
                Console.WriteLine(Nome + " pegou a faca f2");
            }
        }

        protected override void DevolverTalheres()
        {
            Mesa.FacaF2 = true;
            Mesa.GarfoG2 = true;
            Console.WriteLine(Nome + " devolveu f2 e g2");
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Escolha quem comeca: (1) F1 ou (2) F2. (0) para Sair.");
                string escolhaInicio = Console.ReadLine();

                if (escolhaInicio == "0") break;

                Console.WriteLine("F1 pega primeiro: (1) Faca ou (2) Garfo?");
                bool f1Opcao = Console.ReadLine() == "1";

                Console.WriteLine("F2 pega primeiro: (1) Faca ou (2) Garfo?");
                bool f2Opcao = Console.ReadLine() == "1";

                Filosofos.OrdemDeComida.Clear();

                Filosofo1 f1 = new Filosofo1(1, f1Opcao);
                Filosofo2 f2 = new Filosofo2(1, f2Opcao);

                Thread t1 = new Thread(f1.Executar);
                Thread t2 = new Thread(f2.Executar);

                if (escolhaInicio == "1")
                {
                    t1.Start();
                    Thread.Sleep(50);
                    t2.Start();
                }
                else
                {
                    t2.Start();
                    Thread.Sleep(50);
                    t1.Start();
                }

                t1.Join();
                t2.Join();

                Console.WriteLine("\n--- RESULTADO ---");
                Console.WriteLine("O primeiro a comer foi: " + Filosofos.OrdemDeComida[0]);
                Console.WriteLine("O segundo a comer foi: " + Filosofos.OrdemDeComida[1]);
                
                Console.WriteLine("\nPressione Enter para voltar ao menu...");
                Console.ReadLine();
            }
        }
    }
}
