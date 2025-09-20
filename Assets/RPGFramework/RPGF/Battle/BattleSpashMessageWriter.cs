using RPGF.Core.TextWriter;
using System;
using UnityEngine;

namespace RPGF.Battle
{
    public class BattleSpashMessageWriter : TextWriterBase, IDisposable
    {
        [SerializeField]
        private AudioSource _writeSound;

        public string SpashText { get; set; } = "";

        public override void Initialize()
        {
            base.Initialize();

            Dispose();
        }

        public void WriteSpash()
        {
            base.InvokeWrite(new WriterMessage()
            {
                text = SpashText
            });
        }
        public void WriteSplash(string text)
        {
            SpashText = text;
            WriteSpash();
        }


        public override bool ContinueCanExecute()
        {
            return true;
        }

        public override bool SkipCanExecute()
        {
            return false;
        }

        public override void OnStartWriting()
        {
            base.OnStartWriting();
        }

        public override void OnEndWriting()
        {
            base.OnEndWriting();
        }

        public override void OnEveryLetter(char letter)
        {
            base.OnEveryLetter(letter);

            if (_writeSound.clip != null)
                _writeSound.Play();
        }

        public void Dispose()
        {
            CancelWrite();

            textMeshPro.text = "";
        }
    }
}