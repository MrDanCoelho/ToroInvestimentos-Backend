using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ToroInvestimentos.Backend.Domain;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;
using ToroInvestimentos.Backend.Domain.Interfaces.IAdapters;

namespace ToroInvestimentos.Backend.Infra.Adapters
{
    /// <inheritdoc/>
    public class BrokerAdapter : IBrokerAdapter
    {
        private readonly ILogger<BrokerAdapter> _logger;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public BrokerAdapter(ILogger<BrokerAdapter> logger, AppSettings appSettings, HttpClient httpClient)
        {
            _logger = logger;
            _appSettings = appSettings;
            _httpClient = httpClient;
        }

        public async Task<List<QuoteDto>> GetRecommendation()
        {
            HttpRequestMessage requestMessage = null;
            
            try
            {
                requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_appSettings.Broker.GetRecommendationUrl),
                    Headers =
                    {
                        { "x-rapidapi-key", _appSettings.Broker.ApiKey }
                    },
                };
                
                var response = await _httpClient.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Server responded with {response.StatusCode} Status Code");

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BrokerDto>(content);

                if (result == null)
                    throw new HttpRequestException($"Server responded with unexpected response: {content}");
                
                if (!string.IsNullOrEmpty(result.Finance.Error))
                    throw new HttpRequestException(result.Finance.Error);

                return result.Finance.Result.Quotes;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get stock recommendations with request={@RequestMessage}", requestMessage);
                throw;
            }
        }
    }
}