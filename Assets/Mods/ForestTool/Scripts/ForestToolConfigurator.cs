using Bindito.Core;

namespace Mods.ForestTool.Scripts {
    [Context("Game")]
    public class ForestToolConfigurator : IConfigurator
    {

        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ForestToolInitializer>().AsSingleton();
            containerDefinition.Bind<IForestTool>().To<ForestTool>().AsSingleton();
            containerDefinition.Bind<ForestToolConfigUIPanel>().AsSingleton();
            containerDefinition.Bind<ForestToolErrorUIPanel>().AsSingleton();
            containerDefinition.Bind<ForestTool>().AsSingleton();
        }
    }
}