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
    class MovedObjectEntity : Entity
    {
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

    class PickupEntity : Entity
    {
    }

    class DangerousEntity : Entity
    {
        public dangerousTargetType dangerousTargetType;
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
        Pickup,
        Dangerous,
        Teleport,
        MovedObject,
    }

    public enum dangerousTargetType
    {
        All,
        Human,
        Demon,
    }

    enum InteractionType
    {
        Pickup,
        Portal
    }
}