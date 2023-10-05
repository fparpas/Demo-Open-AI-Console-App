using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoOpenAI.Services
{
    // This class is used to read the configuration from the appsettings.json file
    internal class AzureConfiguration
    {
        private IConfiguration _config = null;
        public AzureConfiguration()
        {
            var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);

            if (isDevelopment)
            {
                _config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Development.json", true, true)
                    .Build();
            }
            else
            {
                _config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            }

            AOAIEndpoint = _config["Azure:AOAIEndpoint"];
            AOAIKey = _config["Azure:AOAIKey"];
            AOAIDeploymentId = _config["Azure:AOAIDeploymentId"];
            AOAIEndpointGPT4 = _config["Azure:AOAIEndpointGPT4"];
            AOAIKeyGPT4 = _config["Azure:AOAIKeyGPT4"];
            AOAIDeploymentIdGPT4 = _config["Azure:AOAIDeploymentIdGPT4"];
            SearchEndpoint = _config["Azure:SearchEndpoint"];
            SearchKey = _config["Azure:SearchKey"];
            SearchIndexProducts = _config["Azure:SearchIndexProducts"];
            SearchIndexSupport = _config["Azure:SearchIndexSupport"];
            SearchIndexSupportVector = _config["Azure:SearchIndexSupportVector"];
            SearchSemanticConfiguration = _config["Azure:SearchSemanticConfiguration"];
            AOAIEmbeddingsEndpointGPT4 = _config["Azure:AOAIEmbeddingsEndpointGPT4"];
        }
        public string AOAIEndpoint { get; set; }
        public string AOAIKey { get; set; }
        public string AOAIDeploymentId { get; set; }

        public string AOAIEndpointGPT4 { get; set; }
        public string AOAIKeyGPT4 { get; set; }
        public string AOAIDeploymentIdGPT4 { get; set; }
        public string SearchEndpoint { get; set; }
        public string SearchKey { get; set; }
        public string SearchIndexProducts { get; set; }
        public string SearchIndexSupport { get; set; }
        public string SearchIndexSupportVector { get; set; }
        public string SearchSemanticConfiguration { get; set; }

        public string AOAIEmbeddingsEndpointGPT4 { get; set; }

    }
}
