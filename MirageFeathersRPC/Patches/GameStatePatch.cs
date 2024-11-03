using HarmonyLib;
using MirageFeathersRPC.Managers;

namespace MirageFeathersRPC.Patches
{
    internal class GameStatePatch
	{
		[HarmonyPatch(typeof(MainMenu), "Awake")]
		[HarmonyPostfix]
		static void OnMainMenu()
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.MainMenu);
		}

		[HarmonyPatch(typeof(GalleryMenu), "Awake")]
		[HarmonyPostfix]
		static void OnGalleryMenu()
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.GalleryMenu);
		}

		[HarmonyPatch(typeof(GalleryUiControl), "CloseGallery")]
		[HarmonyPostfix]
		static void OnCloseGallery()
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.MainMenu);
		}

		[HarmonyPatch(typeof(StageManager), "Awake")]
		[HarmonyPatch(typeof(StageManager), "StageChange")]
		[HarmonyPostfix]
		static void OnStageChange(StageManager __instance)
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.Build(__instance));
		}

		[HarmonyPatch(typeof(GameplayManager), "PrologueEnd")]
		[HarmonyPostfix]
		static void OnPrologueEnd(GameplayManager __instance)
		{
			UnityDiscordManager.UpdateActivity(ActivityBuilder.Build(__instance.stageManager, isPrologueEnd: true));
		}
	}
}
