using System.Collections;
using DG.Tweening;
using RPGF.Core;
using RPGF.Core.Enums;
using RPGF.Overworld.Player;
using UnityEngine;

namespace SpiderDance.Misc
{
    [RequireComponent(typeof(Collider2D))]
    public class Jumpad : RPGFrameworkBehaviour
    {
        private readonly int _gizmoSegments = 32;
        private readonly Color _gizmoArcColor = new(0.2f, 1f, 0.4f, 0.9f);
        private readonly Color _gizmoPointColor = new(1f, 0.85f, 0.2f, 0.9f);

        [SerializeField]
        private ViewDirection direction;
        [SerializeField]
        private ParticleSystem passiveParticles;
        [SerializeField]
        private ParticleSystem activeParticles;
        [SerializeField]
        private float length = 10f;
        [SerializeField]
        private float speed = 8f;
        [SerializeField]
        private float arcHeight = 2f;
        [SerializeField]
        private Ease launchEase = Ease.OutCubic;
        [SerializeField]
        private float launchDelay = 0.2f;
        [SerializeField]
        private AudioClip launchSound;

        private bool isBusy;
        private Coroutine launchCoroutine;

        private void Start()
        {
            UpdateParticlesDirection();
            SetParticlesActive(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateParticlesDirection();
        }
#endif

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isBusy || Local == null)
                return;

            if (!collision.CompareTag("Player"))
                return;

            PlayerOverworldManager player = collision.GetComponent<PlayerOverworldManager>();

            if (player == null || player.IsJumping)
                return;

            LaunchPlayer(player);
        }

        private void LaunchPlayer(PlayerOverworldManager player)
        {
            if (launchCoroutine != null)
                StopCoroutine(launchCoroutine);

            launchCoroutine = StartCoroutine(LaunchPlayerCoroutine(player));
        }

        private Vector2 GetCenter()
        {
            if (TryGetComponent<Collider2D>(out var collider))
                return collider.bounds.center;

            return transform.position;
        }

        private void OnLaunchComplete()
        {
            isBusy = false;
            SetParticlesActive(false);
        }

        private void SetParticlesActive(bool active)
        {
            UpdateParticlesDirection();

            if (active)
            {
                passiveParticles?.Stop();
                activeParticles?.Play();
            }
            else
            {
                activeParticles?.Stop();
                passiveParticles?.Play();
            }
        }

        private void UpdateParticlesDirection()
        {
            ApplyParticleDirection(activeParticles);
        }

        private void ApplyParticleDirection(ParticleSystem particles)
        {
            if (particles == null)
                return;

            float angle = direction switch
            {
                ViewDirection.Right => -50f,
                ViewDirection.Left => 50f,
                ViewDirection.Up => 0f,
                ViewDirection.Down => 170f,
                _ => 0f
            };

            ParticleSystem.ShapeModule shape = particles.shape;
            Vector3 rotation = shape.rotation;
            rotation.z = angle;
            shape.rotation = rotation;
        }

        private IEnumerator LaunchPlayerCoroutine(PlayerOverworldManager player)
        {
            isBusy = true;

            player.interaction.CanInteract = false;
            player.movement.SetMovementAccess(false);

            if (Overworld.CharacterManager.Models.Count > 0)
                Overworld.CharacterManager.Models[0].transform.position = GetCenter();

            if (launchDelay > 0f)
                yield return new WaitForSeconds(launchDelay);

            SetParticlesActive(true);
            Global.GameAudio.PlaySE(launchSound);
            player.Jump(GetCenter(), direction, length, speed, arcHeight, launchEase, OnLaunchComplete);

            launchCoroutine = null;
        }

        private void OnDrawGizmosSelected()
        {
            DrawJumpArcGizmo();
        }

        private void DrawJumpArcGizmo()
        {
            Vector2 start = GetCenter();
            Vector2 offset = DirectionHelper.GetVectorByViewDiretion(direction) * length;
            Vector2 end = start + offset;

            int segments = Mathf.Max(2, _gizmoSegments);
            Vector3 previousPoint = start;

            Gizmos.color = _gizmoArcColor;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector2 point = EvaluateArcPoint(start, offset, arcHeight, t);
                Vector3 currentPoint = point;

                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            Gizmos.color = _gizmoPointColor;
            Gizmos.DrawSphere(start, 0.12f);
            Gizmos.DrawSphere(end, 0.12f);
            Gizmos.DrawWireSphere(EvaluateArcPoint(start, offset, arcHeight, 0.5f), 0.1f);
        }

        private static Vector2 EvaluateArcPoint(Vector2 start, Vector2 offset, float height, float t)
        {
            Vector2 end = start + offset;
            Vector2 travelDirection = offset.sqrMagnitude > 0f ? offset.normalized : Vector2.right;
            Vector2 perpendicular = new Vector2(-travelDirection.y, travelDirection.x);
            Vector2 control = (start + end) * 0.5f + perpendicular * height;

            float invertedT = 1f - t;
            return invertedT * invertedT * start + 2f * invertedT * t * control + t * t * end;
        }
    }
}
