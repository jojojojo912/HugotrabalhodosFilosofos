using System;
using System.Threading;

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
        private Random rnd = new Random();

        protected Filosofos(string nome, int ciclos)
        {
            Nome = nome;
            Ciclos = ciclos;
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
                }
            }

            Console.WriteLine(Nome + " terminou de comer e saiu da mesa.");
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
        public Filosofo1(int ciclos) : base("F1", ciclos)
        {
        }

        protected override void PegarTalheres()
        {
            Mesa.FacaF1 = false;
            Console.WriteLine(Nome + " pegou a faca f1");

            Mesa.GarfoG1 = false;
            Console.WriteLine(Nome + " pegou o garfo g1");
        }

        protected override void DevolverTalheres()
        {
            Mesa.FacaF1 = true;
            Mesa.GarfoG1 = true;
            Console.WriteLine(Nome + " devolveu f1 e g1 para a mesa");
        }
    }

    class Filosofo2 : Filosofos
    {
        public Filosofo2(int ciclos) : base("F2", ciclos)
        {
        }

        protected override void PegarTalheres()
        {
            Mesa.FacaF2 = false;
            Console.WriteLine(Nome + " pegou a faca f2");

            Mesa.GarfoG2 = false;
            Console.WriteLine(Nome + " pegou o garfo g2");
        }

        protected override void DevolverTalheres()
        {
            Mesa.FacaF2 = true;
            Mesa.GarfoG2 = true;
            Console.WriteLine(Nome + " devolveu f2 e g2 para a mesa");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Cenario 1: F1 comeu");
            Filosofo1 filosofo1 = new Filosofo1(1);
            Thread thread1 = new Thread(filosofo1.Executar);
            thread1.Start();
            thread1.Join();

            Console.WriteLine("\nCenario 2: F2 comeu");
            Filosofo2 filosofo2 = new Filosofo2(1);
            Thread thread2 = new Thread(filosofo2.Executar);
            thread2.Start();
            thread2.Join();

            Console.WriteLine("\nCenario 3: F2 e F1 comeram");
            Filosofo2 f2cenario3 = new Filosofo2(1);
            Filosofo1 f1cenario3 = new Filosofo1(1);
            Thread threadA = new Thread(f2cenario3.Executar);
            Thread threadB = new Thread(f1cenario3.Executar);
            threadA.Start();
            Thread.Sleep(50);
            threadB.Start();
            threadA.Join();
            threadB.Join();

            Console.WriteLine("\nCenario 4: F1 e F2 comeram");
            Filosofo1 f1cenario4 = new Filosofo1(1);
            Filosofo2 f2cenario4 = new Filosofo2(1);
            Thread threadC = new Thread(f1cenario4.Executar);
            Thread threadD = new Thread(f2cenario4.Executar);
            threadC.Start();
            Thread.Sleep(50);
            threadD.Start();
            threadC.Join();
            threadD.Join();

            Console.WriteLine("\nFim");
        }
    }
}