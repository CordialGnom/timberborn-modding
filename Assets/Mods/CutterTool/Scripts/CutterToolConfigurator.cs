using Bindito.Core;

namespace Mods.CutterTool.Scripts {
  [Context("Game")]
  public class CutterToolConfigurator : IConfigurator {

    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<CutterToolInitializer>().AsSingleton();
    }

  }
}