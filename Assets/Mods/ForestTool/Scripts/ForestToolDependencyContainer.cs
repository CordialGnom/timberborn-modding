using System;
using System.Collections.Generic;
using Bindito.Core;

namespace Mods.ForestTool.Scripts
{
    public static class ForestToolDependencyContainer
    {
        internal static IContainer Container = null!;

        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }

        public static object GetInstance(Type type)
        {
            return Container.GetInstance(type);
        }

        public static IEnumerable<object> GetBoundInstances()
        {
            return Container.GetBoundInstances();
        }
    }
}