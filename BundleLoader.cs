using Il2CppInterop.Runtime;
using UnityEngine;

namespace RF5.HisaCat.NPCDetails;

internal static class BundleLoader
{
    internal static AssetBundle MainBundle { get; private set; }

    internal static bool LoadBundle()
    {
        if (MainBundle is not null)
        {
            BepInExLog.LogDebug("[BundleLoader] Bundle already loaded");
            return true;
        }
        
        var bundleDir = Path.Combine(BepInExLoader.GetPluginRootDirectory(), MyPluginInfo.PLUGIN_GUID);
        var mainBundlePath = Path.Combine(bundleDir, "npcdetails.main.unity3d");
        if (!File.Exists(mainBundlePath))
        {
            BepInExLog.LogError($"[BundleLoader] Bundle missing at \"{mainBundlePath}\"");
            return false;
        }

        MainBundle = AssetBundle.LoadFromFile(mainBundlePath);

        if (MainBundle is null)
        {
            BepInExLog.LogError($"[BundleLoader] Cannot load bundle at \"{mainBundlePath}\"");
            return false;
        }

        BepInExLog.LogInfo("[BundleLoader] Bundle loaded.");
        return true;
    }

    internal static T? LoadIL2CPP<T>(this AssetBundle bundle, string name) where T : UnityEngine.Object
    {
        var asset = bundle?.LoadAsset_Internal(name, Il2CppType.Of<T>());
        return asset?.TryCast<T>();
    }
}

