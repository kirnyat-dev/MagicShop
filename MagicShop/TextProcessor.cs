using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class TextProcessor : IDataProcessor<LegendaryArtifact>
    {
        private static int _idCounter = 1000;

        public List<LegendaryArtifact> LoadData(string filePath)
        {
            var list = new List<LegendaryArtifact>();
            if (!File.Exists(filePath)) return list;

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split('|');
                    if (parts.Length < 5)
                        throw new FormatException($"некорректный формат строки: '{line}'");

                    var artifact = new LegendaryArtifact
                    {
                        Id = ++_idCounter,
                        Name = parts[0].Trim(),
                        PowerLevel = int.Parse(parts[1].Trim()),
                        Rarity = (Rarity)Enum.Parse(typeof(Rarity), parts[2].Trim(), true),
                        CurseDescription = parts[3].Trim(),
                        IsCursed = bool.Parse(parts[4].Trim())
                    };
                    list.Add(artifact);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new IOException($"ошибка чтения текстового файла {filePath}: {ex.Message}", ex);
            }
        }

        public void SaveData(List<LegendaryArtifact> data, string filePath)
        {
            try
            {
                var lines = data.Select(x => x.Serialize());
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                throw new IOException($"ошибка записи текстового файла {filePath}: {ex.Message}", ex);
            }
        }
    }
}