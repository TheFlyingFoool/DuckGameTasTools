using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patchs
{
    public class AutoUpdatablesPatch
    {
        private static bool PrefixUpdate()
        {
            Level level = Level.current;
            if (((int)level.GetMemberValue("_updateWaitFrames")) <= 0 && level.levelIsUpdating && updater.frameadvance && updater.AllowFrameAdvance && (!updater.advancing))
            {
                return false;
            }
            return true;
        }
    }
}
