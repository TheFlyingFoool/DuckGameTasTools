using System;
using DuckGame;

namespace RunTimeEdit
{
	internal class RagdollPatch
	{
		public RagdollPatch()
		{
		}

		private static void PrefixUpdateInput(Ragdoll __instance)
		{
			float num = (float)__instance.GetMemberValue("sleepingBagTimer");
			num -= Maths.IncFrameTimer();
			__instance.SetMemberValue("sleepingBagTimer", num);
			if (num < 0f && __instance.sleepingBagHealth > 20)
			{
				__instance.sleepingBagHealth -= 4;
				num = 1f;
				__instance.SetMemberValue("sleepingBagTimer", num);
			}
			if (!__instance._duck.dead)
			{
				if (__instance._duck.HasEquipment(typeof(FancyShoes)) && !__instance.jetting)
				{
					if (__instance.captureDuck.inputProfile.Pressed("RIGHT", false))
					{
						Vec2 value = (__instance._part1.position - __instance._part2.position).Rotate(1.5707964f, Vec2.Zero);
						__instance.part1.velocity += value * 0.2f;
						value = (__instance._part3.position - __instance._part2.position).Rotate(1.5707964f, Vec2.Zero);
						__instance.part3.velocity += value * 0.2f;
					}
					else if (__instance.captureDuck.inputProfile.Pressed("LEFT", false))
					{
						Vec2 value2 = (__instance._part1.position - __instance._part2.position).Rotate(1.5707964f, Vec2.Zero);
						__instance.part1.velocity += value2 * -0.2f;
						value2 = (__instance._part3.position - __instance._part2.position).Rotate(1.5707964f, Vec2.Zero);
						__instance.part3.velocity += value2 * -0.2f;
					}
				}
				else if (__instance._timeSinceNudge > 1f && !__instance.jetting)
				{
					if (__instance.captureDuck.inputProfile.Pressed("LEFT", false))
					{
						float num2 = NetRand.Float(-2f, 2f);
						__instance._part1.vSpeed += num2;
						__instance._part3.vSpeed += NetRand.Float(-2f, 2f);
						__instance._part2.hSpeed += NetRand.Float(-2f, -1.2f);
						__instance._part2.vSpeed -= NetRand.Float(1f, 1.5f);
						__instance._timeSinceNudge = 0f;
						__instance.ShakeOutOfSleepingBag();
					}
					else if (__instance.captureDuck.inputProfile.Pressed("RIGHT", false))
					{
						__instance._part1.vSpeed += NetRand.Float(-2f, 2f);
						__instance._part3.vSpeed += NetRand.Float(-2f, 2f);
						__instance._part2.hSpeed += NetRand.Float(1.2f, 2f);
						__instance._part2.vSpeed -= NetRand.Float(1f, 1.5f);
						__instance._timeSinceNudge = 0f;
						__instance.ShakeOutOfSleepingBag();
					}
					else if (__instance.captureDuck.inputProfile.Pressed("UP", false))
					{
						if (RagdollPatch.maxragjump)
						{
							__instance._part1.vSpeed += -2f;
							__instance._part3.vSpeed += -2f;
							__instance._part2.vSpeed -= 2f;
						}
						else
						{
							__instance._part1.vSpeed += NetRand.Float(-2f, 1f);
							__instance._part3.vSpeed += NetRand.Float(-2f, 1f);
							__instance._part2.vSpeed -= NetRand.Float(1.5f, 2f);
						}
						__instance._timeSinceNudge = 0f;
						__instance.ShakeOutOfSleepingBag();
					}
				}
			}
			bool flag = false;
			if (__instance.captureDuck.HasEquipment(typeof(FancyShoes)) && Math.Abs(__instance._part1.x - __instance._part3.x) < 9f && __instance._part1.y < __instance._part3.y)
			{
				flag = true;
			}
			if (__instance.tongueStuckThing != null && __instance.tongueStuckThing.removeFromLevel)
			{
				__instance.tongueStuck = Vec2.Zero;
				if (Network.isActive)
				{
					Thing.Fondle(__instance, DuckNetwork.localConnection);
				}
				__instance._makeActive = true;
			}
			if (!__instance._duck.dead && (__instance.captureDuck.inputProfile.Pressed("RAGDOLL", false) || __instance.captureDuck.inputProfile.Pressed("JUMP", false)) && (__instance._part1.framesSinceGrounded < 5 || __instance._part2.framesSinceGrounded < 5 || __instance._part3.framesSinceGrounded < 5 || __instance._part1.doFloat || __instance.part2.doFloat || __instance._part3.doFloat || flag || __instance._part1.owner != null || __instance._part2.owner != null || __instance._part3.owner != null))
			{
				if (__instance.inSleepingBag)
				{
					if (__instance._timeSinceNudge > 1f)
					{
						__instance._part1.vSpeed += NetRand.Float(-2f, 1f);
						__instance._part3.vSpeed += NetRand.Float(-2f, 1f);
						__instance._part2.vSpeed -= NetRand.Float(1.5f, 2f);
						__instance._timeSinceNudge = 0f;
						__instance.ShakeOutOfSleepingBag();
						return;
					}
				}
				else if (!__instance._part1.held && !__instance._part2.held && !__instance._part3.held && (__instance.tongueStuck == Vec2.Zero || __instance.tongueShakes > 5) && __instance.isServerForObject)
				{
					__instance.tongueStuck = Vec2.Zero;
					if (Network.isActive)
					{
						Thing.Fondle(__instance, DuckNetwork.localConnection);
					}
					__instance._makeActive = true;
				}
			}
		}

		public static bool maxragjump;
	}
}
