using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class JsonProcessor : IDataProcessor<ModernArtifact>
    {
        public List<ModernArtifact> LoadData(string filePath)
        {
            var list = new List<ModernArtifact>();
            if (!File.Exists(filePath)) return list;

            try
            {
                string content = File.ReadAllText(filePath);
                var blocks = content.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Where(b => b.Contains("\"Name\"") || b.Contains("Name"));

                foreach (var block in blocks)
                {
                    var artifact = new ModernArtifact();
                    var lines = block.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        if (!line.Contains(":")) continue;
                        var parts = line.Split(':');
                        string key = parts[0].Trim().Replace("\"", "").Replace("'", "");
                        string val = parts[1].Trim().Replace("\"", "").Replace("'", "");

                        switch (key)
                        {
                            case "Id": artifact.Id = int.Parse(val); break;
                            case "Name": artifact.Name = val; break;
                            case "PowerLevel": artifact.PowerLevel = int.Parse(val); break;
                            case "Rarity": artifact.Rarity = (Rarity)Enum.Parse(typeof(Rarity), val, true); break;
                            case "TechLevel": artifact.TechLevel = double.Parse(val, CultureInfo.InvariantCulture); break;
                            case "Manufacturer": artifact.Manufacturer = val; break;
                        }
                    }
                    list.Add(artifact);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.Write($"ошибка парсинга JSON: {ex.Message}\n");
                return list;
            }
        }

        public void SaveData(List<ModernArtifact> data, string filePath)
        {
            try
            {
                var lines = new List<string> { "[" };
                for (int i = 0; i < data.Count; i++)
                {
                    lines.Add("  {");
                    lines.Add($"    \"Id\": {data[i].Id},");
                    lines.Add($"    \"Name\": \"{data[i].Name}\",");
                    lines.Add($"    \"PowerLevel\": {data[i].PowerLevel},");
                    lines.Add($"    \"Rarity\": \"{data[i].Rarity}\",");
                    lines.Add($"    \"TechLevel\": {data[i].TechLevel.ToString(CultureInfo.InvariantCulture)},");
                    lines.Add($"    \"Manufacturer\": \"{data[i].Manufacturer}\"");
                    lines.Add(i == data.Count - 1 ? "  }" : "  },");
                }
                lines.Add("]");
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                throw new IOException($"ошибка записи JSON-файла {filePath}: {ex.Message}", ex);
            }
        }
    }
}