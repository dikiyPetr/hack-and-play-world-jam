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
        public PickupEffect() : base(EffectType.PickupEffect)
        {
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
    class ContactWithDangerousEffect : Effect
    {
        public dangerousTargetType DangerousTargetType;
        public ContactWithDangerousEffect() : base(EffectType.ContactWithDangerous)
        {
        }
    }

    class TeleportEffect : Effect
    {
        public Vector2Int to;

        public TeleportEffect() : base(EffectType.Teleport)
        {
        }
    }    
    class PushMovedObjectEffect : Effect
    {
        public PushMovedObjectEffect() : base(EffectType.PushMovedObject)
        {
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
        ContactWithDangerous,
        PushMovedObject,
    }
}