using System.Collections.Immutable;
using Timberborn.FactionSystem;
using Timberborn.GameFactionSystem;
using UnityEngine;


namespace Mods.ForestTool.Scripts
{
    public static class ForestToolFactionAccess
    {

        // to update configuration based on active faction
        private static FactionSpecificationService _factionSpecService;
        private static FactionService _factionService;
        private static FactionSpecification _factionSpecification;
        private static string _factionId;

        public static string GetFactionName()
        {

            _factionService = ForestToolDependencyContainer.GetInstance<FactionService>();

            _factionId = "";
            FactionSpecification _activeFaction;

            if (null != _factionService)
            {
                _activeFaction = _factionService.Current;
                _factionId = _activeFaction.Id;
            }

            return _factionId;
        }

        public static ImmutableArray<string> GetFactionTrees()
        {
            //    ImmutableArray<string> uniqueNaturalResources;
            ImmutableArray<string> prefabGroups;
            ImmutableArray<string> treeTypes;

            _factionId = GetFactionName();
            _factionSpecService = ForestToolDependencyContainer.GetInstance<FactionSpecificationService>();

            if (null != _factionSpecService)
            {
                _factionSpecification = _factionSpecService.GetFaction(_factionId);

                prefabGroups = _factionSpecification.PrefabGroups;

                foreach (string prefabGroup in prefabGroups)
                {
                    Debug.Log("PrefabGroup: " + prefabGroup);
                }

                /*
                                uniqueNaturalResources = _factionSpecification.PrefabGroups;

                                foreach (string resource in uniqueNaturalResources)
                                {
                                    string search = "Trees";
                                    int pos = resource.IndexOf(search);

                                    if (0 < pos)
                                    {
                                        // shorten whole string
                                        string temp = resource.Substring(pos + search.Length).Trim();
                                        string[] parts = temp.Split('/');
                                        treeTypes.Add(parts[parts.Length - 1]);
                                    }
                                    else
                                    {
                                        // ignore entry
                                    }

                                }
                */
                // returns more resources than trees: 
                //                  [Info: Tree Tool] Found: NaturalResources / Bushes / Dandelion / Dandelion
                //                  [Info: Tree Tool] Found: NaturalResources / Crops / Carrot / Carrot
                //                  [Info: Tree Tool] Found: NaturalResources / Crops / Cattail / Cattail
                //                  [Info: Tree Tool] Found: NaturalResources / Crops / Potato / Potato
                //                  [Info: Tree Tool] Found: NaturalResources / Crops / Spadderdock / Spadderdock
                //                  [Info: Tree Tool] Found: NaturalResources / Crops / Sunflower / Sunflower
                //                  [Info: Tree Tool] Found: NaturalResources / Crops / Wheat / Wheat
                //                  [Info: Tree Tool] Found: NaturalResources / Trees / ChestnutTree / ChestnutTree
                //                  [Info: Tree Tool] Found: NaturalResources / Trees / Maple / Maple
            }
            else
            {
                Debug.Log("Faciton Spec service not available");
            }
            return treeTypes;
        }
    }
}
