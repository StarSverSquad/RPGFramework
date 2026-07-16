using RPGF.Core.Location;
using RPGF.EventSystem;
using RPGF.Overworld;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
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
                if (OverworldManager.Instance.EventHandler.isActiveAndEnabled)
                    OverworldManager.Instance.EventHandler.ForceUnhandle();

                OverworldManager.PlayerMovement.CanWalk = false;

                GlobalManager.Instance.LocationManager.ChangeLocation(Dto);
            }

            yield return new WaitWhile(() => GlobalManager.Instance.LocationManager.IsChanging);

            OverworldManager.PlayerMovement.CanWalk = true;
        }
    }
}
