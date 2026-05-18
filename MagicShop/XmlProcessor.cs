using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class XmlProcessor : IDataProcessor<AntiqueArtifact>
    {
        public List<AntiqueArtifact> LoadData(string source)
        {
            List<AntiqueArtifact> List = new List<AntiqueArtifact>();
            if (!File.Exists(source)) return List;

            string[] Lines = File.ReadAllLines(source);
            foreach (string Line in Lines)
            {
                if (Line.Contains("<AntiqueArtifact>"))
                {
                    AntiqueArtifact Item = new AntiqueArtifact();
                    Item.Id = int.Parse(ExtractXmlValue(Line, "Id"));
                    Item.Name = ExtractXmlValue(Line, "Name");
                    Item.PowerLevel = int.Parse(ExtractXmlValue(Line, "PowerLevel"));
                    Item.Rarity = Enum.Parse<Rarity>(ExtractXmlValue(Line, "Rarity"));
                    Item.Age = int.Parse(ExtractXmlValue(Line, "Age"));
                    Item.OriginRealm = ExtractXmlValue(Line, "OriginRealm");
                    List.Add(Item);
                }
            }
            return List;
        }

        public void SaveData(string destination, List<AntiqueArtifact> data)
        {
            List<string> Lines = new List<string> { "<ArrayOfAntiqueArtifact>" };
            foreach (var Item in data)
            {
                Lines.Add(Item.ExportToXml());
            }
            Lines.Add("</ArrayOfAntiqueArtifact>");
            File.WriteAllLines(destination, Lines);
        }

        private string ExtractXmlValue(string raw, string tag)
        {
            string StartTag = $"<{tag}>";
            string EndTag = $"</{tag}>";
            int StartIndex = raw.IndexOf(StartTag) + StartTag.Length;
            int EndIndex = raw.IndexOf(EndTag);
            return raw.Substring(StartIndex, EndIndex - StartIndex);
        }
    }
}