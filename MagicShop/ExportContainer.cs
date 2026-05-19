using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MagicShop
{
    public class ExportContainer : IExportable
    {
        public List<Artifact> Artifacts { get; set; } = new List<Artifact>();

        public string ExportToJson()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 0; i < Artifacts.Count; i++)
            {
                sb.AppendLine("  {");
                sb.AppendLine($"    \"Id\": {Artifacts[i].Id},");
                sb.AppendLine($"    \"Name\": \"{Artifacts[i].Name}\",");
                sb.AppendLine($"    \"PowerLevel\": {Artifacts[i].PowerLevel},");
                sb.AppendLine($"    \"Rarity\": \"{Artifacts[i].Rarity}\"");
                sb.Append(i == Artifacts.Count - 1 ? "  }" : "  },");
                sb.AppendLine();
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        public string ExportToXml()
        {
            Type[] extraTypes = new Type[]
            {
        typeof(AntiqueArtifact),
        typeof(ModernArtifact),
        typeof(LegendaryArtifact)
            };

            var serializer = new XmlSerializer(typeof(List<Artifact>), extraTypes);
            var writer = new StringWriter();
            serializer.Serialize(writer, Artifacts);
            return writer.ToString();
        }
    }
}