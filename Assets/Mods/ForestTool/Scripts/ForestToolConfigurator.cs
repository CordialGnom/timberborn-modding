using Bindito.Core;
using HarmonyLib;
using TimberApi.Tools.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mods.ForestTool.Scripts {
    [Context("Game")]
    public class ForestToolConfigurator : IConfigurator
    {

        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ForestToolInitializer>().AsSingleton();
            containerDefinition.Bind<IForestTool>().To<ForestTool>().AsSingleton();
            //containerDefinition.Bind<ForestToolErrorUIPanel>().AsSingleton();
            containerDefinition.Bind<ForestTool>().AsSingleton();
            containerDefinition.MultiBind<IToolFactory>().To<ForestToolFactory>().AsSingleton();
        }

        [HarmonyPatch(typeof(ForestTool), "EnterTool")]
        public static class ForestToolEnterPatch
        {
            private static void Postfix(ref VisualElement __result)
            {
                //ForestToolPanel _treePanel = DependencyContainer.GetInstance<ForestToolPanel>();
                //ForestToolErrorUIPanel _errorUIPanel = DependencyContainer.GetInstance<ForestToolErrorUIPanel>();
                //ForestTool _ForestTool = DependencyContainer.GetInstance<ForestTool>();
                //VisualElement root = __result;

                Debug.Log("Forest Tool Entered");

                //if (true == _treePanel.GetUIEnabledByKey())
                //{
                //    root.Insert(0, _treePanel.GetPanel());
                //    _treePanel.OnUIConfirmed();
                //}
                //else if (false == _ForestTool.IsUnlocked)
                //{
                //    // open error panel

                //    root.Insert(0, _errorUIPanel.GetPanel());
                //    _errorUIPanel.OnUIConfirmed();
                //}
                //else
                //{
                //    // tool can be used, do nothing here
                //}
            }
        }
    }
}