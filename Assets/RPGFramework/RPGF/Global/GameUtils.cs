using RPGF.Core.Location;

namespace RPGF
{
    public class GameUtils
    {
        private readonly GameManager _game;

        public GameUtils(GameManager game)
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
