using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.BusinessLayer.Abstract
{
    public interface IAIService
    {
        Task<List<string>> GetTagsFromContentAsync(string content);
    }
}
