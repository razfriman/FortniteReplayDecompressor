using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports;
using Unreal.Core.Models;

namespace FortniteReplayReader.Models
{
    public class GameInformation
    {
        public ICollection<Llama> Llamas => _llamas.Values;
        public ICollection<SafeZone> SafeZones => _safeZones;


        private Dictionary<uint, Llama> _llamas = new Dictionary<uint, Llama>();
        private Dictionary<uint, SupplyDrop> _supplyDrops = new Dictionary<uint, SupplyDrop>();

        private List<SafeZone> _safeZones = new List<SafeZone>();

        public void UpdateLlama(uint channel, SupplyDropLlamaC supplyDropLlama)
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


        public void UpdateSupplyDrop(uint channel, SupplyDropC supplyDrop)
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

        public void UpdateSafeZone(SafeZoneIndicatorC safeZone)
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
    }
}
