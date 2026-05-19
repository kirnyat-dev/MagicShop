using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public class AppEngine
    {
        private ShopManager _shopManager = new ShopManager();
        private string _reportPath = "shop_report.txt";

        public void Run()
        {
            CreateExampleFiles();
            LoadAllData();

            bool exit = false;
            while (!exit)
            {
                Console.Write("\nдоступные команды:\n" +
                              "1. вывести весь список артефактов\n" +
                              "2. добавить новый артефакт в систему\n" +
                              "3. поиск проклятых артефактов (Сила > 50)\n" +
                              "4. показать группировку по редкости\n" +
                              "5. вывести топ сильных артефактов\n" +
                              "6. записать статистический отчет в файл\n" +
                              "7. экспорт базы в XML\n" +
                              "8. экспорт базы в JSON\n" +
                              "9. завершить работу\n" +
                              "ваш выбор: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ShowAllArtifacts(); break;
                    case "2": AddNewArtifactInteractive(); break;
                    case "3": ShowCursedArtifacts(); break;
                    case "4": ShowGroupedByRarity(); break;
                    case "5": ShowTopArtifacts(); break;
                    case "6": _shopManager.GenerateReport(_reportPath); break;
                    case "7": ExportData(true); break;
                    case "8": ExportData(false); break;
                    case "9":
                        new XmlProcessor().SaveData(_shopManager.Artifacts.OfType<AntiqueArtifact>().ToList(), "antique.xml");
                        new JsonProcessor().SaveData(_shopManager.Artifacts.OfType<ModernArtifact>().ToList(), "modern.json");
                        new TextProcessor().SaveData(_shopManager.Artifacts.OfType<LegendaryArtifact>().ToList(), "legends.txt");
                        exit = true; break;
                    default: Console.Write("неправильный ввод\n"); break;
                }
                Console.WriteLine("нажмите чтобы продолжить...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void LoadAllData()
        {
            Console.Write("инициализация импорта данных\n");
            _shopManager.LoadAllData(new XmlProcessor(), "antique.xml");
            _shopManager.LoadAllData(new JsonProcessor(), "modern.json");
            _shopManager.LoadAllData(new TextProcessor(), "legends.txt");
            Console.Write($"всего объектов: {_shopManager.Artifacts.Count}\n");
        }

        private void ShowAllArtifacts()
        {
            Console.Write("\nтекущий ассортимент магазина\n");
            foreach (var art in _shopManager.Artifacts)
            {
                string info = $"ID: {art.Id} | {art.Name} (сила: {art.PowerLevel}, [{art.Rarity}])";
                if (art is AntiqueArtifact antique) info += $" | эпоха: {antique.Age} лет | измерение: {antique.OriginRealm}";
                else if (art is ModernArtifact modern) info += $" | техн. уровень: {modern.TechLevel} | создатель: {modern.Manufacturer}";
                else if (art is LegendaryArtifact legendary) info += $" | проклятие: {legendary.IsCursed} ({legendary.CurseDescription})";

                Console.Write(info + "\n");
            }
        }

        private void AddNewArtifactInteractive()
        {
            Console.Write("\nсоздание нового артефакта\n");
            Console.Write("выберите тип артефакта:\n1. Antique (Старинный)\n2. Modern (Современный)\n3. Legendary (Легендарный)\nВыбор: ");
            string typeChoice = Console.ReadLine();

            if (typeChoice != "1" && typeChoice != "2" && typeChoice != "3")
            {
                Console.Write("неверный выбор типа\n");
                return;
            }

            Console.Write("введите название: ");
            string name = Console.ReadLine();

            Console.Write("введите уровень силы (целое число): ");
            if (!int.TryParse(Console.ReadLine(), out int power))
            {
                Console.Write("неверный формат силы\n");
                return;
            }

            Console.Write("выберите редкость (0 - Common, 1 - Rare, 2 - Epic, 3 - Legendary): ");
            if (!int.TryParse(Console.ReadLine(), out int rarityVal) || rarityVal < 0 || rarityVal > 3)
            {
                Console.Write("некорректная редкость\n");
                return;
            }
            Rarity rarity = (Rarity)rarityVal;

            int nextId = _shopManager.Artifacts.Count > 0 ? _shopManager.Artifacts.Max(a => a.Id) + 1 : 1;

            if (typeChoice == "1")
            {
                Console.Write("введите возраст артефакта (лет): ");
                if (!int.TryParse(Console.ReadLine(), out int age)) age = 100;

                Console.Write("введите родное измерение: ");
                string realm = Console.ReadLine();

                var antique = new AntiqueArtifact { Id = nextId, Name = name, PowerLevel = power, Rarity = rarity, Age = age, OriginRealm = realm };
                _shopManager.Artifacts.Add(antique);
                Console.Write("старинный артефакт успешно добавлен в оперативную память\n");
            }
            else if (typeChoice == "2")
            {
                Console.Write("введите технический уровень (дробное число через точку): ");
                if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double tech)) tech = 1.0;

                Console.Write("введите производителя: ");
                string manufacturer = Console.ReadLine();

                var modern = new ModernArtifact { Id = nextId, Name = name, PowerLevel = power, Rarity = rarity, TechLevel = tech, Manufacturer = manufacturer };
                _shopManager.Artifacts.Add(modern);
                Console.Write("современный артефакт успешно добавлен в оперативную память\n");
            }
            else if (typeChoice == "3")
            {
                Console.Write("опишите проклятие: ");
                string curse = Console.ReadLine();

                Console.Write("он проклят? (true/false): ");
                if (!bool.TryParse(Console.ReadLine(), out bool isCursed)) isCursed = false;

                var legendary = new LegendaryArtifact { Id = nextId, Name = name, PowerLevel = power, Rarity = rarity, CurseDescription = curse, IsCursed = isCursed };
                _shopManager.Artifacts.Add(legendary);
                Console.Write("легендарный артефакт успешно добавлен в оперативную память\n");
            }
        }

        private void ShowCursedArtifacts()
        {
            Console.Write("\nнайденные опасные объекты\n");
            var cursed = _shopManager.FindCursedArtifacts();
            if (cursed.Count == 0) Console.Write("проклятых высокоуровневых артефактов не найдено.\n");
            foreach (var art in cursed)
            {
                var la = (LegendaryArtifact)art;
                Console.Write($"ID: {la.Id} | {la.Name} | описание угрозы: {la.CurseDescription}\n");
            }
        }

        private void ShowGroupedByRarity()
        {
            Console.Write("\nколичество по катеориям редкости\n");
            var groups = _shopManager.GroupByRarity();
            foreach (var kvp in groups)
            {
                Console.Write($"категория {kvp.Key}: {kvp.Value} шт.\n");
            }
        }

        private void ShowTopArtifacts()
        {
            Console.Write("\nвведите размерность топа: ");
            if (int.TryParse(Console.ReadLine(), out int count))
            {
                Console.Write($"\nтоп {count} наиболее мощных артефактов\n");
                var top = _shopManager.TopByPower(count);
                foreach (var art in top)
                {
                    Console.Write($" * {art.Name} [сила: {art.PowerLevel}]\n");
                }
            }
            else
            {
                Console.Write("введено некорректное число\n");
            }
        }

        private void ExportData(bool toXml)
        {
            var container = new ExportContainer { Artifacts = _shopManager.Artifacts };
            try
            {
                if (toXml)
                {
                    string xml = container.ExportToXml();
                    File.WriteAllText("exported_artifacts.xml", xml);
                    Console.Write("выгрузка в exported_artifacts.xml завершена\n");
                }
                else
                {
                    string json = container.ExportToJson();
                    File.WriteAllText("exported_artifacts.json", json);
                    Console.Write("выгрузка в exported_artifacts.json завершена\n");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"ошибка при экспорте: {ex.Message}\n");
            }
        }

        private void CreateExampleFiles()
        {
            if (!File.Exists("antique.xml"))
            {
                var antiques = new List<AntiqueArtifact>
        {
            new AntiqueArtifact { Id = 1, Name = "Amulet of Yendor", PowerLevel = 95, Rarity = Rarity.Legendary, Age = 1200, OriginRealm = "Arcadia" }
        };
                new XmlProcessor().SaveData(antiques, "antique.xml");
            }

            if (!File.Exists("modern.json"))
            {
                var moderns = new List<ModernArtifact>
        {
            new ModernArtifact { Id = 2, Name = "Hyper Phase Blaster", PowerLevel = 88, Rarity = Rarity.Epic, TechLevel = 9.5, Manufacturer = "TechMage Inc." }
        };
                new JsonProcessor().SaveData(moderns, "modern.json");
            }

            if (!File.Exists("legends.txt"))
            {
                var legends = new List<LegendaryArtifact>
        {
            new LegendaryArtifact { Id = 3, Name = "Sword of Destiny", PowerLevel = 100, Rarity = Rarity.Legendary, CurseDescription = "Drains life from the wielder", IsCursed = true }
        };
                new TextProcessor().SaveData(legends, "legends.txt");
            }
        }
    }
}
