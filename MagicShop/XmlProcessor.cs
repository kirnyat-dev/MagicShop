using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MagicShop
{
    public class XmlProcessor : IDataProcessor<AntiqueArtifact>
    {
        public List<AntiqueArtifact> LoadData(string filePath)
        {
            var list = new List<AntiqueArtifact>();
            if (!File.Exists(filePath)) return list;

            try
            {
                string content = File.ReadAllText(filePath);
                string[] itemBlocks = content.Split(new[] { "<AntiqueArtifact>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var block in itemBlocks)
                {
                    if (!block.Contains("</AntiqueArtifact>")) continue;

                    var artifact = new AntiqueArtifact();
                    artifact.Id = int.Parse(GetXmlValue(block, "Id"));
                    artifact.Name = GetXmlValue(block, "Name");
                    artifact.PowerLevel = int.Parse(GetXmlValue(block, "PowerLevel"));
                    artifact.Rarity = (Rarity)Enum.Parse(typeof(Rarity), GetXmlValue(block, "Rarity"), true);
                    artifact.Age = int.Parse(GetXmlValue(block, "Age"));
                    artifact.OriginRealm = GetXmlValue(block, "OriginRealm");

                    list.Add(artifact);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.Write($"ошибка парсинга XML: {ex.Message}\n");
                return list;
            }
        }

        public void SaveData(List<AntiqueArtifact> data, string filePath)
        {
            try
            {
                var lines = new List<string>
                {
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                    "<ArrayOfAntiqueArtifact>"
                };

                foreach (var item in data)
                {
                    lines.Add("  <AntiqueArtifact>");
                    lines.Add($"    <Id>{item.Id}</Id>");
                    lines.Add($"    <Name>{item.Name}</Name>");
                    lines.Add($"    <PowerLevel>{item.PowerLevel}</PowerLevel>");
                    lines.Add($"    <Rarity>{item.Rarity}</Rarity>");
                    lines.Add($"    <Age>{item.Age}</Age>");
                    lines.Add($"    <OriginRealm>{item.OriginRealm}</OriginRealm>");
                    lines.Add("  </AntiqueArtifact>");
                }

                lines.Add("</ArrayOfAntiqueArtifact>");
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                throw new IOException($"ошибка записи XML-файла {filePath}: {ex.Message}", ex);
            }
        }

        private string GetXmlValue(string block, string tag)
        {
            string startTag = $"<{tag}>";
            string endTag = $"</{tag}>";
            int start = block.IndexOf(startTag);
            if (start == -1) return string.Empty;
            start += startTag.Length;
            int end = block.IndexOf(endTag, start);
            if (end == -1) return string.Empty;
            return block.Substring(start, end - start).Trim();
        }
    }
}