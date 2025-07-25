using UnityEngine;
using Object = System.Object;

namespace Data
{
    public class Entity
    {
        public EntityType type;
        public bool isInteractable;
    }

    class PushEntity : Entity
    {
        public Vector2Int direction;
        public PushEntity(Direction directionEnum)
        {
            direction = Utils.DirectionToVector(directionEnum);
        }
        
    }     
    class TeleportEntity : Entity
    {
        public Vector2Int? from;
        public Vector2Int? to;
        public bool isFrom;
    }    
    class CharacterEntity : Entity
    {
        public CharacterType characterType;
    }

    class InteractiveEntity : Entity
    {
        public Object value;
        public InteractionType interactionType;
    }

    class DangerousEntity : Entity
    {
        public DangerousType dangerousType;
    }

    enum CharacterType
    {
        Human,
        Demon
    }

    public enum EntityType
    {
        Groud,
        Push,
        Character,
        Obstacle,
        Interactive,
        Dangerous,
        Teleport,
    }

    enum DangerousType
    {
        Lava,
        Mouse,
        HolyWater,
    }

    enum InteractionType
    {
        Pickup,
        Portal
    }
}