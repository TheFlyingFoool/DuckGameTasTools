using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    //__instance.x.ToString() 
namespace RunTimeEdit
{
    internal class TargetDuckPatch
    {
        public TargetDuckPatch()
        {
        }

        private static void PrefixKill(TargetDuck __instance)
        {
            int order = -10;
            if (__instance.sequence != null)
            {
                order = __instance.sequence.order;
            }
            DevConsole.Log("TD Killed " + tasDevice.currentDevice.currentFrame.ToString() + " " + order.ToString() + " " + __instance.position.x.ToString() + " " + __instance.position.y.ToString());
        }
    }
}