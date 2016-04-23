using System.Threading.Tasks;

namespace WinDev_Emotions.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}