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
        public string Manufacturer { get; set; } = string.Empty;

        public override string Serialize()
        {
            return $"{{\n  \"Id\": {Id},\n  \"Name\": \"{Name}\",\n  \"PowerLevel\": {PowerLevel},\n  \"Rarity\": \"{Rarity}\",\n  \"TechLevel\": {TechLevel},\n  \"Manufacturer\": \"{Manufacturer}\"\n}}";
        }
    }
}