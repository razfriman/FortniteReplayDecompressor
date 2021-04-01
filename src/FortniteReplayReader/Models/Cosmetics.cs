using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteReplayReader.Models
{
    public class Cosmetics
    {
        public int? CharacterGender { get; set; }
        public int? CharacterBodyType { get; set; }
        public string Parts { get; set; }
        public IEnumerable<string> VariantRequiredCharacterParts { get; set; }
        public string HeroType { get; set; }
        public string BannerIconId { get; set; }
        public string BannerColorId { get; set; }
        public IEnumerable<string> ItemWraps { get; set; }
        public string SkyDiveContrail { get; set; }
        public string Glider { get; set; }
        public string Pickaxe { get; set; }
        public bool IsDefaultCharacter { get; set; }
        public string Character { get; set; }
        public string Backpack { get; set; }
        public string LoadingScreen { get; set; }
        public IEnumerable<string> Dances { get; set; }
        public string MusicPack { get; set; }
        public string PetSkin { get; set; }
    }
}
