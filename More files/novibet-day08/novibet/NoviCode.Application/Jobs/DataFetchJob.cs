using MediatR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NoviCode.Jobs
{
    [DisallowConcurrentExecution]
    public class DataFetchJob : IJob
    {

        private readonly IEcbHttpClient _client;

        private readonly ISender _sender;

        public DataFetchJob(IEcbHttpClient client, ISender sender)
        {
            _client = client;
            _sender = sender;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            var dtoresults = await _client.GetLatestRatesAsync(context.CancellationToken);

            //get rates from ecb
            //command to save data on db
        }
    }
}
