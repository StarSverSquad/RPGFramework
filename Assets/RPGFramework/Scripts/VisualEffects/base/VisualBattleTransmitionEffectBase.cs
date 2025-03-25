using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class VisualBattleTransmitionEffectBase : MonoBehaviour
{
    public abstract IEnumerator PartOne();

    public abstract IEnumerator PartTwo();
}