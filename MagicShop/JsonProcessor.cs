using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class JsonProcessor : IDataProcessor<ModernArtifact>
    {
        public List<ModernArtifact> LoadData(string source)
        {
            List<ModernArtifact> List = new List<ModernArtifact>();
            if (!File.Exists(source)) return List;

            string[] Lines = File.ReadAllLines(source);
            foreach (string Line in Lines)
            {
                string Trimmed = Line.Trim(' ', '[', ']', ',');
                if (string.IsNullOrEmpty(Trimmed)) continue;

                ModernArtifact Item = new ModernArtifact();
                Item.Id = int.Parse(ExtractJsonValue(Trimmed, "Id"));
                Item.Name = ExtractJsonValue(Trimmed, "Name");
                Item.PowerLevel = int.Parse(ExtractJsonValue(Trimmed, "PowerLevel"));
                Item.Rarity = Enum.Parse<Rarity>(ExtractJsonValue(Trimmed, "Rarity"));
                Item.TechLevel = double.Parse(ExtractJsonValue(Trimmed, "TechLevel"), System.Globalization.CultureInfo.InvariantCulture);
                Item.Manufacturer = ExtractJsonValue(Trimmed, "Manufacturer");
                List.Add(Item);
            }
            return List;
        }

        public void SaveData(string destination, List<ModernArtifact> data)
        {
            List<string> Lines = new List<string> { "[" };
            for (int i = 0; i < data.Count; i++)
            {
                string Suffix = i < data.Count - 1 ? "," : "";
                Lines.Add("  " + data[i].ExportToJson() + Suffix);
            }
            Lines.Add("]");
            File.WriteAllLines(destination, Lines);
        }

        private string ExtractJsonValue(string raw, string key)
        {
            string Pattern = $"\"{key}\":";
            int Index = raw.IndexOf(Pattern);
            if (Index == -1) return string.Empty;

            int Start = Index + Pattern.Length;
            while (Start < raw.Length && (raw[Start] == ' ' || raw[Start] == '"')) Start++;

            int End = Start;
            while (End < raw.Length && raw[End] != ',' && raw[End] != '}' && raw[End] != '"') End++;

            return raw.Substring(Start, End - Start);
        }
    }
}