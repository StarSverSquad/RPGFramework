using RPGF.Core.Location;
using RPGF.Domain;
using System;
using UnityEngine;

namespace RPGF.Core.Location
{
    [Serializable]
    public class LocationTransimitionDto
    {
        public RpgfLocationInfo Location;

        public string Point;

        public bool TeleportToPoint;

        public Vector2 Position;
        public ViewDirection Direction;

        public LocationTransimitionDto()
        {
            Location = null;
            Point = string.Empty;
            TeleportToPoint = true;
            Position = Vector2.zero;
            Direction = ViewDirection.Down;
        }
    }
}
