using Firestorm.Fluent.IntegrationTests.Models;

namespace Firestorm.Fluent.IntegrationTests.Contexts
{
    public class AutoRootFluentContext : ApiContext
    {
        public AutoRootFluentContext()
        {
        }

        public ApiRoot<Player> Players { get; set; }

        public ApiRoot<Team> Teams { get; set; }

        protected override void OnApiCreating(IApiBuilder apiBuilder)
        {
            apiBuilder.Item<Team>(i =>
            {
                i.Identifier(t => t.Name.Replace(" ", "").ToLower())
                    .HasName("key");

                i.Field(t => t.Players)
                    .IsCollection(b => {
                        b.Identifier(p => p.SquadNumber);
                        b.AutoConfigure(Options.RootConfiguration);
                    });
            });
        }
    }
}