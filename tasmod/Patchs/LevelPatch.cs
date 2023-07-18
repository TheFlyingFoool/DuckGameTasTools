using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patchs
{
    public class LevelPatch
    {
        private static bool PrefixUpdateThings(Level __instance)
        {
            if (((int)__instance.GetMemberValue("_updateWaitFrames")) <= 0 && __instance.levelIsUpdating && updater.frameadvance && updater.AllowFrameAdvance && (!updater.advancing))
            {
                return false;
            }
            return true;
        }
        private static bool PrefixPostUpdate(Level __instance)
        {
            if (((int)__instance.GetMemberValue("_updateWaitFrames")) <= 0 && __instance.levelIsUpdating && updater.frameadvance && updater.AllowFrameAdvance && (!updater.advancing))
            {
                return false;
            }
            return true;
        }
    }
}
