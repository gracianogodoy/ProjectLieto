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
            _input.SetEnable(false);

            Timing.RunCoroutine(flow());
        }

        private void onTalkFinish()
        {
            _input.SetEnable(true);
        }

        private IEnumerator<float> flow()
        {
            yield return Timing.WaitForSeconds(_settings.timeToStart);

            _talk.NewTalk();

        }

        [System.Serializable]
        public class Settings
        {
            public float timeToStart;
        }
    }
}