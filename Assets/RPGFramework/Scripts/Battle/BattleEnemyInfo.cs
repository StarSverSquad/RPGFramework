using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BattleEnemyInfo : BattleEntityInfo
{
    public RPGEnemy Enemy => Entity as RPGEnemy;

    public BattleEnemyInfo(RPGEnemy entity) : base(entity)
    {
        Heal = entity.DefaultHeal;
    }

    public override void UpdateState(RPGEntityState state)
    {
        EntityState st = States.FirstOrDefault(i => i.rpg == state);

        if (st == null)
            return;

        Heal += state.AddHeal;

        st.turnsCount++;

        if (st.turnsCount == state.TurnCount)
            RemoveState(state);

        InvokeStatesChangedCallback();
    }
}