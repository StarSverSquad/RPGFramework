using UnityEngine;

namespace RPGF.Core.Battle.PlayerMode
{
    public class PlayerModeData : RPGFrameworkBehaviour
    {
        public Rigidbody2D Rigidbody;

        public float MoveSpeed = 1;
        public float DefaultMoveSpeed = 1;

        public float AccelerateFactor = 1.3f;

        public bool CanMove = true;

        public bool CanAccelerate = true;
    }
}
