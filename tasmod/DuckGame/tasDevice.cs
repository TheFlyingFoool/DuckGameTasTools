// Decompiled with JetBrains decompiler
// Type: DuckGame.tasDevice
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Reflection;

namespace DuckGame
{
    public class tasDevice : XInputPad
    {
        public static tasDevice currentDevice;
        public inputFrame[] Inputs;
        public inputFrame currentInput;
        public int _currentFrame;
        public static int rng = -1;
        public bool running;
        public AnalogGamePad defaultDevice;
        public FieldInfo defstateField;
        public bool did0once;
        public bool did0oncetwo;

        public tasDevice(int index)
          : base(index)
        {
            currentDevice = this;
            defstateField = typeof(AnalogGamePad).GetField("_state", BindingFlags.Instance | BindingFlags.NonPublic);
            defaultDevice = InputProfile.DefaultPlayer1.genericController.device;
        }

        public void loadInputs(string inputFile) => Inputs = tasInput.read(inputFile);

        public void loadInputs(byte[] bytes) => Inputs = tasInput.read(bytes);

        public tasDevice(int index, byte[] inputBytes)
          : base(index)
        {
            Inputs = tasInput.read(inputBytes);
            currentDevice = this;
        }

        public void reset()
        {
            did0once = false;
            did0oncetwo = false;
            _currentFrame = 0;
            stop();
        }
        public void stop()
        {
            if (running)
                updater.current.StopTAS();
            running = false;
            rng = -1;
        }

        public void start() => running = true;

        public override void Update()
        {
            try
            {
                if (running && (!(updater.frameadvance && updater.AllowFrameSdvance) || updater.advancing))
                {
                    if (_currentFrame >= Inputs.Length)
                    {
                        reset();
                        return;
                    }
                    if (did0once && _currentFrame > 1 && !did0oncetwo && updater.lastsavedstate != null && updater.lastsavedstate.Count > 0)
                    {
                        updater.loadstate(updater.lastsavedstate);
                        did0oncetwo = true;
                    }
                    currentInput = Inputs[_currentFrame];
                    if (Inputs.Length > _currentFrame + 1)
                        rng = Inputs[_currentFrame + 1].rng;
                    if (updater.currentDuck != null)
                    {
                        Vec2 velocity = updater.currentDuck.velocity;
                        string str1 = velocity.x >= 0.0 ? "x  " + velocity.x.ToString() : "x " + velocity.x.ToString();
                        string str2 = velocity.y >= 0.0 ? "y  " + velocity.y.ToString() : "y " + velocity.y.ToString();
                        int num = (int)HelperG.GfieldVal(typeof(Duck), "_wallJump", showframe.currentDucklocal);
                        bool flag1 = (bool)HelperG.GfieldVal(typeof(Duck), "atWall", showframe.currentDucklocal);
                        bool flag2 = showframe.currentDucklocal._groundValid > 0 && !showframe.currentDucklocal.crouchLock || flag1 && num == 0 || showframe.currentDucklocal.doFloat;
                        Vec2 position = updater.currentDuck.position;
                        Vec2 vec2_1 = Vec2.Zero;
                        Vec2 vec2_2 = Vec2.Zero;
                        if (updater.currentDuck.ragdoll != null && updater.currentDuck.ragdoll.part1 != null)
                        {
                            vec2_1 = updater.currentDuck.ragdoll.part1.position;
                            vec2_2 = updater.currentDuck.ragdoll.part1.velocity;
                        }
                        string str3 = position.x >= 0.0 ? "x  " + position.x.ToString() : "x " + position.x.ToString();
                        string str4 = position.y >= 0.0 ? "y  " + position.y.ToString() : "y " + position.y.ToString();
                        string str5 = vec2_1.x >= 0.0 ? "x  " + vec2_1.x.ToString() : "x " + vec2_1.x.ToString();
                        string str6 = vec2_1.y >= 0.0 ? "y  " + vec2_1.y.ToString() : "y " + vec2_1.y.ToString();
                        string str7 = vec2_2.x >= 0.0 ? "x  " + vec2_2.x.ToString() : "x " + vec2_2.x.ToString();
                        string str8 = vec2_2.y >= 0.0 ? "y  " + vec2_2.y.ToString() : "y " + vec2_2.y.ToString();
                        DevConsole.Log("Frame " + _currentFrame.ToString() + " " + str3 + " " + str4 + " " + str1 + " " + str2 + " " + str5 + " " + str6 + " " + str7 + " " + str8 + " can jump" + flag2.ToString());
                    }
                    if (did0once)
                        ++_currentFrame;
                    did0once = true;
                }
            }
            catch (Exception ex)
            {
                Mod.Debug.Log("couldnt update controller " + ex.Message);
                reset();
            }
            base.Update();
        }

        public override bool MapDown(int mapping, bool any = false) => base.MapDown(mapping, any);

        protected override PadState GetState(int index)
        {
            PadState padState = new PadState();
            PadState state;
            if (!running)
                state = padState;
            else if (currentInput == null)
            {
                state = padState;
            }
            else
            {
                foreach (object key in Enum.GetValues(typeof(PadButton)))
                {
                    int index1;
                    if (tasInput.Keys.TryGetValue((int)key, out index1) && currentInput.inputs[index1] > 0)
                        padState.buttons |= (PadButton)key;
                }
                padState.triggers.left = currentInput.lTrigger;
                padState.triggers.right = currentInput.rTrigger;
                state = padState;
            }
            return state;
        }
    }
}
