using UnityEngine;

namespace RF5.HisaCat.NPCDetails.Utils;

internal static class GameObjectExtension
{
    internal static bool TryFindComponent<T>(this Component self, string name, out T component, bool showErrLog = true) where T : Component
    {
        component = self.FindComponent<T>(name);
        if (showErrLog && component is null)
        {
            BepInExLog.LogError($"Cannot find \"{typeof(T).FullName}\" component at \"{name}\" from \"{self.name}\"");
        }

        return component is not null;
    }

    internal static bool TryFindComponent<T>(this GameObject self, string name, out T component, bool showErrLog = true) where T : Component
    {
        component = self.FindComponent<T>(name);
        if (showErrLog && component is null)
        {
            BepInExLog.LogError($"Cannot find \"{typeof(T).FullName}\" component at \"{name}\" from \"{self.name}\"");
        }

        return component is not null;
    }

    internal static bool TryFindGameObject(this Component self, string name, out GameObject gameObject, bool showErrLog = true)
    {
        gameObject = self.FindGameObject(name);
        if (showErrLog && gameObject is null)
        {
            BepInExLog.LogError($"Cannot find GameObject at \"{name}\" from \"{self.name}\"");
        }

        return gameObject is not null;
    }
    
    internal static bool TryFindGameObject(this GameObject self, string name, out GameObject gameObject, bool showErrLog = true)
    {
        gameObject = self.FindGameObject(name);
        if (showErrLog && gameObject is null)
        {
            BepInExLog.LogError($"Cannot find GameObject at \"{name}\" from \"{self.name}\"");
        }

        return gameObject is not null;
    }
}
