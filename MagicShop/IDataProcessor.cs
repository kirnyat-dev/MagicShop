using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop
{
    public interface IDataProcessor<T>
    {
        List<T> LoadData(string source);
        void SaveData(List<T> data, string filePath);
    }
}