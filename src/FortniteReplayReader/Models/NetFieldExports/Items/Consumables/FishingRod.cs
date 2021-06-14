using System;
using System.Collections.Generic;
using System.Text;
using FortniteReplayReader.Models.NetFieldExports.Items.Weapons;
using Unreal.Core.Attributes;
using Unreal.Core.Models;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Items.Consumables
{
    [NetFieldExportGroup("/Game/Athena/Items/Consumables/FloppingRabbit/B_FloppingRabbit_Weap_Athena.B_FloppingRabbit_Weap_Athena_C", ParseType.Normal)]
    public class FishingRod : Consumable
    {
        [NetFieldExport("Projectile", RepLayoutCmdType.Property)]
        public NetworkGUID Projectile { get; set; }

        [NetFieldExport("Wire", RepLayoutCmdType.Property)]
        public NetworkGUID Wire { get; set; }

        [NetFieldExport("HideBobber", RepLayoutCmdType.PropertyBool)]
        public bool? HideBobber { get; set; }

        [NetFieldExport("OneHandGrip", RepLayoutCmdType.PropertyBool)]
        public bool? OneHandGrip { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "Projectile":
					Projectile = (NetworkGUID)value;
					break;
				case "Wire":
					Wire = (NetworkGUID)value;
					break;
				case "HideBobber":
					HideBobber = (bool)value;
					break;
				case "OneHandGrip":
					OneHandGrip = (bool)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/FloppingRabbit/B_Athena_FloppingRabbit_Wire.B_Athena_FloppingRabbit_Wire_C", ParseType.Normal)]
    public class FishingRodWire : Consumable
    {
        [NetFieldExport("ReplicatedMovement", RepLayoutCmdType.RepMovementWholeNumber)]
        public FRepMovement ReplicatedMovement { get; set; }

        [NetFieldExport("Projectile", RepLayoutCmdType.Property)]
        public NetworkGUID Projectile { get; set; }

        [NetFieldExport("Projectile Actor", RepLayoutCmdType.Property)]
        public NetworkGUID ProjectileActor { get; set; }

        [NetFieldExport("PlayerPawn", RepLayoutCmdType.Property)]
        public NetworkGUID PlayerPawn { get; set; }

        [NetFieldExport("CatchParticleOn", RepLayoutCmdType.PropertyBool)]
        public bool? CatchParticleOn { get; set; }

        [NetFieldExport("Weapon", RepLayoutCmdType.Property)]
        public NetworkGUID Weapon { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "ReplicatedMovement":
					ReplicatedMovement = (FRepMovement)value;
					break;
				case "Projectile":
					Projectile = (NetworkGUID)value;
					break;
				case "ProjectileActor":
					ProjectileActor = (NetworkGUID)value;
					break;
				case "PlayerPawn":
					PlayerPawn = (NetworkGUID)value;
					break;
				case "CatchParticleOn":
					CatchParticleOn = (bool)value;
					break;
				case "Weapon":
					Weapon = (NetworkGUID)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }

    [NetFieldExportGroup("/Game/Athena/Items/Consumables/HappyGhost/B_HappyGhost_Athena.B_HappyGhost_Athena_C", ParseType.Normal)]
    public class Harpoon : BaseWeapon
    {
        [NetFieldExport("HideProj", RepLayoutCmdType.PropertyBool)]
        public bool? HideProj { get; set; }

		public override bool ManualRead(string property, object value)
		{
			switch(property)
			{
				case "HideProj":
					HideProj = (bool)value;
					break;
				default:
					return base.ManualRead(property, value);
			}

			return true;
		}

    }
}
