using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MagicShop
{
    [XmlInclude(typeof(AntiqueArtifact))]
    public abstract class Artifact : IExportable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PowerLevel { get; set; }
        public Rarity Rarity { get; set; }

        public abstract string Serialize();

        public virtual string ExportToJson()
        {
            return $"{{\"Id\":{Id},\"Name\":\"{Name}\",\"PowerLevel\":{PowerLevel},\"Rarity\":\"{Rarity}\"}}";
        }

        public virtual string ExportToXml()
        {
            return $"<Artifact><Id>{Id}</Id><Name>{Name}</Name><PowerLevel>{PowerLevel}</PowerLevel><Rarity>{Rarity}</Rarity></Artifact>";
        }

        public override string ToString() => $"id: {Id} | имя: {Name} | сила: {PowerLevel} | редкость: {Rarity}";
    }
}