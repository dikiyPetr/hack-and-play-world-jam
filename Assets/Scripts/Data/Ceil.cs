using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Ceil
    {
        private Vector2Int position;
        private Entity background;
        private List<Entity> foregroud;
        
        public Ceil(Vector2Int position, Entity background)
        {
            this.position = position;
            this.background = background;
        }
    }
}