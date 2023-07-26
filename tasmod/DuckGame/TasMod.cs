using Patchs;
using RunTimeEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DuckGame
{
    public class TasMod : Mod
    {
        public static string DebugFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\DuckGame\\TAS\\debug.dgtas";
        public static string FilePath = "";
        public static string Directory;

        public override Priority priority => Priority.Lowest;

        protected override void OnPreInitialize()
        {
            base.OnPreInitialize();
            Directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\DuckGame\\TAS\\";
        }

        protected override void OnPostInitialize()
        {
            updater.tDev = new tasDevice(0);
            Input.GetInputDevices().Insert(0, updater.tDev);
            updater.tDev.loadInputs(FilePath);
            InputProfile defaultPlayer1 = InputProfile.DefaultPlayer1;
            new Thread(new ThreadStart(wait)).Start();
            base.OnPostInitialize();
        }

        public static void MapToDefault(InputDevice device)
        {
            InputProfile defaultPlayer1 = InputProfile.DefaultPlayer1;
            defaultPlayer1.Map(device, "LEFT", 4, true);
            defaultPlayer1.Map(device, "RIGHT", 8, true);
            defaultPlayer1.Map(device, "UP", 1, true);
            defaultPlayer1.Map(device, "DOWN", 2, true);
            defaultPlayer1.Map(device, "JUMP", 4096, true);
            defaultPlayer1.Map(device, "SHOOT", 16384, true);
            defaultPlayer1.Map(device, "GRAB", 32768, true);
            defaultPlayer1.Map(device, "QUACK", 8192, true);
            defaultPlayer1.Map(device, "START", 16, true);
            defaultPlayer1.Map(device, "STRAFE", 256, true);
            defaultPlayer1.Map(device, "RAGDOLL", 512, true);
            defaultPlayer1.Map(device, "LTRIGGER", 8388608, true);
            defaultPlayer1.Map(device, "RTRIGGER", 4194304, true);
            defaultPlayer1.Map(device, "SELECT", 4096, true);
        }

        private void wait()
        {
            //while (!(Level.current is TitleScreen))
            //    Thread.Sleep(1000);
            //while (Level.current.things[typeof(Duck)].Count() <= 0)
            //    Thread.Sleep(500);
            updater.InitCommands();
            Patch();
            _ = new updater();
        }
        public static void Patch()
        {
            bool flag = false;
            List<MPatch> mpatchList = new List<MPatch>();
            Assembly key1 = null;
            foreach (Assembly key2 in (typeof(ModLoader).GetField("_modAssemblies", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Assembly, Mod>).Keys)
            {
                if (key2.GetType("HarmonyLoader.Loader") != null)
                {
                    key1 = key2;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                key1 = Assembly.Load(System.IO.File.ReadAllBytes(GetPath<TasMod>("HarmonyLoader") + ".dll"));
                (typeof(ModLoader).GetField("_modAssemblies", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Assembly, Mod>).Add(key1, new DisabledMod());
            }
            if (!(key1 != null))
                return;
            try
            {
                FieldInfo fieldInfo = null;
                try
                {
                    fieldInfo = key1.GetType("HarmonyLoader.Loader").GetField("expectionstring", BindingFlags.Static | BindingFlags.Public);
                }
                catch
                {
                }
                MethodInfo method = key1.GetType("HarmonyLoader.Loader").GetMethod("Patch2", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);


                //mpatchList.Add(new MPatch(SGMI(typeof(Camera), "DoUpdate"), SGMI(typeof(CameraPatch), "PrefixDoUpdate")));
                mpatchList.Add(new MPatch(SGMI(typeof(AutoUpdatables), "Update"), SGMI(typeof(AutoUpdatablesPatch), "PrefixUpdate")));
                mpatchList.Add(new MPatch(SGMI(typeof(QuadTreeObjectList), "RefreshState"), SGMI(typeof(QuadTreeObjectListPatch), "PrefixRefreshState")));
                mpatchList.Add(new MPatch(SGMI(typeof(Level), "UpdateThings"), SGMI(typeof(LevelPatch), "PrefixUpdateThings")));
                mpatchList.Add(new MPatch(SGMI(typeof(Level), "PostUpdate"), SGMI(typeof(LevelPatch), "PrefixPostUpdate")));
                mpatchList.Add(new MPatch(SGMI(typeof(ChallengeLevel), "Update"), SGMI(typeof(ChallengeLevelPatch), "PrefixUpdate")));
                mpatchList.Add(new MPatch(SGMI(typeof(ChallengeLevel), "PauseLogic"), SGMI(typeof(ChallengeLevelPatch), "PrefixPausingLogic")));

                mpatchList.Add(new MPatch(SGMI(typeof(TargetDuck), "Kill"), SGMI(typeof(TargetDuckPatch), "PrefixKill")));
                mpatchList.Add(new MPatch(SGMI(typeof(Ragdoll), "UpdateInput"), SGMI(typeof(RagdollPatch), "PrefixUpdateInput")));
                mpatchList.Add(new MPatch(SGMI(typeof(Chainsaw), "Shing"), SGMI(typeof(ChainsawPatch), "PrefixShing")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "Down"), SGMI(typeof(PatchInputProfile), "PrefixDown")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "Pressed"), SGMI(typeof(PatchInputProfile), "PrefixPressed")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "Released"), SGMI(typeof(PatchInputProfile), "PrefixReleased")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "get_hasMotionAxis"), SGMI(typeof(PatchInputProfile), "Prefixget_hasMotionAxis")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "get_rightTrigger"), SGMI(typeof(PatchInputProfile), "Prefixget_rightTrigger")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "get_leftTrigger"), SGMI(typeof(PatchInputProfile), "Prefixget_leftTrigger")));
                mpatchList.Add(new MPatch(SGMI(typeof(InputProfile), "get_motionAxis"), SGMI(typeof(PatchInputProfile), "Prefixget_motionAxis")));

                mpatchList.Add(new MPatch(SGMI(typeof(MonoMain), "RunUpdate"), null, null, SGMI(typeof(MonoMainPatch), "Transpiler")));

                mpatchList.Add(new MPatch(SGMI(typeof(Spring), "Touch"), SGMI(typeof(SpringPatch), "BPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringDown), "Touch"), SGMI(typeof(SpringPatch), "DownPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringDownLeft), "Touch"), SGMI(typeof(SpringPatch), "DownLeftPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringDownRight), "Touch"), SGMI(typeof(SpringPatch), "DownRightPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringLeft), "Touch"), SGMI(typeof(SpringPatch), "LeftPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringRight), "Touch"), SGMI(typeof(SpringPatch), "RightPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringUpLeft), "Touch"), SGMI(typeof(SpringPatch), "UpLeftPrefix")));
                mpatchList.Add(new MPatch(SGMI(typeof(SpringUpRight), "Touch"), SGMI(typeof(SpringPatch), "UpRightPrefix")));
                foreach (MPatch mpatch in mpatchList)
                {
                    string str1 = "";
                    if (mpatch.original != null)
                        str1 = mpatch.original.ReflectedType.Name + "." + mpatch.original.Name + " ";
                    if (mpatch.prefix != null)
                        str1 = str1 + mpatch.prefix.ReflectedType.Name + "." + mpatch.prefix.Name + " ";
                    if (mpatch.postfix != null)
                        str1 = str1 + mpatch.postfix.ReflectedType.Name + "." + mpatch.postfix.Name + " ";
                    if (mpatch.transpiler != null)
                        str1 = str1 + mpatch.transpiler.ReflectedType.Name + "." + mpatch.transpiler.Name + " ";
                    try
                    {
                        method.Invoke(null, new object[4]
                        {
                           mpatch.original,
                           mpatch.prefix,
                           mpatch.postfix,
                           mpatch.transpiler
                        });
                        DevConsole.Log(mpatchList.IndexOf(mpatch).ToString() + " " + str1, Color.Green);
                    }
                    catch (Exception ex)
                    {
                        string str2 = "";
                        if (mpatch.original != null)
                            str2 = mpatch.original.ReflectedType.Name + "." + mpatch.original.Name + " ";
                        if (mpatch.prefix != null)
                            str2 = str2 + mpatch.prefix.ReflectedType.Name + "." + mpatch.prefix.Name + " ";
                        if (mpatch.postfix != null)
                            str2 = str2 + mpatch.postfix.ReflectedType.Name + "." + mpatch.postfix.Name + " ";
                        if (mpatch.transpiler != null)
                            str2 = str2 + mpatch.transpiler.ReflectedType.Name + "." + mpatch.transpiler.Name + " ";
                        if (fieldInfo != null)
                        {
                            str2 += (string)fieldInfo.GetValue(null);
                            fieldInfo.SetValue(null, "");
                        }
                        DevConsole.Log(mpatchList.IndexOf(mpatch).ToString() + " " + str2, Color.Red);
                        DevConsole.Log(ex.Message, Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                DevConsole.Log("Patching crash Tas mod", Color.Red);
                DevConsole.Log(ex.Message, Color.Red);
            }
        }

        public static MethodInfo SGMI(Type type, string Methodname)
        {
            MethodInfo methodInfo = !(type == null) ? type.GetMethod(Methodname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentException("type can not be Null", "helperS.GMI");
            return !(methodInfo == null) ? methodInfo : throw new ArgumentException("MethodInfo Not found, Null " + type.Name + Methodname, "helperS.GMI");
        }
    }
}
