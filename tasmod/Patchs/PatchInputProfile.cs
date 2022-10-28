using DuckGame;
using HarmonyInternal;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace RunTimeEdit
{
    public class PatchInputProfile
    {
        private static FieldInfo _ignoreInputField = typeof(Input).GetField("_ignoreInput");
        private static bool dothing;

        private static VirtualInput _virtualInput(InputProfile t) => typeof(InputProfile).GetField(nameof(_virtualInput), BindingFlags.Instance | BindingFlags.NonPublic).GetValue(t) as VirtualInput;

        private static Dictionary<InputDevice, MultiMap<string, int>> _mappings(
          InputProfile t)
        {
            return typeof(InputProfile).GetField(nameof(_mappings), BindingFlags.Instance | BindingFlags.NonPublic).GetValue(t) as Dictionary<InputDevice, MultiMap<string, int>>;
        }

        private static Dictionary<PadButton, InputState> _leftStickStates(
          InputProfile t)
        {
            return typeof(InputProfile).GetField(nameof(_leftStickStates), BindingFlags.Instance | BindingFlags.NonPublic).GetValue(t) as Dictionary<PadButton, InputState>;
        }

        private static bool PrefixDown(InputProfile __instance, string trigger, ref bool __result)
        {
            if (Input.ignoreInput && NotTasInput() && _virtualInput(__instance) == null)
            {
                __result = false;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                InputDevice key = mapping.Key;
                List<int> list = new List<int>();
                if ((_virtualInput(__instance) == null || key is VirtualInput) && mapping.Value.TryGetValue(trigger, out list))
                {
                    foreach (int num in list)
                    {
                        if ((key is AnalogGamePad || key is GenericController) && _leftStickStates(__instance).ContainsKey((PadButton)num) && (_leftStickStates(__instance)[(PadButton)num] == InputState.Down || _leftStickStates(__instance)[(PadButton)num] == InputState.Pressed))
                        {
                            __instance.lastPressFrame = Graphics.frame;
                            __result = true;
                            return false;
                        }
                        if (key.MapDown(num))
                        {
                            if ((!(key is Keyboard) || !DuckNetwork.core.enteringText) && !(key is VirtualInput))
                            {
                                __instance.lastActiveDevice = mapping.Key;
                                Input.lastActiveProfile = __instance;
                            }
                            __instance.lastPressFrame = Graphics.frame;
                            __result = true;
                            return false;
                        }
                    }
                }
            }
            __result = false;
            return false;
        }

        private static Vec2 _mouseAnchor(InputProfile t) => (Vec2)typeof(InputProfile).GetField(nameof(_mouseAnchor), BindingFlags.Instance | BindingFlags.NonPublic).GetValue(t);

        private static bool Prefixget_hasMotionAxis(InputProfile __instance, ref bool __result)
        {
            if (Input.ignoreInput && NotTasInput())
            {
                __result = false;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                if (_virtualInput(__instance) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        return analogGamePad.hasMotionAxis;
                    }
                }
            }
            __result = false;
            return false;
        }

        private static bool Prefixget_leftTrigger(InputProfile __instance, ref float __result)
        {
            if (_virtualInput(__instance) != null)
            {
                __result = _virtualInput(__instance).leftTrigger;
                return false;
            }
            if (Input.ignoreInput && NotTasInput())
            {
                __result = 0.0f;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                if (_virtualInput(__instance) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        List<int> list = null;
                        if (!mapping.Value.TryGetValue("LTRIGGER", out list) || list.Count <= 0)
                        {
                            __result = analogGamePad.leftTrigger;
                            return false;
                        }
                        int num = list[0];
                        if (num == 8388608)
                        {
                            __result = analogGamePad.leftTrigger;
                            return false;
                        }
                        if (num == 4194304)
                        {
                            __result = analogGamePad.rightTrigger;
                            return false;
                        }
                    }
                }
            }
            __result = 0.0f;
            return false;
        }

        private static bool Prefixget_motionAxis(InputProfile __instance, ref float __result)
        {
            if (Input.ignoreInput && NotTasInput())
            {
                __result = 0.0f;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                if (_virtualInput(__instance) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        __result = analogGamePad.motionAxis;
                        return false;
                    }
                }
            }
            __result = 0.0f;
            return false;
        }

        private static bool Prefixget_rightStick(InputProfile __instance, ref Vec2 __result)
        {
            if (_virtualInput(__instance) != null)
            {
                __result = _virtualInput(__instance).rightStick;
                return false;
            }
            if (Input.ignoreInput && NotTasInput())
            {
                __result = Vec2.Zero;
                return false;
            }
            if (Mouse.left == InputState.Pressed || _mouseAnchor(__instance) == Vec2.Zero)
            {
                typeof(InputProfile).GetField("_mouseAnchor", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, Mouse.position);
            }
            else
            {
                switch (Mouse.left)
                {
                    case InputState.None:
                        typeof(InputProfile).GetField("_mouseAnchor", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, Vec2.Zero);
                        break;
                    case InputState.Down:
                        Vec2 vec2 = (Mouse.position - _mouseAnchor(__instance)) / 16f;
                        vec2.y *= -1f;
                        float num = vec2.length;
                        if (num > 1.0)
                            num = 1f;
                        __result = vec2.normalized * num;
                        return false;
                }
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                if (_virtualInput(__instance) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        List<int> list = null;
                        if (!mapping.Value.TryGetValue("RSTICK", out list) || list.Count <= 0)
                        {
                            __result = analogGamePad.rightStick;
                            return false;
                        }
                        switch (list[0])
                        {
                            case 64:
                                __result = analogGamePad.leftStick;
                                return false;
                            case 128:
                                __result = analogGamePad.rightStick;
                                return false;
                            default:
                                continue;
                        }
                    }
                }
            }
            __result = new Vec2(0.0f, 0.0f);
            return false;
        }

        private static bool Prefixget_rightTrigger(InputProfile __instance, ref float __result)
        {
            if (_virtualInput(__instance) != null)
            {
                __result = _virtualInput(__instance).rightTrigger;
                return false;
            }
            if (Input.ignoreInput && NotTasInput())
            {
                __result = 0.0f;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                if (_virtualInput(__instance) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        List<int> list = null;
                        if (!mapping.Value.TryGetValue("RTRIGGER", out list) || list.Count <= 0)
                        {
                            __result = analogGamePad.rightTrigger;
                            return false;
                        }
                        switch (list[0])
                        {
                            case 4194304:
                                __result = analogGamePad.rightTrigger;
                                return false;
                            case 8388608:
                                __result = analogGamePad.leftTrigger;
                                return false;
                            default:
                                continue;
                        }
                    }
                }
            }
            __result = 0.0f;
            return false;
        }

        private static bool PrefixPressed(
          InputProfile __instance,
          ref bool __result,
          string trigger,
          bool any)
        {
            if (Input.ignoreInput && NotTasInput() && _virtualInput(__instance) == null)
            {
                __result = false;
                return false;
            }
            if (trigger == "ANY")
                any = true;
            if ((typeof(InputProfile).GetField("_repeatList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as List<string>).Contains(trigger))
            {
                __result = true;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                InputDevice key = mapping.Key;
                if ((!(key is Keyboard) || !InputProfile.ignoreKeyboard) && (_virtualInput(__instance) == null || key is VirtualInput))
                {
                    if (any)
                    {
                        if (key.MapPressed(-1, true))
                        {
                            __instance.lastPressFrame = Graphics.frame;
                            __result = true;
                            return false;
                        }
                    }
                    else
                    {
                        List<int> list;
                        if (mapping.Value.TryGetValue(trigger, out list))
                        {
                            foreach (int num in list)
                            {
                                if ((key is AnalogGamePad || key is GenericController) && _leftStickStates(__instance).ContainsKey((PadButton)num) && _leftStickStates(__instance)[(PadButton)num] == InputState.Pressed)
                                {
                                    __result = true;
                                    return false;
                                }
                                if (key.MapPressed(num, any))
                                {
                                    __instance.lastPressFrame = Graphics.frame;
                                    __result = true;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            __result = false;
            return false;
        }

        private static bool PrefixReleased(InputProfile __instance, ref bool __result, string trigger)
        {
            if (Input.ignoreInput && NotTasInput() && _virtualInput(__instance) == null)
            {
                __result = false;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                InputDevice key = mapping.Key;
                List<int> list;
                if ((_virtualInput(__instance) == null || key is VirtualInput) && mapping.Value.TryGetValue(trigger, out list))
                {
                    foreach (int num in list)
                    {
                        if ((key is AnalogGamePad || key is GenericController) && _leftStickStates(__instance).ContainsKey((PadButton)num) && _leftStickStates(__instance)[(PadButton)num] == InputState.Released)
                        {
                            __instance.lastPressFrame = Graphics.frame;
                            __result = true;
                            return false;
                        }
                        if (key.MapReleased(num))
                        {
                            __instance.lastPressFrame = Graphics.frame;
                            __result = true;
                            return false;
                        }
                    }
                }
            }
            __result = false;
            return false;
        }

        private static bool Prefixget_leftStick(InputProfile __instance, ref object __result)
        {
            if (_virtualInput(__instance) != null)
            {
                __result = _virtualInput(__instance).leftStick;
                return false;
            }
            if (Input.ignoreInput && NotTasInput())
            {
                __result = Vec2.Zero;
                return false;
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(__instance))
            {
                if (_virtualInput(__instance) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        List<int> list = null;
                        if (!mapping.Value.TryGetValue("LSTICK", out list) || list.Count <= 0)
                        {
                            __result = analogGamePad.leftStick;
                            return false;
                        }
                        switch (list[0])
                        {
                            case 64:
                                __result = analogGamePad.leftStick;
                                return false;
                            case 128:
                                __result = analogGamePad.rightStick;
                                return false;
                            default:
                                continue;
                        }
                    }
                }
            }
            __result = new Vec2(0.0f, 0.0f);
            return false;
        }

        private static IEnumerable<CodeInstruction> Transpilerget_leftStick(
          IEnumerable<CodeInstruction> instructions)
        {
            return new List<CodeInstruction>()
      {
        new CodeInstruction(OpCodes.Ldarg_0),
        new CodeInstruction(OpCodes.Call,  TasMod.SGMI(typeof (PatchInputProfile), "getleftStick")),
        new CodeInstruction(OpCodes.Ret)
      };
        }

        private static IEnumerable<CodeInstruction> Transpilerget_rightStick(
          IEnumerable<CodeInstruction> instructions)
        {
            return new List<CodeInstruction>()
      {
        new CodeInstruction(OpCodes.Ldarg_0),
        new CodeInstruction(OpCodes.Call,  TasMod.SGMI(typeof (PatchInputProfile), "getrightStick")),
        new CodeInstruction(OpCodes.Ret)
      };
        }

        public static Vec2 getrightStick(InputProfile p)
        {
            if (_virtualInput(p) != null)
                return _virtualInput(p).rightStick;
            if (Input.ignoreInput && NotTasInput())
                return Vec2.Zero;
            if (Mouse.left == InputState.Pressed || _mouseAnchor(p) == Vec2.Zero)
            {
                typeof(InputProfile).GetField("_mouseAnchor", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, Mouse.position);
            }
            else
            {
                switch (Mouse.left)
                {
                    case InputState.None:
                        typeof(InputProfile).GetField("_mouseAnchor", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, Vec2.Zero);
                        break;
                    case InputState.Down:
                        Vec2 vec2 = (Mouse.position - _mouseAnchor(p)) / 16f;
                        vec2.y *= -1f;
                        float num = vec2.length;
                        if (num > 1.0)
                            num = 1f;
                        return vec2.normalized * num;
                }
            }
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(p))
            {
                if (_virtualInput(p) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        List<int> list = null;
                        if (!mapping.Value.TryGetValue("RSTICK", out list) || list.Count <= 0)
                            return analogGamePad.rightStick;
                        switch (list[0])
                        {
                            case 64:
                                return analogGamePad.leftStick;
                            case 128:
                                return analogGamePad.rightStick;
                            default:
                                continue;
                        }
                    }
                }
            }
            return new Vec2(0.0f, 0.0f);
        }

        public static Vec2 getleftStick(InputProfile p)
        {
            if (_virtualInput(p) != null)
                return _virtualInput(p).leftStick;
            if (Input.ignoreInput && NotTasInput())
                return Vec2.Zero;
            foreach (KeyValuePair<InputDevice, MultiMap<string, int>> mapping in _mappings(p))
            {
                if (_virtualInput(p) == null || mapping.Key is VirtualInput)
                {
                    AnalogGamePad analogGamePad = mapping.Key as AnalogGamePad;
                    if (analogGamePad == null && mapping.Key is GenericController)
                    {
                        analogGamePad = (mapping.Key as GenericController).device;
                    }
                    if (analogGamePad != null)
                    {
                        List<int> list = null;
                        if (!mapping.Value.TryGetValue("LSTICK", out list) || list.Count <= 0)
                            return analogGamePad.leftStick;
                        switch (list[0])
                        {
                            case 64:
                                return analogGamePad.leftStick;
                            case 128:
                                return analogGamePad.rightStick;
                            default:
                                continue;
                        }
                    }
                }
            }
            return new Vec2(0.0f, 0.0f);
        }

        private static bool NotTasInput() => InputProfile.DefaultPlayer1 == null || InputProfile.DefaultPlayer1.GetDevice(typeof(tasDevice)) == null;
    }
}
