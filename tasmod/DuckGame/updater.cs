using Microsoft.Xna.Framework;
using RunTimeEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
    public class updater : IUpdateable
    {
        public static updater current;
        public static bool shotgun;
        public static bool debug;
        public static FieldInfo gunField;
        public static FieldInfo startedField;
        public static tasDevice tDev;
        public static Duck currentDuck;
        private bool waitingforDuck;
        private Level prevLevel;
        public static bool frameshow;
        public static bool recording;
        public static List<inputFrame> frames = new List<inputFrame>();
        public static bool doclip;
        public bool doupdateable = true;
        private static FieldInfo spawndingsField = typeof(ChallengeLevel).GetField("_waitAfterSpawnDings", BindingFlags.Instance | BindingFlags.NonPublic);
        public static FieldInfo waitAfterSpawnDingsField = typeof(ChallengeLevel).GetField("_waitAfterSpawnDings", BindingFlags.Instance | BindingFlags.NonPublic);
        public static FieldInfo _waitSpawnField = typeof(ChallengeLevel).GetField("_waitSpawn", BindingFlags.Instance | BindingFlags.NonPublic);
        public static Dictionary<string, List<object>> savestates = new Dictionary<string, List<object>>();
        public static List<object> lastsavedstate;

        public void StopTAS()
        {
            if (currentDuck != null)
                currentDuck.inputProfile.ClearMappings();
            Input.InitDefaultProfiles();
        }

        public void startTAS()
        {
            InputProfile.DefaultPlayer1.ClearMappings();
            TasMod.MapToDefault(tDev);
            tDev.start();
        }
        private void checkloadCommand()
        {
            List<string> list = ((IEnumerable<string>)DevConsole.core.previousLines.Last().ToLower().Split(' ')).Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (list.Count < 1)
                return;
            if (list[0] == "loadtas")
            {
                DevConsole.core.lines.Last().color = Color.Black;
                if (list.Count < 2)
                {
                    DevConsole.Log("invalid syntax: LoadTas <tas name>", Color.Red);
                }
                else
                {
                    list.RemoveAt(0);
                    string str1 = string.Join(" ", list);
                    string str2 = TasMod.Directory + str1 + ".dgtas";
                    if (!File.Exists(str2))
                    {
                        DevConsole.Log("FILE NOT FOUND!", Color.Red);
                        DevConsole.Log("Found Tas files:", Color.Blue);
                        foreach (string path in ((IEnumerable<FileInfo>)new DirectoryInfo(Path.GetDirectoryName(str2)).GetFiles("*.dgtas")).Select(x => x.Name))
                            DevConsole.Log(Path.GetFileNameWithoutExtension(path), Color.Blue);
                    }
                    else
                    {
                        tDev.loadInputs(str2);
                        DevConsole.Log("Loaded " + str2, Color.Green);
                    }
                }
            }
            else if (list[0] == "ss")
            {
                DevConsole.core.lines.Last().color = Color.Black;
                if (list.Count < 2)
                {
                    DevConsole.Log("invalid syntax: ss <state name>", Color.Red);
                    return;
                }
                foreach(Duck current in Level.current.things[typeof(Duck)])
                {
                    if (current is TargetDuck)
                        continue;
                    List<object> objectList = new List<object>();
                    Dictionary<string, Dictionary<FieldInfo, object>> dictionary = new Dictionary<string, Dictionary<FieldInfo, object>>();
                    objectList.Add(current.sleeping);
                    objectList.Add(current.position);
                    objectList.Add(current.velocity);
                    objectList.Add(current.sliding);
                    objectList.Add(current.jumping);
                    objectList.Add(current._jumpValid);
                    objectList.Add(current.framesSinceJump);
                    objectList.Add(current._hovering);
                    objectList.Add(current._groundValid);
                    objectList.Add(current.skipPlatFrames);
                    objectList.Add(current.strafing);
                    objectList.Add(current.sliding);
                    objectList.Add(current.crouch);
                    objectList.Add(current.slideBuildup);
                    objectList.Add(tasDevice.currentDevice.currentFrame);
                    int num1 = -1;
                    foreach (Holdable holdable in Level.current.things[typeof(Holdable)])
                    {
                        object obj = holdable;
                        int num2 = 0;
                        string key;
                        while (true)
                        {
                            key = obj.GetType().ToString() + num2.ToString();
                            if (dictionary.Keys.Contains(key))
                                ++num2;
                            else
                                break;
                        }
                        dictionary[key] = new Dictionary<FieldInfo, object>() {
                                {
                                    typeof(Holdable).GetField("position",BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic),
                                    holdable.position
                                },
                                {
                                    typeof(Holdable).GetField("_angle", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic),
                                     holdable._angle
                                },
                                {
                                    typeof(Holdable).GetField("_hSpeed", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic),
                                    holdable._hSpeed
                                },
                                {
                                    typeof(Holdable).GetField("_grounded", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic),
                                    holdable.grounded
                                }
                            };

                    }
                    objectList.Add(num1);
                    objectList.Add(dictionary);
                    savestates[list[1]] = objectList;
                    break;
                }
            }
            else if (list[0] == "sl")
            {
                DevConsole.core.lines.Last().color = Color.Black;
                if (list.Count < 2)
                {
                    DevConsole.Log("invalid syntax: sl <state name>", Color.Red);
                    return;
                }
                if (!savestates.Keys.Contains(list[1]))
                {
                    DevConsole.Log("no save state found", Color.Red);
                    return;
                }
                lastsavedstate = savestates[list[1]];
                loadstate(savestates[list[1]]);
            }
            else if (list[0] == "unloadtas")
            {
                DevConsole.core.lines.Last().color = Color.Black;
                DevConsole.Log("UNLOADED CURRENT TAS", Color.Green);
                tDev.loadInputs("");
            }
            else if(list[0] == "duck")
                RecDuck.physicsfcksit = !RecDuck.physicsfcksit;
            else if(list[0] == "maxragjump")
            {
                RagdollPatch.maxragjump = !RagdollPatch.maxragjump;
                DevConsole.Log("maxragjump " + RagdollPatch.maxragjump.ToString(), Color.Green);
            }
            else if (list[0] == "order")
            {
                DevConsole.core.lines.Last().color = Color.Black;
                foreach (Thing thing in Level.current.things)
                {
                    string[] strArray = new string[5]
                    {
                          thing.GetType().ToString(),
                          " ",
                          null,
                          null,
                          null
                    };
                    float num = thing.x;
                    strArray[2] = num.ToString();
                    strArray[3] = " ";
                    num = thing.y;
                    strArray[4] = num.ToString();
                    DevConsole.Log(string.Concat(strArray), Color.Green);
                }
                DevConsole.Log("UNLOADED CURRENT TAS", Color.Green);
                tDev.loadInputs("");
            }
        }

        private void LevelChanged()
        {
            currentDuck = null;
            shotgun = false;
            if (Level.current is ChallengeLevel)
            {
                waitingforDuck = true;
                if (debug)
                    tDev.loadInputs(TasMod.DebugFilePath);
            }
            tDev.reset();
        }

        private void insertDevice()
        {
        }

        public updater()
        {
            if (doupdateable)
                (typeof(Game).GetField("updateableComponents", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).GetValue(MonoMain.instance) as List<IUpdateable>).Add(this);
            current = this;
        }

        static updater()
        {
            gunField = typeof(Gun).GetField("_numBulletsPerFire", BindingFlags.Instance | BindingFlags.NonPublic);
            startedField = typeof(ChallengeLevel).GetField("_started", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public void Update(GameTime t)
        {
            try
            {
                UpdateBase();
            }
            catch
            {
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public int UpdateOrder => 1;

        public bool Enabled => true;

        public void UpdateBase()
        {
            if (Level.current != prevLevel)
                LevelChanged();
            prevLevel = Level.current;
            if (Keyboard.Released(Keys.F7))
            {
                if (recording)
                {
                    DevConsole.Log("saved");
                    List<byte> byteList = new List<byte>();
                    foreach (inputFrame frame in frames)
                        byteList.AddRange(frame.write2());
                    if (byteList.Count > 0)
                        File.WriteAllBytes(TasMod.Directory + "saved.dgtas", byteList.ToArray());
                    frames.Clear();
                    recording = false;
                }
                else
                {
                    DevConsole.Log("recording");
                    recording = true;
                }
            }
            if (currentDuck != null && recording)
                frames.Add(new inputFrame(currentDuck.inputProfile));
            //if (Keyboard.Pressed(Keys.F5)) // I Never liked it
            //{
            //    debug = !debug; 
            //    DevConsole.Log(debug ? "Enabled" : "Disabled Debug", debug ? Color.Green : Color.Red);
            //}
            if (Keyboard.Pressed(Keys.F9))
            {
                lastsavedstate = null;
                showframe.cancerisbad = new List<List<Vec4>>();
                showframe.CancerFrame = new List<List<object>>();
                showframe.lowest = float.PositiveInfinity;
            }
            if (Keyboard.Pressed(Keys.F6))
            {
                frameshow = !frameshow;
                DevConsole.Log(frameshow ? "Enabled FrameShow" : "Disabled FrameShow", frameshow ? Color.Green : Color.Red);
            }
            if (Keyboard.Pressed(Keys.F8))
            {
                doclip = !doclip;
                DevConsole.Log(doclip ? "clip finder" : "Disabled clip finder", doclip ? Color.Green : Color.Red);
            }
            if (Keyboard.Pressed(Keys.Enter) && DevConsole.open)
                checkloadCommand();
            if (tDev != null && tDev.Inputs.Length != 0 && tDev.currentFrame == 0)
                tasDevice.rng = tDev.Inputs[0].rng;
            if (shotgun || tasDevice.rng > 0)
            {
                Rando.generator = new fixedRandom(tasDevice.rng);
                ChallengeRando.generator = new fixedRandom(tasDevice.rng);
                NetRand.generator = new fixedRandom(tasDevice.rng);
            }
            else
            {
                Rando.generator = new Random(tasDevice.rng);
                ChallengeRando.generator = new Random(tasDevice.rng);
                NetRand.generator = new Random(tasDevice.rng);
            }
            if (currentDuck != null)
            {
                shotgun = false;
                if (currentDuck.holdObject is Gun && (int)gunField.GetValue(currentDuck.holdObject) > 1)
                    shotgun = true;
            }
            if (!(Level.current is ChallengeLevel) || !waitingforDuck)
                return;
            Duck duck = null;
            foreach (Duck duck2 in Level.current.things[typeof(Duck)])
            {
                if (!(duck2 is TargetDuck))
                {
                    duck = duck2;
                }
            }
            if (duck != null && (float)updater._waitSpawnField.GetValue(Level.current as ChallengeLevel) - 0.06f <= 0f && (int)updater.waitAfterSpawnDingsField.GetValue(Level.current as ChallengeLevel) == 2)
            {
                try
                {
                    waitingforDuck = false;
                    currentDuck = duck;
                    bool flag = false;
                    using (IEnumerator<Thing> enumerator = Level.current.things[typeof(showframe)].GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            Thing current = enumerator.Current;
                            flag = true;
                        }
                    }
                    if (!flag)
                        Level.Add(new showframe());
                    startTAS();
                }
                catch
                {
                    Mod.Debug.Log("error 404: duck not found");
                }
            }
        }

        public static void loadstate(List<object> state)
        {
            Duck duck = null;
            foreach(Duck duck1 in Level.current.things[typeof(Duck)])
            {
                if (duck1 is TargetDuck)
                    continue;
                duck = duck1;
                break;
            }
            if (duck != null)
            {
                duck.sleeping = (bool)state[0];
                duck.position = (Vec2)state[1];
                duck.velocity = (Vec2)state[2];
                duck.sliding = (bool)state[3];
                duck.jumping = (bool)state[4];
                duck._jumpValid = (int)state[5];
                duck.framesSinceJump = (int)state[6];
                duck._hovering = (bool)state[7];
                duck._groundValid = (int)state[8];
                duck.skipPlatFrames = (int)state[9];
                duck.strafing = (bool)state[10];
                duck.sliding = (bool)state[11];
                duck.crouch = (bool)state[12];
                duck.slideBuildup = (float)state[13];
                tasDevice.currentDevice.currentFrame = (int)state[14];
                foreach (Mod accessibleMod in ModLoader.accessibleMods)
                {
                    if (!(accessibleMod is CoreMod) && accessibleMod.configuration != null && !(bool)typeof(ModConfiguration).GetProperty("disabled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(accessibleMod.configuration))
                    {
                        FieldInfo field = accessibleMod.GetType().GetField("currentFrame", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        if (field != null)
                        {
                            field.SetValue(null, (int)state[14]);
                            break;
                        }
                    }
                }
                int num1 = (int)state[15];
                Dictionary<string, Dictionary<FieldInfo, object>> dictionary1 = state[16] as Dictionary<string, Dictionary<FieldInfo, object>>;
                List<string> stringList = new List<string>();
                using (IEnumerator<Thing> enumerator = Level.current.things[typeof(Holdable)].GetEnumerator())
                {
                label_27:
                    if (enumerator.MoveNext())
                    {
                        Holdable current = (Holdable)enumerator.Current;
                        object obj = current;
                        int num2 = 0;
                        do
                        {
                            string key1 = obj.GetType().ToString() + num2.ToString();
                            if (!stringList.Contains(key1) && dictionary1.Keys.Contains(key1))
                            {
                                Dictionary<FieldInfo, object> dictionary2 = dictionary1[key1];
                                foreach (FieldInfo key2 in dictionary2.Keys)
                                    key2.SetValue(current, dictionary2[key2]);
                                stringList.Add(key1);
                            }
                            ++num2;
                        }
                        while (num2 <= num1 + 10);
                        goto label_27;
                    }
                }
                DevConsole.Log("state set", Color.Red);
            }
            else
                DevConsole.Log("no duck in level", Color.Red);
        }
    }
}
