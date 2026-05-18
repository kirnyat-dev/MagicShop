using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class LegendaryArtifact : Artifact
    {
        public string CurseDescription { get; set; }
        public bool IsCursed { get; set; }

        public override string Serialize() => $"txt-строка: {Name} | {PowerLevel} | {Rarity} | {CurseDescription} | {IsCursed}";

        public override string ExportToJson()
        {
            return $"{{\"Id\":{Id},\"Name\":\"{Name}\",\"PowerLevel\":{PowerLevel}\",\"Rarity\":\"{Rarity}\",\"CurseDescription\":\"{CurseDescription}\",\"IsCursed\":{IsCursed.ToString().ToLower()}}}";
        }

        public override string ExportToXml()
        {
            return $"<LegendaryArtifact><Id>{Id}</Id><Name>{Name}</Name><PowerLevel>{PowerLevel}</PowerLevel><Rarity>{Rarity}</Rarity><CurseDescription>{CurseDescription}</CurseDescription><IsCursed>{IsCursed.ToString().ToLower()}</IsCursed></LegendaryArtifact>";
        }

        public override string ToString() => $"{base.ToString()} | проклятие: {CurseDescription} | проклят: {IsCursed}";
    }
}