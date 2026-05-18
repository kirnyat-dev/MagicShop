using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class AntiqueArtifact : Artifact
    {
        public int Age { get; set; }
        public string OriginRealm { get; set; }

        public override string Serialize() => $"xml-формат: {ExportToXml()}";

        public override string ExportToJson()
        {
            return $"{{\"Id\":{Id},\"Name\":\"{Name}\",\"PowerLevel\":{PowerLevel},\"Rarity\":\"{Rarity}\",\"Age\":{Age},\"OriginRealm\":\"{OriginRealm}\"}}";
        }

        public override string ExportToXml()
        {
            return $"<AntiqueArtifact><Id>{Id}</Id><Name>{Name}</Name><PowerLevel>{PowerLevel}</PowerLevel><Rarity>{Rarity}</Rarity><Age>{Age}</Age><OriginRealm>{OriginRealm}</OriginRealm></AntiqueArtifact>";
        }

        public override string ToString() => $"{base.ToString()} | возраст: {Age} | мир: {OriginRealm}";
    }
}