using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Ceil
    {
        public Vector2Int position { get; }
        public Entity background { get; }
        public List<Entity> foreground { get; }

        public Ceil(Vector2Int position, Entity background)
        {
            this.position = position;
            this.background = background;
        }
    }
}