using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class TextProcessor : IDataProcessor<LegendaryArtifact>
    {
        public List<LegendaryArtifact> LoadData(string source)
        {
            List<LegendaryArtifact> List = new List<LegendaryArtifact>();
            if (!File.Exists(source)) return List;

            string[] Lines = File.ReadAllLines(source);
            foreach (string Line in Lines)
            {
                if (string.IsNullOrWhiteSpace(Line)) continue;

                string[] Parts = Line.Split('|').Select(p => p.Trim()).ToArray();
                if (Parts.Length < 6) continue;

                LegendaryArtifact Item = new LegendaryArtifact();
                Item.Id = int.Parse(Parts[0]);
                Item.Name = Parts[1];
                Item.PowerLevel = int.Parse(Parts[2]);
                Item.Rarity = Enum.Parse<Rarity>(Parts[3]);
                Item.CurseDescription = Parts[4];
                Item.IsCursed = bool.Parse(Parts[5]);
                List.Add(Item);
            }
            return List;
        }

        public void SaveData(string destination, List<LegendaryArtifact> data)
        {
            List<string> Lines = data.Select(Item => $"{Item.Id} | {Item.Name} | {Item.PowerLevel} | {Item.Rarity} | {Item.CurseDescription} | {Item.IsCursed}").ToList();
            File.WriteAllLines(destination, Lines);
        }
    }
}