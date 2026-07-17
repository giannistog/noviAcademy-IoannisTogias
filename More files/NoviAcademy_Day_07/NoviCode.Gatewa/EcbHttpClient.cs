
using Gateway;
using System.Xml.Serialization;
namespace NoviCode.Gateway;

public class EcbHttpClient : IEcbHttpClient
{
    private readonly HttpClient httpClient;
public async Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync(CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var serializer = new XmlSerializer(typeof(ResponseDto));
        var responsedto = (ResponseDto)serializer.Deserialize(stream); 
        var cube=Responsedto.Cube.
    }
}