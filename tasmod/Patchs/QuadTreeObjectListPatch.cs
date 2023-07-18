using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patchs
{
    public class QuadTreeObjectListPatch
    {
        private static bool PrefixRefreshState()
        {
            Level level = Level.current;
            if (level != null && ((int)level.GetMemberValue("_updateWaitFrames")) <= 0 && level.levelIsUpdating && updater.frameadvance && updater.AllowFrameAdvance && (!updater.advancing))
            {
                return false;
            }
            return true;
        }
    }
}
