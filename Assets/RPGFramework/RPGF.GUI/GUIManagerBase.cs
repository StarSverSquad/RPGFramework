using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.GUI.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.GUI
{
    public abstract class GUIManagerBase : RPGFrameworkBehaviour, IGUIManager 
    {
        [Inject]
        private readonly DependencyInjection DI;

        [SerializeField]
        protected GUIBlockBase firstBlock;
        [SerializeField]
        protected GameObject content;

        public bool IsOpened { get; private set; }

        public Stack<IGUIBlock> GUIStack { get; private set; }

        #region EVENTS

        public event Action OnOpenEvent;
        public event Action OnCloseEvent;
        
        public event Action<IGUIBlock> OnNextBlockEvent;
        public event Action OnPreviewBlockEvent;

        #endregion

        public override void Initialize()
        {
            GUIStack = new Stack<IGUIBlock>();

            foreach (var item in GetComponentsInChildren<GUIBlockBase>(true))
            {
                DI.InjectInto(item);
                item.Initialize(this);
            }

            content.SetActive(false);

            IsOpened = false;
        }

        public void NextBlock(IGUIBlock block)
        {
            if (GUIStack.Count > 0)
                GUIStack.Peek().SetFocus(false);

            GUIStack.Push(block);

            block.Activate();
            block.SetFocus(true);

            OnNext(block);
            OnNextBlockEvent?.Invoke(block);
        }
        public void PreviewBlock()
        {
            var block = GUIStack.Pop();

            block.SetFocus(false);
            block.Diativate();

            if (GUIStack.Count > 0)
            {
                GUIStack.Peek().Activate();
                GUIStack.Peek().SetFocus(true);
            }
                
            OnPreviewBlock();
            OnPreviewBlockEvent?.Invoke();

            if (GUIStack.Count == 0)
                Close();
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

        public virtual void OnNext(IGUIBlock block) { }
        public virtual void OnPreviewBlock() { }

        #endregion
    }
}