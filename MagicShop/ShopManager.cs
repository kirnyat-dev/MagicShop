using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class ShopManager
    {        
        public List<Artifact> Artifacts { get; } = new List<Artifact>();

        public void LoadAllData<T>(IDataProcessor<T> processor, string filePath) where T : Artifact
        {
            try
            {
                List<T> loaded = processor.LoadData(filePath);
                Artifacts.AddRange(loaded);
                Console.Write($"успешно загружено элементов: {loaded.Count} из {Path.GetFileName(filePath)}.\n");
            }
            catch (Exception ex)
            {
                Console.Write($"ошибка при обработке файла {filePath}: {ex.Message}\n");
            }
        }

        public void GenerateReport(string filePath)
        {
            try
            {
                var stats = Artifacts
                    .GroupBy(a => a.Rarity)
                    .Select(g => new {
                        Rarity = g.Key,
                        Count = g.Count(),
                        AvgPower = g.Average(a => a.PowerLevel)
                    })
                    .OrderBy(s => s.Rarity);

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"отчет магазинеа артефактов ({DateTime.Now})");
                    writer.WriteLine($"всего товаров на складе: {Artifacts.Count}");
                    writer.WriteLine($"{"Редкость",-15} | {"Количество",-10} | {"Средняя Сила",-12}");

                    foreach (var stat in stats)
                    {
                        writer.WriteLine($"{stat.Rarity,-15} | {stat.Count,-10} | {stat.AvgPower,-12:F2}");
                    }
                }
                Console.Write($"отчет сохранен: {filePath}\n");
            }
            catch (Exception ex)
            {
                Console.Write($"не удалось выгрузить отчет: {ex.Message}\n");
            }
        }

        public List<Artifact> FindCursedArtifacts()
        {
            return Artifacts
                .OfType<LegendaryArtifact>()
                .Where(la => la.IsCursed && la.PowerLevel > 50)
                .Cast<Artifact>()
                .ToList();
        }

        public Dictionary<Rarity, int> GroupByRarity()
        {
            return Artifacts
                .GroupBy(a => a.Rarity)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<Artifact> TopByPower(int count)
        {
            return Artifacts
                .OrderByDescending(a => a.PowerLevel)
                .Take(count)
                .ToList();
        }
    }
}