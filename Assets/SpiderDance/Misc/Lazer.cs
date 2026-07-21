using RPGF.Core;
using RPGF.Overworld.Player;
using UnityEngine;

namespace SpiderDance.Misc
{
    public class Lazer : RPGFrameworkBehaviour
    {
        public static readonly Color BLUE_MODE_COLOR = new(0.0f, 0.75f, 1.0f, 0.5f);
        public static readonly Color ORANGE_MODE_COLOR = new(1.0f, 0.6f, 0.0f, 0.5f);

        private const string ANIM_PARAM_IS_BLUE = "IsBlue";

        [Header("References")]
        [SerializeField]
        private Animator bodyAnimator;
        [SerializeField]
        private SpriteRenderer lineRenderer;
        [SerializeField]
        private Collider2D lineCollider;
        [Header("Settings")]
        [SerializeField]
        private float damage;
        [SerializeField]
        private bool isBlueMode;

        private readonly Collider2D[] _overlapBuffer = new Collider2D[8];
        private bool _hasDamaged;

        private void Start()
        {
            ApplyMode();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (lineRenderer != null)
                lineRenderer.color = isBlueMode ? BLUE_MODE_COLOR : ORANGE_MODE_COLOR;
        }
#endif

        private void FixedUpdate()
        {
            if (Local == null || lineCollider == null || !lineCollider.enabled)
                return;

            PlayerOverworldManager player = FindPlayerInBeam();

            if (player == null)
            {
                _hasDamaged = false;
                return;
            }

            if (_hasDamaged)
                return;

            bool isMoving = player.movement.IsMoving;
            bool shouldDamage = isBlueMode ? isMoving : !isMoving;

            if (!shouldDamage)
                return;

            Overworld.DamageService.DamageParty(Mathf.RoundToInt(damage));
            _hasDamaged = true;
        }

        private void ApplyMode()
        {
            if (lineRenderer != null)
                lineRenderer.color = isBlueMode ? BLUE_MODE_COLOR : ORANGE_MODE_COLOR;

            if (bodyAnimator != null)
                bodyAnimator.SetBool(ANIM_PARAM_IS_BLUE, isBlueMode);
        }

        private PlayerOverworldManager FindPlayerInBeam()
        {
            var overlapFilter = ContactFilter2D.noFilter;
            overlapFilter.useTriggers = true;

            int count = lineCollider.Overlap(overlapFilter, _overlapBuffer);

            for (int i = 0; i < count; i++)
            {
                Collider2D hit = _overlapBuffer[i];

                if (hit == null || !hit.CompareTag(TagConstants.PlayerTag))
                    continue;

                if (hit.TryGetComponent(out PlayerOverworldManager player))
                    return player;
            }

            return null;
        }
    }
}
