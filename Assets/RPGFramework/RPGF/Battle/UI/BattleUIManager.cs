using UnityEngine;

namespace RPGF.Battle.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        public CharacterBoxManager CharacterBox;
        public BattleUIDescription Description;
        public BattleUIPlayerTurnSide PlayerTurnSide;
        public BattleUICharacterSide CharacterSide;
        public BattleUIConcentration Concentration;
        public CharacterQueryManager CharacterQuery;
    }
}