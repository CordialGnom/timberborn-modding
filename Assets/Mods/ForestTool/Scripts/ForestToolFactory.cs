using Timberborn.ToolSystem;

namespace Mods.ForestTool.Scripts
{
    public class ForestToolFactory
    {
        private readonly IForestTool _ForestTool;
        public string Id => "ForestTool";
        public ForestToolFactory(IForestTool ForestTool)
        {
            _ForestTool = ForestTool;
        }

        public Tool Create(ToolGroup toolGroup = null)
        {
            // somehow create a new tool, loading the according specs

            _ForestTool.SetToolGroup(toolGroup);
            return (Tool)_ForestTool;
        }
    }
}
