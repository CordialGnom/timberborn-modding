using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timberborn.Localization;
using Timberborn.SelectionToolSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using UnityEngine;

namespace Mods.CutterTool.Scripts
{
    public class CutterToolService : Tool, ILoadableSingleton, ICutterTool
    {
        // tool descriptions
        private static readonly string TitleLocKey = "Cordial.CutterTool.DisplayName";
        private static readonly string DescriptionLocKey = "Cordial.CutterTool.Description";
        private static readonly string CursorKey = "PlantingCursor";

        // tool setup
        private readonly ILoc _loc;
        private readonly ToolButtonService _toolButtonService;  // todo check if required
        private ToolDescription _toolDescription;               // is used
        private readonly ToolUnlockingService _toolUnlockingService;
        private readonly SelectionToolProcessor _selectionToolProcessor;



        public CutterToolService(   SelectionToolProcessorFactory selectionToolProcessorFactory,
                                    ToolUnlockingService toolUnlockingService ) 
        {
            _selectionToolProcessor = selectionToolProcessorFactory.Create(new Action<IEnumerable<Vector3Int>,
                                                                                    Ray>(this.PreviewCallback),
                                                                                    new Action<IEnumerable<Vector3Int>,
                                                                                    Ray>(this.ActionCallback),
                                                                                    new Action(ShowNoneCallback),
                                                                                   CursorKey);
            _toolUnlockingService = toolUnlockingService;

        }

        public void Load()
        {
            _toolDescription = new ToolDescription.Builder(_loc.T(TitleLocKey)).AddSection(_loc.T(DescriptionLocKey)).Build();

        }
        public override void Enter()
        {
            // activate tool
            this._selectionToolProcessor.Enter();
        }
        public override void Exit()
        {
            this._selectionToolProcessor.Exit();
        }


        public void SetToolGroup(ToolGroup toolGroup)
        {
            ToolGroup = toolGroup;
        }
        public override ToolDescription Description() => _toolDescription;

        public void PostProcessInput()      // originally virtual (to be called elsewhere)
        {
            // currently no implementation, just placeholder
            return;
        }
        private void PreviewCallback(IEnumerable<Vector3Int> inputBlocks, Ray ray)
        {
            // apply preview -> e.g. cutter area highlighting
        }

        private void ActionCallback(IEnumerable<Vector3Int> inputBlocks, Ray ray)
        {
            if (this.Locker != null)
            {
                this._toolUnlockingService.TryToUnlock((Tool)this);
            }
            else
            {
                // execute code -> call cutter function
            }

        }
        private static void ShowNoneCallback()
        {
        }

    }
}
