using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace queens
{
    class Board
    {
        private int[,] board;


        private List<Player> players;
        private Position lastHoverPosition;
        private int playerOnTurn;

        public enum posEnum : sbyte
        {
            Empty = 0,
            Taken = 1,
        }


        /// <summary>
        /// Init Board
        /// </summary>
        /// <param name="size"></param>
        public Board(int sizeN, int sizeM)
        {
            this.board = new int[sizeN, sizeM];
            this.players = new List<Player>();
            this.lastHoverPosition = new Position(0, 0);
            this.playerOnTurn = 1;
        }

        public void AddPlayer(Player player)
        {
            this.players.Add(player);
        }

        public void MovePlayer(Player player, Position position)
        {
            if (position == null)
            {
                this.SetPlayerPosition(player);
            }
            else
            {
                // fill current position
                this.board[position.X, position.Y] = (int)posEnum.Taken;
                player.SetPosition(position);

                // fill right
                int tempY = player.GetCurrentPosition().Y;
                while (tempY < this.board.GetLength(1) - 1)
                {
                    tempY++;
                    this.board[player.GetCurrentPosition().X, tempY] = (int)posEnum.Taken;
                }

                // fill left
                tempY = player.GetCurrentPosition().Y;
                while (tempY != 0)
                {
                    tempY--;
                    this.board[player.GetCurrentPosition().X, tempY] = (int)posEnum.Taken;
                }

                // fill up
                int tempX = player.GetCurrentPosition().X;
                while (tempX != 0)
                {
                    tempX--;
                    this.board[tempX, player.GetCurrentPosition().Y] = (int)posEnum.Taken;
                }

                // fill down
                tempX = player.GetCurrentPosition().X;
                while (tempX < this.board.GetLength(0) - 1)
                {
                    tempX++;
                    this.board[tempX, player.GetCurrentPosition().Y] = (int)posEnum.Taken;
                }


                // fill diagonal top left
                tempX = player.GetCurrentPosition().X;
                tempY = player.GetCurrentPosition().Y;
                while (tempX != 0 && tempY != 0)
                {
                    tempY--;
                    tempX--;
                    this.board[tempX, tempY] = (int)posEnum.Taken;
                }

                // fill diagonal top right
                tempX = player.GetCurrentPosition().X;
                tempY = player.GetCurrentPosition().Y;
                while (tempX != 0 && tempY < this.board.GetLength(1) - 1)
                {
                    tempY++;
                    tempX--;
                    this.board[tempX, tempY] = (int)posEnum.Taken;
                }

                // fill diagonal bottom left
                tempX = player.GetCurrentPosition().X;
                tempY = player.GetCurrentPosition().Y;
                while (tempX < this.board.GetLength(0) - 1 && tempY != 0)
                {
                    tempY--;
                    tempX++;
                    this.board[tempX, tempY] = (int)posEnum.Taken;
                }

                // fill diagonal bottom left
                tempX = player.GetCurrentPosition().X;
                tempY = player.GetCurrentPosition().Y;
                while (tempX < this.board.GetLength(0) - 1 && tempY < this.board.GetLength(1) - 1)
                {
                    tempY++;
                    tempX++;
                    this.board[tempX, tempY] = (int)posEnum.Taken;
                }
            }

            this.playerOnTurn++;
            if (this.playerOnTurn > this.players.Count())
            {
                this.playerOnTurn = 1;
            }
        }


        /// <summary>
        /// Print The Board
        /// </summary>
        public void Print(Position hoverPosition = null)
        {
            Console.Clear();
            Console.Write($"[  ]", Console.ForegroundColor = ConsoleColor.DarkYellow);

            for (int i = 0; i < this.board.GetLength(1); i++)
            {
                if (i < 10)
                {
                    Console.Write($"[0{i}]");
                }
                else
                {
                    Console.Write($"[{i}]");
                }
            }
            
            for (int row = 0; row < this.board.GetLength(0); row++)
            {
                if (row < 10)
                {
                    Console.Write($"\n[0{row}]", Console.ForegroundColor = ConsoleColor.DarkYellow);
                }
                else
                {
                    Console.Write($"\n[{row}]", Console.ForegroundColor = ConsoleColor.DarkYellow);
                }
                
                for (int col = 0; col < this.board.GetLength(1); col++)
                {
                    int playerNumber = this.IsPlayerOnPosition(new Position(row, col));
                    
                    if (hoverPosition != null && (hoverPosition.X == row && hoverPosition.Y == col))
                    {
                        Console.Write($"[0{this.playerOnTurn}]", Console.ForegroundColor = ConsoleColor.DarkRed);
                    }
                    else if (playerNumber >= 1)
                    {
                        Console.Write($"[0{playerNumber}]", Console.ForegroundColor = ConsoleColor.Green);
                    }
                    else if (this.board[row, col] == (int)posEnum.Taken)
                    {
                        Console.Write($"[**]", Console.ForegroundColor = ConsoleColor.White);
                    }
                    else
                    {
                        Console.Write($"[  ]", Console.ForegroundColor = ConsoleColor.DarkGray);
                    }                     
                }
            }
        }

        /// <summary>
        /// Returns the Player number on the Position or 0 if no Player on Position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private int IsPlayerOnPosition(Position position)
        {
            for (int p = 0; p < this.players.Count(); p++)
            {
                if (this.players[p].takenPositions == null)
                {
                    return 0;
                }
                foreach (var playerTakenPosition in this.players[p].takenPositions)
                {
                    if (playerTakenPosition.X == position.X && playerTakenPosition.Y == position.Y)
                    {
                        return p + 1;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Check if Position is within the array bounds
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool InBounds(Position move)
        {
            return (move.X >= 0) && (move.X < this.board.GetLength(0)) && (move.Y >= 0) && (move.Y < this.board.GetLength(1));
        }


        /// <summary>
        /// Returns the player on turn
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <returns></returns>
        public Player GetPlayerOnTurn(int playerNumber)
        {
            if (playerNumber >= this.players.Count())
            {
                return null;
            }

            return players.ElementAt(playerNumber);
        }

        /// <summary>
        /// Sets the Player Position
        /// </summary>
        /// <param name="player"></param>
        public void SetPlayerPosition(Player player)
        {
            if (player.Name == "BOT")
            {
                List<Position> possibleBotPositions = new List<Position>();
                for (int x = 0; x < this.board.GetLength(0); x++)
                {
                    for (int y = 0; y < this.board.GetLength(1); y++)
                    {
                        if (this.board[x, y] == (int)posEnum.Empty)
                        {
                            possibleBotPositions.Add(new Position(x, y));
                        }
                    }
                }
                Random rnd = new Random();

                Position randomPosition = possibleBotPositions.ElementAt(rnd.Next(possibleBotPositions.Count));

                this.MovePlayer(player, randomPosition);
                player.takenPositions.Add(randomPosition);
                this.lastHoverPosition = new Position(0, 0);
            }
            else
            {
                while (true)
                {
                    Position tempPosition;

                    switch (Console.ReadKey(false).Key)
                    {
                        case ConsoleKey.Enter:
                            if (this.board[this.lastHoverPosition.X, this.lastHoverPosition.Y] != (int)posEnum.Taken)
                            {
                                this.MovePlayer(player, this.lastHoverPosition);
                                player.takenPositions.Add(this.lastHoverPosition);
                                this.lastHoverPosition = new Position(0, 0);
                            }
                            else
                            {
                                // todo return error
                                this.SetPlayerPosition(player);
                            }

                            return;
                        case ConsoleKey.UpArrow:

                            tempPosition = new Position(this.lastHoverPosition.X - 1, this.lastHoverPosition.Y);
                            if (this.InBounds(tempPosition))
                            {
                                this.lastHoverPosition = tempPosition;
                                Print(tempPosition);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            tempPosition = new Position(this.lastHoverPosition.X + 1, this.lastHoverPosition.Y);
                            if (this.InBounds(tempPosition))
                            {
                                this.lastHoverPosition = tempPosition;
                                Print(tempPosition);
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            tempPosition = new Position(this.lastHoverPosition.X, this.lastHoverPosition.Y - 1);
                            if (this.InBounds(tempPosition))
                            {
                                this.lastHoverPosition = tempPosition;
                                Print(tempPosition);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            tempPosition = new Position(this.lastHoverPosition.X, this.lastHoverPosition.Y + 1);
                            if (this.InBounds(tempPosition))
                            {
                                this.lastHoverPosition = tempPosition;
                                Print(tempPosition);
                            }
                            break;
                    }
                }
            }
        }



        /// <summary>
        /// Returns if Player has any possible moves
        /// </summary>
        /// <returns></returns>
        public bool IsLooser()
        {

            for (int x = 0; x < this.board.GetLength(0); x++)
            {
                for (int y = 0; y < this.board.GetLength(1); y++)
                {
                    if (this.board[x, y] == (int)posEnum.Empty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
    class Player
    {
        public string Name { get; set; }

        public Position position;

        public List<Position> takenPositions;

        public Player(string name)
        {
            this.Name = name;
            this.position = new Position(0, 0);
            takenPositions = new List<Position>();
        }

        /// <summary>
        /// Returns Player current Position
        /// </summary>
        /// <returns></returns>
        public Position GetCurrentPosition()
        {
            return this.position;
        }

        /// <summary>
        /// Set Player current Position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Position position)
        {
            this.position = position;
        }
    }
    class Position
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    class Game
    {
        private List<Player> players;
        private int sizeN = 5;
        private int sizeM = 5;

        public Game()
        {
            players = new List<Player>();
            players.Add(new Player("Player 1"));
            players.Add(new Player("Player 2"));
        }

        public void Start()
        {
            RunMainMethod();
        }
        private void RunMainMethod()
        {
            Clear();
            string prompt = @"
________                                      
\_____  \  __ __   ____   ____   ____   ______
 /  / \  \|  |  \_/ __ \_/ __ \ /    \ /  ___/
/   \_/.  \  |  /\  ___/\  ___/|   |  \\___ \ 
\_____\ \_/____/  \___  >\___  >___|  /____  >
       \__>           \/     \/     \/     \/ 

Use Up and Down arrow keys and Enter to play
";
            
            List<string> options = new List<string>();
            options.Add("Play");          
            options.Add("Rules");
            options.Add("Exit");

            Menu mainMenu = new Menu(prompt, options);
            int SelectedIndex = mainMenu.Run();

            switch (SelectedIndex)
            {
                case 0:
                    Play();
                    break;
                case 1:
                    PrintRules();
                    break;
                case 2:
                    ExitGame();
                    break;
            }
        }

        private void PrintRules()
        {
            Clear();
            string prompt = @"
________                                      
\_____  \  __ __   ____   ____   ____   ______
 /  / \  \|  |  \_/ __ \_/ __ \ /    \ /  ___/
/   \_/.  \  |  /\  ___/\  ___/|   |  \\___ \ 
\_____\ \_/____/  \___  >\___  >___|  /____  >
       \__>           \/     \/     \/     \/ 

Use Up and Down arrow keys and Enter to play
";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("******************INTRODUCTION*****************");
            sb.AppendLine("The queen is a chess piece, ");
            sb.AppendLine("which can attack other figures.");
            sb.AppendLine("She can be placed on the horizontal,");
            sb.AppendLine("vertical and diagonal.");
            sb.AppendLine("The Players are playing one after another.");
            sb.AppendLine("Each player must place a queen on the board,");
            sb.AppendLine("so she can't attack none of the other queens");
            sb.AppendLine("A player looses, when they");
            sb.AppendLine("cannot place a new queen");
            sb.AppendLine("In the beggning there are two positive numbers,");
            sb.AppendLine("which sets the board");
            sb.AppendLine("On every turn the program expects to");
            sb.AppendLine("get a position on which a queen will be placed");
            sb.AppendLine("************************************************\n");
            sb.AppendLine("*******************MOVEMENT*********************");
            sb.AppendLine("On our game you can place your queen ");
            sb.AppendLine("with the arrows of the keyboard");
            sb.AppendLine("************************************************\n");
            sb.AppendLine("******************PLAYER/BOT********************");
            sb.AppendLine("You can play vs bot or vs another player/s");
            sb.AppendLine("************************************************");

            Console.WriteLine(sb.ToString());


            List<string> options = new List<string>();
            options.Add("Back");



            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run(0, sb.ToString());

            if (selectedIndex == 0)
            {
                RunMainMethod();
            }


        }
    

        private void Players()
        {
            Clear();
            string prompt = @"
________                                      
\_____  \  __ __   ____   ____   ____   ______
 /  / \  \|  |  \_/ __ \_/ __ \ /    \ /  ___/
/   \_/.  \  |  /\  ___/\  ___/|   |  \\___ \ 
\_____\ \_/____/  \___  >\___  >___|  /____  >
       \__>           \/     \/     \/     \/ 

Use Up and Down arrow keys and Enter to play
";
            List<string> options = new List<string>();
            options.Add("Back");
            options.Add("Start game");
            options.Add("Add Player");
            for (int i = 0; i < this.players.Count(); i++)
            {
                options.Add($"{this.players.ElementAt(i).Name}");
            }

            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            if (selectedIndex == 0)
            {
                Play();
            }
            else if (selectedIndex == 1)
            {
                StartGame();
            }
            else if (selectedIndex == 2)
            {
                this.players.Add(new Player($"Player {this.players.Count() + 1}"));
                Players();
            }
           
        }
        private void BotMenu()
        {
            Clear();
            string prompt = @"
________                                      
\_____  \  __ __   ____   ____   ____   ______
 /  / \  \|  |  \_/ __ \_/ __ \ /    \ /  ___/
/   \_/.  \  |  /\  ___/\  ___/|   |  \\___ \ 
\_____\ \_/____/  \___  >\___  >___|  /____  >
       \__>           \/     \/     \/     \/ 

Use Up and Down arrow keys and Enter to play
";
            List<string> botOptions = new List<string>();

            botOptions.Add("Back");
            botOptions.Add("Start");

            Menu botMenu = new Menu(prompt, botOptions);
            int selectedIndex = botMenu.Run();

            if (selectedIndex == 0)
            {
                Play();
            }
            else if (selectedIndex == 1)
            {
                StartGameBot();
            }

        }
        private void StartGameBot()
        {
            Board board = new Board(sizeN, sizeM);
            this.players.Clear();
            this.players.Add(new Player("You"));
            this.players.Add(new Player("BOT"));

            foreach (var player in this.players)
            {
                board.AddPlayer(player);
            }

            int playerNumber = 0;

            while (true)
            {
                board.Print(new Position(0, 0));

                Player player = board.GetPlayerOnTurn(playerNumber);
                if (player == null)
                {
                    playerNumber = 0;
                    player = board.GetPlayerOnTurn(playerNumber);
                }

                if (board.IsLooser())
                {
                    Console.WriteLine($"\n{player.Name} losses");
                    break;
                }
                board.SetPlayerPosition(player);
                playerNumber++;
            }
        }
        private void Play()
        {
            Clear();
            string prompt = @"
________                                      
\_____  \  __ __   ____   ____   ____   ______
 /  / \  \|  |  \_/ __ \_/ __ \ /    \ /  ___/
/   \_/.  \  |  /\  ___/\  ___/|   |  \\___ \ 
\_____\ \_/____/  \___  >\___  >___|  /____  >
       \__>           \/     \/     \/     \/ 

Use Up and Down arrow keys and Enter to play
";
            List<string> gameModes = new List<string>();

            gameModes.Add("vs Players");
            gameModes.Add("vs Bot");
            gameModes.Add("Edit board size");
            gameModes.Add("Back");

            Menu playMenu = new Menu(prompt, gameModes);
            int selectedIndex = playMenu.Run();

            if (selectedIndex == 0)
            {
                Players();
            }
            else if (selectedIndex == 1)
            {
                BotMenu();
            }
            else if (selectedIndex == 2)
            {
                BoardSize();
            }
             else if (selectedIndex == 3)
            {
                RunMainMethod();
            }
        }
        private void BoardSize(int selcectedItem = 0)
        {
            Clear();
            string prompt = @"
________                                      
\_____  \  __ __   ____   ____   ____   ______
 /  / \  \|  |  \_/ __ \_/ __ \ /    \ /  ___/
/   \_/.  \  |  /\  ___/\  ___/|   |  \\___ \ 
\_____\ \_/____/  \___  >\___  >___|  /____  >
       \__>           \/     \/     \/     \/ 

Use Up and Down arrow keys and Enter to play
";
            List<string> boardSizeMenuOptions = new List<string>();

            boardSizeMenuOptions.Add("Back");
            boardSizeMenuOptions.Add($"N - {sizeN}");
            boardSizeMenuOptions.Add($"M - {sizeM}");

            Menu boardSizeMenu = new Menu(prompt, boardSizeMenuOptions);
            int selectedIndex = boardSizeMenu.Run(selcectedItem);

            if (selectedIndex == 0)
            {
                Play();
            }
            else if (selectedIndex == 1)
            {
                bool readSize = true;
                while (readSize)
                {
                    switch (Console.ReadKey(false).Key)
                    {

                        case ConsoleKey.Enter:
                            readSize = false;
                            break;
                        case ConsoleKey.UpArrow:
                            if (sizeN < 99)
                            {
                                this.sizeN++;
                            }
                            
                            BoardSize(1);
                            break;
                        case ConsoleKey.DownArrow:
                            if (sizeN > 3)
                            {
                                this.sizeN--;
                            }
                            BoardSize(1);
                            break;
                    }
                }

                BoardSize();

            }
            else if (selectedIndex == 2)
            {
                bool readSize = true;
                while (readSize)
                {
                    switch (Console.ReadKey(false).Key)
                    {

                        case ConsoleKey.Enter:
                            readSize = false;
                            break;
                        case ConsoleKey.UpArrow:
                            if (sizeM < 99)
                            {
                                this.sizeM++;
                            }
                            BoardSize(2);
                            break;
                        case ConsoleKey.DownArrow:
                            if (sizeM > 3)
                            {
                                this.sizeM--;
                            }
                            BoardSize(2);
                            break;
                    }
                }

                BoardSize();
            }
        }
        private void StartGame()
        {
            Board board = new Board(this.sizeN, this.sizeM);


            foreach (var player in this.players)
            {
                board.AddPlayer(player);
            }

            int playerNumber = 0;

            while (true)
            {
                board.Print(new Position(0, 0));

                Player player = board.GetPlayerOnTurn(playerNumber);
                if (player == null)
                {
                    playerNumber = 0;
                    player = board.GetPlayerOnTurn(playerNumber);
                }

                if (board.IsLooser())
                {
                    Console.WriteLine($"\n{player.Name} losses");
                    break;
                }
                board.SetPlayerPosition(player);
                playerNumber++;
            }
        }
        private void ExitGame()
        {
            Environment.Exit(0);
        }
    }
}
