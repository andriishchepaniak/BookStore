using BooksScrapping;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreProducer
{
    [DisallowConcurrentExecution]
    public class ParseAndSendDataJob : IJob
    {
        private readonly BooksScrapper _booksScrapper;
        private readonly BookClubSettings _settings;
        public ParseAndSendDataJob(IOptions<BookClubSettings> options)
        {
            _settings = options.Value;
            _booksScrapper = new BooksScrapper();
        }
        public Task Execute(IJobExecutionContext context)
        {
            foreach (var category in _settings.Categories)
            {
                var data = _booksScrapper.GetDataFromUrl(_settings.BaseUrl + category);
                Producer.SendObjectMessage(data);
            }

            _booksScrapper.QuitDriver();
            return Task.CompletedTask;
        }
    }
}
