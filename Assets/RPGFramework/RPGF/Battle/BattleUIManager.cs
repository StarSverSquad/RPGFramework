using RPGF.Battle.UI;
using UnityEngine;

namespace RPGF.Battle
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