using BattleShipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel player)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E",
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };



            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(player, letter, number);
                }
            }
        }



        private static void AddGridSpot(PlayerInfoModel player, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };

            player.ShotGrid.Add(spot);
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;
            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    shotCount++;
                }
            }
            return shotCount;
        }

        public static bool PlaceShip(PlayerInfoModel player, string location)
        {
            bool output = false;
            (string rowLetter, int columnNumber) = SplitShotIntoRowAndColumn(location);

            //Is input on a grid location
            bool isValidLocation = ValidateGridLocation(player, rowLetter, columnNumber);

            //Is there already a ship there
            bool isSpotOpen = ValidateShipLocation(player, rowLetter, columnNumber);

            if (isValidLocation && isSpotOpen)
            {
                player.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = rowLetter.ToUpper(),
                    SpotNumber = columnNumber,
                    Status = GridSpotStatus.Ship

                });

                output = true;
            }
            return output;
        }

        private static bool ValidateShipLocation(PlayerInfoModel player, string rowLetter, int columnNumber)
        {
            bool isValidLocation = true;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.SpotLetter == rowLetter.ToUpper() && ship.SpotNumber == columnNumber)
                {
                    isValidLocation = false;
                }
            }
            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel player, string rowLetter, int columnNumber)
        {
            bool isValidLocation = false;

            foreach (var ship in player.ShotGrid)
            {
                if (ship.SpotLetter == rowLetter.ToUpper() && ship.SpotNumber == columnNumber)
                {
                    isValidLocation = true;
                }
            }
            return isValidLocation;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string rowLetter = string.Empty;
            int columnNumber = 0;


            if (shot.Length != 2)
            {
                throw new ArgumentOutOfRangeException("shot", "invalid shot location.");
            }

            char[] shotArray = shot.ToArray();

            rowLetter = shotArray[0].ToString();
            columnNumber = int.Parse(shotArray[1].ToString());

            return (rowLetter, columnNumber);
        }

        public static bool ValidateShot(PlayerInfoModel player, string rowLetter, int columnNumber)
        {
            bool isValidLocation = false;

            //Is shot spot on grid?
            foreach (var gridSpot in player.ShotGrid)
            {
                //is shot spot empty?
                if (gridSpot.SpotLetter == rowLetter.ToUpper() && gridSpot.SpotNumber == columnNumber)
                {
                    if (gridSpot.Status == GridSpotStatus.Empty)
                    {
                        isValidLocation = true;
                    }

                }
            }
            return isValidLocation;
        }

        public static bool IdentifyShotResult(PlayerInfoModel player, string rowLetter, int columnNumber)
        {
            bool isAHit = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.SpotLetter == rowLetter.ToUpper() && ship.SpotNumber == columnNumber)
                {
                    isAHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }
            return isAHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string rowLetter, int columnNumber, bool isAHit)
        {
            foreach (var shot in player.ShotGrid)
            {
                if (shot.SpotLetter == rowLetter.ToUpper() && shot.SpotNumber == columnNumber)
                {
                    if (isAHit == true)
                    {
                        shot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        shot.Status = GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}
//TEST