using RPGF.Core;
using RPGF.Core.Location;
using UnityEngine;

namespace RPGF.Global
{
    public class DebugModeManager : RPGFrameworkBehaviour
    {
        [SerializeField]
        private RpgfLocationInfo location;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                Global.LocationManager.ChangeLocation(location);
        }
    }
}
