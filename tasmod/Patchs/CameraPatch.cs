//using DuckGame;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Patchs
//{
//    public class CameraPatch
//    {
//        private static bool PrefixDoUpdate()
//        {
//            Level level = Level.current;
//            if (level != null && ((int)level.GetMemberValue("_updateWaitFrames")) <= 0 && level.levelIsUpdating && updater.frameadvance && updater.AllowFrameSdvance && (!updater.advancing))
//            {
//                return false;
//            }
//            return true;
//        }
//    }
//}
