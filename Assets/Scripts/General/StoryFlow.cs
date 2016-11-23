using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GG
{
    public class BossFightStartSignal : Signal<BossFightStartSignal> { }
    public class BossDeathSignal : Signal<BossDeathSignal> { }

    public class StoryFlow : IInitializable
    {
        public enum State
        {
            BossAct,
            PrincessAct,
            FinalAct,
            None
        }

        private TalkCallback _talkCallback;
        private RPGTalk _talk;
        private InputHandler _input;
        private Settings _settings;
        private CountDeadSquires _countDeadSquires;
        private CameraFade _cameraFade;
        [Inject]
        private BossFightStartSignal _bossFightStartSignal;
        [Inject]
        private BossDeathSignal _bossDeathSignal;

        private int _currentAct;
        private State _currentState;

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

            var backgroundMusic = SoundKit.instance.backgroundSound;

            if (backgroundMusic == null)
            {
                SoundKit.instance.playBackgroundMusic(_settings.bgm, 1);
            }
            else if (backgroundMusic.audioSource.clip != _settings.bgm)
                SoundKit.instance.playBackgroundMusic(_settings.bgm, 1);

            _bossDeathSignal += onBossDeath;

            _currentState = State.None;
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
                case Act.ActType.PrincessAct:
                    _currentState = State.PrincessAct;
                    break;
            }

            startAct(act);
        }

        private void finalAct()
        {
            _currentState = State.FinalAct;
            _talk.textUI = GameObject.FindWithTag("Text").GetComponent<Text>();
            _talk.textUI.enabled = true;
            _talk.variables[0].variableValue = _countDeadSquires.Count.ToString();
            _talk.variables[1].variableValue = _countDeadSquires.TotalSquires.ToString();
            var storyCanvas = GameObject.Find("story_canvas").GetComponent<Image>();
            storyCanvas.enabled = true;
        }

        #region Boss Act
        private void bossAct()
        {
            _currentState = State.BossAct;
            fadeToMusic(_settings.bgm_boss);

            _cameraFade.GetComponent<CameraFollow>().enabled = false;
            Timing.RunCoroutine(moveCameraTo("camera_target"));

        }

        private void controlFlame()
        {
            var fire = GameObject.FindObjectOfType<BossFire>();
            fire.LightOn();
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

        private void onBossDeath()
        {
            fadeToMusic(_settings.bgm);
            Timing.RunCoroutine(moveCameraTo("camera_target"));
            NextAct();
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
            if (_currentState == State.FinalAct)
            {
                Timing.RunCoroutine(backToMenu());
            }
            else if (_currentState == State.BossAct)
            {
                _input.SetEnable(true);
                controlFlame();
                Timing.RunCoroutine(moveCameraTo("camera_target2"));
                _bossFightStartSignal.Fire();
            }
            else if (_currentState == State.PrincessAct)
            {
                Timing.RunCoroutine(wait());
            }
            else
                _input.SetEnable(true);
        }

        private IEnumerator<float> startStory()
        {
            yield return Timing.WaitForSeconds(_settings.timeToStart);

            NextAct();
        }

        private IEnumerator<float> wait()
        {
            yield return Timing.WaitForSeconds(2);
            NextAct();
        }

        private void startAct(Act act)
        {
            _talk.txtToParse = act.dialogueText;
            _talk.lineToStart = act.startLine;
            _talk.lineToBreak = act.endLine;

            _talk.NewTalk();
        }

        private IEnumerator<float> backToMenu()
        {
            yield return Timing.WaitForSeconds(_settings.finalActFadeTime);

            var loadScenes = GameObject.FindObjectOfType<LoadScene>();
            loadScenes.Credits();
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
                PrincessAct,
            }

            public TextAsset dialogueText;
            public int startLine;
            public int endLine;
            public ActType actType;
        }
    }
}
