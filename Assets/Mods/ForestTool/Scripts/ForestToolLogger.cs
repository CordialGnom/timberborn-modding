using Timberborn.ModManagerScene;
using UnityEngine;

namespace Mods.ForestTool.Scripts {
  internal class ForestToolLogger : IModStarter {

    public void StartMod() {
      var playerLogPath = Application.persistentDataPath + "/Player.log";
      Debug.Log("ForestTool, but in the Player.log file at: " + playerLogPath);
    }

  }
}