using RPGF.Core.Character;
using RPGF.Core.Inventory;
using RPGF.Core.Location;
using RPGF.Domain.DI;

namespace RPGF
{
    public class GameUtils : ISupportDI
    {
        [Inject]
        private readonly InventoryService _inventory;
        [Inject]
        private readonly CharacterService _character;
        [Inject]
        private readonly GameData _gameData;
        [Inject]
        private readonly GlobalLocationManager _locationManager;

        public void StartNewGame(RpgfLocationInfo startLocation)
        {
            _inventory.Dispose();
            _character.Dispose();
            _gameData.Dispose();

            _locationManager.ChangeLocation(startLocation);
        }
    }
}
