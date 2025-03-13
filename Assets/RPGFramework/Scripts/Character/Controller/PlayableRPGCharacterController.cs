using UnityEngine;
using System.Collections;

namespace RPGF.Character
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PlayableRPGCharacterController : RPGCharacterControllerBase
    {
        
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        public override void Initialize()
        {
            base.Initialize();

            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}