using System;
using Discord;
using UnityEngine;
using System.Threading;
using System.Collections;

namespace MirageFeathersRPC.Managers
{
    internal class UnityDiscordManager
    {
        // RPC
        private static Discord.Discord _client;
        private static Activity _activity = ActivityBuilder.Default;

        // States
        private static bool _notified = false;
        private static bool _initCoroutine = false;
        private static CancellationTokenSource _cancellationToken;


        public static bool Init()
        {
            try
            {
                // New client instance
                _client?.Dispose();
                _client = null;
                _client = new Discord.Discord(Constants.DiscordClientId, (ulong)CreateFlags.NoRequireDiscord);
                _client.GetActivityManager().RegisterSteam(Constants.SteamAppId);

                Plugin.Logger.LogInfo("Discord RPC Client is listening...");

				// Default or last activity
				UpdateActivity(_activity);

				// Reset notified state
				_notified = false;

                return true;
            }
            catch (ResultException)
            {
                if (!_notified)
                {
                    _notified = true;

                    Plugin.Logger.LogInfo("Waiting for Discord...");
                }
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError($"Unhandled exception: {e.Message}");

                Dispose();
            }

            return false;
        }

        public static IEnumerator InitCoroutine()
        {
            // Init coroutine is running
            _initCoroutine = true;

            // New cancellation token instance
            _cancellationToken = new();

            while (!_cancellationToken.IsCancellationRequested)
            {
                if (Init())
                {
                    _initCoroutine = false;

                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(5);
                }
            }
        }

        public static bool RunCallbacks()
        {
            if (!_initCoroutine)
            {
                try
                {
                    _client?.RunCallbacks();
                }
                catch (ResultException)
                {
                    return false;
                }
            }

            return true;
        }

        public static void UpdateActivity(Activity activity)
        {
            _activity = activity;

            _client?.GetActivityManager().UpdateActivity(activity, null);
        }

        public static void Dispose()
        {
            _cancellationToken?.Dispose();

            _client?.GetActivityManager().ClearActivity(null);
            _client?.Dispose();
            _client = null;

            Plugin.Logger.LogInfo("Discord Manager disposed.");
        }
    }
}
