using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace queens
{
    class Menu
    {
        private int SelectedIndex;
        private List<string> Options;
        private string Prompt;
        public Menu(string prompt, List<string> options)
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }
        private void DisplayOptions(int selcectedItem = 0, string sb = null)
        {
            Console.WriteLine(Prompt);
            for (int i = 0; i < Options.Count(); i++)
            {
                string currentOption = Options.ElementAt(i);
                string prefix = " ";
             
                if (i == SelectedIndex)
                {
                    prefix = "*";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
       
                Console.WriteLine($"{prefix} << {currentOption} >>");
                if (sb != null )
                {
                    ResetColor();
                    Console.WriteLine($"\n{sb}");
                }
              
            }
            ResetColor();
        }
        public int Run(int selcectedItem = 0, string sb =null)
        {
            if (selcectedItem > 0)
            {
                Clear();
                DisplayOptions(selcectedItem);

                return selcectedItem;
            }
            else 
            {
                ConsoleKey keyPressed;
                do
                {
                    Clear();
                    DisplayOptions(selcectedItem, sb);

                    ConsoleKeyInfo keyinfo = ReadKey(true);
                    keyPressed = keyinfo.Key;

                    //Update SelectedIndex based on arrow keys.
                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        SelectedIndex--;
                        if (SelectedIndex == -1)
                        {
                            SelectedIndex = Options.Count() - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        SelectedIndex++;
                        if (SelectedIndex == Options.Count())
                        {
                            SelectedIndex = 0;
                        }
                    }

                } while (keyPressed != ConsoleKey.Enter);
                return SelectedIndex;
            }
        }
    }
}
