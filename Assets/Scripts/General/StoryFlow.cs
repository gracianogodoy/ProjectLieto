using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GG
{
    public class BossFightStartSignal : Signal<BossFightStartSignal> { }

    public class StoryFlow : IInitializable
    {
        private TalkCallback _talkCallback;
        private RPGTalk _talk;
        private InputHandler _input;
        private Settings _settings;
        private CountDeadSquires _countDeadSquires;
        private CameraFade _cameraFade;
        [Inject]
        private BossFightStartSignal _bossFightStartSignal;

        private int _currentAct;
        private bool _isFinalAct;
        private bool _isBossAct;

        public StoryFlow(TalkCallback talkCallback, InputHandler input, Settings settings, RPGTalk talk,
            CountDeadSquires countDeadSquires, CameraFade cameraFade)
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
            _currentAct = _settings.startAct;

            if (_currentAct < 0)
            {
                _input.SetEnable(false);
                Timing.RunCoroutine(startStory());
            }

            SoundKit.instance.playBackgroundMusic(_settings.bgm, 1);
        }

        public void NextAct()
        {
            _currentAct++;
            _input.SetEnable(false);
            _currentAct = Mathf.Clamp(_currentAct, 0, _settings.acts.Length - 1);
            var act = _settings.acts[_currentAct];

            if (act.actType == Act.ActType.FinalAct)
            {
                finalAct();
            }

            switch (act.actType)
            {
                case Act.ActType.FinalAct:
                    finalAct();
                    break;
                case Act.ActType.BossAct:
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

        #region Boss Act
        private void bossAct()
        {
            _isBossAct = true;
            fadeToMusic(_settings.bgm_boss);

            _cameraFade.GetComponent<CameraFollow>().enabled = false;
            Timing.RunCoroutine(moveCameraTo("camera_target"));

            //fadeToMusic(_settings.bgm);
        }

        private void controlFlame(string trigger)
        {
            var fire = GameObject.FindGameObjectWithTag("Fire");
            fire.GetComponent<Animator>().SetTrigger(trigger);
        }

        private IEnumerator<float> moveCameraTo(string targetName)
        {
            var bossPosition = GameObject.Find(targetName);

            while (Vector3.Distance(_cameraFade.transform.position, bossPosition.transform.position) > 0.1f)
            {

                _cameraFade.transform.position = Vector3.MoveTowards(_cameraFade.transform.position, bossPosition.transform.position, 10 * Time.deltaTime);

                yield return 0;
            }
        }
        #endregion

        private void fadeToMusic(AudioClip nextMusic)
        {
            var backgroundMusic = SoundKit.instance.backgroundSound;
            if (backgroundMusic != null)
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
                controlFlame("LightUp");
                Timing.RunCoroutine(moveCameraTo("camera_target2"));
                _bossFightStartSignal.Fire();
            }

            _input.SetEnable(true);
        }

        private IEnumerator<float> startStory()
        {
            yield return Timing.WaitForSeconds(_settings.timeToStart);

            NextAct();
        }

        private void startAct(Act act)
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
            public int startAct;
        }

        [System.Serializable]
        public class Act
        {
            public enum ActType
            {
                NormalAct,
                BossAct,
                FinalAct,
                TutorialAct,
            }

            public TextAsset dialogueText;
            public int startLine;
            public int endLine;
            public ActType actType;
        }
    }
}
