using RPGF.Core.Location;

namespace RPGF
{
    public class GameUtils
    {
        private readonly GlobalManager _game;

        public GameUtils(GlobalManager game)
        {
            _game = game;
        }

        public void StartNewGame(RpgfLocationInfo startLocation)
        {
            _game.Inventory.Dispose();
            _game.Character.Dispose();
            _game.GameData.Dispose();

            _game.LocationManager.ChangeLocation(startLocation);
        }
    }
}
