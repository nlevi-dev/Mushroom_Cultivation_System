using System;
using System.Net.Http;

namespace SEP4_Data.Data
{
    public class SampleService : ISampleService
    {
        private readonly IConfigService _config;
        private readonly HttpClient _client;
        
        public SampleService(IConfigService config)
        {
            _config = config;
            _client = new HttpClient {BaseAddress = new Uri("http://localhost:8080/game_rental/")};
        }
    }
}