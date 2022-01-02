using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace valheim_orbital_camera
{
    [BepInPlugin("de.torbilicious", "valheim-orbital-camera", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class OrbitalCameraMod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("de.torbilicious.valheim-orbital-camera");
        private static ConfigEntry<float> newMaxCameraDistance;
        private static bool patchPending = true;

        void Awake()
        {
            newMaxCameraDistance = Config.Bind("valheim-orbital-camera", "newMaxCameraDistance", 16.0f, "Max Camera Distance To Patch");
            newMaxCameraDistance.SettingChanged += (sender, args) => patchPending = true;
            
            harmony.PatchAll();
        }
        
        [HarmonyPatch(typeof(GameCamera), "LateUpdate")]
        class Camera_Patch
        {
            static void Prefix(ref float ___m_maxDistance)
            {
                if (patchPending)
                {
                    Debug.Log($"Previous max Camera Distance: {___m_maxDistance}");
                    ___m_maxDistance = newMaxCameraDistance.Value;
                    Debug.Log($"Patched max Camera Distance:  {___m_maxDistance}");

                    patchPending = false;
                }
            }
        }
    }
}
