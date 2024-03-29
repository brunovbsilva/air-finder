﻿using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Common;
using AirFinder.Domain.Games.Models.Enums;
using AirFinder.Domain.Games.Models.Requests;
using AirFinder.Domain.People;
using AirFinder.Domain.Users;

namespace AirFinder.Domain.Games
{
    public class Game : BaseModel
    {
        public Game(string name, string description, long millisDateFrom, long millisDateUpTo, int maxPlayers, Guid idBattleground, Guid idCreator)
        {
            Name = name;
            Description = description;
            MillisDateFrom = millisDateFrom;
            MillisDateUpTo = millisDateUpTo;
            MaxPlayers = maxPlayers;
            IdBattleground = idBattleground;
            IdCreator = idCreator;
            BattleGroud = null;
            Creator = null;
        }
        public Game(CreateGameRequest request, Guid idCreator)
        {
            Name = request.Name;
            Description = request.Description;
            MillisDateFrom = request.DateFrom;
            MillisDateUpTo = request.DateUpTo;
            MaxPlayers = request.MaxPlayers ?? 0;
            IdBattleground = request.IdBattleground;
            IdCreator = idCreator;
        }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public long MillisDateFrom { get; set; } = 0;
        public long MillisDateUpTo { get; set; } = 0;
        public int MaxPlayers { get; set; } = 0;
        public Guid IdBattleground { get; set; }
        public Guid IdCreator { get; set; }
        public virtual Battleground? BattleGroud { get; set; }
        public virtual User? Creator { get; set; }

        public void Update(UpdateGameRequest request)
        {
            Name = request.Name;
            Description = request.Description;
            MillisDateFrom = request.DateFrom;
            MillisDateUpTo = request.DateUpTo;
            MaxPlayers = request.MaxPlayers;
        }
    }
}
