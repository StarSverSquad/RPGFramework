using NUnit.Framework;
using RPGF.Core.Battle.BattleField.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Battle.BattleField
{
    public class SpiderBattleField : BattleFieldBase
    {
        public const float WebGap = 0.65f;

        [SerializeField]
        private List<SpriteRenderer> webPoints = new();
        [SerializeField]
        private int initialMaxVerticalOffset = 1;
        [SerializeField]
        private float ladderModeSpeed = 1f;
        public float LadderModeSpeed => ladderModeSpeed;

        public int MaxVerticalOffset { get; private set; }

        private bool ladderMode = false;
        public bool LadderMode
        {
            get => ladderMode;
            set
            {
                ladderMode = value;
                OnLadderModeChanged(value);
                OnLadderModeChangedCallback?.Invoke(value);
            }
        }

        public event Action<bool> OnLadderModeChangedCallback;

        private Coroutine reorderWebpointsCoroutine;

        public override void Initialize()
        {
            MaxVerticalOffset = initialMaxVerticalOffset;
        }

        private void FixedUpdate()
        {
            if (ladderMode)
            {
                foreach (var web in webPoints)
                {
                    web.transform.Translate(ladderModeSpeed * Time.fixedDeltaTime * Vector2.down);
                }
            }
        }

        public Transform GetWebPoint(int verticalOffset)
        {
            return webPoints[Mathf.RoundToInt(webPoints.Count / 2f) + verticalOffset].transform;
        }

        public void SetVerticalOffset(int verticalOffset)
        {
            MaxVerticalOffset = verticalOffset;
        }

        private void OnLadderModeChanged(bool isLadderMode)
        {
            if (reorderWebpointsCoroutine != null)
            {
                StopCoroutine(reorderWebpointsCoroutine);
                reorderWebpointsCoroutine = null;
            }

            if (ladderMode)
            {
                reorderWebpointsCoroutine = StartCoroutine(ReorderWebpointsCoroutine());
            }
        }

        private IEnumerator ReorderWebpointsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(WebGap / ladderModeSpeed);

                webPoints = webPoints.Skip(1).Concat(webPoints.Take(1)).ToList();
                var last = webPoints[^1];
                var preLast = webPoints[^2];

                last.transform.localPosition = preLast.transform.localPosition + new Vector3(0, WebGap);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var item in webPoints)
            {
                item.enabled = false;
            }
        }
    }
}
