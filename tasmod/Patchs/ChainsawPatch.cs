
using DuckGame;

namespace RunTimeEdit
{
    public class ChainsawPatch
    {
        private static void PrefixShing(Chainsaw __instance, Thing wall)
        {
            if ((bool)__instance.GetMemberValue("_shing") || __instance.duck == null)
                return;
            Duck duck = __instance.duck;
            if (wall.bottom < (double)duck.top)
            {
                string[] strArray = new string[5]
                {
                  tasDevice.currentDevice.currentFrame.ToString(),
                  " chainsaw 1 vSpeed += 2f ",
                  null,
                  null,
                  null
                };
                float num = wall.bottom;
                strArray[2] = num.ToString();
                strArray[3] = " ";
                num = duck.top;
                strArray[4] = num.ToString();
                DevConsole.Log(string.Concat(strArray));
            }
            else
            {
                if (wall.x > (double)duck.x)
                {
                    string[] strArray = new string[5]
                    {
                        tasDevice.currentDevice.currentFrame.ToString(),
                        " chainsaw 2 hSpeed -= 5f ",
                        null,
                        null,
                        null
                    };
                    float x = wall.x;
                    strArray[2] = x.ToString();
                    strArray[3] = " ";
                    x = duck.x;
                    strArray[4] = x.ToString();
                    DevConsole.Log(string.Concat(strArray));
                    duck.hSpeed -= 5f;
                }
                else
                {
                    string[] strArray = new string[5]
                    {
            tasDevice.currentDevice.currentFrame.ToString(),
            " chainsaw 3 hSpeed += 5f ",
            null,
            null,
            null
                    };
                    float x = wall.x;
                    strArray[2] = x.ToString();
                    strArray[3] = " ";
                    x = duck.x;
                    strArray[4] = x.ToString();
                    DevConsole.Log(string.Concat(strArray));
                }
                DevConsole.Log(tasDevice.currentDevice.currentFrame.ToString() + " chainsaw 4 vSpeed -= 2f ");
            }
        }
    }
}
