using RPGF.Core;
using RPGF.Core.Character;
using RPGF.Core.Enums;
using RPGF.Overworld.Player;
using UnityEngine;

namespace SpiderDance.Misc
{
    [RequireComponent(typeof(Collider2D))]
    public class PushZone : RPGFrameworkBehaviour
    {
        [SerializeField]
        private ViewDirection direction;
        [SerializeField]
        private float force = 5f;

        private PlayerOverworldManager _player;

        private void FixedUpdate()
        {
            if (Local == null || _player == null)
                return;

            if (_player.IsJumping)
            {
                _player.movement.ExternalVelocity = Vector2.zero;
                return;
            }

            _player.movement.ExternalVelocity = DirectionHelper.GetVectorByViewDiretion(direction) * force;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Local == null || !collision.CompareTag(TagConstants.PlayerTag))
                return;

            if (!collision.TryGetComponent<PlayerOverworldManager>(out var player))
                return;

            _player = player;

            Overworld.CharacterManager.SetOtherModelsInvisible(CharacterManager.DEFAULT_FADE_TIME);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (Local == null || !collision.CompareTag(TagConstants.PlayerTag))
                return;

            var player = collision.GetComponent<PlayerOverworldManager>();

            if (player == null || player != _player)
                return;

            _player.movement.ExternalVelocity = Vector2.zero;
            _player = null;

            Overworld.CharacterManager.ResetOthersPositions();
            Overworld.CharacterManager.SetOtherModelsVisible(CharacterManager.DEFAULT_FADE_TIME);
        }

        private void OnDisable()
        {
            if (_player == null)
                return;

            _player.movement.ExternalVelocity = Vector2.zero;
            _player = null;
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 origin = transform.position;

            if (TryGetComponent<Collider2D>(out var collider))
                origin = collider.bounds.center;

            Vector2 end = origin + DirectionHelper.GetVectorByViewDiretion(direction) * Mathf.Max(0.5f, force * 0.2f);

            Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.9f);
            Gizmos.DrawLine(origin, end);
            Gizmos.DrawSphere(end, 0.1f);
        }
    }
}
