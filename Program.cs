using System;
using System.Text;
using static System.Console;

namespace queens
{
    class Program
    {
        static void Main(string[] args)
        {

            //StringBuilder sb = new StringBuilder("");
            //sb.Append("Queens\n");
            //Console.WriteLine(sb);
            //sb.Append("game\n");
            //Console.WriteLine(sb);
            //sb.Append("info\n");
            //Console.WriteLine(sb);
            Game myGame = new Game();
            myGame.Start();
        }
    }
}
