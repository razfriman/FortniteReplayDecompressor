using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using FortniteReplayReader.Models.NetFieldExports.ClassNetCaches.Functions;
using FortniteReplayReader.Models.Weapons;
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
        public ICollection<KillFeedEntry> KillFeed => _killFeed;

        public GameState GameState { get; private set; } = new GameState();
        public EncryptionKey PlayerStateEncryptionKey { get; internal set; }

        private Dictionary<uint, uint> _actorToChannel = new Dictionary<uint, uint>();
        private Dictionary<uint, Llama> _llamas = new Dictionary<uint, Llama>();
        private Dictionary<uint, SupplyDrop> _supplyDrops = new Dictionary<uint, SupplyDrop>();
        private Dictionary<uint, InventoryItem> _items = new Dictionary<uint, InventoryItem>(); //Channel Id to InventoryItem

        private Dictionary<uint, Player> _players = new Dictionary<uint, Player>(); //Channel id to Player
        private Dictionary<uint, PlayerPawn> _playerPawns = new Dictionary<uint, PlayerPawn>(); //Channel Id to Actor
        private Dictionary<uint, List<QueuedPlayerPawn>> _queuedPlayerPawns = new Dictionary<uint, List<QueuedPlayerPawn>>();
        private Dictionary<uint, FortInventory> _queuedInventories = new Dictionary<uint, FortInventory>(); //PlayerPawn Actor to inventory items
        private Dictionary<uint, FortInventory> _inventories = new Dictionary<uint, FortInventory>(); //Channel to inventory items
        private Dictionary<uint, Weapon> _weapons = new Dictionary<uint, Weapon>(); //Channel to Weapon

        private Dictionary<int, Team> _teams = new Dictionary<int, Team>();
        private List<SafeZone> _safeZones = new List<SafeZone>();
        private List<PlayerReboot> _resurrections = new List<PlayerReboot>();
        private List<KillFeedEntry> _killFeed = new List<KillFeedEntry>();

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
            KillFeedEntry entry = new KillFeedEntry();
            entry.DeltaGameTimeSeconds = GameState.CurrentWorldTime - GameState.GameWorldStartTime;
            entry.DeathTags = playerState.DeathTags?.Tags.Select(x => x.TagName).ToArray();
            entry.Distance = playerState.Distance ?? 0;

            if(!_players.TryGetValue(channelId, out Player channelPlayer))
            {
                //Shouldn't happen
                entry.HasError = true;

                _killFeed.Add(entry);

                return;
            }

            if (playerState.bResurrectingNow != null)
            {
                //Rebooted?
                return;
            }
            else
            {
                Player eliminator = channelPlayer.LastKnockedEntry?.FinisherOrDowner;
                
                if(playerState.FinisherOrDowner != null)
                {
                    if (playerState.FinisherOrDowner == 0)
                    {
                        //DBNO revives?
                        if(playerState.DeathCause != null)
                        {
                            entry.Player = channelPlayer;
                            entry.CurrentPlayerState = PlayerState.Alive;
                            channelPlayer.LastKnockedEntry = null;
                            channelPlayer.StatusChanges.Add(entry);
                            _killFeed.Add(entry);
                        }

                        return;
                    }

                    if (!_actorToChannel.TryGetValue(playerState.FinisherOrDowner.Value, out uint channel) || !_players.TryGetValue(channel, out eliminator))
                    {
                        entry.HasError = true;

                        _killFeed.Add(entry);

                        return;
                    }
                }

                if (playerState.bDBNO != null)
                {
                    if (playerState.bDBNO == true)
                    {
                        channelPlayer.LastKnockedEntry = entry;
                    }

                    entry.Player = channelPlayer;
                    entry.FinisherOrDowner = eliminator;
                    entry.CurrentPlayerState = playerState.bDBNO == true ? PlayerState.Knocked : PlayerState.Killed;
                    entry.ItemId = playerState.DeathCause ?? channelPlayer.LastKnockedEntry?.ItemId ?? 0; //0 would technically be unknown. Occurs with grenades, but we can pull that information through tags

                    if (entry.DeathTags == null)
                    {
                        entry.DeathTags = channelPlayer.LastKnockedEntry?.DeathTags ?? null;
                    }
                }
                else //Full kill
                {
                    entry.Player = channelPlayer;
                    entry.FinisherOrDowner = eliminator;
                    entry.CurrentPlayerState = PlayerState.Killed;
                    entry.ItemId = playerState.DeathCause ?? 0;
                }
            }

            channelPlayer.StatusChanges.Add(entry);
            _killFeed.Add(entry);
        }

        internal void HandleGameplayCue(uint channelId, GameplayCue cue)
        {
            /*
	            GameplayCue.Abilities.Activation.DBNOResurrect.Athena -  - 7862
	            GameplayCue.Abilities.Activation.Traps.ActivateTrap -  - 7911
	            GameplayCue.Abilities.Activation.Traps.DelayBegin -  - 7912
	            GameplayCue.Abilities.Activation.Traps.Placed -  - 7917
	            GameplayCue.Abilities.Activation.Traps.ReloadBegin -  - 7918
	            GameplayCue.Athena.DBNOBleed -  - 8224
	            GameplayCue.Athena.EnvCampfire.Doused -  - 8235
	            GameplayCue.Athena.EnvCampfire.Fire -  - 8236
	            GameplayCue.Athena.EnvItem.Slurp -  - 8240
	            GameplayCue.Athena.Equipping -  - 8241
	            GameplayCue.Athena.Item.FloppingRabbit.Cast -  - 8353
	            GameplayCue.Athena.OutsideSafeZoneDamage -  - 8448
	            GameplayCue.Athena.Player.BeingRevivedFromDBNO -  - 8454
	            GameplayCue.Athena.Player.BigBushMovement -  - 8455
	            GameplayCue.Athena.Player.CornFieldMovement -  - 8458
	            GameplayCue.Damage.Physical -  - 8610
	            GameplayCue.Shield.PotionConsumed -  - 9012
	            GameplayCue.Weapons.Activation -  - 9080
            */
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
            newPlayer.TotalKills = playerState.KillScore ?? newPlayer.TotalKills;
            newPlayer.TeamKills = playerState.TeamKillScore ?? newPlayer.TeamKills;

            if(playerState.DeathCause != null)
            {
                newPlayer.LastDeathOrKnockTime = GameState.CurrentWorldTime;

                //Only occurs on main player or server replays
                if (newPlayer.CurrentInventory.Count > 0)
                {
                    newPlayer.InventoryOnDeath.Clear();
                    newPlayer.InventoryOnDeath.AddRange(newPlayer.InventoryBeforeDeletes);
                }
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
            
            //Add to killfeed
            if (playerState.bResurrectingNow != null || playerState.bDBNO != null || playerState.FinisherOrDowner != null)
            {
                UpdateKillFeed(channelId, playerState);
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

        internal void UpdatePlayerPawn(uint channelId, PlayerPawnC playerPawnC, Actor actor)
        {
            if(!_playerPawns.TryGetValue(channelId, out PlayerPawn playerpawn))
            {
                //Check for PlayerState
                if(playerPawnC.PlayerState != null)
                {
                    if (_actorToChannel.TryGetValue(playerPawnC.PlayerState.Value, out uint playerStateChannel) && _players.TryGetValue(playerStateChannel, out Player player))
                    {
                        _players.TryGetValue(playerStateChannel, out Player p);

                        _playerPawns.TryAdd(channelId, player);
                        playerpawn = player;
                    }
                    else
                    {
                        //Queues up player pawn to process for later
                        if (!_queuedPlayerPawns.TryGetValue(playerPawnC.PlayerState.Value, out var playerPawns))
                        {
                            playerPawns = new List<QueuedPlayerPawn>();

                            _queuedPlayerPawns.TryAdd(playerPawnC.PlayerState.Value, playerPawns);
                        }

                        playerPawns.Add(new QueuedPlayerPawn
                        {
                            ChannelId = channelId,
                            PlayerPawn = playerPawnC,
                            Actor = actor
                        });

                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            switch (playerpawn)
            {
                case Player playerActor:
                    if(playerPawnC.ReplicatedMovement != null) //Update location
                    {
                        playerActor.Locations.Add(new PlayerLocation
                        {
                            RepLocation = playerPawnC.ReplicatedMovement,
                            WorldTime = GameState.CurrentWorldTime,
                            LastUpdateTime = playerPawnC.ReplayLastTransformUpdateTimeStamp
                        });
                    }

                    //Update current weapon
                    if(playerPawnC.CurrentWeapon != null)
                    {
                        if(_actorToChannel.TryGetValue(playerPawnC.CurrentWeapon.Value, out uint weaponChannel))
                        {
                            if(_weapons.TryGetValue(weaponChannel, out Weapon weapon))
                            {
                                //Handle weapon changes
                                playerActor.WeaponSwitches.Add(new WeaponSwitch
                                {
                                    Weapon = weapon,
                                    State = WeaponSwitchState.Equipped,
                                    WorldTime = GameState.CurrentWorldTime
                                });

                                playerActor.CurrentWeapon = weapon;
                            }
                            else
                            {
                                //Pickaxe

                            }
                        }
                        else
                        {
                            //Ignore as it's most likely their pickaxe
                        }
                    }

                    break;
            }
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
            if(inventory.ReplayPawn > 0)
            {
                //Normal replays only have your inventory. Every time you die, there's a new player pawn.
                _inventories.TryAdd(channelId, inventory);
            }
        }

        //Only occur on main player's inventory or server replays
        internal void UpdateNetDeltaFortInventory(NetDeltaUpdate deltaUpdate)
        {
            if (!_inventories.TryGetValue(deltaUpdate.ChannelIndex, out FortInventory inventory) || 
               !_actorToChannel.TryGetValue(inventory.ReplayPawn.Value, out uint channel) || 
               !_playerPawns.TryGetValue(channel, out PlayerPawn playerPawn))
            {
                return;
            }

            if (!(playerPawn is Player player))
            {
                return;
            }


            if (deltaUpdate.Deleted)
            {
                //Cache inventory when deleting to hold on to inventory prior to death
                if (player.InventoryBaseReplicationKey != deltaUpdate.Header.BaseReplicationKey)
                {
                    player.InventoryBeforeDeletes.Clear();
                    player.InventoryBeforeDeletes.AddRange(player.CurrentInventory.Items);
                }

                player.CurrentInventory.DeleteIndex(deltaUpdate.ElementIndex);

                player.InventoryBaseReplicationKey = deltaUpdate.Header.BaseReplicationKey;
            }
            else
            {
                FortInventory fortInventory = deltaUpdate.Export as FortInventory;

                if (player.CurrentInventory.TryGetItem(deltaUpdate.ElementIndex, out InventoryItem item))
                {
                    item.LoadedAmmo = fortInventory.LoadedAmmo ?? item.LoadedAmmo;
                    item.Count = fortInventory.Count ?? item.Count;
                }
                else
                {
                    string itemName = String.Empty;

                    if(fortInventory.ItemDefinition != null)
                    {
                        NetGUIDToPathName.TryGetValue(fortInventory.ItemDefinition.Value, out itemName);
                    }

                    InventoryItem inventoryItem = new InventoryItem
                    {
                        Count = fortInventory.Count ?? 0,
                        ItemDefinition = fortInventory.ItemDefinition ?? 0,
                        Item = new ItemName(itemName),
                        LoadedAmmo = fortInventory.LoadedAmmo ?? 0,
                        UniqueWeaponId = new UniqueItemId
                        {
                            A = fortInventory.A ?? 0,
                            B = fortInventory.B ?? 0,
                            C = fortInventory.C ?? 0,
                            D = fortInventory.D ?? 0
                        }
                    };

                    player.CurrentInventory.TryAddItem(deltaUpdate.ElementIndex, inventoryItem);
                }
            }
        }

        internal void UpdateFortPickup(uint channelId, FortPickup pickup)
        {
            /*

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
            }*/
        }

        internal void HandleWeapon(uint channelId, BaseWeapon weapon)
        {
            bool isNewWeapon = !_weapons.TryGetValue(channelId, out Weapon newWeapon);

            if (isNewWeapon)
            {
                newWeapon = new Weapon();

                _weapons.TryAdd(channelId, newWeapon);
            }

            newWeapon.IsEquipping = weapon.bIsEquippingWeapon ?? newWeapon.IsEquipping;
            newWeapon.IsReloading = weapon.bIsReloadingWeapon ?? newWeapon.IsReloading;
            newWeapon.WeaponLevel = weapon.WeaponLevel ?? newWeapon.WeaponLevel;
            newWeapon.Ammo = weapon.AmmoCount ?? newWeapon.Ammo;

            if (weapon.A != null)
            {
                newWeapon.UniqueItemId = new UniqueItemId
                {
                    A = weapon.A ?? 0,
                    B = weapon.B ?? 0,
                    C = weapon.C ?? 0,
                    D = weapon.D ?? 0
                };
            }

            if (weapon.WeaponData != null)
            {
                NetGUIDToPathName.TryGetValue(weapon.WeaponData.Value, out string itemName);

                newWeapon.Item = new ItemName(itemName);
                newWeapon.ItemId = weapon.WeaponData.Value;
            }

            if (weapon.Owner != null)
            {
                if (_actorToChannel.TryGetValue(weapon.Owner.Value, out uint channel))
                {
                    if(_playerPawns.TryGetValue(channel, out PlayerPawn playerPawn))
                    {
                        Player player = playerPawn as Player;

                        newWeapon.Owner = player;

                        if(player.CurrentInventory.Count > 0)
                        {
                            InventoryItem inventoryItem = player.CurrentInventory.Items.FirstOrDefault(x => x.UniqueWeaponId.Equals(newWeapon.UniqueItemId));

                            if(inventoryItem != null)
                            {
                                inventoryItem.Weapon = newWeapon;
                                newWeapon.InventoryItem = inventoryItem;
                            }
                        }
                    }
                }
            }
        }

        internal void HandleDeltaNetRead(NetDeltaUpdate deltaUpdate)
        {
            if (_inventories.ContainsKey(deltaUpdate.ChannelIndex))
            {
                UpdateNetDeltaFortInventory(deltaUpdate);
            }
        }

        private void HandleQueuedPlayerPawns(Player player)
        {
            if(_queuedPlayerPawns.Remove(player.Actor.ActorNetGUID.Value, out var playerPawns))
            {
                foreach(QueuedPlayerPawn playerPawn in playerPawns)
                {
                    UpdatePlayerPawn(playerPawn.ChannelId, playerPawn.PlayerPawn, playerPawn.Actor);
                }
            }
        }
    }
}
