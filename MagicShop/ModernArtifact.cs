using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class ModernArtifact : Artifact
    {
        public double TechLevel { get; set; }
        public string Manufacturer { get; set; }

        public override string Serialize() => $"json-формат: {ExportToJson()}";

        public override string ExportToJson()
        {
            return $"{{\"Id\":{Id},\"Name\":\"{Name}\",\"PowerLevel\":{PowerLevel},\"Rarity\":\"{Rarity}\",\"TechLevel\":{TechLevel.ToString(System.Globalization.CultureInfo.InvariantCulture)},\"Manufacturer\":\"{Manufacturer}\"}}";
        }

        public override string ExportToXml()
        {
            return $"<ModernArtifact><Id>{Id}</Id><Name>{Name}</Name><PowerLevel>{PowerLevel}</PowerLevel><Rarity>{Rarity}</Rarity><TechLevel>{TechLevel.ToString(System.Globalization.CultureInfo.InvariantCulture)}</TechLevel><Manufacturer>{Manufacturer}</Manufacturer></ModernArtifact>";
        }

        public override string ToString() => $"{base.ToString()} | тех.уровень: {TechLevel} | производитель: {Manufacturer}";
    }
}