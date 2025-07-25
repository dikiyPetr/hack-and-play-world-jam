using UnityEngine;

namespace Data
{
    public abstract class Effect
    {
        public EffectType type;
        public bool breakMove = false;

        public Effect(EffectType type)
        {
            this.type = type;
        }
    }

    class MoveEffect : Effect
    {
        public Vector2Int target;

        public MoveEffect(Vector2Int target) : base(EffectType.Move)
        {
            this.target = target;
        }
    }

    class PushEffect : MoveEffect
    {
        public PushEffect(Vector2Int target) : base(target)
        {
        }
    }

    class PickupEffect : Effect
    {
        public Object value;

        public PickupEffect(Object value) : base(EffectType.PickupEffect)
        {
            this.value = value;
        }
    }

    class EmptyEffect : Effect
    {
        public EmptyEffect() : base(EffectType.None)
        {
        }
    }

    class ContactEffect : Effect
    {
        public ContactEffect(EffectType type) : base(type)
        {
        }
    }

    class TeleportEffect : Effect
    {
        public Vector2Int from;

        public TeleportEffect(Vector2Int from) : base(EffectType.Teleport)
        {
            this.from = from;
        }
    }

    public enum EffectType
    {
        None,
        Move,
        Teleport,
        PickupEffect,
        ContactWithDemon,
        ContactWithHuman,
        ContactWithLava,
        ContactWithMouse,
        ContactWithHolyWater
    }
}