using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class GameInformation
    {
#if DEBUG
        public UChannel[] Channels { get; internal set; }
#endif

        public ICollection<Llama> Llamas => _llamas.Values;
        public ICollection<SafeZone> SafeZones => _safeZones;
        public ICollection<Player> Players => _players.Values;
        public ICollection<Team> Teams => _teams.Values;
        public ICollection<SupplyDrop> SupplyDrops => _supplyDrops.Values;
        public ICollection<PlayerReboot> Resurrections => _resurrections;
        public ICollection<InventoryItem> Items => _items.Values;

        public GameState GameState { get; private set; } = new GameState();
        public EncryptionKey PlayerStateEncryptionKey { get; internal set; }

        private Dictionary<uint, uint> _actorToChannel = new Dictionary<uint, uint>();
        private Dictionary<uint, Llama> _llamas = new Dictionary<uint, Llama>();
        private Dictionary<uint, SupplyDrop> _supplyDrops = new Dictionary<uint, SupplyDrop>();
        private Dictionary<uint, InventoryItem> _items = new Dictionary<uint, InventoryItem>(); //Channel Id to InventoryItem

        private Dictionary<uint, Player> _players = new Dictionary<uint, Player>(); //Channel id to Player
        private Dictionary<uint, PlayerPawn> _playerPawns = new Dictionary<uint, PlayerPawn>(); //Channel Id to Actor
        private Dictionary<uint, List<QueuedPlayerPawn>> _queuedPlayerPawns = new Dictionary<uint, List<QueuedPlayerPawn>>();
        private Dictionary<uint, FortInventory> _inventories = new Dictionary<uint, FortInventory>(); //Channel to inventory items

        private Dictionary<int, Team> _teams = new Dictionary<int, Team>();
        private List<SafeZone> _safeZones = new List<SafeZone>();
        private List<PlayerReboot> _resurrections = new List<PlayerReboot>();
        private List<dynamic> _killFeed = new List<dynamic>();

        internal Dictionary<uint, string> NetGUIDToPathName { get; set; }

        internal void AddActor(uint channel, Actor actor)
        {
            _actorToChannel.TryAdd(actor.ActorNetGUID.Value, channel);
        }

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
                    GameState.BusPaths.Add(new Aircraft
                    {
                        FlightRotation = flightPath.FlightStartRotation,
                        FlightStartLocation = flightPath.FlightStartLocation
                    });
                }
            }
        }

        internal void UpdateKillFeed(uint channelId, FortPlayerState playerState)
        {

        }

        internal void UpdatePlayerState(uint channelId, FortPlayerState playerState, Actor actor, NetFieldExportGroup networkGameplayTagNode)
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
            }

            if (playerState.DeathTags != null && networkGameplayTagNode != null)
            {
                playerState.DeathTags.UpdateTags(networkGameplayTagNode);

                UpdateKillFeed(channelId, playerState);
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

            if(playerState.DeathCause != null)
            {
                newPlayer.LastDeathTime = GameState.CurrentWorldTime;
            }

            if (playerState.TeamIndex != null)
            {
                if(!_teams.TryGetValue(playerState.TeamIndex.Value, out Team team))
                {
                    team = new Team();

                    _teams.TryAdd(playerState.TeamIndex.Value, team);
                }
                
                team.Players.Add(newPlayer);
                newPlayer.Team = team;
            }

            if (playerState.bResurrectingNow == true)
            {
                UpdateKillFeed(channelId, playerState);

                _resurrections.Add(new PlayerReboot
                {
                    Player = newPlayer,
                    WorldTime = GameState.CurrentWorldTime
                });
            }

            if(playerState.bHasEverSkydivedFromBusAndLanded != null)
            {
                if(newPlayer.Locations.Count > 0)
                {
                    newPlayer.LandingLocation = newPlayer.Locations.Last();
                }
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
                    if (_actorToChannel.TryGetValue(playerPawn.PlayerState.Value, out uint playerStateChannel) && _players.TryGetValue(playerStateChannel, out Player player))
                    {
                        _players.TryGetValue(playerStateChannel, out Player p);

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
                            RepLocation = playerPawn.ReplicatedMovement,
                            WorldTime = GameState.CurrentWorldTime,
                            LastUpdateTime = playerPawn.ReplayLastTransformUpdateTimeStamp
                        });
                    }

                    //Update current weapon
                    if(playerPawn.CurrentWeapon != null)
                    {
                        if(_actorToChannel.TryGetValue(playerPawn.CurrentWeapon.Value, out uint weaponChannel))
                        {
                            //Can't do this until Weapon parsing is done
                        }
                        else
                        {
                            //?
                        }
                    }

                    break;
            }

            //Currently only updating movement
        }

        internal void UpdateBatchedDamage(uint channelId, BatchedDamage batchedDamage)
        {
            if(!_playerPawns.TryGetValue(channelId, out PlayerPawn playerPawn))
            {
                //Shouldn't happen
                return;
            }

            if(!(playerPawn is Player))
            {
                //Nothing else currently
                return;
            }

            Player player = playerPawn as Player;

            WeaponShot shot = new WeaponShot
            {
                ShotByPlayerPawn = player,
                CriticalHitNonPlayer = batchedDamage.NonPlayerbIsCritical,
                IsCritical = batchedDamage.bIsCritical,
                FatalHitNonPlayer = batchedDamage.NonPlayerbIsFatal,
                IsFatal = batchedDamage.bIsFatal,
                WeaponActivate = batchedDamage.bWeaponActivate,
                IsBallistic = batchedDamage.bIsBallistic,
                IsShield = batchedDamage.bIsShield,
                Location = batchedDamage.Location,
                Magnitude = batchedDamage.Magnitude,
                Normal = batchedDamage.Normal,
                IsShieldDestroyed = batchedDamage.bIsShieldDestroyed,
                WorldTime = GameState.CurrentWorldTime,
                Weapon = player.CurrentWeapon
            };

            if(batchedDamage.HitActor > 0)
            {
                if (_actorToChannel.TryGetValue(batchedDamage.HitActor, out uint actorChannel))
                {
                    if (_playerPawns.TryGetValue(actorChannel, out PlayerPawn hitPlayerPawn))
                    {
                        shot.HitPlayerPawn = hitPlayerPawn;

                        Player hitPlayer = hitPlayerPawn as Player;
                        hitPlayer.DamageTaken.Add(shot);
                    }
                    else
                    {
                        //These are non player actors and other objects
                    }
                }
                else
                {
                    //Miss?
                }
            }

            player.Shots.Add(shot);
        }

        internal void UpdatePoiManager(FortPoiManager poiManager, NetFieldExportGroup networkGameplayTagNode)
        {
            if(networkGameplayTagNode == null || poiManager.PoiTagContainerTable == null)
            {
                return;
            }

            foreach (FGameplayTagContainer tagContainer in poiManager.PoiTagContainerTable)
            {
                tagContainer.UpdateTags(networkGameplayTagNode);
            }

            GameState.PoiManager = poiManager;
        }

        internal void UpdateFortInventory(uint channelId, FortInventory inventory)
        {

        }

        internal void UpdateNetDeltaFortInventory(NetDeltaUpdate deltaUpdate)
        {
        }

        private Dictionary<uint, List<FortPickup>> _pickups = new Dictionary<uint, List<FortPickup>>();

        internal void UpdateFortPickup(uint channelId, FortPickup pickup)
        {
            if(!_pickups.TryGetValue(channelId, out var asdfasd))
            {
                asdfasd = new List<FortPickup>();

                _pickups.TryAdd(channelId, asdfasd);
            }

            asdfasd.Add(pickup);

            bool isNewItem = !_items.TryGetValue(channelId, out InventoryItem newItem);

            //Create a new item. Same channel seems to be used for multiple items. Dropped items can be on a different channel
            if(pickup.ItemDefinition != null)
            {
                if (isNewItem || pickup.PickupTarget == null)
                {
                    newItem = new InventoryItem();
                    _items[channelId] = newItem;
                }
            }

            if (pickup.ItemDefinition != null)
            {
                if (NetGUIDToPathName.TryGetValue(pickup.ItemDefinition.Value, out string weaponPathName))
                {
                    newItem.ItemIdName = weaponPathName;
                }
            }

            newItem.Channel = channelId;
            newItem.ItemDefinition = pickup.ItemDefinition ?? newItem.ItemDefinition;
            newItem.InitialPosition = pickup.LootInitialPosition ?? newItem.InitialPosition;
            newItem.Count = pickup.Count ?? newItem.Count;
            newItem.Level = pickup.Level ?? newItem.Level;
            newItem.Ammo = pickup.LoadedAmmo ?? newItem.Ammo;

            //Need to test if it's correct
            if (pickup.CombineTarget != null)
            {
                if (_actorToChannel.TryGetValue(pickup.CombineTarget.Value, out uint actorChannel))
                {
                    if(_items.TryGetValue(actorChannel, out InventoryItem combinedItem))
                    {
                        newItem.CombineTarget = combinedItem;
                    }
                }
            }

            //Only handle weapons
            if (newItem.ItemIdName.StartsWith("WID"))
            {
                //Item picked up
                if (pickup.PickupTarget != null)
                {
                    if (_actorToChannel.TryGetValue(pickup.PickupTarget.Value, out uint actorChannel))
                    {
                        if (_playerPawns.TryGetValue(actorChannel, out PlayerPawn playerPawn))
                        {
                            Player player = (Player)playerPawn;

                            player.InventoryChanges.Add(new InventoryItemChange
                            {
                                Item = newItem,
                                State = ItemChangeState.PickedUp,
                                WorldTime = GameState.CurrentWorldTime,
                            });

                            newItem.CurrentOwner = player;
                        }
                    }
                }
                else if (pickup.PawnWhoDroppedPickup != null) //Item dropped
                {
                    if (_actorToChannel.TryGetValue(pickup.PawnWhoDroppedPickup.Value, out uint actorChannel))
                    {
                        if (_playerPawns.TryGetValue(actorChannel, out PlayerPawn playerPawn))
                        {
                            Player player = (Player)playerPawn;

                            player.InventoryChanges.Add(new InventoryItemChange
                            {
                                Item = newItem,
                                State = ItemChangeState.Dropped,
                                WorldTime = GameState.CurrentWorldTime,
                            });

                            newItem.CurrentOwner = null;
                            newItem.LastDroppedBy = player;
                        }
                    }
                }
            }
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
