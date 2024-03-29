﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.RecDuck
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;

namespace DuckGame
{
    public class RecDuck : Duck
    {
        private static Comparison<MaterialThing> XHspeedPositive = new Comparison<MaterialThing>(SortCollisionXHspeedPositive);
        private static Comparison<MaterialThing> XHspeedNegative = new Comparison<MaterialThing>(SortCollisionXHspeedNegative);
        private static Comparison<MaterialThing> YVspeedPositive = new Comparison<MaterialThing>(SortCollisionYVspeedPositive);
        private static Comparison<MaterialThing> YVspeedNegative = new Comparison<MaterialThing>(SortCollisionYVspeedNegative);
        public static bool doduckonduck;
        public static bool physicsfcksit = true;

        public RecDuck(float xval, float yval)
          : base(xval, yval, null)
        {
            _didImpactSound = true;
        }

        public override void UpdatePhysics()
        {
            if (physicsfcksit)
                return;
            if (framesSinceGrounded > 10)
                framesSinceGrounded = 10;
            _lastPosition = position;
            _lastVelocity = velocity;
            if (!solid || !enablePhysics || level != null && !level.simulatePhysics)
            {
                lastGrounded = DateTime.Now;
                if (solid)
                    return;
                solidImpacting.Clear();
                impacting.Clear();
            }
            else
            {
                if (!(HelperG.GfieldVal("PhysicsObject", "_collisionPred", this) is Predicate<MaterialThing> match))
                {
                    match = thing => thing == null || !Collision.Rect(topLeft, bottomRight, thing) || thing is Duck;
                    HelperG.SfieldVal("PhysicsObject", "_collisionPred", match, this);
                }
                _curPuddle = null;
                if (!skipClip)
                {
                    clip.RemoveWhere(match);
                    impacting.RemoveWhere(match);
                }
                if (_sleeping)
                {
                    bool flag = (bool)HelperG.GfieldVal("PhysicsObject", "_awaken", this);
                    if (hSpeed == 0.0 && this.vSpeed == 0.0 && heat <= 0.0 && !flag)
                        return;
                    _sleeping = false;
                    HelperG.SfieldVal("PhysicsObject", "_awaken", false, this);
                }
                if (!skipClip)
                    solidImpacting.RemoveWhere(match);
                float currentFriction = this.currentFriction;
                if (sliding || crouch)
                    currentFriction *= 0.28f;
                float num1 = currentFriction * specialFrictionMod;
                if (owner is Duck)
                    gravMultiplier = 1f;
                if (hSpeed > -(double)num1 && hSpeed < (double)num1)
                    hSpeed = 0.0f;
                if (duck)
                {
                    if (hSpeed > 0.0)
                        hSpeed -= num1;
                    if (hSpeed < 0.0)
                        hSpeed += num1;
                }
                else if (grounded)
                {
                    if (hSpeed > 0.0)
                        hSpeed -= num1;
                    if (hSpeed < 0.0)
                        hSpeed += num1;
                }
                else
                {
                    if (isServerForObject && y > Level.current.lowestPoint + 500.0)
                    {
                        removedFromFall = true;
                        if (this != null)
                            return;
                        Level.Remove(this);
                    }
                    if (hSpeed > 0.0)
                        hSpeed -= num1 * 0.7f * airFrictionMult;
                    if (hSpeed < 0.0)
                        hSpeed += num1 * 0.7f * airFrictionMult;
                }
                if (hSpeed > (double)hMax)
                    hSpeed = hMax;
                if (hSpeed < -(double)hMax)
                    hSpeed = -hMax;
                Vec2 p1_1 = topLeft + new Vec2(0.0f, 0.5f);
                Vec2 p2_1 = bottomRight + new Vec2(0.0f, -0.5f);
                lastHSpeed = hSpeed;
                float num2 = 0.0f;
                bool flag1 = false;
                if (hSpeed != 0.0)
                {
                    int num3 = (int)Math.Ceiling(Math.Abs(this.hSpeed) / 4.0);
                    float hSpeed = this.hSpeed;
                    if (this.hSpeed < 0.0)
                    {
                        p1_1.x += this.hSpeed;
                        p2_1.x -= 2f;
                    }
                    else
                    {
                        p2_1.x += this.hSpeed;
                        p1_1.x += 2f;
                    }
                    bool flag2 = (bool)HelperG.GfieldVal("PhysicsObject", "_awaken", this);
                    if (this.hSpeed == 0.0 && this.vSpeed == 0.0 && heat <= 0.0 && !flag2)
                        return;
                    _sleeping = false;
                    List<MaterialThing> materialThingList = new List<MaterialThing>();
                    HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList, this);
                    Level.CheckRectAll(p1_1, p2_1, materialThingList);
                    HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList, this);
                    if (Network.isActive && !isServerForObject && Math.Abs(this.hSpeed) > 0.5 && doduckonduck)
                    {
                        List<Duck> outList = HelperG.GfieldVal("PhysicsObject", "_hitDucks", this) as List<Duck>;
                        outList.Clear();
                        Level.CheckRectAll(p1_1 + new Vec2(this.hSpeed * 2f, 0.0f), p2_1 + new Vec2(this.hSpeed * 2f, 0.0f), outList);
                        for (int index = outList.Count - 1; index >= 0; --index)
                            outList.RemoveAt(index);
                        foreach (Duck duck in outList)
                        {
                            if (this.hSpeed > 0.0)
                                duck.Impact(this, ImpactedFrom.Left, true);
                            else if (this.hSpeed < 0.0)
                                duck.Impact(this, ImpactedFrom.Right, true);
                        }
                        HelperG.SfieldVal("PhysicsObject", "_hitDucks", outList, this);
                    }
                    if (this.hSpeed > 0.0)
                        DGList.Sort(materialThingList, XHspeedPositive);
                    else
                        DGList.Sort(materialThingList, XHspeedNegative);
                    HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList, this);
                    for (int index1 = 0; index1 < num3; ++index1)
                    {
                        float num4 = this.hSpeed / num3;
                        if (num4 != 0.0 && Math.Sign(num4) == Math.Sign(hSpeed))
                        {
                            x += num4;
                            _inPhysicsLoop = true;
                            bool flag3 = false;
                            for (int index2 = materialThingList.Count - 1; index2 >= 0; --index2)
                            {
                                MaterialThing materialThing = materialThingList[index2];
                                if (materialThing is Duck)
                                    materialThingList.RemoveAt(index2);
                                if (materialThing is RagdollPart && !(materialThing is RecRagdollPart))
                                    materialThingList.RemoveAt(index2);
                            }
                            foreach (MaterialThing with in materialThingList)
                            {
                                if ((doduckonduck || !(with is Duck) && !(with is RagdollPart)) && !(with is Goody) && with != this && !clip.Contains(with) && !with.clip.Contains(this) && with.solid && (planeOfExistence == 4 || with.planeOfExistence == planeOfExistence) && (!flag3 || with is Block))
                                {
                                    Vec2 position = this.position;
                                    bool flag4 = false;
                                    if (with.left <= (double)right && with.left > (double)left)
                                    {
                                        flag4 = true;
                                        if (this.hSpeed > 0.0)
                                        {
                                            HelperG.SfieldVal("PhysicsObject", "_collideRight", with, this);
                                            if (with is Block)
                                            {
                                                HelperG.SfieldVal("PhysicsObject", "_wallCollideRight", with, this);
                                                flag3 = true;
                                            }
                                            with.Impact(this, ImpactedFrom.Left, true);
                                            Impact(with, ImpactedFrom.Right, true);
                                        }
                                    }
                                    if (with.right >= (double)left && with.right < (double)right)
                                    {
                                        flag4 = true;
                                        if (this.hSpeed < 0.0)
                                        {
                                            HelperG.SfieldVal("PhysicsObject", "_collideLeft", with, this);
                                            if (with is Block)
                                            {
                                                HelperG.SfieldVal("PhysicsObject", "_wallCollideLeft", with, this);
                                                flag3 = true;
                                            }
                                            with.Impact(this, ImpactedFrom.Right, true);
                                            Impact(with, ImpactedFrom.Left, true);
                                        }
                                    }
                                    if (with is IBigStupidWall && (position - this.position).length > 64.0)
                                        this.position = position;
                                    if (flag4)
                                    {
                                        with.Touch(this);
                                        Touch(with);
                                    }
                                }
                            }
                            _inPhysicsLoop = false;
                        }
                        else
                            break;
                    }
                }
                if (flag1)
                    x = num2;
                if (this.vSpeed > (double)vMax)
                    this.vSpeed = vMax;
                if (this.vSpeed < -(double)vMax)
                    this.vSpeed = -vMax;
                this.vSpeed += currentGravity;
                if (this.vSpeed < 0.0)
                    grounded = false;
                grounded = false;
                ++framesSinceGrounded;
                if (this.vSpeed <= 0.0)
                    Math.Floor(this.vSpeed);
                else
                    Math.Ceiling(this.vSpeed);
                Vec2 p1_2 = topLeft + new Vec2(0.5f, 0.0f);
                Vec2 p2_2 = bottomRight + new Vec2(-0.5f, 0.0f);
                float num5 = -9999f;
                bool flag5 = false;
                float vSpeed = this.vSpeed;
                lastVSpeed = this.vSpeed;
                if (this.vSpeed < 0.0)
                {
                    p1_2.y += this.vSpeed;
                    p2_2.y -= 2f;
                }
                else
                {
                    p2_2.y += this.vSpeed;
                    p1_2.y += 2f;
                }
                List<MaterialThing> materialThingList1 = new List<MaterialThing>();
                HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList1, this);
                Level.CheckRectAll(p1_2, p2_2, materialThingList1);
                if (this.vSpeed > 0.0)
                    DGList.Sort(materialThingList1, YVspeedPositive);
                else
                    DGList.Sort(materialThingList1, YVspeedNegative);
                HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList1, this);
                double top = this.top;
                double bottom = this.bottom;
                if (this != null && inputProfile.Down("DOWN"))
                {
                    int jumpValid = _jumpValid;
                }
                int num6 = (int)Math.Ceiling(Math.Abs(this.vSpeed) / 4.0);
                for (int index3 = 0; index3 < num6; ++index3)
                {
                    float num7 = this.vSpeed / num6;
                    if (num7 != 0.0 && Math.Sign(num7) == Math.Sign(vSpeed))
                    {
                        y += num7;
                        _inPhysicsLoop = true;
                        for (int index4 = materialThingList1.Count - 1; index4 >= 0; --index4)
                        {
                            MaterialThing materialThing = materialThingList1[index4];
                            if (materialThing is Duck)
                                materialThingList1.RemoveAt(index4);
                            if (materialThing is RagdollPart && !(materialThing is RecRagdollPart))
                                materialThingList1.RemoveAt(index4);
                        }
                        for (int index5 = 0; index5 < materialThingList1.Count; ++index5)
                        {
                            MaterialThing with = materialThingList1[index5];
                            if (doduckonduck || !(with is Duck) && !(with is RagdollPart))
                            {
                                if (with is FluidPuddle)
                                {
                                    flag5 = true;
                                    _curPuddle = with as FluidPuddle;
                                    if (with.top < this.bottom - 2.0 && with.collisionSize.y > 2.0)
                                        num5 = with.top;
                                }
                                if (!(with is Goody) && with != this && !clip.Contains(with) && !with.clip.Contains(this) && with.solid && (planeOfExistence == 4 || with.planeOfExistence == planeOfExistence))
                                {
                                    Vec2 position = this.position;
                                    bool flag6 = false;
                                    if (with.bottom >= (double)this.top && with.top < (double)this.top)
                                    {
                                        flag6 = true;
                                        if (this.vSpeed < 0.0)
                                        {
                                            double y = this.y;
                                            HelperG.SfieldVal("PhysicsObject", "_collideTop", with, this);
                                            with.Impact(this, ImpactedFrom.Bottom, true);
                                            Impact(with, ImpactedFrom.Top, true);
                                        }
                                    }
                                    if (with.top <= (double)this.bottom && with.bottom > (double)this.bottom)
                                    {
                                        flag6 = true;
                                        if (this.vSpeed > 0.0)
                                        {
                                            HelperG.SfieldVal("PhysicsObject", "_collideBottom", with, this);
                                            with.Impact(this, ImpactedFrom.Top, true);
                                            Impact(with, ImpactedFrom.Bottom, true);
                                        }
                                    }
                                    if (with is IBigStupidWall && (position - this.position).length > 64.0)
                                        this.position = position;
                                    if (flag6)
                                    {
                                        with.Touch(this);
                                        Touch(with);
                                    }
                                }
                            }
                        }
                        _inPhysicsLoop = false;
                    }
                    else
                        break;
                }
                if (grounded)
                {
                    lastGrounded = DateTime.Now;
                    framesSinceGrounded = 0;
                    Thing thing = HelperG.GfieldVal("PhysicsObject", "_collideBottom", this) as Thing;
                    if (!doFloat && hSpeed == 0.0 && this.vSpeed == 0.0)
                    {
                        switch (thing)
                        {
                            case Block _:
                            case IPlatform _:
                                if (!(thing is ItemBox) || (thing as ItemBox).canBounce)
                                {
                                    _sleeping = true;
                                    break;
                                }
                                break;
                        }
                    }
                }
                if (num5 > -999.0)
                {
                    if (!doFloat && this.vSpeed > 1.0)
                    {
                        FluidData fluidData = (FluidData)HelperG.GfieldVal("PhysicsObject", "_curFluid", this);
                        SFX.Play("largeSplash", Rando.Float(0.6f, 0.7f), Rando.Float(-0.7f, -0.2f));
                    }
                    doFloat = true;
                }
                else
                    doFloat = false;
                if (_curPuddle != null)
                {
                    HelperG.SfieldVal("PhysicsObject", "_curFluid", _curPuddle.data, this);
                    if (onFire && _curPuddle.data.flammable <= 0.5 && _curPuddle.data.heat <= 0.5)
                        Extinquish();
                    else if (_curPuddle.data.heat > 0.5)
                    {
                        if (flammable > 0.0 && isServerForObject)
                        {
                            bool onFire = this.onFire;
                            Burn(position, this);
                            if (this != null && this.onFire && !onFire)
                                ThrowItem();
                        }
                        DoHeatUp(0.015f, position);
                    }
                    else
                        DoHeatUp(-0.05f, position);
                }
                if (doFloat)
                {
                    if (this != null && crouch)
                    {
                        if (floatMultiplier > 0.98f)
                            this.vSpeed *= 0.8f;
                        floatMultiplier = 0.8f;
                    }
                    else
                    {
                        if (floatMultiplier > 0.98f)
                            this.vSpeed *= 0.4f;
                        this.vSpeed *= 0.95f;
                        floatMultiplier = 0.4f;
                    }
                }
                else
                {
                    if (flag5 && vSpeed > 1.0 && Math.Abs(this.vSpeed) < 0.01f)
                    {
                        FluidData fluidData = (FluidData)HelperG.GfieldVal("PhysicsObject", "_curFluid", this);
                        SFX.Play("littleSplash", Rando.Float(0.8f, 0.9f), Rando.Float(-0.2f, 0.2f));
                    }
                    floatMultiplier = 1f;
                }
                Recorder.LogVelocity(Math.Abs(hSpeed) + Math.Abs(this.vSpeed));
                if (_sleeping)
                    return;
                if (modFric)
                    modFric = false;
                else
                    specialFrictionMod = 1f;
            }
        }

        public void baseupdate() => _didImpactSound = false;

        public override bool PlayCollideSound(ImpactedFrom from) => true;

        public override bool Destroy(DestroyType type = null)
        {
            _destroyed = true;
            return true;
        }
    }
}
