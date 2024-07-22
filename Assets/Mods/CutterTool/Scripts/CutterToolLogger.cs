using HarmonyLib;
using Timberborn.ModManagerScene;
using UnityEngine;

namespace Mods.CutterTool.Scripts {
  internal class CutterToolLogger : IModStarter {

    public void StartMod()
    {
        new Harmony("Cordial.CutterTool").PatchAll();
        var playerLogPath = Application.persistentDataPath + "/Player.log";
        Debug.Log("Cutter Tool, but in the Player.log file at: " + playerLogPath);
    }

  }
}