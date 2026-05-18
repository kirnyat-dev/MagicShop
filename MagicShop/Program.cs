namespace MagicShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShopManager.CreateMockFiles();

            ShopManager Manager = new ShopManager();
            Manager.LoadAllData();

            Console.WriteLine("все загруженные артефакты в магазине:");
            foreach (var Art in Manager.AllArtifacts)
            {
                Console.WriteLine(Art);
            }

            Console.WriteLine("\nпоиск сильных проклятых легендарных артефактов (сила > 50):");
            var Cursed = Manager.FindCursedArtifacts();
            foreach (var Art in Cursed)
            {
                Console.WriteLine(Art);
            }

            Console.WriteLine("\nгруппировка артефактов по редкости с подсчетом количества:");
            var Groups = Manager.GroupByRarity();
            foreach (var Pair in Groups)
            {
                Console.WriteLine($"редкость: {Pair.Key} -> количество: {Pair.Value}");
            }

            Console.WriteLine("\nтоп-3 артефактов по силе:");
            var Top = Manager.TopByPower(3);
            foreach (var Art in Top)
            {
                Console.WriteLine(Art);
            }

            Manager.GenerateReport("report.txt");
            Console.WriteLine("\nотчет со средней силой успешно сгенерирован в 'report.txt'");

            ShopManager.CleanMockFiles();
            Console.ReadLine();
        }
    }
}