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
        private CountDeadSquires _countDeadSquires;
        private CameraFade _cameraFade;

        private int _currentAct;
        private bool _isFinalAct;

        public StoryFlow(TalkCallback talkCallback, InputHandler input, Settings settings, RPGTalk talk, CountDeadSquires countDeadSquires, CameraFade cameraFade)
        {
            _talkCallback = talkCallback;
            _input = input;
            _settings = settings;
            _talk = talk;
            _countDeadSquires = countDeadSquires;
            _cameraFade = cameraFade;
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
            _currentAct = Mathf.Clamp(_currentAct, 0, _settings.acts.Length - 1);

            if (_currentAct == _settings.acts.Length - 1)
            {
                finalAct();
            }

            startAct(_settings.acts[_currentAct]);
        }

        private void finalAct()
        {
            _talk.variables[0].variableValue = _countDeadSquires.Count.ToString();
            _talk.variables[1].variableValue = _countDeadSquires.TotalSquires.ToString();
            _isFinalAct = true;
        }

        private void onTalkFinish()
        {
            if (_isFinalAct)
            {
                Timing.RunCoroutine(fadeIn());
            }
            else
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

        private IEnumerator<float> fadeIn()
        {
            _cameraFade.StartFade(Color.black, _settings.finalActFadeTime);
            yield return Timing.WaitForSeconds(_settings.finalActFadeTime);


        }

        [System.Serializable]
        public class Settings
        {
            public float timeToStart;
            public float finalActFadeTime;
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
