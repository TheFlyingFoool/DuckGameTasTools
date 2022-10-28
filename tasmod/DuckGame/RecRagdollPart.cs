// Decompiled with JetBrains decompiler
// Type: DuckGame.RecRagdollPart
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;

namespace DuckGame
{
    public class RecRagdollPart : RagdollPart
    {
        private static Comparison<MaterialThing> XHspeedPositive = new Comparison<MaterialThing>(SortCollisionXHspeedPositive);
        private static Comparison<MaterialThing> XHspeedNegative = new Comparison<MaterialThing>(SortCollisionXHspeedNegative);
        private static Comparison<MaterialThing> YVspeedPositive = new Comparison<MaterialThing>(SortCollisionYVspeedPositive);
        private static Comparison<MaterialThing> YVspeedNegative = new Comparison<MaterialThing>(SortCollisionYVspeedNegative);
        public static bool doduckonduck;

        public RecRagdollPart(
          float xpos,
          float ypos,
          int p,
          DuckPersona persona,
          int off,
          Ragdoll doll)
          : base(xpos, ypos, p, persona, off, doll)
        {
            this._didImpactSound = true;
        }

        public override void UpdatePhysics()
        {
            if (this.framesSinceGrounded > 10)
                this.framesSinceGrounded = 10;
            this._lastPosition = this.position;
            this._lastVelocity = this.velocity;
            if (!this.solid || !this.enablePhysics || this.level != null && !this.level.simulatePhysics)
            {
                this.lastGrounded = DateTime.Now;
                if (this.solid)
                    return;
                this.solidImpacting.Clear();
                this.impacting.Clear();
            }
            else
            {
                if (!(HelperG.GfieldVal("PhysicsObject", "_collisionPred", this) is Predicate<MaterialThing> match))
                {
                    match = thing => thing == null || !Collision.Rect(this.topLeft, this.bottomRight, thing);
                    HelperG.SfieldVal("PhysicsObject", "_collisionPred", match, this);
                }
                HelperG.SfieldVal("PhysicsObject", "_collideLeft", null, this);
                HelperG.SfieldVal("PhysicsObject", "_collideRight", null, this);
                HelperG.SfieldVal("PhysicsObject", "_collideTop", null, this);
                HelperG.SfieldVal("PhysicsObject", "_collideBottom", null, this);
                HelperG.SfieldVal("PhysicsObject", "_wallCollideLeft", null, this);
                HelperG.SfieldVal("PhysicsObject", "_wallCollideRight", null, this);
                this._curPuddle = null;
                if (!this.skipClip)
                {
                    this.clip.RemoveWhere(match);
                    this.impacting.RemoveWhere(match);
                }
                if (this._sleeping)
                {
                    bool flag = (bool)HelperG.GfieldVal("PhysicsObject", "_awaken", this);
                    if (hSpeed == 0.0 && this.vSpeed == 0.0 && heat <= 0.0 && !flag)
                        return;
                    this._sleeping = false;
                    HelperG.SfieldVal("PhysicsObject", "_awaken", false, this);
                }
                if (!this.skipClip)
                    this.solidImpacting.RemoveWhere(match);
                float currentFriction = this.currentFriction;
                if (this.sliding || this.crouch)
                    currentFriction *= 0.28f;
                float num1 = currentFriction * this.specialFrictionMod;
                if (this.owner is Duck)
                    this.gravMultiplier = 1f;
                if (hSpeed > -(double)num1 && hSpeed < (double)num1)
                    this.hSpeed = 0.0f;
                if ((bool)HelperG.GfieldVal("PhysicsObject", "duck", this))
                {
                    if (hSpeed > 0.0)
                        this.hSpeed -= num1;
                    if (hSpeed < 0.0)
                        this.hSpeed += num1;
                }
                else if (this.grounded)
                {
                    if (hSpeed > 0.0)
                        this.hSpeed -= num1;
                    if (hSpeed < 0.0)
                        this.hSpeed += num1;
                }
                else
                {
                    if (this.isServerForObject && y > Level.current.lowestPoint + 500.0)
                    {
                        this.removedFromFall = true;
                        if (this != null)
                            return;
                        Level.Remove(this);
                    }
                    if (hSpeed > 0.0)
                        this.hSpeed -= num1 * 0.7f * this.airFrictionMult;
                    if (hSpeed < 0.0)
                        this.hSpeed += num1 * 0.7f * this.airFrictionMult;
                }
                if (hSpeed > (double)this.hMax)
                    this.hSpeed = this.hMax;
                if (hSpeed < -(double)this.hMax)
                    this.hSpeed = -this.hMax;
                Vec2 p1_1 = this.topLeft + new Vec2(0.0f, 0.5f);
                Vec2 p2_1 = this.bottomRight + new Vec2(0.0f, -0.5f);
                this.lastHSpeed = this.hSpeed;
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
                    this._sleeping = false;
                    List<MaterialThing> materialThingList = new List<MaterialThing>();
                    HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList, this);
                    Level.CheckRectAll(p1_1, p2_1, materialThingList);
                    HelperG.SfieldVal("PhysicsObject", "_hitThings", materialThingList, this);
                    if (Network.isActive && !this.isServerForObject && Math.Abs(this.hSpeed) > 0.5)
                    {
                        List<Duck> outList = HelperG.GfieldVal("PhysicsObject", "_hitDucks", this) as List<Duck>;
                        outList.Clear();
                        Level.CheckRectAll(p1_1 + new Vec2(this.hSpeed * 2f, 0.0f), p2_1 + new Vec2(this.hSpeed * 2f, 0.0f), outList);
                        for (int index = outList.Count - 1; index >= 0; --index)
                        {
                            if (!(outList[index] is RecDuck))
                                outList.RemoveAt(index);
                        }
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
                            this.x += num4;
                            this._inPhysicsLoop = true;
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
                                if (!(with is Goody) && with != this && !this.clip.Contains(with) && !with.clip.Contains(this) && with.solid && (this.planeOfExistence == 4 || with.planeOfExistence == planeOfExistence) && (!flag3 || with is Block))
                                {
                                    Vec2 position = this.position;
                                    bool flag4 = false;
                                    if (with.left <= (double)this.right && with.left > (double)this.left)
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
                                            this.Impact(with, ImpactedFrom.Right, true);
                                        }
                                    }
                                    if (with.right >= (double)this.left && with.right < (double)this.right)
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
                                            this.Impact(with, ImpactedFrom.Left, true);
                                        }
                                    }
                                    if (with is IBigStupidWall && (position - this.position).length > 64.0)
                                        this.position = position;
                                    if (flag4)
                                    {
                                        with.Touch(this);
                                        this.Touch(with);
                                    }
                                }
                            }
                            this._inPhysicsLoop = false;
                        }
                        else
                            break;
                    }
                }
                if (flag1)
                    this.x = num2;
                if (this.vSpeed > (double)this.vMax)
                    this.vSpeed = this.vMax;
                if (this.vSpeed < -(double)this.vMax)
                    this.vSpeed = -this.vMax;
                this.vSpeed += this.currentGravity;
                if (this.vSpeed < 0.0)
                    this.grounded = false;
                this.grounded = false;
                ++this.framesSinceGrounded;
                if (this.vSpeed <= 0.0)
                    Math.Floor(this.vSpeed);
                else
                    Math.Ceiling(this.vSpeed);
                Vec2 p1_2 = this.topLeft + new Vec2(0.5f, 0.0f);
                Vec2 p2_2 = this.bottomRight + new Vec2(-0.5f, 0.0f);
                float num5 = -9999f;
                bool flag5 = false;
                float vSpeed = this.vSpeed;
                this.lastVSpeed = this.vSpeed;
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
                Duck duck1 = null;
                if (duck1 != null && duck1.inputProfile.Down("DOWN"))
                {
                    int jumpValid = duck1._jumpValid;
                }
                int num6 = (int)Math.Ceiling(Math.Abs(this.vSpeed) / 4.0);
                for (int index3 = 0; index3 < num6; ++index3)
                {
                    float num7 = this.vSpeed / num6;
                    if (num7 != 0.0 && Math.Sign(num7) == Math.Sign(vSpeed))
                    {
                        this.y += num7;
                        this._inPhysicsLoop = true;
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
                            if (RecDuck.doduckonduck || !(with is Duck) && !(with is RagdollPart))
                            {
                                if (with is FluidPuddle)
                                {
                                    flag5 = true;
                                    this._curPuddle = with as FluidPuddle;
                                    if (with.top < this.bottom - 2.0 && with.collisionSize.y > 2.0)
                                        num5 = with.top;
                                }
                                if (!(with is Goody) && with != this && !this.clip.Contains(with) && !with.clip.Contains(this) && with.solid && (this.planeOfExistence == 4 || with.planeOfExistence == planeOfExistence))
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
                                            this.Impact(with, ImpactedFrom.Top, true);
                                        }
                                    }
                                    if (with.top <= (double)this.bottom && with.bottom > (double)this.bottom)
                                    {
                                        flag6 = true;
                                        if (this.vSpeed > 0.0)
                                        {
                                            HelperG.SfieldVal("PhysicsObject", "_collideBottom", with, this);
                                            with.Impact(this, ImpactedFrom.Top, true);
                                            this.Impact(with, ImpactedFrom.Bottom, true);
                                        }
                                    }
                                    if (with is IBigStupidWall && (position - this.position).length > 64.0)
                                        this.position = position;
                                    if (flag6)
                                    {
                                        with.Touch(this);
                                        this.Touch(with);
                                    }
                                }
                            }
                        }
                        this._inPhysicsLoop = false;
                    }
                    else
                        break;
                }
                if (this.grounded)
                {
                    this.lastGrounded = DateTime.Now;
                    this.framesSinceGrounded = 0;
                    Thing thing = HelperG.GfieldVal("PhysicsObject", "_collideBottom", this) as Thing;
                    if (!this.doFloat && hSpeed == 0.0 && this.vSpeed == 0.0)
                    {
                        switch (thing)
                        {
                            case Block _:
                            case IPlatform _:
                                if (!(thing is ItemBox) || (thing as ItemBox).canBounce)
                                {
                                    this._sleeping = true;
                                    break;
                                }
                                break;
                        }
                    }
                }
                if (num5 > -999.0)
                {
                    if (!this.doFloat && this.vSpeed > 1.0)
                    {
                        FluidData fluidData = (FluidData)HelperG.GfieldVal("PhysicsObject", "_curFluid", this);
                        SFX.Play("largeSplash", Rando.Float(0.6f, 0.7f), Rando.Float(-0.7f, -0.2f));
                    }
                    this.doFloat = true;
                }
                else
                    this.doFloat = false;
                if (this._curPuddle != null)
                {
                    HelperG.SfieldVal("PhysicsObject", "_curFluid", _curPuddle.data, this);
                    if (this.onFire && _curPuddle.data.flammable <= 0.5 && _curPuddle.data.heat <= 0.5)
                        this.Extinquish();
                    else if (_curPuddle.data.heat > 0.5)
                    {
                        if (flammable > 0.0 && this.isServerForObject)
                        {
                            int num8 = this.onFire ? 1 : 0;
                            this.Burn(this.position, this);
                        }
                        this.DoHeatUp(0.015f, this.position);
                    }
                    else
                        this.DoHeatUp(-0.05f, this.position);
                }
                if (this.doFloat)
                {
                    if (this != null && this.crouch)
                    {
                        if (floatMultiplier > 0.980000019073486)
                            this.vSpeed *= 0.8f;
                        this.floatMultiplier = 0.8f;
                    }
                    else
                    {
                        if (floatMultiplier > 0.980000019073486)
                            this.vSpeed *= 0.4f;
                        this.vSpeed *= 0.95f;
                        this.floatMultiplier = 0.4f;
                    }
                }
                else
                {
                    if (flag5 && vSpeed > 1.0 && Math.Abs(this.vSpeed) < 0.00999999977648258)
                    {
                        FluidData fluidData = (FluidData)HelperG.GfieldVal("PhysicsObject", "_curFluid", this);
                        SFX.Play("littleSplash", Rando.Float(0.8f, 0.9f), Rando.Float(-0.2f, 0.2f));
                    }
                    this.floatMultiplier = 1f;
                }
                Recorder.LogVelocity(Math.Abs(this.hSpeed) + Math.Abs(this.vSpeed));
                if (this._sleeping)
                    return;
                if (this.modFric)
                    this.modFric = false;
                else
                    this.specialFrictionMod = 1f;
            }
        }

        public void baseupdate() => this._didImpactSound = false;

        public override bool PlayCollideSound(ImpactedFrom from) => true;

        public override bool Destroy(DestroyType type = null)
        {
            this._destroyed = true;
            return true;
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (this._doll == null || !this.isServerForObject || !with.isServerForObject || with is RagdollPart || with is FeatherVolume || with == this.owner || with == this._doll.holdingOwner || with == this._doll.captureDuck || !(with is Duck))
                return;
            Holdable lastHoldItem = (with as Duck)._lastHoldItem;
            if ((with as Duck)._timeSinceThrow >= 15 || lastHoldItem == this._doll.part1 || lastHoldItem == this._doll.part2)
                return;
            _ = this._doll.part3;
        }

        protected override bool OnDestroy(DestroyType type = null) => this._doll != null && type is DTIncinerate;
    }
}
