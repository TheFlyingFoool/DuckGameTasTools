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
        public RecRagdoll(float xpos, float ypos, Duck who, bool slide, float degrees, int off, Vec2 v, DuckPersona p = null) : base(xpos, ypos, who, slide, degrees, off, v, p)
        {
        }

        public void RunInit2()
        {
            _editorName = nameof(RecRagdoll);
            Organize2();
            if (Network.isActive && !GhostManager.inGhostLoop)
                GhostManager.context.MakeGhost(this);
            if (Math.Abs(hSpeed) < 0.2f)
                hSpeed = NetRand.Float(0.3f, 1f) * (NetRand.Float(1f) >= 0.5 ? 1f : -1f);
            float num1 = _slide ? 1f : 1.05f;
            float num2 = _slide ? 1f : 0.95f;
            _part1.hSpeed = hSpeed * num1;
            _part1.vSpeed = vSpeed;
            _part2.hSpeed = hSpeed;
            _part2.vSpeed = vSpeed;
            _part3.hSpeed = hSpeed * num2;
            _part3.vSpeed = vSpeed;
            _part1.enablePhysics = false;
            _part2.enablePhysics = false;
            _part3.enablePhysics = false;
            _part1.Update();
            _part2.Update();
            _part3.Update();
            _part1.enablePhysics = true;
            _part2.enablePhysics = true;
            _part3.enablePhysics = true;
            if (Network.isActive)
            {
                Fondle(this, DuckNetwork.localConnection);
                Fondle(_part1, DuckNetwork.localConnection);
                Fondle(_part2, DuckNetwork.localConnection);
                Fondle(_part3, DuckNetwork.localConnection);
            }
            if (_duck == null || !_duck.onFire)
                return;
            _part1.Burn(_part1.position, _duck.lastBurnedBy);
            _part2.Burn(_part2.position, _duck.lastBurnedBy);
        }

        public void Organize2()
        {
            Vec2 vec = Maths.AngleToVec(angle);
            if (_part1 == null)
            {
                _part1 = new RecRagdollPart(x - vec.x * partSep, y - vec.y * partSep, 0, _duck != null ? _duck.persona : persona, offDir, this);
                if (Network.isActive && !GhostManager.inGhostLoop)
                    GhostManager.context.MakeGhost(_part1);
                _part2 = new RecRagdollPart(x, y, 2, _duck != null ? _duck.persona : persona, offDir, this);
                if (Network.isActive && !GhostManager.inGhostLoop)
                    GhostManager.context.MakeGhost(_part2);
                _part3 = new RecRagdollPart(x + vec.x * partSep, y + vec.y * partSep, 1, _duck != null ? _duck.persona : persona, offDir, this);
                if (Network.isActive && !GhostManager.inGhostLoop)
                    GhostManager.context.MakeGhost(_part3);
            }
            else
            {
                _part1.SortOutDetails(x - vec.x * partSep, y - vec.y * partSep, 0, _duck != null ? _duck.persona : persona, offDir, this);
                _part2.SortOutDetails(x, y, 2, _duck != null ? _duck.persona : persona, offDir, this);
                _part3.SortOutDetails(x + vec.x * partSep, y + vec.y * partSep, 1, _duck != null ? _duck.persona : persona, offDir, this);
            }
            _part1.joint = _part2;
            _part3.joint = _part2;
            _part1.connect = _part3;
            _part3.connect = _part1;
            _part1.framesSinceGrounded = 99;
            _part2.framesSinceGrounded = 99;
            _part3.framesSinceGrounded = 99;
            if (_duck == null)
                return;
            if (!(Level.current is GameLevel) || !(Level.current as GameLevel).isRandom)
            {
                _duck.ReturnItemToWorld(_part1);
                _duck.ReturnItemToWorld(_part2);
                _duck.ReturnItemToWorld(_part3);
            }
            _part3.depth = new Depth(_duck.depth.value);
            _part1.depth = _part3.depth - 1;
        }

        public override void Update()
        {
            if (removeFromLevel || isOffBottomOfLevel && captureDuck != null && captureDuck.dead)
                return;
            _timeSinceNudge += 0.07f;
            if (_part1 == null || _part2 == null || _part3 == null)
                return;
            float num1 = (float)HelperG.GfieldVal(typeof(Ragdoll), "_zap", this);
            if (num1 > 0.0)
            {
                _part1.vSpeed += Rando.Float(-1f, 0.5f);
                _part1.hSpeed += Rando.Float(-0.5f, 0.5f);
                _part2.vSpeed += Rando.Float(-1f, 0.5f);
                _part2.hSpeed += Rando.Float(-0.5f, 0.5f);
                _part3.vSpeed += Rando.Float(-1f, 0.5f);
                _part3.hSpeed += Rando.Float(-0.5f, 0.5f);
                _part1.x += Rando.Int(-2, 2);
                _part1.y += Rando.Int(-2, 2);
                _part2.x += Rando.Int(-2, 2);
                _part2.y += Rando.Int(-2, 2);
                _part3.x += Rando.Int(-2, 2);
                _part3.y += Rando.Int(-2, 2);
                HelperG.SfieldVal(typeof(Ragdoll), "_zap", num1 - 0.05f, this);
                HelperG.SfieldVal(typeof(Ragdoll), "_wasZapping", true, this);
            }
            else if ((bool)HelperG.GfieldVal(typeof(Ragdoll), "_wasZapping", this))
            {
                HelperG.SfieldVal(typeof(Ragdoll), "_wasZapping", false, this);
                if (captureDuck != null)
                {
                    if (captureDuck.dead)
                    {
                        captureDuck.Ressurect();
                        return;
                    }
                    captureDuck.Kill(new DTElectrocute(_zapper));
                    return;
                }
            }
            if (captureDuck != null && captureDuck.inputProfile != null && captureDuck.isServerForObject)
            {
                if (captureDuck.inputProfile.Pressed("JUMP") && captureDuck.HasEquipment(typeof(Jetpack)))
                    captureDuck.GetEquipment(typeof(Jetpack)).PressAction();
                if (captureDuck.inputProfile.Released("JUMP") && captureDuck.HasEquipment(typeof(Jetpack)))
                    captureDuck.GetEquipment(typeof(Jetpack)).ReleaseAction();
            }
            partSep = 6f;
            if ((bool)HelperG.GfieldVal(typeof(Ragdoll), "_zekeBear", this))
                partSep = 4f;
            Vec2 vec2_1 = _part1.position - _part3.position;
            if (vec2_1.length > partSep * 5.0)
            {
                if (_part1.owner != null)
                {
                    RagdollPart part2 = _part2;
                    _part3.position = vec2_1 = _part1.position;
                    Vec2 vec2_2 = vec2_1;
                    part2.position = vec2_2;
                }
                else if (_part3.owner != null)
                {
                    RagdollPart part1 = _part1;
                    _part2.position = vec2_1 = _part3.position;
                    Vec2 vec2_3 = vec2_1;
                    part1.position = vec2_3;
                }
                else
                {
                    RagdollPart part1 = _part1;
                    _part3.position = vec2_1 = _part2.position;
                    Vec2 vec2_4 = vec2_1;
                    part1.position = vec2_4;
                }
                _part1.vSpeed = _part2.vSpeed = _part3.vSpeed = 0.0f;
                _part1.hSpeed = _part2.hSpeed = _part3.hSpeed = 0.0f;
                Solve(_part1, _part2, partSep);
                Solve(_part2, _part3, partSep);
                Solve(_part1, _part3, partSep * 2f);
            }
            Solve(_part1, _part2, partSep);
            Solve(_part2, _part3, partSep);
            Solve(_part1, _part3, partSep * 2f);
            if (_part1.owner is Duck && _part3.owner is Duck)
            {
                SpecialSolve(_part3, _part1.owner as Duck, 16f);
                SpecialSolve(_part1, _part3.owner as Duck, 16f);
            }
            if (tongueStuck != Vec2.Zero && captureDuck != null)
            {
                Vec2 vec = tongueStuck + new Vec2(captureDuck.offDir * -4, -6f);
                if (_part1.owner is Duck)
                {
                   SpecialSolve(_part3, _part1.owner as Duck, 16f);
                   SpecialSolve(_part1, vec, 16f);
                }
                if (_part3.owner is Duck)
                {
                    SpecialSolve(_part1, _part3.owner as Duck, 16f);
                    SpecialSolve(_part3, vec, 16f);
                }
                vec2_1 = part1.position - vec;
                if (vec2_1.length > 4.0)
                {
                    SpecialSolve(_part1, vec, 4f);
                    Vec2 normalized = (vec - base.part1.position).normalized;
                    if ((base.part1.position - vec).length > 12f)
                    {
                        base.part1.position = Lerp.Vec2Smooth(base.part1.position, vec, 0.2f);
                    }
                }
            }
            position = (_part1.position + _part2.position + _part3.position) / 3f;
            float num9 = (float)HelperG.GfieldVal(typeof(Ragdoll), "_zap", this);
            if (_duck == null || num9 > 0.0)
                return;
            if (_duck.eyesClosed)
                _part1.frame = 20;
            if (_duck.fancyShoes && _duck.framesSinceRagdoll < 1)
                return;
            UpdateInput2();
        }

        public void UpdateInput2()
        {
        }
    }
}
