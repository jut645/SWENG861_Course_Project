using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FlightPrices.Skyscanner.WebAPI.Models;
using Newtonsoft.Json;

namespace FlightPrices.Skyscanner.WebAPI
{
    public class SkyscannerUrls
    {
        public readonly Dictionary<Type, string> Urls = new Dictionary<Type, string>();
        public readonly string SkyscannerBaseUrl;
        private List<Type> ValidTypes;

        public SkyscannerUrls(IConfiguration configuration)
        {
            SkyscannerBaseUrl = ImportBaseUrl(configuration);
            Urls =  ImportUrlConfiguration(configuration);
        }

        private string ImportBaseUrl(IConfiguration configuration)
        {
            return configuration.GetSection(nameof(SkyscannerBaseUrl)).Value;
        }

        private Dictionary<Type, string> ImportUrlConfiguration(IConfiguration configuration)
        {
            var urls = ImportUrlExtensions(configuration);
            var skyscannerModelTypes = ValidTypes = GetTypesInNamespaces("FlightPrices.Skyscanner.WebAPI.Responses");

            var typesToUrl = new Dictionary<Type, string>();
            foreach (var type in skyscannerModelTypes)
            {
                var url = urls[type.Name];
                typesToUrl[type] = url;
            }

            return typesToUrl;
        }

        private Dictionary<string, string> ImportUrlExtensions(IConfiguration configuration)
        {
            var urlJson = configuration.GetSection(nameof(SkyscannerUrls));
            var typesToUrls = new Dictionary<string, string>();
            foreach (var child in urlJson.GetChildren())
            {
                typesToUrls[child.Key] = child.Value;
            }

            return typesToUrls;

        }

        private List<Type> GetTypesInNamespaces(string nspace)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == nspace)
                .ToList();
        }
    }
}
