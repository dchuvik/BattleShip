
using BattleShip;
using BattleShipLibrary.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        WelcomeMessage();

        PlayerInfoModel activePlayer = CreateUser("Player1");
        PlayerInfoModel opponent = CreateUser("Player2");
        PlayerInfoModel winner = null;

        do
        {
            DisplayShotGrid(activePlayer);

            RecordPlayerShot(activePlayer, opponent);

            bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

            if (doesGameContinue == true)
            {
                //Swap player variables
                (activePlayer, opponent) = (opponent, activePlayer);
            }
            else
            {
                winner = activePlayer;
            }

        } while (winner == null);

        IndentifyWinner(winner);

        Console.ReadLine();


    }

    private static void IndentifyWinner(PlayerInfoModel winner)
    {
        Console.WriteLine();
        Console.WriteLine($"Congratulations to {winner.PlayerName} for winning!");
        Console.WriteLine($"{winner.PlayerName} took {GameLogic.GetShotCount(winner)} shots to win.");
    }

    private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
    {
        //Ask for Shot
        bool isValidShot = false;
        string row = string.Empty;
        int column = 0;

        do
        {
            string shot = AskForShot(activePlayer);
            try
            {
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
            }
            catch (Exception ex)
            {

                //Console.WriteLine($"Error: {ex.Message}");
                isValidShot=false;
            }

            if (isValidShot == false)
            {
                Console.WriteLine("Invalid shot location, Please try again.");
            }
        } while (isValidShot == false);

        bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

        GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

        DisplayShotResults(row, column, isAHit);
    }

    private static void DisplayShotResults(string row, int column, bool isAHit)
    {
        if (isAHit)
        {
            Console.WriteLine($"{row.ToUpper()}{column} is a Hit!");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"{row}{column} is a miss.");
            Console.WriteLine();
        }
    }

    private static string AskForShot(PlayerInfoModel activePlayer)
    {
        Console.Write($"{activePlayer.PlayerName}, please enter your shot selection: ");
        string output = Console.ReadLine();
        return output;
    }

    private static void DisplayShotGrid(PlayerInfoModel activePlayer)
    {
        string currentRow = activePlayer.ShotGrid[0].SpotLetter;

        foreach (var gridSpot in activePlayer.ShotGrid)
        {
            if (gridSpot.SpotLetter != currentRow)
            {
                Console.WriteLine();
                currentRow = gridSpot.SpotLetter;
            }

            

            if (gridSpot.Status == GridSpotStatus.Empty)
            {
                Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
            }
            else if (gridSpot.Status == GridSpotStatus.Hit)
            {
                Console.Write(" X  ");
            }
            else if (gridSpot.Status == GridSpotStatus.Miss)
            {
                Console.Write(" O  ");
            }
            else
            {
                Console.Write(" ?  ");
            }
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void WelcomeMessage()
    {
        Console.WriteLine("Welcome to the Battleship Lite App!");
        Console.WriteLine("Created by David Chuvik");
        Console.WriteLine();
    }
    private static PlayerInfoModel CreateUser(string playerTitle)
    {
        
        PlayerInfoModel player = new PlayerInfoModel();

        Console.WriteLine($"Player information for {playerTitle}");

        //Ask for name
        AskForUsersName(player);

        //Load shot grid
        GameLogic.InitializeGrid(player);

        //Ask for 5 ship placements
        PlaceShips(player);

        //Clear Console
        Console.Clear();

        return player;
    }



    private static void AskForUsersName(PlayerInfoModel player)
    {
        Console.Write("What is your name: ");
        player.PlayerName = Console.ReadLine();
    }    
    
    private static void PlaceShips(PlayerInfoModel player)
    {
        do
        {
            Console.Write($"Where do you want to place ship number {player.ShipLocations.Count + 1}: ");
            string location = Console.ReadLine();

            bool isValidLocation = false;

            try
            {
                isValidLocation = GameLogic.PlaceShip(player, location);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (isValidLocation == false)
            {
                Console.WriteLine("That was not a valid location. Please try again.");
            }

        } while (player.ShipLocations.Count < 5);
    }
}