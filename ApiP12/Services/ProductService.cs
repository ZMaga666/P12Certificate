using ApiP12.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ApiP12.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("P12ApiClient");
            _apiUrl = configuration["ApiSettings:BaseUrl"]; 
        }

        public async Task<List<Product>> GetAllasync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/product/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch(Exception ex)
            {
            }
                return new List<Product>();
            
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task CreateProductAsync(Product product)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_apiUrl}/product", jsonContent);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/product/{product.Id}", jsonContent);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _httpClient.DeleteAsync($"{_apiUrl}/product/{id}");
        }
    }
}
