using Firestorm.Endpoints.Responses.Pagination;

namespace Firestorm.Endpoints.Responses
{
    /// <summary>
    /// The options used to build the responses to return to the client.
    /// </summary>
    /// <remarks>
    /// Response builders are built in the <see cref="Firestorm.Endpoints.Responses"/> library, but this config object needs to be in the overall <see cref="EndpointConfiguration"/>,
    /// so these settings feel like they're defined at the wrong level.
    /// </remarks>
    public class ResponseConfiguration
    {
        /// <summary>
        /// Set to true to enable details exception messages in the error responses.
        /// This should not be used in production.
        /// </summary>
        public bool ShowDeveloperErrors { get; set; } = false;

        /// <summary>
        /// Behaviour of an additional field added to response bodies to indicate status.
        /// </summary>
        public ResponseStatusField StatusField { get; set; }

        /// <summary>
        /// If a <see cref="StatusField"/> is defined, this flag configures resource objects to also be wrapped in an object with the status field.
        /// </summary>
        public bool WrapResourceObject { get; set; }

        /// <summary>
        /// The system-wide configuration defining how pagination can work.
        /// </summary>
        public PaginationConfiguration Pagination { get; set; } = new PaginationConfiguration();

        /// <summary>
        /// If true, a successful command request (e.g. POST or PUT) will respond with the current state of the resource
        /// </summary>
        public bool ResourceOnSuccessfulCommand { get; set; } = false;
    }
}