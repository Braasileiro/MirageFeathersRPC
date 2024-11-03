using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using MirageFeathersRPC.Patches;
using MirageFeathersRPC.Managers;

namespace MirageFeathersRPC
{
    [BepInPlugin(MyPluginInfoEx.PLUGIN_GUID, MyPluginInfoEx.PLUGIN_NAME, MyPluginInfoEx.PLUGIN_VERSION)]
	[BepInProcess(MyPluginInfoEx.PLUGIN_PROCESS)]
	public class Plugin : BaseUnityPlugin
	{
		internal static Harmony HarmonyInstance;
		internal static new ManualLogSource Logger;
		internal static readonly long TimeElapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		public void Awake()
		{
			// Global logger
			Logger = base.Logger;

			Logger.LogInfo($"{MyPluginInfoEx.PLUGIN_GUID} loaded.");

			// Application settings
			Application.runInBackground = true;

			Logger.LogInfo("Run in background enabled.");

			// Patches
			HarmonyInstance = new(MyPluginInfoEx.PLUGIN_GUID);

			HarmonyInstance.PatchAll(typeof(GameStatePatch));

			Logger.LogInfo("Patches applied.");

			// Discord client
			if (!UnityDiscordManager.Init())
			{
				StartCoroutine(UnityDiscordManager.InitCoroutine());
			}
		}

		public void Update()
		{
			if (!UnityDiscordManager.RunCallbacks())
			{
				StartCoroutine(UnityDiscordManager.InitCoroutine());
			}
		}

		public void OnApplicationQuit()
		{
			// Dispose things here
			UnityDiscordManager.Dispose();
		}
	}
}
