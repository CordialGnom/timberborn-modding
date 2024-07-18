﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Timberborn.Buildings;
using Timberborn.Localization;
using Timberborn.Planting;
using Timberborn.PlantingUI;
using Timberborn.ScienceSystem;
using Timberborn.SelectionToolSystem;
using Timberborn.SingletonSystem;
using Timberborn.TerrainSystem;
using Timberborn.ToolSystem;
using TimberApi.Tools.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mods.ForestTool.Scripts
{
    public class ForestTool : Tool, ILoadableSingleton, IPlantingToolGroup, IForestTool
    {
        private static readonly string TitleLocKey = "Cordial.ForestTool.DisplayName";
        private static readonly string DescriptionLocKey = "Cordial.ForestTool.Description";
        private static readonly string CursorKey = "PlantingCursor";

        private static readonly string _defaultResource = "Pine";

        private readonly ToolManager _toolManager;
        private readonly ILoc _loc;
        private EventBus _eventBus;
        private static VisualElement _root;

        // area selection 
        private readonly SelectionToolProcessor _selectionToolProcessor;
        private readonly ToolUnlockingService _toolUnlockingService;

        // planting
        private PlantingAreaValidator _plantingAreaValidator;
        private readonly PlantingSelectionService _plantingSelectionService;
        private PlantingService _plantingService;
        private TerrainAreaService _terrainAreaService;

        // availability 
        private BuildingUnlockingService _buildingUnlockingService; 
        private BuildingService _buildingService;


        //private static readonly string[] _asResource = { "Oak", "Birch", "ChestnutTree", "Pine", "Maple" };

        // todo check how to access generic specifications, and beaver faction specifications
        //"C:\Users\simon\TreeMix\Assets\Timberborn\Resources\specifications\prefabcollections\PrefabCollectionSpecification.NaturalResources.naturalresources"

        // services and objects
        private readonly ToolButtonService _toolButtonService;  // todo check if required
        private ToolDescription _toolDescription;       // is used

        protected readonly MethodInfo EnterPlantingModeMethod;
        protected readonly MethodInfo ExitPlantingModeMethod;

        public ForestTool(SelectionToolProcessorFactory selectionToolProcessorFactory,
                            PlantingSelectionService plantingSelectionService,
                            PlantingAreaValidator plantingAreaValidator,
                            PlantingService plantingService,
                            TerrainAreaService terrainAreaService,
                            ToolUnlockingService toolUnlockingService,
                            ILoc loc,
                            ToolManager toolManager,
                            ToolButtonService toolButtonService,
                            EventBus eventBus )
        {

            _selectionToolProcessor =       selectionToolProcessorFactory.Create(   new Action<IEnumerable<Vector3Int>,
                                                                                    Ray>(this.PreviewCallback),
                                                                                    new Action<IEnumerable<Vector3Int>,
                                                                                    Ray>(this.ActionCallback),
                                                                                    new Action(ShowNoneCallback),
                                                                                   CursorKey );

            _plantingAreaValidator =        plantingAreaValidator;
            _plantingService =              plantingService;
            _plantingSelectionService =     plantingSelectionService;
            _terrainAreaService =           terrainAreaService;
            _toolUnlockingService =         toolUnlockingService;

            _eventBus = eventBus;
            _loc = loc;
            _toolManager = toolManager;
            _toolButtonService = toolButtonService;
            _root = new VisualElement();

            EnterPlantingModeMethod = typeof(PlantingModeService).GetMethod("EnterPlantingMode", BindingFlags.NonPublic | BindingFlags.Instance);
            ExitPlantingModeMethod = typeof(PlantingModeService).GetMethod("ExitPlantingMode", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public void Load()
        {
            _toolDescription = new ToolDescription.Builder(_loc.T(TitleLocKey)).AddSection(_loc.T(DescriptionLocKey)).Build();
            _buildingUnlockingService = ForestToolDependencyContainer.GetInstance<BuildingUnlockingService>();
            _buildingService = ForestToolDependencyContainer.GetInstance<BuildingService>();


            // did not achieve natural resource access through timberapi
            // ForestToolSpecificationService.GetAll();
            // ForestToolSpecificationService.GetDefaultTreeNames();

            // only call parameter init once
            if (false == ForestToolParam.ParamInitDone)
            {
                ForestToolParam.UpdateFromConfig();
            }
        }

        public void SetToolGroup(ToolGroup toolGroup)
        {
            ToolGroup = toolGroup;
        }


        public override ToolDescription Description() => _toolDescription;

        public override void Enter()
        {
            // check if tool can be entered (forester available)
            // require access to either "Forester" or the "Trees". Therefore check if
            // the trees can be planted...

            // get faction (forester specific building)
            string factionName =        ForestToolFactionAccess.GetFactionName();
            string prefabName =         "";

            if ("" != factionName)
            {
                prefabName = "Forester." + factionName;
                Debug.Log(prefabName);
            }
            else
            {
                Debug.Log("Faction not found");
            }

            // create a forester: 
            Building _forester = _buildingService.GetBuildingPrefab(prefabName);

            if (null != _forester)
            {
                Debug.Log("Forester Built");
            }

            IsUnlocked = _buildingUnlockingService.Unlocked(_forester);

            if (true == IsUnlocked)
            {
                // activate tool
                this._selectionToolProcessor.Enter();
            }

            Debug.Log("Lock: " + IsUnlocked);

//[Info: Tree Tool] Forester.Folktails
//[Info: Tree Tool] Forester Built
//[Info   : Tree Tool] Lock: True

            // hook for UI
            EnterTool();
        }

        public VisualElement EnterTool()
        {
            return _root;
        }    

        public bool IsUnlocked { get; set; }

        public override void Exit()
        {
            this._plantingSelectionService.UnhighlightAll();
            this._selectionToolProcessor.Exit();
        }

        // Preview and Action Callbacks required for selection Tool Processor Factor
        private void PreviewCallback(IEnumerable<Vector3Int> inputBlocks, Ray ray)
        {
            string resourceName = GetRandomPlantableName();

            // workaround so that highlighting doesn't toggle at a high cycle, 
            // add preview of empty spots as pine trees. available to both default factions
            if ("" == resourceName)
            {
                resourceName = _defaultResource;
            }

            this._plantingSelectionService.HighlightMarkableArea(inputBlocks, ray, resourceName);
        }

        private void ActionCallback(IEnumerable<Vector3Int> inputBlocks, Ray ray)
        {
            if (this.Locker != null)
                this._toolUnlockingService.TryToUnlock((Tool)this);
            else
                this.Plant(inputBlocks, ray);
        }
        private static void ShowNoneCallback()
        {
        }

        private void Plant(IEnumerable<Vector3Int> inputBlocks, Ray ray)        // based on PlantingTool function
        {
            string resourceName; ;
            Boolean boCanPlant;
            Vector2Int v2coordinate;

            // here randomize plant name for each entry in list

            foreach (Vector3Int leveledCoordinate in this._terrainAreaService.InMapLeveledCoordinates(inputBlocks, ray))
            {
                resourceName = GetRandomPlantableName();

                if (ForestToolParam.NameEmpty == resourceName)
                {
                    v2coordinate = new Vector2Int(leveledCoordinate.x, leveledCoordinate.y);
                    boCanPlant = true; //_plantingService.IsResourceAt(v2coordinate);

                    // leave empty spot in forest
                    if (true == boCanPlant)
                    {
                        _plantingService.UnsetPlantingCoordinates(leveledCoordinate);
                    }
                }
                else
                {
                    boCanPlant = _plantingAreaValidator.CanPlant(leveledCoordinate, resourceName);
                                  
                    if (true == boCanPlant)
                    {
                        _plantingService.SetPlantingCoordinates(leveledCoordinate, resourceName);
                    }
                }
            }
            _eventBus.Post((object)new PlantingAreaMarkedEvent());
        }

        private string GetRandomPlantableName()
        {
            return ForestToolParam.GetNextRandomResourceName();
        }

        public void PostProcessInput()      // originally virtual (to be called elsewhere)
        {
            // currently no implementation, just placeholder
            return;
        }
    }
}