using System.Threading.Tasks;
using Firestorm.Host.Infrastructure;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;

namespace Firestorm.Endpoints
{
    internal class RestDirectoryEndpoint : IRestEndpoint
    {
        internal RestDirectoryEndpoint(IEndpointContext endpointContext, IRestDirectory directory)
        {
            Context = endpointContext;
            Directory = directory;
        }

        private IEndpointContext Context { get; }

        private IRestDirectory Directory { get; }

        public IRestEndpoint Next(INextPath resourceName)
        {
            IRestResource resource = Directory.GetChild(resourceName.GetCoded());
            if (resource == null)
                return null;

            return Context.Configuration.Resolver.GetFromResource(Context, resource);
        }

        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            RestDirectoryInfo directory = await Directory.GetInfoAsync();
            return new DirectoryBody(directory, Context.Configuration.NamingConventionSwitcher.ConvertCodedToDefault);
        }

        public async Task<Options> OptionsAsync()
        {
            var options = new Options();

            RestDirectoryInfo info = await Directory.GetInfoAsync();

            options.SubResources.AddRange(info.Resources);

            return options;
        }

        public Task<Feedback> UnsafeAsync(UnsafeMethod method, ResourceBody body)
        {
            throw new MethodNotAllowedException("Only safe methods are allowed on the directory.");
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}