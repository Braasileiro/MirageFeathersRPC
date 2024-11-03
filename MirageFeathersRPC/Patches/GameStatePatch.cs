using HarmonyLib;
using MirageFeathersRPC.Managers;

namespace MirageFeathersRPC.Patches
{
    internal class GameStatePatch
	{
		[HarmonyPatch(typeof(MainMenu), "Awake")]
		[HarmonyPostfix]
		static void MainMenu_Awake()
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.MainMenu);
		}

		[HarmonyPatch(typeof(GalleryMenu), "Awake")]
		[HarmonyPostfix]
		static void GalleryMenu_Awake()
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.GalleryMenu);
		}

		[HarmonyPatch(typeof(GalleryUiControl), "CloseGallery")]
		[HarmonyPostfix]
		static void GalleryUiControl_CloseGallery()
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.MainMenu);
		}

		[HarmonyPatch(typeof(StageManager), "Awake")]
		[HarmonyPatch(typeof(StageManager), "StageChange")]
		[HarmonyPostfix]
		static void StageManager_Awake_StageChange(StageManager __instance)
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.Build(__instance));
		}
	}
}
