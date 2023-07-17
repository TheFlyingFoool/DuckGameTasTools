using DuckGame;

//__instance.x.ToString() 
namespace RunTimeEdit
{
    public class TargetDuckPatch
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
            DevConsole.Log("TD Killed " + tasDevice.currentDevice._currentFrame.ToString() + " " + order.ToString() + " " + __instance.position.x.ToString() + " " + __instance.position.y.ToString());
        }
    }
}