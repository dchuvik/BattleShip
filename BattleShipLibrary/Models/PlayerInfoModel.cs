﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLibrary.Models
{
    public class PlayerInfoModel
    {
        public string PlayerName { get; set; }
        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();
        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
    }
}
