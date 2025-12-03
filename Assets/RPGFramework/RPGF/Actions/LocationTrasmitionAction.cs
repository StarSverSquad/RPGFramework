using RPGF.Core.Location;
using RPGF.EventSystem;
using RPGF.Explorer;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    public class LocationTrasmitionAction : ActionBase
    {
        public LocationTransimitionDto Dto;

        public LocationTrasmitionAction() : base()
        {
            Dto = new LocationTransimitionDto();
        }

        public override IEnumerator ActionCoroutine()
        {
            if (Dto.Location != null)
            {
                if (ExplorerManager.Instance.EventHandler.isActiveAndEnabled)
                    ExplorerManager.Instance.EventHandler.ForceUnhandle();

                ExplorerManager.PlayerMovement.CanWalk = false;

                GlobalManager.Instance.LocationManager.ChangeLocation(Dto);
            }

            yield return new WaitWhile(() => GlobalManager.Instance.LocationManager.IsChanging);

            ExplorerManager.PlayerMovement.CanWalk = true;
        }
    }
}
