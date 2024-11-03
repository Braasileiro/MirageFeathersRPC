using Discord;

namespace MirageFeathersRPC.Managers
{
    internal class ActivityBuilder
    {
        public static readonly Activity Default = new()
        {
            Timestamps =
            {
                Start = Plugin.TimeElapsed
            },

            Assets =
            {
                LargeImage = "notari",
                LargeText = $"MFRPC {MyPluginInfoEx.PLUGIN_VERSION}"
            }
        };

        public static readonly Activity MainMenu = new()
        {
            State = "Main Menu",
            Timestamps = Default.Timestamps,
            Assets = Default.Assets
        };

        public static readonly Activity GalleryMenu = new()
        {
            State = "Gallery",
            Timestamps = Default.Timestamps,
            Assets = Default.Assets
        };

        public static Activity Build(StageManager stageManager, bool isPrologueEnd = false)
        {
            Activity activity = new()
            {
                State = GetState(stageManager, isPrologueEnd),
                Details = GetDetails(stageManager),
                Timestamps = Default.Timestamps,
                Assets = GetAssets(stageManager, isPrologueEnd)
            };

            return activity;
        }

        private static string GetState(StageManager stageManager, bool isPrologueEnd)
        {
			if (!stageManager.tutorial)
            {
                if (stageManager.realStage > 0 || stageManager.rogueMode)
                {
                    return $"STAGE {stageManager.realStage:000}";
                }
                else if (isPrologueEnd)
                {
                    return "STAGE 001";
                }
                else
                {
                    return "Prologue";
                }
			}

            return null;
        }

        private static string GetDetails(StageManager stageManager)
        {
            if (stageManager.tutorial)
            {
                return "Tutorial Mode";
            }
            else if (stageManager.rogueMode)
            {
                return "Endless Mode";
            }
            else
            {
                return "Story Mode";
            }
        }

        private static ActivityAssets GetAssets(StageManager stageManager, bool isPrologueEnd)
        {
            ActivityAssets assets = Default.Assets;

            if (!stageManager.tutorial)
            {
                if (isPrologueEnd || stageManager.realStage > 0 || stageManager.rogueMode)
                {
                    assets.SmallImage = "miroita";
                    assets.SmallText = $"Playing • {GetDifficulty(stageManager.gamePlaySettings.difficulty)}";
                }
            }

            return assets;
        }

        private static string GetDifficulty(int difficulty)
        {
            return difficulty switch
            {
                1 => "Easy",
                2 => "Medium",
                3 => "Hard",
                _ => "Unknown"
            };
        }
    }
}
