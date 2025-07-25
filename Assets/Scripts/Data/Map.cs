using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Map
    {
        public Dictionary<Vector2Int, Entity> ceils;
        public Vector2Int humanPos;
        public Vector2Int demonPos;

        public Map(Dictionary<Vector2Int, Entity> ceils, Vector2Int humanPos, Vector2Int demonPos)
        {
            this.ceils = ceils;
        }

        void Move(Vector2Int move)
        {
            // var humanCell = cells[humanPos.x + move.x, humanPos.y + move.x];
            // var demonCell = cells[demonPos.x + move.x, demonPos.y + move.x];
            // if (humanCell.rootEntity.isInteractable)
            // {
            //     for (var i = 0; i < humanCell.additionalEntity.Count; i++)
            //     {
            //         var additionalEntry = humanCell.additionalEntity[i];
            //         if (additionalEntry.isInteractable)
            //         {
            //         }
            //     }
            // }
            //
            // if (demonCell.rootEntity.isInteractable)
            // {
            //     for (var i = 0; i < demonCell.additionalEntity.Count; i++)
            //     {
            //         var additionalEntry = demonCell.additionalEntity[i];
            //         if (additionalEntry.isInteractable)
            //         {
            //         }
            //     }
            // }
        }

        void GetEffect(Entity entity)
        {
            // if (entity.isInteractable)
            // {
            //     switch (entity.type)
            //     {
            //         case EntityType.Obstacle:
            //             break;
            //         case EntityType.Groud:
            //             break;
            //         case EntityType.Character:
            //             switch (((Character)entity).characterType)
            //             {
            //                 case CharacterType.Human:
            //                     return Effect.ContactWithHuman;
            //                 case CharacterType.Demon:
            //                     return Effect.ContactWithDemon;
            //             }
            //
            //             break;
            //         case EntityType.Dangerous:
            //             switch (((DangerousEntity)entity).dangerousType)
            //             {
            //                 case DangerousType.Lava:
            //                     return Effect.ContactWithLava;
            //                 case DangerousType.Mouse:
            //                     return Effect.ContactWithMouse;
            //                 case DangerousType.HolyWater:
            //                     return Effect.ContactWithHolyWatter;
            //             }
            //
            //             break;
            //         case EntityType.Interactive:
            //             switch (((InteractiveEntity)entity).interactionType)
            //             {
            //                 case InteractionType.Pickup:
            //                     break;
            //                 case InteractionType.Portal:
            //                     break;
            //             }
            //
            //             break;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
            // }
            //
            // return Effect.None;
        }
    }

    class Effect
    {
        public EffectType type;
    }

    enum EffectType
    {
        None,
        ContactWithDemon,
        ContactWithHuman,
        ContactWithLava,
        ContactWithMouse,
        ContactWithHolyWatter
    }
}