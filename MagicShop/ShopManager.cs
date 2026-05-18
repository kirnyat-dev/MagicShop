using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class ShopManager
    {
        private List<Artifact> _artifacts = new List<Artifact>();

        public List<Artifact> AllArtifacts => _artifacts;

        public void LoadAllData()
        {
            XmlProcessor XmlProc = new XmlProcessor();
            var AntiqueItems = XmlProc.LoadData("antique.xml");
            _artifacts.AddRange(AntiqueItems);

            JsonProcessor JsonProc = new JsonProcessor();
            var ModernItems = JsonProc.LoadData("modern.json");
            _artifacts.AddRange(ModernItems);

            TextProcessor TxtProc = new TextProcessor();
            var LegendaryItems = TxtProc.LoadData("legends.txt");
            _artifacts.AddRange(LegendaryItems);
        }

        public List<LegendaryArtifact> FindCursedArtifacts()
        {
            return _artifacts
                .OfType<LegendaryArtifact>()
                .Where(a => a.IsCursed && a.PowerLevel > 50)
                .ToList();
        }

        public Dictionary<Rarity, int> GroupByRarity()
        {
            return _artifacts
                .GroupBy(a => a.Rarity)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<Artifact> TopByPower(int count)
        {
            return _artifacts
                .OrderByDescending(a => a.PowerLevel)
                .Take(count)
                .ToList();
        }

        public void GenerateReport(string destination)
        {
            var GroupedStats = _artifacts
                .GroupBy(a => a.Rarity)
                .Select(g => $"редкость: {g.Key} | средняя сила: {g.Average(a => a.PowerLevel):F2}");

            File.WriteAllLines(destination, GroupedStats);
        }

        public static void CreateMockFiles()
        {
            List<string> XmlData = new List<string>
            {
                "<ArrayOfAntiqueArtifact>",
                "  <AntiqueArtifact><Id>1</Id><Name>амулет йендора</Name><PowerLevel>95</PowerLevel><Rarity>Legendary</Rarity><Age>1200</Age><OriginRealm>аркадия</OriginRealm></AntiqueArtifact>",
                "  <AntiqueArtifact><Id>2</Id><Name>древний щит</Name><PowerLevel>45</PowerLevel><Rarity>Rare</Rarity><Age>600</Age><OriginRealm>атлантида</OriginRealm></AntiqueArtifact>",
                "</ArrayOfAntiqueArtifact>"
            };
            File.WriteAllLines("antique.xml", XmlData);

            List<string> JsonData = new List<string>
            {
                "[",
                "  {\"Id\":3,\"Name\":\"гипербластер\",\"PowerLevel\":88,\"Rarity\":\"Epic\",\"TechLevel\":9.5,\"Manufacturer\":\"техномаги\"},",
                "  {\"Id\":4,\"Name\":\"лазерный клинок\",\"PowerLevel\":55,\"Rarity\":\"Rare\",\"TechLevel\":4.2,\"Manufacturer\":\"киберкорп\"}",
                "]"
            };
            File.WriteAllLines("modern.json", JsonData);

            List<string> TxtData = new List<string>
            {
                "5 | проклятый меч | 72 | Legendary | велодранская ярость | True",
                "6 | кольцо всевластия | 99 | Legendary | слепота владельца | True",
                "7 | обычный кинжал | 12 | Common | отсутствует | False"
            };
            File.WriteAllLines("legends.txt", TxtData);
        }

        public static void CleanMockFiles()
        {
            if (File.Exists("antique.xml")) File.Delete("antique.xml");
            if (File.Exists("modern.json")) File.Delete("modern.json");
            if (File.Exists("legends.txt")) File.Delete("legends.txt");
        }
    }
}