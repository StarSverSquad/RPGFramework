using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.GUI
{
    public class GUIManagerBase : RPGFrameworkBehaviour
    {
        [SerializeField]
        protected GUIBlockBase firstBlock;
        [SerializeField]
        protected GameObject content;

        public bool IsOpened { get; private set; }

        public Stack<GUIBlockBase> GUIStack { get; private set; }

        #region EVENTS

        public event Action OnOpenEvent;
        public event Action OnCloseEvent;

        public event Action<GUIBlockBase> OnNextBlockEvent;
        public event Action OnPreviewBlockEvent;

        #endregion

        public override void Initialize()
        {
            GUIStack = new Stack<GUIBlockBase>();

            foreach (var item in GetComponentsInChildren<GUIBlockBase>())
            {
                item.Initialize(this);
            }

            content.SetActive(false);

            IsOpened = false;
        }

        public void NextBlock(GUIBlockBase block)
        {
            if (GUIStack.Count > 0)
                GUIStack.Peek().Diativate();

            GUIStack.Push(block);

            block.Activate();

            OnNext(block);
            OnNextBlockEvent?.Invoke(block);
        }
        public void PreviewBlock()
        {
            var block = GUIStack.Pop();

            block.Diativate();
            block.Dispose();

            GUIStack.Peek().Activate();

            OnPreviewBlock();
            OnPreviewBlockEvent?.Invoke();
        }

        public void Open()
        {
            content.SetActive(true);

            NextBlock(firstBlock);

            IsOpened = true;

            OnOpen();
            OnOpenEvent?.Invoke();
        }
        public void Close()
        {
            foreach (var item in GUIStack)
            {
                item.Diativate();
                item.Dispose();
            }
            GUIStack.Clear();

            content.SetActive(false);

            IsOpened = false;

            OnClose();
            OnCloseEvent?.Invoke();
        }

        #region VIRTUALS

        public virtual void OnOpen() { }
        public virtual void OnClose() { }

        public virtual void OnNext(GUIBlockBase block) { }
        public virtual void OnPreviewBlock() { }

        #endregion
    }
}