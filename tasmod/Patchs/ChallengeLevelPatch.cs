using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patchs
{
    public class ChallengeLevelPatch
    {
        private static bool PrefixUpdate(ChallengeLevel __instance)
        {
            Level level = __instance as Level;
            if (((int)level.GetMemberValue("_updateWaitFrames")) <= 0 && __instance.levelIsUpdating && updater.frameadvance && !updater.advancing)
            {
                PausingLogic(__instance);
                return false;
            }
            return true;
        }
        public static void PausingLogic(ChallengeLevel __instance)
        {
            if (Input.Pressed(Triggers.Start)/* || (updater.PlayerOneBackup != null && updater.PlayerOneBackup.Pressed(Triggers.Start))*/)
            {
                UIComponent _pauseGroup = ((UIComponent)__instance.GetMemberValue("_pauseGroup"));
                UIComponent _pauseMenu = ((UIComponent)__instance.GetMemberValue("_pauseMenu"));
                _pauseGroup.Open();
                _pauseMenu.Open();
                MonoMain.pauseMenu = _pauseGroup;
                if (!((bool)__instance.GetMemberValue("_paused")))
                {
                    SFX.Play("pause", 0.6f);
                    ChallengeLevel.timer.Stop();
                    __instance.SetMemberValue("_paused", true);
                }
                __instance.simulatePhysics = false;
            }
            else if ((bool)__instance.GetMemberValue("_paused") && MonoMain.pauseMenu == null)
            {
                __instance.SetMemberValue("_paused", false);
                SFX.Play("resume", 0.6f);
                __instance.SetMemberValue("_waitAfterSpawn", 1f);
                __instance.SetMemberValue("_waitAfterSpawnDings", 0);
                __instance.SetMemberValue("_started", false);
                __instance.SetMemberValue("_fontFade", 1f);
                if (((MenuBoolean)__instance.GetMemberValue("_restart")).value)
                {
                    __instance.SetMemberValue("_restarting", true);
                }
                else if (((MenuBoolean)__instance.GetMemberValue("_quit")).value)
                {
                    __instance.SetMemberValue("_fading", true);
                }
            }
        }
        private static bool PrefixPausingLogic(ChallengeLevel __instance)
        {
            PausingLogic(__instance);
            return false;
        }
    }
}
