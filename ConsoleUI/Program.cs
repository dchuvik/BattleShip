using BattleshipLibrary;
using BattleShipLibrary.Models;
using System.IO.Pipes;

internal class Program
{
    private static void Main(string[] args)
    {
        {
            //Ask user for name:
            GreetMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                // Display grid from activePlayer on where they fired

                DisplayShotGrid(activePlayer);

                // Ask activePlayer for shot
                // test is valid shot
                // determine shot results
                RecordPlayerShot(activePlayer, opponent);

                // determine if game is over
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
                // if over, set activePlayer as winner

                if (doesGameContinue == true)
                {
                    //Swap using Tuple
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }
                // else, swap positions (activePlayer to opponent))

                Console.Clear();
            } while (winner == null);

            IdentifyWinner(winner);

            Console.ReadLine();

        }


    }

    private static void IdentifyWinner(PlayerInfoModel winner)
    {
        Console.WriteLine($"Congradulations to {winner.PlayerName} for winning!");
        Console.WriteLine($"{winner.PlayerName} took {GameLogic.GetShotCount(winner)} shots.");
    }

    private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
    {
        bool isValidShot = false;
        string row = string.Empty;
        int column = 0;
        //Ask for a shot ( Ask in this format "B2")
        //Determine what row and column that is - split apart B2
        //Determine if valid shot
        //Go back to beginning if not valid shot
        do
        {
            string shot = AskForShot();
            ( row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
            isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

            if (isValidShot == false)
            {
                Console.WriteLine("Invalid shot location. Please try again.");
            }

        } while (isValidShot == false);

        //Determine shot result (is there a ship there?)
        bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);


        //Record Results
        GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
    }

    private static string AskForShot()
    {
        Console.Write("Please enter your shot selection: ");
        string output = Console.ReadLine();

        return output;
    }

    private static void DisplayShotGrid(PlayerInfoModel activePlayer)
    {
        string currentRow = activePlayer.ShotGrid[0].SpotLetter;

        foreach (var gridspot in activePlayer.ShotGrid)
        {
            if (gridspot.SpotLetter != currentRow)
            {
                Console.WriteLine();
                currentRow = gridspot.SpotLetter;
            }

            if (gridspot.Status == GridSpotStatus.Empty)
            {
                Console.Write($" {gridspot.SpotLetter}{gridspot.SpotNumber} ");
            }
            else if (gridspot.Status == GridSpotStatus.Hit)
            {
                Console.Write(" X ");
            }
            else if (gridspot.Status == GridSpotStatus.Miss)
            {
                Console.Write(" O ");
            }
            else
            {
                Console.Write(" ? ");
            }
        }
    }

    private static void GreetMessage()
    {
        Console.WriteLine("Welcome to BattleShip Lite");
        Console.WriteLine("Created by David Chuvik");
        Console.WriteLine();
    }

    private static PlayerInfoModel CreatePlayer(string playerTitle)
    {
        /// Ask the user for name
        /// Load up shot grid
        /// Ask user for thier 5 ship placements
        PlayerInfoModel player = new PlayerInfoModel();

        Console.WriteLine($"Player information for {playerTitle}");

        //Ask user for name:
        player.PlayerName = AskForUsersName();

        //Load up the shot grid
        GameLogic.InitializeGrid(player);

        //Ask user for 5 ship placements
        PlaceShips(player);

        //Clear
        Console.Clear();

        return player;

    }

    private static string AskForUsersName()
    {
        Console.Write("What is your name: ");
        string output = Console.ReadLine();

        return output;
    }

    private static void PlaceShips(PlayerInfoModel player)
    {
        do
        {
            Console.Write($"Where do you want to place ship number {player.ShipLocations.Count + 1}: ");
            string location = Console.ReadLine();

            bool isValidLocation = GameLogic.PlaceShip(player, location);

            if (isValidLocation == false)
            {
                Console.WriteLine("That was an invalid ship location. Please try again.");
            }
        } while (player.ShipLocations.Count < 5);
    }
}