using DG.Tweening;
using RPGF.Core.VisualEffects.Abstractions;
using System.Collections;
using UnityEngine;

namespace RPGF.Core.VisualEffects.Defaults
{
    public class VisualDamageEffectDefault : VisualEffectBase
    {
        [SerializeField]
        private RectTransform _bodyTransofrm;
        [SerializeField]
        private float _duration = 0.5f;
        [SerializeField]
        private float _strength = 0.5f;
        [SerializeField]
        private int _vibrato = 10;
        [SerializeField]
        private float _randomness = 90;


        protected override IEnumerator EffectCoroutine()
        {
            _bodyTransofrm
                .DOShakePosition(_duration, _strength, _vibrato, _randomness)
                .Play();

            yield return new WaitForSeconds(_duration);
        }
    }
}