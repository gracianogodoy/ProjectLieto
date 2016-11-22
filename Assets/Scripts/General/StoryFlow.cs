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
        private bool _isBossAct;

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
            SoundKit.instance.playBackgroundMusic(_settings.bgm, 1);
        }

        public void NextAct()
        {
            _currentAct++;
            _input.SetEnable(false);
            _currentAct = Mathf.Clamp(_currentAct, 0, _settings.acts.Length - 1);
            var act = _settings.acts[_currentAct];

            if (act.actType == Settings.ActType.FinalAct)
            {
                finalAct();
            }

            switch (act.actType)
            {
                case Settings.ActType.FinalAct:
                    finalAct();
                    break;
                case Settings.ActType.BossAct:
                    bossAct();
                    break;
            }

            startAct(act);
        }

        private void finalAct()
        {
            _isFinalAct = true;
            _talk.variables[0].variableValue = _countDeadSquires.Count.ToString();
            _talk.variables[1].variableValue = _countDeadSquires.TotalSquires.ToString();
        }

        private void bossAct()
        {
            _isBossAct = true;
            fadeTo(_settings.bgm_boss);
        }

        private void fadeTo(AudioClip nextMusic)
        {
            var backgroundMusic = SoundKit.instance.backgroundSound;
            backgroundMusic.fadeOut(1f, () => SoundKit.instance.playBackgroundMusic(nextMusic, 1).fadeIn(1f));
        }

        private void onTalkFinish()
        {
            if (_isFinalAct)
            {
                Timing.RunCoroutine(fadeIn());
            }
            else if (_isBossAct)
            {
                _input.SetEnable(true);
                fadeTo(_settings.bgm);
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

            var loadScenes = GameObject.FindObjectOfType<LoadScene>();
            loadScenes.BackToMenu();
        }

        [System.Serializable]
        public class Settings
        {
            public float timeToStart;
            public float finalActFadeTime;
            public Act[] acts;
            public AudioClip bgm;
            public AudioClip bgm_boss;

            public enum ActType
            {
                NormalAct,
                BossAct,
                FinalAct,
                TutorialAct,
            }

            [System.Serializable]
            public class Act
            {
                public TextAsset dialogueText;
                public int startLine;
                public int endLine;
                public ActType actType;
            }
        }
    }
}
