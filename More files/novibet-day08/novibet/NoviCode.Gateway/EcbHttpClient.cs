using NoviCode;
using System.Xml.Serialization;

namespace NoviCode.Gateway
{

    public class EcbHttpClient : IEcbHttpClient
    {
        
        private readonly HttpClient _httpClient;

        public EcbHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var serializer = new XmlSerializer(typeof(Envelope));

            var responsedto = (Envelope) serializer.Deserialize(stream);

            var cube = responsedto.Cube.Cube1;
            var datetime = cube.time;

            var rates = cube.Cube
                .Select(x => new CurrencyRateDto(x.currency, x.rate, datetime))
                .ToArray();

            return rates;


           
            //Map the response Dto to the Currency Dto
            //return the Currency Dtos
        }
    }
}
