using System;

namespace Data
{
    public class Entity
    {
        public EntityType type;
        public bool isInteractable;
    }

    class Character : Entity
    {
        public CharacterType characterType; // Human, Demon, NPC
    }

    class InteractiveEntity : Entity
    {
        public Object value;
        public InteractionType interactionType; // Human, Demon, NPC
    }

    class DangerousEntity : Entity
    {
        public DangerousType dangerousType; // Human, Demon, NPC
    }

    enum CharacterType
    {
        Human,
        Demon
    }

    public enum EntityType
    {
        Groud,
        Character,
        Obstacle,
        Interactive,
        Dangerous,
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