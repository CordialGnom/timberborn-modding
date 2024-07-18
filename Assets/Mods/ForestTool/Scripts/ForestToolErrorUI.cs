using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.CoreUI;
using Timberborn.UILayoutSystem;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using UnityEditor;
using UnityEngine.UIElements;

namespace Mods.ForestTool.Scripts
{
    public class ForestToolErrorUIPanel : IPanelController
    {
        private readonly PanelStack _panelStack;
        private VisualElement _root;


        public ForestToolErrorUIPanel( PanelStack panelStack )
        {
            this._panelStack = panelStack;

            this._root = new VisualElement();
        }

        public VisualElement GetPanel()
        {
            // get required panel
            ForestToolInitializer uiInitializer =  ForestToolDependencyContainer.GetInstance<ForestToolInitializer>();

            this._root = uiInitializer.GetErrorUiElement();

            this._root.Q<Button>("Mods.Cordial.ForestTool.ButtonErrorConfirm").clicked += OnUICancelled;

            return _root;
        }

        public bool OnUIConfirmed()
        {
            this._panelStack.HideAndPushOverlay((IPanelController)this);
            return false;
        }
        public void OnUICancelled()
        {
            this._panelStack.Pop((IPanelController)this);
        }


    }
}
