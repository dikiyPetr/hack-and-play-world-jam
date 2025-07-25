﻿using System;
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
            return new MoveResult(MoveByPos(move, humanPos), MoveByPos(move, demonPos));
        }

        public List<Effect> MoveByPos(Vector2Int move, PosHolder posHolder)
        {
            var ceil = ceils[posHolder.pos + move];
            var effects = GetEffectForActor(ceil, posHolder.pos + move);

            return effects;
        }

        List<Effect> GetEffectForActor(Ceil ceil, Vector2Int position)
        {
            var effectList = new List<Effect>();
            if (ceil.background.isInteractable)
            {
                bool canMove = true;

                var additionalEntry = ceil.foreground;
                if (additionalEntry != null && additionalEntry.isInteractable)
                {
                    var foregroundEffects = GetEffect(additionalEntry, position);
                    foreach (var effect in foregroundEffects)
                    {
                        if (effect.breakMove)
                        {
                            canMove = false;
                        }

                        effectList.Add(effect);
                    }
                }


                var backgroundEffects = GetEffect(ceil.background, position);
                foreach (var backgroundEffect in backgroundEffects)
                {
                    if (backgroundEffect is MoveEffect)
                    {
                        if (canMove)
                        {
                            effectList.Add(backgroundEffect);
                        }
                    }
                    else
                    {
                        effectList.Add(backgroundEffect);
                    }
                }
            }

            // если на клетку нельзя пойти, то будет пустой список. Можно доделать, если добавить else выше
            return effectList;
        }

        Effect[] GetEffect(Entity entity, Vector2Int position)
        {
            if (entity.isInteractable)
            {
                switch (entity.type)
                {
                    case EntityType.Obstacle:
                        return new Effect[]
                        {
                            new EmptyEffect()
                            {
                                breakMove = true
                            }
                        };
                    case EntityType.Groud:
                        return new Effect[]
                        {
                            new MoveEffect(position)
                        };
                    case EntityType.Push:
                        return new Effect[]
                        {
                            new MoveEffect(position),
                            new PushEffect(position + ((PushEntity)entity).direction)
                        };
                    case EntityType.Teleport:
                        return new Effect[]
                        {
                            new MoveEffect(position),
                            new TeleportEffect() { to = ((TeleportEntity)entity).to.Value }
                        };
                    case EntityType.Dangerous:
                        return new Effect[]
                        {
                            new MoveEffect(position),
                            new ContactWithDangerousEffect
                                { DangerousTargetType = ((DangerousEntity)entity).dangerousTargetType }
                        };
                    case EntityType.Pickup:
                        return new Effect[]
                        {
                            new MoveEffect(position),
                            new PickupEffect() { }
                        };
                    case EntityType.MovedObject:
                        return new Effect[]
                        {
                            new PushMovedObjectEffect(),
                            new MoveEffect(position),
                        };
                    case EntityType.Character:
                        switch (((CharacterEntity)entity).characterType)
                        {
                            case CharacterType.Human:
                                return new Effect[]
                                {
                                    new ContactEffect(EffectType.ContactWithHuman)
                                };
                            case CharacterType.Demon:
                                return
                                    new Effect[]
                                    {
                                        new ContactEffect(EffectType.ContactWithDemon)
                                    };
                        }

                        break;
                }
            }

            return new Effect[]
            {
                new EmptyEffect()
            };
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
}