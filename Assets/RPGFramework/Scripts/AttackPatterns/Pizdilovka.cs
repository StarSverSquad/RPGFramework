using System.Collections;
using UnityEngine;

public class Pizdilovka : RPGAttackPattern
{
    [SerializeField] // Аттрибут, который позволяет юнити обращаться к приватным полям
    private GameObject bulletPrefabH; // Поле для хранения привата пули 

    [SerializeField] 
    private GameObject bulletPrefabV;
    protected override IEnumerator PatternCoroutine() // Метод основного цикла битвы
    {
        BattleManager.Instance.BattleField.Resize(new Vector2 (3, 2), 3);

        while(true)
        {
            yield return new WaitForSeconds(.5f); // ожидает 1.5 секунд (позволяет игре загрузить действие)
            CreateObjectRelativeBattleField(bulletPrefabH, new Vector2(-3, Random.Range(-1, 1)));

            yield return new WaitForSeconds(.5f); 
            CreateObjectRelativeBattleField(bulletPrefabV, new Vector2(Random.Range(-1.5f, 1.5f), 4));
        }
    }
}