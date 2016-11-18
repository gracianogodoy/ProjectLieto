using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GG
{
    public class StoryFlow : IInitializable
    {
        private TalkCallback _talkCallback;
        private RPGTalk _talk;
        private InputHandler _input;
        private Settings _settings;
        private int _currentAct;

        public StoryFlow(TalkCallback talkCallback, InputHandler input, Settings settings, RPGTalk talk)
        {
            _talkCallback = talkCallback;
            _input = input;
            _settings = settings;
            _talk = talk;
        }

        public void Initialize()
        {
            _talkCallback.OnTalkFinish += onTalkFinish;
            _currentAct = -1;
            _input.SetEnable(false);

            Timing.RunCoroutine(startStory());
        }

        public void NextAct()
        {
            _currentAct++;
            _input.SetEnable(false);
            startAct(_settings.acts[_currentAct]);
        }

        private void onTalkFinish()
        {
            _input.SetEnable(true);
        }

        private IEnumerator<float> startStory()
        {
            yield return Timing.WaitForSeconds(_settings.timeToStart);

            NextAct();
        }

        private void startAct(Settings.Act act)
        {
            _talk.txtToParse = act.dialogueText;
            _talk.lineToStart = act.startLine;
            _talk.lineToBreak = act.endLine;

            _talk.NewTalk();
        }

        [System.Serializable]
        public class Settings
        {
            public float timeToStart;
            public Act[] acts;

            [System.Serializable]
            public class Act
            {
                public TextAsset dialogueText;
                public int startLine;
                public int endLine;
            }
        }
    }
}
