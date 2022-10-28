// Decompiled with JetBrains decompiler
// Type: DuckGame.RecRagdoll
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;

namespace DuckGame
{
    public class RecRagdoll : Ragdoll
    {
        public RecRagdoll(
          float xpos,
          float ypos,
          Duck who,
          bool slide,
          float degrees,
          int off,
          Vec2 v,
          DuckPersona p = null)
          : base(xpos, ypos, who, slide, degrees, off, v, p)
        {
        }

        public void RunInit2()
        {
            this._editorName = nameof(RecRagdoll);
            this.Organize2();
            if (Network.isActive && !GhostManager.inGhostLoop)
                GhostManager.context.MakeGhost(this);
            if (Math.Abs(this.hSpeed) < 0.200000002980232)
                this.hSpeed = NetRand.Float(0.3f, 1f) * (NetRand.Float(1f) >= 0.5 ? 1f : -1f);
            float num1 = this._slide ? 1f : 1.05f;
            float num2 = this._slide ? 1f : 0.95f;
            this._part1.hSpeed = this.hSpeed * num1;
            this._part1.vSpeed = this.vSpeed;
            this._part2.hSpeed = this.hSpeed;
            this._part2.vSpeed = this.vSpeed;
            this._part3.hSpeed = this.hSpeed * num2;
            this._part3.vSpeed = this.vSpeed;
            this._part1.enablePhysics = false;
            this._part2.enablePhysics = false;
            this._part3.enablePhysics = false;
            this._part1.Update();
            this._part2.Update();
            this._part3.Update();
            this._part1.enablePhysics = true;
            this._part2.enablePhysics = true;
            this._part3.enablePhysics = true;
            if (Network.isActive)
            {
                Fondle(this, DuckNetwork.localConnection);
                Fondle(_part1, DuckNetwork.localConnection);
                Fondle(_part2, DuckNetwork.localConnection);
                Fondle(_part3, DuckNetwork.localConnection);
            }
            if (this._duck == null || !this._duck.onFire)
                return;
            this._part1.Burn(this._part1.position, this._duck.lastBurnedBy);
            this._part2.Burn(this._part2.position, this._duck.lastBurnedBy);
        }

        public void Organize2()
        {
            Vec2 vec = Maths.AngleToVec(this.angle);
            if (this._part1 == null)
            {
                this._part1 = new RecRagdollPart(this.x - vec.x * this.partSep, this.y - vec.y * this.partSep, 0, this._duck != null ? this._duck.persona : this.persona, offDir, this);
                if (Network.isActive && !GhostManager.inGhostLoop)
                    GhostManager.context.MakeGhost(_part1);
                this._part2 = new RecRagdollPart(this.x, this.y, 2, this._duck != null ? this._duck.persona : this.persona, offDir, this);
                if (Network.isActive && !GhostManager.inGhostLoop)
                    GhostManager.context.MakeGhost(_part2);
                this._part3 = new RecRagdollPart(this.x + vec.x * this.partSep, this.y + vec.y * this.partSep, 1, this._duck != null ? this._duck.persona : this.persona, offDir, this);
                if (Network.isActive && !GhostManager.inGhostLoop)
                    GhostManager.context.MakeGhost(_part3);
            }
            else
            {
                this._part1.SortOutDetails(this.x - vec.x * this.partSep, this.y - vec.y * this.partSep, 0, this._duck != null ? this._duck.persona : this.persona, offDir, this);
                this._part2.SortOutDetails(this.x, this.y, 2, this._duck != null ? this._duck.persona : this.persona, offDir, this);
                this._part3.SortOutDetails(this.x + vec.x * this.partSep, this.y + vec.y * this.partSep, 1, this._duck != null ? this._duck.persona : this.persona, offDir, this);
            }
            this._part1.joint = this._part2;
            this._part3.joint = this._part2;
            this._part1.connect = this._part3;
            this._part3.connect = this._part1;
            this._part1.framesSinceGrounded = 99;
            this._part2.framesSinceGrounded = 99;
            this._part3.framesSinceGrounded = 99;
            if (this._duck == null)
                return;
            if (!(Level.current is GameLevel) || !(Level.current as GameLevel).isRandom)
            {
                this._duck.ReturnItemToWorld(_part1);
                this._duck.ReturnItemToWorld(_part2);
                this._duck.ReturnItemToWorld(_part3);
            }
            this._part3.depth = new Depth(this._duck.depth.value);
            this._part1.depth = this._part3.depth - 1;
        }

        public override void Update()
        {
            if (this.removeFromLevel || this.isOffBottomOfLevel && this.captureDuck != null && this.captureDuck.dead)
                return;
            this._timeSinceNudge += 0.07f;
            if (this._part1 == null || this._part2 == null || this._part3 == null)
                return;
            float num1 = (float)HelperG.GfieldVal(typeof(Ragdoll), "_zap", this);
            if (num1 > 0.0)
            {
                this._part1.vSpeed += Rando.Float(-1f, 0.5f);
                this._part1.hSpeed += Rando.Float(-0.5f, 0.5f);
                this._part2.vSpeed += Rando.Float(-1f, 0.5f);
                this._part2.hSpeed += Rando.Float(-0.5f, 0.5f);
                this._part3.vSpeed += Rando.Float(-1f, 0.5f);
                this._part3.hSpeed += Rando.Float(-0.5f, 0.5f);
                this._part1.x += Rando.Int(-2, 2);
                this._part1.y += Rando.Int(-2, 2);
                this._part2.x += Rando.Int(-2, 2);
                this._part2.y += Rando.Int(-2, 2);
                this._part3.x += Rando.Int(-2, 2);
                this._part3.y += Rando.Int(-2, 2);
                HelperG.SfieldVal(typeof(Ragdoll), "_zap", num1 - 0.05f, this);
                HelperG.SfieldVal(typeof(Ragdoll), "_wasZapping", true, this);
            }
            else if ((bool)HelperG.GfieldVal(typeof(Ragdoll), "_wasZapping", this))
            {
                HelperG.SfieldVal(typeof(Ragdoll), "_wasZapping", false, this);
                if (this.captureDuck != null)
                {
                    if (this.captureDuck.dead)
                    {
                        this.captureDuck.Ressurect();
                        return;
                    }
                    this.captureDuck.Kill(new DTElectrocute(this._zapper));
                    return;
                }
            }
            if (this.captureDuck != null && this.captureDuck.inputProfile != null && this.captureDuck.isServerForObject)
            {
                if (this.captureDuck.inputProfile.Pressed("JUMP") && this.captureDuck.HasEquipment(typeof(Jetpack)))
                    this.captureDuck.GetEquipment(typeof(Jetpack)).PressAction();
                if (this.captureDuck.inputProfile.Released("JUMP") && this.captureDuck.HasEquipment(typeof(Jetpack)))
                    this.captureDuck.GetEquipment(typeof(Jetpack)).ReleaseAction();
            }
            this.partSep = 6f;
            if ((bool)HelperG.GfieldVal(typeof(Ragdoll), "_zekeBear", this))
                this.partSep = 4f;
            Vec2 vec2_1 = this._part1.position - this._part3.position;
            if (vec2_1.length > partSep * 5.0)
            {
                if (this._part1.owner != null)
                {
                    RagdollPart part2 = this._part2;
                    this._part3.position = vec2_1 = this._part1.position;
                    Vec2 vec2_2 = vec2_1;
                    part2.position = vec2_2;
                }
                else if (this._part3.owner != null)
                {
                    RagdollPart part1 = this._part1;
                    this._part2.position = vec2_1 = this._part3.position;
                    Vec2 vec2_3 = vec2_1;
                    part1.position = vec2_3;
                }
                else
                {
                    RagdollPart part1 = this._part1;
                    this._part3.position = vec2_1 = this._part2.position;
                    Vec2 vec2_4 = vec2_1;
                    part1.position = vec2_4;
                }
                this._part1.vSpeed = this._part2.vSpeed = this._part3.vSpeed = 0.0f;
                this._part1.hSpeed = this._part2.hSpeed = this._part3.hSpeed = 0.0f;
                this.Solve(_part1, _part2, this.partSep);
                this.Solve(_part2, _part3, this.partSep);
                this.Solve(_part1, _part3, this.partSep * 2f);
            }
            this.Solve(_part1, _part2, this.partSep);
            this.Solve(_part2, _part3, this.partSep);
            this.Solve(_part1, _part3, this.partSep * 2f);
            if (this._part1.owner is Duck && this._part3.owner is Duck)
            {
                _ = (double)this.SpecialSolve(_part3, this._part1.owner as Duck, 16f);
                _ = (double)this.SpecialSolve(_part1, this._part3.owner as Duck, 16f);
            }
            if (this.tongueStuck != Vec2.Zero && this.captureDuck != null)
            {
                Vec2 vec2_5 = this.tongueStuck + new Vec2(captureDuck.offDir * -4, -6f);
                if (this._part1.owner is Duck)
                {
                    _ = (double)this.SpecialSolve(_part3, this._part1.owner as Duck, 16f);
                    _ = (double)this.SpecialSolve(_part1, vec2_5, 16f);
                }
                if (this._part3.owner is Duck)
                {
                    _ = (double)this.SpecialSolve(_part1, this._part3.owner as Duck, 16f);
                    _ = (double)this.SpecialSolve(_part3, vec2_5, 16f);
                }
                vec2_1 = this.part1.position - vec2_5;
                if (vec2_1.length > 4.0)
                {
                    _ = (double)this.SpecialSolve(_part1, vec2_5, 4f);
                    vec2_1 = vec2_5 - this.part1.position;
                    _ = vec2_1.normalized;
                    vec2_1 = this.part1.position - vec2_5;
                    if (vec2_1.length > 12.0)
                        this.part1.position = Lerp.Vec2Smooth(this.part1.position, vec2_5, 0.2f);
                }
            }
            this.position = (this._part1.position + this._part2.position + this._part3.position) / 3f;
            float num9 = (float)HelperG.GfieldVal(typeof(Ragdoll), "_zap", this);
            if (this._duck == null || num9 > 0.0)
                return;
            if (this._duck.eyesClosed)
                this._part1.frame = 20;
            if (this._duck.fancyShoes && this._duck.framesSinceRagdoll < 1)
                return;
            this.UpdateInput2();
        }

        public void UpdateInput2()
        {
        }
    }
}
