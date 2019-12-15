using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class GameInformation
    {
        public ICollection<Llama> Llamas => _llamas.Values;
        public ICollection<SafeZone> SafeZones => _safeZones;
        public ICollection<Player> Players => _players.Values;
        public ICollection<Team> Teams => _teams.Values;
        public GameState GameState { get; private set; } = new GameState();
        public EncryptionKey PlayerStateEncryptionKey { get; internal set; }

        private Dictionary<uint, Llama> _llamas = new Dictionary<uint, Llama>();
        private Dictionary<uint, SupplyDrop> _supplyDrops = new Dictionary<uint, SupplyDrop>();

        private Dictionary<uint, Player> _players = new Dictionary<uint, Player>(); //Channel id to Player
        private Dictionary<uint, Player> _fortPlayerStateGuids = new Dictionary<uint, Player>(); //NetGUID to Player
        private Dictionary<uint, PlayerPawn> _playerPawns = new Dictionary<uint, PlayerPawn>(); //Channel Id to Actor
        private Dictionary<uint, List<QueuedPlayerPawn>> _queuedPlayerPawns = new Dictionary<uint, List<QueuedPlayerPawn>>();

        private Dictionary<int, Team> _teams = new Dictionary<int, Team>();
        private List<SafeZone> _safeZones = new List<SafeZone>();

        internal void UpdateLlama(uint channel, SupplyDropLlamaC supplyDropLlama)
        {
            Llama newLlama = new Llama();

            if(!_llamas.TryAdd(channel, newLlama))
            {
                _llamas.TryGetValue(channel, out newLlama);
            }

            newLlama.Location = supplyDropLlama.ReplicatedMovement?.Location ?? newLlama.Location;
            newLlama.Opened = supplyDropLlama.Looted ?? newLlama.Opened;
            newLlama.Destroyed = supplyDropLlama.bDestroyed ?? newLlama.Destroyed;
            newLlama.SpawnedItems = supplyDropLlama.bHasSpawnedPickups ?? newLlama.SpawnedItems;
        }

        internal void UpdateSupplyDrop(uint channel, SupplyDropC supplyDrop)
        {
            SupplyDrop newSupplyDrop = new SupplyDrop();

            if (!_supplyDrops.TryAdd(channel, newSupplyDrop))
            {
                _supplyDrops.TryGetValue(channel, out newSupplyDrop);
            }

            newSupplyDrop.Location = supplyDrop.LandingLocation ?? newSupplyDrop.Location;
            newSupplyDrop.Opened = supplyDrop.Opened ?? newSupplyDrop.Opened;
            newSupplyDrop.Destroyed = supplyDrop.bDestroyed ?? newSupplyDrop.Destroyed;
            newSupplyDrop.SpawnedItems = supplyDrop.bHasSpawnedPickups ?? newSupplyDrop.SpawnedItems;
            newSupplyDrop.BalloonPopped = supplyDrop.BalloonPopped ?? newSupplyDrop.BalloonPopped;
        }

        internal void UpdateSafeZone(SafeZoneIndicatorC safeZone)
        {
            //Zone shrink updates, ignore
            if(safeZone.SafeZoneFinishShrinkTime == null)
            {
                return;
            }

            SafeZone newSafeZone = new SafeZone();

            newSafeZone.NextNextRadius = safeZone.NextNextRadius ?? newSafeZone.NextNextRadius;
            newSafeZone.NextRadius = safeZone.NextRadius ?? newSafeZone.NextRadius;
            newSafeZone.Radius = safeZone.Radius ?? newSafeZone.Radius;
            newSafeZone.ShringEndTime = safeZone.SafeZoneFinishShrinkTime ?? newSafeZone.ShringEndTime;
            newSafeZone.ShrinkStartTime = safeZone.SafeZoneStartShrinkTime ?? newSafeZone.ShrinkStartTime;
            newSafeZone.LastCenter = safeZone.LastCenter ?? newSafeZone.LastCenter;
            newSafeZone.NextCenter = safeZone.NextCenter ?? newSafeZone.NextCenter;
            newSafeZone.NextNextCenter = safeZone.NextNextCenter ?? newSafeZone.NextNextCenter;

            _safeZones.Add(newSafeZone);
        }

        internal void UpdateGameState(GameStateC gameState)
        {
            GameState.AirCraftStartTime = gameState.AircraftStartTime ?? GameState.AirCraftStartTime;
            GameState.InitialSafeZoneStartTime = gameState.SafeZonesStartTime ?? GameState.InitialSafeZoneStartTime;
            GameState.SessionId = gameState.GameSessionId ?? GameState.SessionId;
            GameState.MatchTime = gameState.UtcTimeStartedMatch?.Time ?? GameState.MatchTime;
            GameState.EventTournamentRound = gameState.EventTournamentRound ?? GameState.EventTournamentRound;
            GameState.LargeTeamGame = gameState.bIsLargeTeamGame ?? GameState.LargeTeamGame;
            GameState.MaxPlayers = gameState.TeamCount ?? GameState.MaxPlayers;
            GameState.MatchEndTime = gameState.EndGameStartTime ?? GameState.MatchEndTime;
            GameState.TotalTeams = gameState.ActiveTeamNums?.Length ?? GameState.TotalTeams;
            GameState.TotalBots = gameState.PlayerBotsLeft > GameState.TotalBots ? gameState.PlayerBotsLeft.Value : GameState.TotalBots;

            //Internal information to keep track of current state of the game
            GameState.CurrentWorldTime = gameState.ReplicatedWorldTimeSeconds ?? GameState.CurrentWorldTime;
            GameState.RemainingPlayers = gameState.PlayersLeft ?? GameState.RemainingPlayers;
            GameState.CurrentTeams = gameState.TeamsLeft ?? GameState.CurrentTeams;
            GameState.SafeZonePhase = gameState.SafeZonePhase ?? GameState.SafeZonePhase;
            GameState.RemainingBots = gameState.PlayerBotsLeft ?? GameState.RemainingBots;
            GameState.ElapsedTime = gameState.ElapsedTime ?? GameState.ElapsedTime;
            GameState.OldTeamSize = gameState.TeamSize ?? GameState.OldTeamSize;
            GameState.TotalPlayerStructures = gameState.TotalPlayerStructures ?? GameState.TotalPlayerStructures;

            if (GameState.GameWorldStartTime == 0)
            {
                GameState.GameWorldStartTime = gameState.ReplicatedWorldTimeSeconds ?? GameState.AirCraftStartTime;
            }

            if(gameState.TeamFlightPaths != null)
            {
                foreach(GameStateC flightPath in gameState.TeamFlightPaths)
                {
                    GameState.PlanePaths.Add(new Airplane
                    {
                        FlightRotation = flightPath.FlightStartRotation,
                        FlightStartLocation = flightPath.FlightStartLocation
                    });
                }
            }
        }

        internal void UpdatePlayerState(uint channelId, FortPlayerState playerState, Actor actor)
        {
            if(playerState.bOnlySpectator == true)
            {
                return;
            }

            bool isNewPlayer = !_players.TryGetValue(channelId, out Player newPlayer);

            if (isNewPlayer)
            {
                newPlayer = new Player();

                _players.TryAdd(channelId, newPlayer);
                _fortPlayerStateGuids.TryAdd(actor.ActorNetGUID.Value, newPlayer);
            }

            newPlayer.Actor = actor;
            newPlayer.EpicId = playerState.UniqueId ?? newPlayer.EpicId;
            newPlayer.Platform = playerState.Platform ?? newPlayer.Platform;
            newPlayer.Teamindex = playerState.TeamIndex ?? newPlayer.Teamindex;
            newPlayer.PartyOwnerEpicId = playerState.PartyOwnerUniqueId ?? newPlayer.PartyOwnerEpicId;
            newPlayer.IsBot = playerState.bIsABot ?? newPlayer.IsBot;
            newPlayer.BotId = playerState.BotUniqueId ?? newPlayer.BotId;
            newPlayer.IsGameSessionOwner = playerState.bIsGameSessionOwner ?? newPlayer.IsGameSessionOwner;
            newPlayer.Level = playerState.Level ?? newPlayer.Level;
            newPlayer.FinishedLoading = playerState.bHasFinishedLoading ?? newPlayer.FinishedLoading;
            newPlayer.StartedPlaying = playerState.bHasStartedPlaying ?? newPlayer.StartedPlaying;
            newPlayer.IsPlayersReplay = playerState.Ping > 0 ? true : newPlayer.IsPlayersReplay;
            newPlayer.StreamerMode = playerState.bUsingStreamerMode ?? newPlayer.StreamerMode;
            newPlayer.ThankedBusDriver = playerState.bThankedBusDriver ?? newPlayer.ThankedBusDriver;
            newPlayer.Placement = playerState.Place ?? newPlayer.Placement;
            newPlayer.BannerId = playerState.IconId ?? newPlayer.BannerId;
            newPlayer.ColorId = playerState.ColorId ?? newPlayer.ColorId;

            if (playerState.TeamIndex != null)
            {
                if(!_teams.TryGetValue(playerState.TeamIndex.Value, out Team team))
                {
                    team = new Team();

                    _teams.TryAdd(playerState.TeamIndex.Value, team);
                }
                
                team.Players.Add(newPlayer);
            }

            //Internal info
            newPlayer.WorldPlayerId = playerState.WorldPlayerId ?? newPlayer.WorldPlayerId;

            if(isNewPlayer)
            {
                HandleQueuedPlayerPawns(newPlayer);
            }
        }

        internal void UpdatePlayerPawn(uint channelId, PlayerPawnC playerPawn)
        {
            if(!_playerPawns.TryGetValue(channelId, out PlayerPawn actor))
            {
                //Check for PlayerState
                if(playerPawn.PlayerState != null)
                {
                    if(_fortPlayerStateGuids.TryGetValue(playerPawn.PlayerState.Value, out Player player))
                    {
                        _playerPawns.TryAdd(channelId, player);
                        actor = player;
                    }
                    else
                    {
                        //Queues up player pawn to process for later
                        if (!_queuedPlayerPawns.TryGetValue(playerPawn.PlayerState.Value, out var playerPawns))
                        {
                            playerPawns = new List<QueuedPlayerPawn>();

                            _queuedPlayerPawns.TryAdd(playerPawn.PlayerState.Value, playerPawns);
                        }

                        playerPawns.Add(new QueuedPlayerPawn
                        {
                            ChannelId = channelId,
                            PlayerPawn = playerPawn
                        });

                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            switch (actor)
            {
                case Player playerActor:
                    if(playerPawn.ReplicatedMovement != null) //Update location
                    {
                        playerActor.Locations.Add(new PlayerLocation
                        {
                            Location = playerPawn.ReplicatedMovement.Location,
                            WorldTime = GameState.CurrentWorldTime,
                            LastUpdateTime = playerPawn.ReplayLastTransformUpdateTimeStamp
                        });
                    }
                    break;
            }

            //Currently only updating movement
        }

        private void HandleQueuedPlayerPawns(Player player)
        {
            if(_queuedPlayerPawns.Remove(player.Actor.ActorNetGUID.Value, out var playerPawns))
            {
                foreach(var playerPawn in playerPawns)
                {
                    UpdatePlayerPawn(playerPawn.ChannelId, playerPawn.PlayerPawn);
                }
            }
        }
    }
}
