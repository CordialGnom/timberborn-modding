using Timberborn.CoreUI;
using Timberborn.SingletonSystem;
using Timberborn.UILayoutSystem;

namespace Mods.CutterTool.Scripts {
  public class CutterToolInitializer : ILoadableSingleton {

    private readonly UILayout _uiLayout;
    private readonly VisualElementLoader _visualElementLoader;

    public CutterToolInitializer(UILayout uiLayout, 
                                 VisualElementLoader visualElementLoader) {
      _uiLayout = uiLayout;
      _visualElementLoader = visualElementLoader;
    }

    public void Load() {
      var visualElement = _visualElementLoader.LoadVisualElement("CutterTool");
      _uiLayout.AddBottomLeft(visualElement, 0);
    }

  }
}