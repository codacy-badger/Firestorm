using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// The options and services required to setup a Firestorm REST API server.
    /// Describes how to interact with the resources in this API.
    /// </summary>
    public class EndpointConfiguration
    {
        /// <summary>
        /// The configuration used to build the <see cref="QueryStringCollectionQuery"/> from a requested query string.
        /// </summary>
        public QueryStringConfiguration QueryString { get; set; } = new QueryStringConfiguration();

        /// <summary>
        /// The options used to build the responses to return to the client.
        /// </summary>
        public ResponseConfiguration Response { get; set; } = new ResponseConfiguration();

        /// <summary>
        /// The options used to configure the URL paths. 
        /// </summary>
        public UrlConfiguration Url { get; set; } = new UrlConfiguration();

        /// <summary>
        /// The options used to configure field and URL naming conventions used in the requests and responses.
        /// </summary>
        public NamingConventionConfiguration NamingConventions { get; set; } = new NamingConventionConfiguration();
    }
}