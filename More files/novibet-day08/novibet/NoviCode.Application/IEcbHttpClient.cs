using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode
{
    public interface IEcbHttpClient
    {
        Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync(CancellationToken cancellationToken = default);

    }
}
