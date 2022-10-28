// Decompiled with JetBrains decompiler
// Type: DuckGame.showframe
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
    internal class showframe : Thing
    {
        public static bool olddoclip;
        public static Duck currentDucklocal;
        public static List<PhysicsObject> SimphysicsObjects;
        public static List<List<Vec4>> cancerisbad;
        public static Dictionary<Thing, Dictionary<string, object>> windowvalues;
        private bool purplething;
        private bool dootherthing;
        public static Random Offset;
        public static float lowest = float.PositiveInfinity;
        public static List<List<object>> CancerFrame;
        public HashSet<List<object>> linestodraw;

        public showframe()
          : base()
        {
            dootherthing = true;
            layer = Layer.Foreground;
            Level.Add(new w());
            linestodraw = new HashSet<List<object>>();
        }

        public override void Update()
        {
            lock (linestodraw)
                linestodraw.Clear();
            if (updater.doclip)
            {
                windowvalues.Clear();
                foreach (Window key in Level.current.things[typeof(Window)])
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("position", key.position);
                    dictionary.Add("hitPoints", key.hitPoints);
                    dictionary.Add("damageMultiplier", key.damageMultiplier);
                    dictionary.Add("shakeTimes", key.shakeTimes);
                    dictionary.Add("_shakeVal", (float)typeof(Window).GetField("_shakeVal", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key));
                    dictionary.Add("_localShakeTimes", typeof(Window).GetField("_localShakeTimes", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key));
                    dictionary.Add("_hits", typeof(Window).GetField("_hits", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key));
                    dictionary.Add("_enter", typeof(Window).GetField("_enter", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key));
                    SinWaveManualUpdate waveManualUpdate = typeof(Window).GetField("_shake", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key) as SinWaveManualUpdate;
                    dictionary.Add("_wave", typeof(SinWaveManualUpdate).GetField("_wave", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(waveManualUpdate));
                    dictionary.Add("_value", typeof(SinWaveManualUpdate).GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(waveManualUpdate));
                    windowvalues.Add(key, dictionary);
                }
            }
            if (!updater.debug && !updater.frameshow || showframe.currentDucklocal == null)
                return;
            Ragdoll ragdoll1 = showframe.currentDucklocal.ragdoll;
            if (ragdoll1 != null && ragdoll1.part2 != null)
            {
                Duck duck = ragdoll1._duck;
                if (duck != null && updater.doclip)
                {
                    SFX.enabled = false;
                    Vec2 vec2 = TestArea(ragdoll1, duck);
                    RecDuck recDuck = Setup(ragdoll1, vec2);
                    SimphysicsObjects.Add(recDuck);
                    SimphysicsObjects.Add(ragdoll1.part1);
                    SimphysicsObjects.Add(ragdoll1.part2);
                    SimphysicsObjects.Add(ragdoll1.part3);
                    if (recDuck != null)
                    {
                        Vec2 position = recDuck.position;
                        for (int index = 0; index < 10; ++index)
                        {
                            recDuck.UpdatePhysics();
                            linestodraw.Add(Line(position, recDuck.position, Color.Red));
                            position = recDuck.position;
                            if (recDuck._destroyed)
                                break;
                        }
                    }
                    SimphysicsObjects.Clear();
                    SFX.enabled = true;
                    linestodraw.Add(Line(showframe.currentDucklocal.ragdoll.part2.position, vec2, Color.Green));
                }
            }
            bool flag1 = false;
            Duck currentDucklocal = showframe.currentDucklocal;
            Ragdoll ragdoll2 = showframe.currentDucklocal.ragdoll;
            if (ragdoll2 != null && ragdoll2.part2 != null || currentDucklocal == null || !updater.doclip)
                return;
            float ypos = currentDucklocal.y + 4f;
            float degrees;
            if (currentDucklocal.sliding)
            {
                ypos += 6f;
                degrees = currentDucklocal.offDir >= 0 ? 0.0f : 180f;
            }
            else
                degrees = -90f;
            foreach (Window key in windowvalues.Keys)
            {
                Dictionary<string, object> windowvalue = windowvalues[key];
                key.position = (Vec2)windowvalue["position"];
                key.hitPoints = (float)windowvalue["hitPoints"];
                key.damageMultiplier = (float)windowvalue["damageMultiplier"];
                key.shakeTimes = (NetIndex4)windowvalue["shakeTimes"];
                typeof(Window).GetField("_shakeVal", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_shakeVal"]);
                typeof(Window).GetField("_hits", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_hits"]);
                typeof(Window).GetField("_localShakeTimes", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_localShakeTimes"]);
                typeof(Window).GetField("_enter", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_enter"]);
                SinWaveManualUpdate waveManualUpdate = typeof(Window).GetField("_shake", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key) as SinWaveManualUpdate;
                typeof(SinWaveManualUpdate).GetField("_wave", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(waveManualUpdate, windowvalue["_wave"]);
                typeof(SinWaveManualUpdate).GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(waveManualUpdate, windowvalue["_value"]);
            }
            float hSpeed = currentDucklocal.hSpeed;
            float vSpeed = currentDucklocal.vSpeed;
            Vec2 v = new Vec2(currentDucklocal._hSpeed, currentDucklocal._vSpeed);
            currentDucklocal.hSpeed = 0.0f;
            currentDucklocal.vSpeed = 0.0f;
            RecRagdoll recRagdoll = new RecRagdoll(currentDucklocal.x, ypos, currentDucklocal, currentDucklocal.sliding, degrees, currentDucklocal.offDir, v);
            recRagdoll.Initialize();
            recRagdoll.RunInit2();
            SFX.enabled = false;
            SimphysicsObjects.Add(recRagdoll.part3);
            SimphysicsObjects.Add(recRagdoll.part2);
            SimphysicsObjects.Add(recRagdoll.part1);
            SimphysicsObjects.Add(currentDucklocal);
            Vec2 position1 = recRagdoll.part2.position;
            List<Vec4> vec4List1 = new List<Vec4>();
            bool flag2 = false;
            Vec2 vec2_1 = Vec2.Zero;
            int num1 = 0;
            int num2 = -1;
            for (int index1 = 0; index1 < 98; ++index1)
            {
                num2 = index1;
                recRagdoll.part2.Update();
                recRagdoll.part3.Update();
                recRagdoll.part1.Update();
                recRagdoll.Update();
                if (vec2_1.x == (double)recRagdoll.part2.velocity.x && vec2_1.y == (double)recRagdoll.part2.velocity.y)
                    ++num1;
                else
                    num1 = 0;
                if (num1 != 5)
                {
                    vec2_1 = recRagdoll.part2.velocity;
                    Vec2 position2 = recRagdoll.part2.position;
                    bool flag3 = false;
                    if (recRagdoll.captureDuck.HasEquipment(typeof(FancyShoes)) && Math.Abs(recRagdoll._part1.x - recRagdoll._part3.x) < 9.0 && recRagdoll._part1.y < (double)recRagdoll._part3.y)
                        flag3 = true;
                    if (!recRagdoll._duck.dead && (((recRagdoll._part1.framesSinceGrounded < 5 || recRagdoll._part2.framesSinceGrounded < 5 || recRagdoll._part3.framesSinceGrounded < 5 || recRagdoll._part1.doFloat || recRagdoll.part2.doFloat ? 1 : (recRagdoll._part3.doFloat ? 1 : 0)) | (flag3 ? 1 : 0)) != 0 || recRagdoll._part1.owner != null || recRagdoll._part2.owner != null || recRagdoll._part3.owner != null))
                    {
                        List<Vec4> vec4List2 = new List<Vec4>();
                        Vec2 vec3 = TestArea(recRagdoll, currentDucklocal);
                        RecDuck recDuck = Setup(recRagdoll, vec3);
                        SimphysicsObjects.Add(recDuck);
                        if (recDuck != null)
                        {
                            bool flag4 = false;
                            Vec2 vec2_2 = Vec2.Zero;
                            Vec2 position3 = recRagdoll.part2.position;
                            Vec2 position4 = recRagdoll.part2.position;
                            Vec2 position5 = recRagdoll.part2.position;
                            bool flag5 = false;
                            int num3 = 0;
                            int num4 = -1;
                            for (int index2 = 0; index2 < 10; ++index2)
                            {
                                num4 = index2;
                                recDuck.UpdatePhysics();
                                vec4List2.Add(new Vec4(position5.x, position5.y, recDuck.x, recDuck.y));
                                position5 = recDuck.position;
                                if (flag4)
                                {
                                    linestodraw.Add(Line(position5, recDuck.position, Color.Yellow));
                                }
                                else
                                {
                                    if (!flag4)
                                        flag4 = Level.CheckLine<Block>(position5, recDuck.position) != null;
                                    if (!flag4)
                                        flag4 = Level.CheckLine<Block>(position5, position4) != null;
                                    if (!flag4)
                                        flag4 = Level.CheckLine<Block>(position3, position5) != null;
                                    if (!flag4)
                                        flag4 = Level.CheckLine<Block>(recRagdoll.part2.position, position5) != null;
                                }
                                vec2_2 = recDuck.velocity;
                                if (flag5)
                                    position4 = recDuck.position;
                                ++num3;
                                if (num3 == 3)
                                    position3 = recDuck.position;
                                flag5 = !flag5;
                                if (recDuck._destroyed)
                                    break;
                            }
                            if (flag4 && !cancerisbad.Contains(vec4List2))
                            {
                                cancerisbad.Add(vec4List2);
                                List<object> objectList = new List<object>()
                {
                   tasDevice.currentDevice.currentFrame.ToString() + " " + index1.ToString(),
                   new Vec2(0.0f, (float) Offset.NextDouble())
                };
                                CancerFrame.Add(objectList);
                                DevConsole.Log("thing " + tasDevice.currentDevice.currentFrame.ToString() + " " + index1.ToString() + " " + (num4.ToString() + " x " + position5.x + " y " + position5.y + " vx " + vec2_2.x + " vy " + vec2_2.y));
                            }
                        }
                        if (flag1)
                        {
                            vec4List1.Add(new Vec4(position1.x, position1.y, position2.x, position2.y));
                            linestodraw.Add(Line(position1, position2, Color.Purple));
                            if (olddoclip && (ragdoll2 == null || ragdoll2.part2 == null) && currentDucklocal != null)
                            {
                                currentDucklocal.hSpeed = hSpeed;
                                currentDucklocal.vSpeed = vSpeed;
                                currentDucklocal.GoRagdoll();
                                currentDucklocal.hSpeed = 0.0f;
                                currentDucklocal.vSpeed = 0.0f;
                            }
                        }
                        else
                        {
                            flag1 = Level.CheckLine<Block>(position1, position2) != null;
                            if (flag1)
                                flag2 = true;
                            linestodraw.Add(Line(position1, recRagdoll.part2.position, Color.Orange));
                        }
                        position1 = recRagdoll.part2.position;
                        if (recRagdoll.part2.destroyed || recRagdoll.part1.destroyed || recRagdoll.part3.destroyed)
                            break;
                    }
                }
                else
                    break;
            }
            if (((!purplething ? 0 : (!cancerisbad.Contains(vec4List1) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
            {
                cancerisbad.Add(vec4List1);
                List<object> objectList = new List<object>()
        {
           tasDevice.currentDevice.currentFrame.ToString() + " " + num2.ToString(),
           new Vec2(0.0f, (float) Offset.NextDouble())
        };
                CancerFrame.Add(objectList);
            }
            currentDucklocal.hSpeed = hSpeed;
            currentDucklocal.vSpeed = vSpeed;
            foreach (Window key in windowvalues.Keys)
            {
                Dictionary<string, object> windowvalue = windowvalues[key];
                key.position = (Vec2)windowvalue["position"];
                key.hitPoints = (float)windowvalue["hitPoints"];
                key.damageMultiplier = (float)windowvalue["damageMultiplier"];
                key.shakeTimes = (NetIndex4)windowvalue["shakeTimes"];
                typeof(Window).GetField("_shakeVal", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_shakeVal"]);
                typeof(Window).GetField("_hits", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_hits"]);
                typeof(Window).GetField("_localShakeTimes", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_localShakeTimes"]);
                typeof(Window).GetField("_hits", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_hits"]);
                typeof(Window).GetField("_enter", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(key, windowvalue["_enter"]);
                SinWaveManualUpdate waveManualUpdate = typeof(Window).GetField("_shake", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(key) as SinWaveManualUpdate;
                typeof(SinWaveManualUpdate).GetField("_wave", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(waveManualUpdate, windowvalue["_wave"]);
                typeof(SinWaveManualUpdate).GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(waveManualUpdate, windowvalue["_value"]);
            }
            SimphysicsObjects.Clear();
            SFX.enabled = true;
        }

        public override void Draw()
        {
            HashSet<List<object>> objectListSet = null;
            lock (linestodraw)
            {
                objectListSet = new HashSet<List<object>>(linestodraw);
                linestodraw.Clear();
            }
            foreach (List<object> objectList in objectListSet)
                Graphics.DrawLine((Vec2)objectList[0], (Vec2)objectList[1], (Color)objectList[2], 2f);
            for (int index1 = 0; index1 < cancerisbad.Count; ++index1)
            {
                List<object> objectList = CancerFrame[index1];
                string text = (string)objectList[0];
                Vec2 vec2 = (Vec2)objectList[1];
                List<Vec4> vec4List = cancerisbad[index1];
                bool flag = false;
                for (int index2 = 0; index2 < vec4List.Count; ++index2)
                {
                    Vec4 vec4 = vec4List[index2];
                    Vec2 p1 = new Vec2(vec4.x, vec4.y);
                    Vec2 p2 = new Vec2(vec4.z, vec4.w);
                    if (flag)
                    {
                        Graphics.DrawLine(p1, p2, Color.Purple, 2f, 1f);
                    }
                    else
                    {
                        flag = Level.CheckLine<Block>(p1, p2) != null;
                        if (index2 + 1 < vec4List.Count)
                            Graphics.DrawLine(p1, new Vec2(vec4List[index2 + 1].x, vec4List[index2 + 1].y), Color.Orange, 2f, 1f);
                    }
                    if (index2 + 1 >= vec4List.Count)
                        Graphics.DrawString(text, p2 - new Vec2(0.0f, (float)(18.0 * (2.0 * vec2.y))), Color.White);
                }
            }
            currentDucklocal = updater.currentDuck;
            if (currentDucklocal == null)
            {
                using (IEnumerator<Thing> enumerator = Level.current.things[typeof(Duck)].GetEnumerator())
                {
                    if (enumerator.MoveNext())
                        currentDucklocal = enumerator.Current as Duck;
                }
            }
            if (!updater.debug && !updater.frameshow || currentDucklocal == null)
                return;
            Graphics.DrawCircle(new Vec2(currentDucklocal.x, currentDucklocal.y + 4f), 18f, Color.Orange);
            foreach (Holdable holdable1 in Level.CheckCircleAll<Holdable>(new Vec2(currentDucklocal.x, currentDucklocal.y + 4f), 18f).OrderBy(h => h, new CompareHoldablePriorities(currentDucklocal)))
            {
                Holdable holdable2 = HelperG.GfieldVal(typeof(Duck), "_lastHoldItem", currentDucklocal) as Holdable;
                byte num = (byte)HelperG.GfieldVal(typeof(Duck), "_timeSinceThrow", currentDucklocal);
                if (holdable1.owner == null && holdable1.canPickUp && (holdable1 != holdable2 || num >= 30.0) && holdable1.active && holdable1.visible && Level.CheckLine<Block>(holdable1.position, holdable1.position) == null)
                {
                    Graphics.DrawRect(holdable1.topLeft, holdable1.bottomRight, Color.Green, filled: false);
                    break;
                }
            }
            try
            {
                if (currentDucklocal.holdObject is Gun)
                {
                    Gun holdObject = currentDucklocal.holdObject as Gun;
                    AmmoType ammoType = HelperG.GfieldVal(typeof(Gun), "_ammoType", holdObject) as AmmoType;
                    ATTracer type = new ATTracer();
                    type.range = ammoType.range;
                    float ang = holdObject.angleDegrees * -1f;
                    if (holdObject.offDir < 0)
                        ang += 180f;
                    Vec2 p1 = holdObject.Offset(holdObject.barrelOffset);
                    type.penetration = ammoType.penetration;
                    Bullet bullet = new Bullet(p1.x, p1.y, type, ang, holdObject.owner, tracer: true);
                    Graphics.DrawLine(p1, bullet.end, Color.MediumPurple);
                    foreach (TargetDuck targetDuck in Level.CheckLineAll<TargetDuck>(p1, bullet.end))
                        Graphics.DrawRect(targetDuck.topLeft, targetDuck.bottomRight, Color.Turquoise, filled: false);
                }
            }
            catch
            {
            }
            Graphics.DrawRect(currentDucklocal.topLeft, currentDucklocal.bottomRight, Color.Green, filled: false);
            int num1 = Level.CheckLine<Block>(currentDucklocal.topLeft + new Vec2(0.0f, 4f), currentDucklocal.bottomLeft + new Vec2(-3f, -4f)) != null ? 1 : 0;
            Block block = Level.CheckLine<Block>(currentDucklocal.topRight + new Vec2(3f, 4f), currentDucklocal.bottomRight + new Vec2(0.0f, -4f));
            if (num1 != 0)
                Graphics.DrawLine(currentDucklocal.topLeft + new Vec2(0.0f, 4f), currentDucklocal.bottomLeft + new Vec2(-3f, -4f), Color.Red);
            else
                Graphics.DrawLine(currentDucklocal.topLeft + new Vec2(0.0f, 4f), currentDucklocal.bottomLeft + new Vec2(-3f, -4f), Color.AliceBlue);
            if (block != null)
                Graphics.DrawLine(currentDucklocal.topRight + new Vec2(3f, 4f), currentDucklocal.bottomRight + new Vec2(0.0f, -4f), Color.Red);
            else
                Graphics.DrawLine(currentDucklocal.topRight + new Vec2(3f, 4f), currentDucklocal.bottomRight + new Vec2(0.0f, -4f), Color.AliceBlue);
            Graphics.DrawString(tasDevice.currentDevice.currentFrame.ToString(), currentDucklocal.position - new Vec2(0.0f, 16f), Color.White);
            Vec2 velocity = currentDucklocal.velocity;
            Vec2 position1 = currentDucklocal.position;
            bool flag1 = false;
            bool flag2 = false;
            int num2 = (int)HelperG.GfieldVal(typeof(Duck), "_wallJump", currentDucklocal);
            bool flag3 = (bool)HelperG.GfieldVal(typeof(Duck), "atWall", currentDucklocal);
            bool flag4 = currentDucklocal._groundValid > 0 && !currentDucklocal.crouchLock || flag3 && num2 == 0 || currentDucklocal.doFloat;
            try
            {
                Ragdoll ragdoll = currentDucklocal.ragdoll;
                if (ragdoll != null)
                {
                    if (ragdoll.part2 != null)
                    {
                        velocity = ragdoll.part2.velocity;
                        position1 = ragdoll.part2.position;
                        Graphics.DrawString(tasDevice.currentDevice.currentFrame.ToString(), ragdoll.part2.position - new Vec2(0.0f, 16f), Color.White);
                        Duck duck = ragdoll._duck;
                        if (duck.framesSinceRagdoll >= 1)
                        {
                            bool flag5 = false;
                            if (ragdoll.captureDuck.HasEquipment(typeof(FancyShoes)) && Math.Abs(ragdoll._part1.x - ragdoll._part3.x) < 9.0 && ragdoll._part1.y < (double)ragdoll._part3.y)
                                flag5 = true;
                            flag4 = !ragdoll._duck.dead && (((ragdoll._part1.framesSinceGrounded < 5 || ragdoll._part2.framesSinceGrounded < 5 || ragdoll._part3.framesSinceGrounded < 5 || ragdoll._part1.doFloat || ragdoll.part2.doFloat ? 1 : (ragdoll._part3.doFloat ? 1 : 0)) | (flag5 ? 1 : 0)) != 0 || ragdoll._part1.owner != null || ragdoll._part2.owner != null || ragdoll._part3.owner != null);
                        }
                        if (duck != null)
                        {
                            if (duck.framesSinceRagdoll >= 1)
                            {
                                bool flag6 = false;
                                if (ragdoll.captureDuck.HasEquipment(typeof(FancyShoes)) && Math.Abs(ragdoll._part1.x - ragdoll._part3.x) < 9.0 && ragdoll._part1.y < (double)ragdoll._part3.y)
                                    flag6 = true;
                                if (!ragdoll._duck.dead && (((ragdoll._part1.framesSinceGrounded < 5 || ragdoll._part2.framesSinceGrounded < 5 || ragdoll._part3.framesSinceGrounded < 5 || ragdoll._part1.doFloat || ragdoll.part2.doFloat ? 1 : (ragdoll._part3.doFloat ? 1 : 0)) | (flag6 ? 1 : 0)) != 0 || ragdoll._part1.owner != null || ragdoll._part2.owner != null || ragdoll._part3.owner != null))
                                {
                                    if (ragdoll.inSleepingBag)
                                    {
                                        if (ragdoll._timeSinceNudge > 1.0)
                                            flag1 = true;
                                    }
                                    else if (!ragdoll._part1.held && !ragdoll._part2.held && !ragdoll._part3.held && (ragdoll.tongueStuck == Vec2.Zero || ragdoll.tongueShakes > 5) && isServerForObject)
                                        flag2 = true;
                                }
                                if (!ragdoll._duck.dead)
                                {
                                    if (ragdoll._duck.HasEquipment(typeof(FancyShoes)) && !ragdoll.jetting)
                                        flag1 = true;
                                    else if (ragdoll._timeSinceNudge > 1.0)
                                    {
                                        if (!ragdoll.jetting)
                                            flag1 = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DevConsole.Log(ex.ToString());
            }
            if (currentDucklocal != null)
            {
                if (currentDucklocal.visible && currentDucklocal.position.y < (double)lowest && currentDucklocal.position.x >= -4000.0)
                {
                    float y = currentDucklocal.y;
                    if (y >= -4000.0)
                        lowest = y;
                }
                if (currentDucklocal.ragdoll != null)
                {
                    if (currentDucklocal.ragdoll.part1 != null && currentDucklocal.ragdoll.part1.visible && currentDucklocal.ragdoll.part1.y < (double)lowest && currentDucklocal.ragdoll.part1.position.y >= -4000.0)
                    {
                        float y = currentDucklocal.ragdoll.part1.position.y;
                        if (y >= -4000.0)
                            lowest = y;
                    }
                    if (currentDucklocal.ragdoll.part2 != null && currentDucklocal.ragdoll.part2.visible && currentDucklocal.ragdoll.part2.y < (double)lowest && currentDucklocal.ragdoll.part2.position.y >= -4000.0)
                    {
                        float y = currentDucklocal.ragdoll.part2.position.y;
                        if (y >= -4000.0)
                            lowest = y;
                    }
                    if (currentDucklocal.ragdoll.part3 != null && currentDucklocal.ragdoll.part3.visible && currentDucklocal.ragdoll.part3.y < (double)lowest && currentDucklocal.ragdoll.part3.position.y >= -4000.0)
                    {
                        float y = currentDucklocal.ragdoll.part3.position.y;
                        if (y >= -4000.0)
                            lowest = y;
                    }
                }
            }
            string text1 = "";
            if (!double.IsInfinity(lowest))
                text1 = "lowest " + lowest.ToString();
            string text2 = velocity.x >= 0.0 ? "x  " + velocity.x.ToString() : "x " + velocity.x.ToString();
            string text3 = velocity.y >= 0.0 ? "y  " + velocity.y.ToString() : "y " + velocity.y.ToString();
            string text4 = position1.x >= 0.0 ? "x  " + position1.x.ToString() : "x " + position1.x.ToString();
            string text5 = position1.y >= 0.0 ? "y  " + position1.y.ToString() : "y " + position1.y.ToString();
            string text6 = "Frame #" + tasDevice.currentDevice.currentFrame.ToString();
            string text7 = "can nudge " + flag1.ToString();
            string text8 = "can unragdoll " + flag2.ToString();
            string text9 = "can jump " + flag4.ToString();
            float scale1 = (float)((Level.current.camera.transformScreenVector(new Vec2(2f, 2f)).x - (double)Level.current.camera.transformScreenVector(new Vec2(1f, 1f)).x) * 2.0);
            Graphics.DrawStringOutline(text2, Level.current.camera.transformScreenVector(new Vec2(0.0f, 8f)), Color.White, Color.Black, scale: scale1);
            Graphics.DrawStringOutline(text3, Level.current.camera.transformScreenVector(new Vec2(0.0f, 23f)), Color.White, Color.Black, scale: scale1);
            Graphics.DrawStringOutline(text4, Level.current.camera.transformScreenVector(new Vec2(0.0f, 42f)), Color.White, Color.Black, scale: scale1);
            Graphics.DrawStringOutline(text5, Level.current.camera.transformScreenVector(new Vec2(0.0f, 57f)), Color.White, Color.Black, scale: scale1);
            Vec2 position2 = Level.current.camera.transformScreenVector(new Vec2(0.0f, 72f));
            Color white1 = Color.White;
            Color black1 = Color.Black;
            Depth depth1 = new Depth();
            double scale2 = scale1;
            Graphics.DrawStringOutline(text8, position2, white1, black1, depth1, scale: ((float)scale2));
            Graphics.DrawStringOutline(text9, Level.current.camera.transformScreenVector(new Vec2(0.0f, 87f)), Color.White, Color.Black, scale: scale1);
            Vec2 position3 = Level.current.camera.transformScreenVector(new Vec2(0.0f, 102f));
            Color white2 = Color.White;
            Color black2 = Color.Black;
            Depth depth2 = new Depth();
            double scale3 = scale1;
            Graphics.DrawStringOutline(text7, position3, white2, black2, depth2, scale: ((float)scale3));
            Vec2 position4 = Level.current.camera.transformScreenVector(new Vec2(0.0f, 117f));
            Color white3 = Color.White;
            Color black3 = Color.Black;
            Depth depth3 = new Depth();
            double scale4 = scale1;
            Graphics.DrawStringOutline(text6, position4, white3, black3, depth3, scale: ((float)scale4));
            Graphics.DrawStringOutline(text1, Level.current.camera.transformScreenVector(new Vec2(0.0f, 132f)), Color.White, Color.Black, scale: scale1);
            if (currentDucklocal == null)
                return;
            Graphics.DrawLine(currentDucklocal.bottomLeft + new Vec2(0.1f, 1f), currentDucklocal.bottomRight + new Vec2(-0.1f, 1f), Color.Purple, depth: 1f);
        }

        public static float gety(bool top, Thing thing, float value) => top ? value + (thing.position.y - thing.top) : value + (thing.position.y - thing.bottom);

        public static float getx(bool right, Thing thing, float value) => right ? value + (thing.x - thing.right) : value + (thing.x - thing.left);

        public static float getducktop(Thing thing, float value) => value + thing.collisionOffset.y;

        public static float getduckbottom(Thing thing, float value) => value + thing.collisionOffset.y + thing.collisionSize.y;

        public static float getduckleft(Thing thing, float value) => thing.offDir <= 0 ? value - thing.collisionSize.x - thing.collisionOffset.x : value + thing.collisionOffset.x;

        public static float getduckright(Thing thing, float value) => thing.offDir <= 0 ? value - thing.collisionOffset.x : value + thing.collisionOffset.x + thing.collisionSize.x;

        public static Vec2 TestArea(Ragdoll ragdoll, Duck duck)
        {
            Vec2 p1 = Vec2.Zero;
            try
            {
                Vec2 zero = Vec2.Zero;
                int num = duck.HasEquipment(typeof(FancyShoes)) ? 1 : 0;
                if (Network.isActive)
                {
                    ragdoll.part2.UpdateLastReasonablePosition(ragdoll.part2.position);
                    p1 = ragdoll._part2._lastReasonablePosition;
                }
                else
                    p1 = ragdoll._part2.position;
                if (num == 0)
                    p1.y -= 20f;
                if (duck.sliding)
                    p1.y += 10f;
                else if (duck.crouch)
                    p1.y += 8f;
                Block block1 = Level.CheckLine<Block>(p1, p1 + new Vec2(16f, 0.0f));
                if (block1 != null && block1.solid && getduckright(duck, p1.x) > (double)block1.left)
                    p1.x = getx(true, duck, block1.left);
                Block block2 = Level.CheckLine<Block>(p1, p1 - new Vec2(16f, 0.0f));
                if (block2 != null && block2.solid && getduckleft(duck, p1.x) < (double)block2.right)
                    p1.x = getx(false, duck, block1.right);
                Block block3 = Level.CheckLine<Block>(p1, p1 + new Vec2(0.0f, -16f));
                if (block3 != null && block3.solid && getducktop(duck, p1.y) < (double)block3.bottom)
                    p1.y = gety(true, duck, block3.bottom);
                Block block4 = Level.CheckLine<Block>(p1, p1 + new Vec2(0.0f, 16f));
                if (block4 != null)
                {
                    if (block4.solid)
                    {
                        if (getduckbottom(duck, p1.y) > (double)block4.top)
                            p1.y = gety(false, duck, block3.top);
                    }
                }
            }
            catch
            {
            }
            return p1;
        }

        static showframe()
        {
            Offset = new Random(69);
            cancerisbad = new List<List<Vec4>>();
            windowvalues = new Dictionary<Thing, Dictionary<string, object>>();
            CancerFrame = new List<List<object>>();
            SimphysicsObjects = new List<PhysicsObject>();
        }

        public static RecDuck Setup(Ragdoll ragdoll, Vec2 vec3)
        {
            RecDuck t = new RecDuck(vec3.x, vec3.y);
            if (t != null)
            {
                int num = t.HasEquipment(typeof(FancyShoes)) ? 1 : 0;
                t.visible = true;
                if (Network.isActive)
                {
                    ragdoll.part2.UpdateLastReasonablePosition(ragdoll.part2.position);
                    t.position = ragdoll.part2._lastReasonablePosition;
                }
                else
                    t.position = ragdoll.part2.position;
                if (num == 0)
                    t.position.y -= 20f;
                t.hSpeed = ragdoll.part2.hSpeed;
                t.immobilized = false;
                t.enablePhysics = true;
                t._jumpValid = 0;
                t._lastHoldItem = null;
                ragdoll.part2.ReturnItemToWorld(t);
                if (num == 0)
                    t.vSpeed = -2f;
                else
                    t.vSpeed = ragdoll.part2.vSpeed;
                _ = t.position;
            }
            return t;
        }

        public List<object> Line(Vec2 v1, Vec2 v2, Color C) => new List<object>()
    {
       v1,
       v2,
       C
    };
    }
}
