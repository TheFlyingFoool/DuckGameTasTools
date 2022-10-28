using System;
using DuckGame;

namespace RunTimeEdit
{
	internal class SpringPatch
	{
		public SpringPatch()
		{
		}

		private static void DoRumble(Duck duck)
		{
			RumbleManager.AddRumbleEvent(duck.profile, new RumbleEvent(RumbleIntensity.Kick, RumbleDuration.Short, RumbleFalloff.None, RumbleType.Gameplay));
		}

		private static bool BPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (with.vSpeed > -22f * num)
				{
					with.vSpeed = -22f * num;
				}
				if (with is RagdollPart)
				{
					if (Math.Abs(with.hSpeed) < 0.1f)
					{
						with.hSpeed = ((Rando.Float(1f) >= 0.5f) ? 1.3f : -1.3f);
					}
					else
					{
						with.hSpeed *= Rando.Float(1.1f, 1.4f);
					}
				}
				if (with is Mine)
				{
					if (Math.Abs(with.hSpeed) < 0.1f)
					{
						with.hSpeed = ((Rando.Float(1f) >= 0.5f) ? 1.2f : -1.2f);
					}
					else
					{
						with.hSpeed *= Rando.Float(1.1f, 1.2f);
					}
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
				if (with is Duck)
				{
					(with as Duck).jumping = false;
					if (!flag)
					{
						SpringPatch.DoRumble(with as Duck);
					}
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool DownPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (with.vSpeed < 12f * num)
				{
					with.vSpeed = 12f * num;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
					if (!flag)
					{
						SpringPatch.DoRumble(with as Duck);
					}
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool DownLeftPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (with.vSpeed < 22f * num)
				{
					with.vSpeed = 22f * num;
				}
				if (!__instance.flipHorizontal)
				{
					if (__instance.purple)
					{
						if (with.hSpeed > -7f)
						{
							with.hSpeed = -7f;
						}
					}
					else if (with.hSpeed > -10f)
					{
						with.hSpeed = -10f;
					}
				}
				else if (__instance.purple)
				{
					if (with.hSpeed < 7f)
					{
						with.hSpeed = 7f;
					}
				}
				else if (with.hSpeed < 10f)
				{
					with.hSpeed = 10f;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool DownRightPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (with.vSpeed < 22f * num)
				{
					with.vSpeed = 22f * num;
				}
				if (__instance.flipHorizontal)
				{
					if (__instance.purple)
					{
						if (with.hSpeed > -7f)
						{
							with.hSpeed = -7f;
						}
					}
					else if (with.hSpeed > -10f)
					{
						with.hSpeed = -10f;
					}
				}
				else if (__instance.purple)
				{
					if (with.hSpeed < 7f)
					{
						with.hSpeed = 7f;
					}
				}
				else if (with.hSpeed < 10f)
				{
					with.hSpeed = 10f;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool LeftPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			bool flag2 = (bool)__instance.GetMemberValue("_flipHorizontal");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (!flag2)
				{
					if (with.hSpeed > -12f * num)
					{
						with.hSpeed = -12f * num;
					}
				}
				else if (with.hSpeed < 12f * num)
				{
					with.hSpeed = 12f * num;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
					if (!flag)
					{
						SpringPatch.DoRumble(with as Duck);
					}
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool RightPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			bool flag2 = (bool)__instance.GetMemberValue("_flipHorizontal");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (!flag2)
				{
					if (with.hSpeed < 12f * num)
					{
						with.hSpeed = 12f * num;
					}
				}
				else if (with.hSpeed > -12f * num)
				{
					with.hSpeed = -12f * num;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
					if (!flag)
					{
						SpringPatch.DoRumble(with as Duck);
					}
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool UpLeftPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (with.vSpeed > -22f * num)
				{
					with.vSpeed = -22f * num;
				}
				if (!__instance.flipHorizontal)
				{
					if (__instance.purple)
					{
						if (with.hSpeed > -7f)
						{
							with.hSpeed = -7f;
						}
					}
					else if (with.hSpeed > -10f)
					{
						with.hSpeed = -10f;
					}
				}
				else if (__instance.purple)
				{
					if (with.hSpeed < 7f)
					{
						with.hSpeed = 7f;
					}
				}
				else if (with.hSpeed < 10f)
				{
					with.hSpeed = 10f;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
					if (!flag)
					{
						SpringPatch.DoRumble(with as Duck);
					}
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}

		private static bool UpRightPrefix(Spring __instance, MaterialThing with)
		{
			bool flag = !(with is PhysicsObject) || showframe.SimphysicsObjects.Contains(with as PhysicsObject);
			float num = (float)__instance.GetMemberValue("_mult");
			if (with.isServerForObject && with.Sprung(__instance))
			{
				if (with.vSpeed > -22f * num)
				{
					with.vSpeed = -22f * num;
				}
				if (__instance.flipHorizontal)
				{
					if (__instance.purple)
					{
						if (with.hSpeed > -7f)
						{
							with.hSpeed = -7f;
						}
					}
					else if (with.hSpeed > -10f)
					{
						with.hSpeed = -10f;
					}
				}
				else if (__instance.purple)
				{
					if (with.hSpeed < 7f)
					{
						with.hSpeed = 7f;
					}
				}
				else if (with.hSpeed < 10f)
				{
					with.hSpeed = 10f;
				}
				if (with is Gun)
				{
					(with as Gun).PressAction();
				}
				if (with is Duck)
				{
					(with as Duck).jumping = false;
					if (!flag)
					{
						SpringPatch.DoRumble(with as Duck);
					}
				}
				with.lastHSpeed = with._hSpeed;
				with.lastVSpeed = with._vSpeed;
			}
			if (!flag)
			{
				__instance.SpringUp();
			}
			return false;
		}
	}
}
