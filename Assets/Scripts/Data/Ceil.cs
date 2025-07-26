using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    public class Ceil
    {
        public Vector2Int position { get; }
        public Entity background { get; }
        [CanBeNull] public Entity foreground { get; set; }

        public Ceil(Vector2Int position, Entity background)
        {
            this.position = position;
            this.background = background;
        }
    }
}