using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Data
{
    public class PosHolder
    {
        public Vector2Int pos;

        public PosHolder(Vector2Int pos)
        {
            this.pos = pos;
        }
    }

    public class Map
    {
        public Dictionary<Vector2Int, Ceil> ceils;
        public PosHolder humanPos;
        public PosHolder demonPos;

        public Map(Dictionary<Vector2Int, Ceil> ceils, Vector2Int humanPos, Vector2Int demonPos)
        {
            this.ceils = ceils;
            this.humanPos = new PosHolder(humanPos);
            this.demonPos = new PosHolder(demonPos);
        }

        public MoveResult Move(Vector2Int move)
        {
            var humanCeil = ceils[humanPos.pos + move];
            var demonCeil = ceils[demonPos.pos + move];
            var humanEffects = GetEffectForActor(humanCeil, humanPos.pos + move);
            var demonEffects = GetEffectForActor(demonCeil, demonPos.pos + move);

            return new MoveResult(humanEffects, demonEffects);
        }

        List<Effect> GetEffectForActor(Ceil ceil, Vector2Int position)
        {
            var effectList = new List<Effect>();
            if (ceil.background.isInteractable)
            {
                bool canMove = true;
                for (var i = 0; i < ceil.foreground.Count; i++)
                {
                    var additionalEntry = ceil.foreground[i];
                    if (additionalEntry.isInteractable)
                    {
                        var foregroundEffect = GetEffect(additionalEntry, position);
                        if (foregroundEffect.breakMove)
                        {
                            canMove = false;
                        }

                        effectList.Add(foregroundEffect);
                    }
                }


                var backgroundEffect = GetEffect(ceil.background, position);
                if (canMove && backgroundEffect is MoveEffect)
                {
                    effectList.Add(backgroundEffect);
                }
            }

            // если на клетку нельзя пойти, то будет пустой список. Можно доделать, если добавить else выше
            return effectList;
        }

        Effect GetEffect(Entity entity, Vector2Int position)
        {
            if (entity.isInteractable)
            {
                switch (entity.type)
                {
                    case EntityType.Obstacle:
                        return new EmptyEffect()
                        {
                            breakMove = true
                        };
                    case EntityType.Groud:
                        return new MoveEffect(position);
                    case EntityType.Character:
                        switch (((CharacterEntity)entity).characterType)
                        {
                            case CharacterType.Human:
                                return new ContactEffect(EffectType.ContactWithHuman);
                            case CharacterType.Demon:
                                return new ContactEffect(EffectType.ContactWithDemon);
                        }

                        break;
                    case EntityType.Dangerous:
                        switch (((DangerousEntity)entity).dangerousType)
                        {
                            case DangerousType.Lava:
                                return new ContactEffect(EffectType.ContactWithLava);
                            case DangerousType.Mouse:
                                return new ContactEffect(EffectType.ContactWithMouse);
                            case DangerousType.HolyWater:
                                return new ContactEffect(EffectType.ContactWithHolyWater);
                        }

                        break;
                    case EntityType.Interactive:
                        switch (((InteractiveEntity)entity).interactionType)
                        {
                            case InteractionType.Pickup:
                                return new PickupEffect(((InteractiveEntity)entity).value);
                            case InteractionType.Portal:
                                return new TeleportEffect(position);
                        }

                        break;
                }
            }

            return new EmptyEffect();
        }
    }

    public class MoveResult
    {
        public List<Effect> demonEffect;
        public List<Effect> humanEffect;

        public MoveResult(List<Effect> humanEffect, List<Effect> demonEffect)
        {
            this.demonEffect = demonEffect;
            this.humanEffect = humanEffect;
        }
    }

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