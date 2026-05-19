using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class LegendaryArtifact : Artifact
    {
        public string CurseDescription { get; set; } = string.Empty;
        public bool IsCursed { get; set; }

        public override string Serialize()
        {
            return $"{Name}|{PowerLevel}|{Rarity}|{CurseDescription}|{IsCursed}";
        }
    }
}