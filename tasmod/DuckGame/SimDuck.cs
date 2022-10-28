// Decompiled with JetBrains decompiler
// Type: DuckGame.SimDuck
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;

namespace DuckGame
{
    [EditorGroup("Stuff")]
    public class SimDuck : Thing
    {
        private SpriteMap _sprite;
        private Sprite _bottom;
        private Sprite _top;
        private float _open;
        private float _desiredOpen;
        private bool _opened;
        private Vec2 _topLeft;
        private Vec2 _topRight;
        private Vec2 _bottomLeft;
        private Vec2 _bottomRight;
        private bool _cornerInit;
        private bool firstgo;
        private bool locked;
        private Vec2 Velocity;
        private bool redosim;
        public List<List<Vec4>> Lcancerisbad;
        public List<Vec2> LCancerFrame;
        public float hspeed = 5f;
        public float vspeed = 5f;

        public SimDuck(float xpos, float ypos)
          : base(xpos, ypos)
        {
            firstgo = true;
            depth = -0.5f;
            solid = false;
            redosim = true;
            Velocity = Vec2.Zero;
            Lcancerisbad = new List<List<Vec4>>();
            LCancerFrame = new List<Vec2>();
        }

        public override void Update()
        {
            if (Level.current == null || !Level.current.simulatePhysics || !redosim)
                return;
            redosim = false;
            RecDuck who = new RecDuck(x, y);
            who.velocity = Velocity;
            bool flag1 = false;
            float ypos = who.y + 4f;
            float degrees;
            if (who.sliding)
            {
                ypos += 6f;
                degrees = who.offDir >= 0 ? 0.0f : 180f;
            }
            else
                degrees = -90f;
            _ = (double)who.hSpeed;
            _ = (double)who.vSpeed;
            Vec2 v = new Vec2(who._hSpeed, who._vSpeed);
            who.hSpeed = 0.0f;
            who.vSpeed = 0.0f;
            RecRagdoll recRagdoll = new RecRagdoll(who.x, ypos, who, who.sliding, degrees, who.offDir, v);
            recRagdoll.Initialize();
            recRagdoll.RunInit2();
            SFX.enabled = false;
            showframe.SimphysicsObjects.Add(recRagdoll.part3);
            showframe.SimphysicsObjects.Add(recRagdoll.part2);
            showframe.SimphysicsObjects.Add(recRagdoll.part1);
            showframe.SimphysicsObjects.Add(who);
            Vec2 position1 = recRagdoll.part2.position;
            List<Vec4> vec4List1 = new List<Vec4>();
            Vec2 vec2 = Vec2.Zero;
            int num1 = 0;
            for (int index1 = 0; index1 < 100; ++index1)
            {
                recRagdoll.part3.Update();
                recRagdoll.part2.Update();
                recRagdoll.part1.Update();
                recRagdoll.Update();
                if (vec2.x == (double)recRagdoll.part2.velocity.x && vec2.y == (double)recRagdoll.part2.velocity.y)
                    ++num1;
                else
                    num1 = 0;
                if (num1 != 5)
                {
                    vec2 = recRagdoll.part2.velocity;
                    Vec2 position2 = recRagdoll.part2.position;
                    bool flag2 = false;
                    if (recRagdoll.captureDuck.HasEquipment(typeof(FancyShoes)) && Math.Abs(recRagdoll._part1.x - recRagdoll._part3.x) < 9.0 && recRagdoll._part1.y < (double)recRagdoll._part3.y)
                        flag2 = true;
                    if (!recRagdoll._duck.dead && (((recRagdoll._part1.framesSinceGrounded < 5 || recRagdoll._part2.framesSinceGrounded < 5 || recRagdoll._part3.framesSinceGrounded < 5 || recRagdoll._part1.doFloat || recRagdoll.part2.doFloat ? 1 : (recRagdoll._part3.doFloat ? 1 : 0)) | (flag2 ? 1 : 0)) != 0 || recRagdoll._part1.owner != null || recRagdoll._part2.owner != null || recRagdoll._part3.owner != null))
                    {
                        List<Vec4> vec4List2 = new List<Vec4>();
                        Vec2 vec3 = showframe.TestArea(recRagdoll, who);
                        RecDuck recDuck = showframe.Setup(recRagdoll, vec3);
                        showframe.SimphysicsObjects.Add(recDuck);
                        if (recDuck != null)
                        {
                            bool flag3 = false;
                            Vec2 position3 = recRagdoll.part2.position;
                            Vec2 position4 = recRagdoll.part2.position;
                            Vec2 position5 = recRagdoll.part2.position;
                            bool flag4 = false;
                            int num2 = 0;
                            int num3 = -1;
                            for (int index2 = 0; index2 < 17; ++index2)
                            {
                                num3 = index2;
                                recDuck.UpdatePhysics();
                                vec4List2.Add(new Vec4(position5.x, position5.y, recDuck.x, recDuck.y));
                                if (!flag3)
                                {
                                    flag3 = Level.CheckLine<Block>(position5, recDuck.position) != null;
                                    if (!flag3)
                                        flag3 = Level.CheckLine<Block>(position5, position4) != null;
                                    if (!flag3)
                                        flag3 = Level.CheckLine<Block>(position3, position5) != null;
                                    if (!flag3)
                                        flag3 = Level.CheckLine<Block>(recRagdoll.part2.position, position5) != null;
                                }
                                position5 = recDuck.position;
                                if (flag4)
                                    position4 = recDuck.position;
                                ++num2;
                                if (num2 == 3)
                                    position3 = recDuck.position;
                                flag4 = !flag4;
                                if (recDuck._destroyed)
                                    break;
                            }
                            if (!Lcancerisbad.Contains(vec4List2))
                            {
                                Lcancerisbad.Add(vec4List2);
                                LCancerFrame.Add(new Vec2(tasDevice.currentDevice.currentFrame, (float)showframe.Offset.NextDouble()));
                                DevConsole.Log("thing " + tasDevice.currentDevice.currentFrame.ToString() + " " + index1.ToString() + " " + num3.ToString());
                            }
                        }
                        if (flag1)
                            vec4List1.Add(new Vec4(position1.x, position1.y, position2.x, position2.y));
                        else
                            flag1 = Level.CheckLine<Block>(position1, position2) != null;
                        position1 = recRagdoll.part2.position;
                        if (recRagdoll.part2.destroyed || recRagdoll.part1.destroyed || recRagdoll.part3.destroyed)
                            break;
                    }
                }
                else
                    break;
            }
            showframe.SimphysicsObjects.Clear();
        }

        public override void Draw()
        {
            Graphics.DrawLine(new Vec2(x - 1f, y), new Vec2(x + 1f, y), Color.Green);
            Graphics.DrawLine(new Vec2(x, y - 1f), new Vec2(x, y + 1f), Color.Green);
            for (int index1 = 0; index1 < Lcancerisbad.Count; ++index1)
            {
                Vec2 vec2 = LCancerFrame[index1];
                List<Vec4> vec4List = Lcancerisbad[index1];
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
                        Graphics.DrawString(vec2.x.ToString(), p2 - new Vec2(0.0f, (float)(18.0 * (2.0 * vec2.y))), Color.White);
                }
            }
            base.Draw();
        }

        public override ContextMenu GetContextMenu()
        {
            EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
            contextMenu.AddItem(new ContextSlider("hspeedx", null, new FieldBinding(this, "hspeed", -20f, 20f), 0.01f));
            contextMenu.AddItem(new ContextSlider("vspeedy", null, new FieldBinding(this, "vspeed", -20f, 20f), 0.01f));
            return contextMenu;
        }

        public override void Initialize()
        {
            Velocity = new Vec2(hspeed, vspeed);
            base.Initialize();
        }

        public override BinaryClassChunk Serialize()
        {
            BinaryClassChunk binaryClassChunk = base.Serialize();
            binaryClassChunk.AddProperty("hspeed", hspeed);
            binaryClassChunk.AddProperty("vspeed", vspeed);
            return binaryClassChunk;
        }

        public override bool Deserialize(BinaryClassChunk node)
        {
            base.Deserialize(node);
            hspeed = node.GetProperty<float>("hspeed");
            vspeed = node.GetProperty<float>("vspeed");
            return true;
        }

        public override DXMLNode LegacySerialize()
        {
            DXMLNode dxmlNode = base.LegacySerialize();
            dxmlNode.Add(new DXMLNode("hspeed", Change.ToString(hspeed)));
            dxmlNode.Add(new DXMLNode("vspeed", Change.ToString(vspeed)));
            return dxmlNode;
        }

        public override bool LegacyDeserialize(DXMLNode node)
        {
            base.LegacyDeserialize(node);
            DXMLNode dxmlNode1 = node.Element("hspeed");
            if (dxmlNode1 != null)
                hspeed = Convert.ToSingle(dxmlNode1.Value);
            DXMLNode dxmlNode2 = node.Element("vspeed");
            if (dxmlNode2 != null)
                vspeed = Convert.ToSingle(dxmlNode2.Value);
            return true;
        }
    }
}
