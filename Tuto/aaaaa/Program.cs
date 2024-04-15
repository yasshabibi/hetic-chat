using System;

namespace Tuto.aaaaa
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            var player = new Player();
            Console.WriteLine(player.CalculateHealth());

            player.RemoveHealth(50);
            Console.WriteLine(player.CalculateHealth());



        }
    }
}

