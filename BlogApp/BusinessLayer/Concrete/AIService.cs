using BlogApp.BusinessLayer.Abstract;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogApp.BusinessLayer.Concrete
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetTagsFromContentAsync(string content)
        {
            var requestBody = new
            {
                inputs = content
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("models/facebook/bart-large-cnn", httpContent);

            if (!response.IsSuccessStatusCode)
                return new List<string> { "etiket", "yok" };

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            var summary = doc.RootElement[0].GetProperty("summary_text").GetString();

            return summary?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct().Take(5).ToList() ?? new List<string>();
        }
    }
}
