using UnityEngine;

namespace RPGF.RPG
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapon")]
    public class RPGWeapon : RPGWerable
    {
        public VisualAttackEffect Effect;
    }
}
