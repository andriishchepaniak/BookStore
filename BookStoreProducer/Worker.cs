using BooksScrapping;
using BookStoreModels;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStoreProducer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly BooksScrapper _booksScrapper;
        private readonly BookClubSettings _settings;
        public Worker(ILogger<Worker> logger, IHost host, IOptions<BookClubSettings> options)
        {
            _logger = logger;
            _host = host;
            _settings = options.Value;
            _booksScrapper = new BooksScrapper();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
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
