using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundKit : MonoBehaviour
{
    public enum SoundType
    {
        BGM,
        SFX,
        AmbienceSFX,
    }

    public static SoundKit instance = null;
    [Tooltip("Anytime you play a sound and set the scaledVolume it is multiplied by this value")]
    public float soundEffectVolume = 1f;
    public float musicVolume = 1f;
    public float ambienceVolume = 1f;
    public int initialCapacity = 10;
    public int maxCapacity = 15;
    public bool clearAllAudioClipsOnLevelLoad = true;
    [NonSerialized]
    public SKSound backgroundSound;
    private SKSound oneShotSound;

    private Stack<SKSound> _availableSounds;
    private List<SKSound> _playingSounds;


    #region MonoBehaviour

    void Awake()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        // avoid duplicates
        if (instance != null)
        {
            // we set dontDestroyOnLoad to false here due to the Destroy being delayed. it will avoid issues
            // with OnLevelWasLoaded being called while the object is being destroyed.
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Create the _soundList to speed up sound playing in game
        _availableSounds = new Stack<SKSound>(maxCapacity);
        _playingSounds = new List<SKSound>();

        for (int i = 0; i < initialCapacity; i++)
            _availableSounds.Push(new SKSound(this));
    }

    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (clearAllAudioClipsOnLevelLoad)
        {
            for (var i = _playingSounds.Count - 1; i >= 0; i--)
            {
                var s = _playingSounds[i];
                s.audioSource.clip = null;

                _availableSounds.Push(s);
                _playingSounds.RemoveAt(i);
            }
        }
    }

    void OnApplicationQuit()
    {
        instance = null;
    }

    void Update()
    {
        if (backgroundSound != null)
        {
            checkSound(backgroundSound);
        }

        for (var i = _playingSounds.Count - 1; i >= 0; i--)
        {
            var sound = _playingSounds[i];
            if (sound._playingLoopingAudio)
                continue;

            checkSound(sound);
        }
    }

    private void checkSound(SKSound sound)
    {
        sound._elapsedTime += Time.deltaTime;
        if (sound._elapsedTime > sound.audioSource.clip.length)
            sound.stop();
    }

    #endregion


    /// <summary>
    /// fetches the next available sound and adds the sound to the _playingSounds List
    /// </summary>
    /// <returns>The available sound.</returns>
    private SKSound nextAvailableSound()
    {
        SKSound sound = null;

        if (_availableSounds.Count > 0)
            sound = _availableSounds.Pop();

        // if we didnt find an available found, bail out
        if (sound == null)
            sound = new SKSound(this);
        _playingSounds.Add(sound);

        return sound;
    }


    /// <summary>
    /// starts up the background music and optionally loops it. You can access the SKSound via the backgroundSound field.
    /// </summary>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="loop">If set to <c>true</c> loop.</param>
    public SKSound playBackgroundMusic(AudioClip audioClip, float volume, bool loop = true)
    {
        if (backgroundSound == null)
            backgroundSound = new SKSound(this);

        backgroundSound.playAudioClip(audioClip, volume * getVolume(SoundType.BGM), 1f, 0f, SoundType.BGM);
        backgroundSound.setLoop(loop);

        return backgroundSound;
    }

    /// <summary>
    /// fetches any AudioSource it can find and uses the standard PlayOneShot to play. Use this if you don't require any
    /// extra control over a clip and don't care about when it completes. It avoids the call to StartCoroutine.
    /// nb. pan/pitch are not supported as the chosen AudioSource might be in use with another pan/pitch setting and Unity does not support setting
    /// them natively in PlayOneShot, so updating them here can result in bad audio.
    /// </summary>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volumeScale">Volume scale.</param>
    public void playOneShot(AudioClip audioClip, float volumeScale = 1f, SoundType soundType = SoundType.SFX)
    {
        if (oneShotSound == null)
            oneShotSound = new SKSound(this);

        oneShotSound.audioSource.PlayOneShot(audioClip, volumeScale * getVolume(soundType));
    }


    /// <summary>
    /// plays the AudioClip with the default volume (soundEffectVolume)
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    public SKSound playSound(AudioClip audioClip, SoundType soundType = SoundType.SFX)
    {
        return playSound(audioClip, 1f, soundType);
    }


    /// <summary>
    /// plays the AudioClip with the specified volume
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volume">Volume.</param>
    public SKSound playSound(AudioClip audioClip,  float volume, SoundType soundType = SoundType.SFX)
    {
        AudioSource source = null;
        return playSound(audioClip, volume, 1f, 0f, soundType);
    }


    /// <summary>
    /// plays the AudioClip with the specified pitch
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="pitch">Pitch.</param>
    public SKSound playPitchedSound(AudioClip audioClip, float pitch, SoundType soundType = SoundType.SFX)
    {
        return playSound(audioClip, 1f, pitch, 0f, soundType);
    }

    /// <summary>
    /// plays the AudioClip with the specified pan
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="pan">Pan.</param>
    public SKSound playPannedSound(AudioClip audioClip, float pan, SoundType soundType = SoundType.SFX)
    {
        return playSound(audioClip, 1f, 1f, pan, soundType);
    }


    /// <summary>
    /// plays the AudioClip with the specified volumeScale, pitch and pan
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volume">Volume.</param>
    /// <param name="pitch">Pitch.</param>
    /// <param name="pan">Pan.</param>
    public SKSound playSound(AudioClip audioClip, float volumeScale, float pitch, float pan, SoundType soundType = SoundType.SFX)
    {
        // Find the first SKSound not being used. if they are all in use, create a new one
        SKSound sound = nextAvailableSound();
        sound.playAudioClip(audioClip, volumeScale * getVolume(soundType), pitch, pan, soundType);

        return sound;
    }


    /// <summary>
    /// loops the AudioClip. Do note that you are responsible for calling either stop or fadeOutAndStop on the SKSound
    /// or it will not be recycled
    /// </summary>
    /// <returns>The sound looped.</returns>
    /// <param name="audioClip">Audio clip.</param>
    public SKSound playSoundLooped(AudioClip audioClip, SoundType soundType = SoundType.SFX)
    {
        // find the first SKSound not being used. if they are all in use, create a new one
        SKSound sound = nextAvailableSound();
        sound.playAudioClip(audioClip, getVolume(soundType), 1f, 0f, soundType);
        sound.setLoop(true);

        return sound;
    }


    /// <summary>
    /// used internally to recycle SKSounds and their AudioSources
    /// </summary>
    /// <param name="sound">Sound.</param>
    public void recycleSound(SKSound sound)
    {
        // we dont recycle the backgroundSound since it always stays alive
        if (sound == backgroundSound)
            return;

        if (!_playingSounds.Contains(sound))
            return;

        var index = 0;
        while (index < _playingSounds.Count)
        {
            if (_playingSounds[index] == sound)
                break;
            index++;
        }

        _playingSounds.RemoveAt(index);


        // if we are already over capacity dont recycle this sound but destroy it instead
        if (_availableSounds.Count + _playingSounds.Count >= maxCapacity)
            Destroy(sound.audioSource);
        else
            _availableSounds.Push(sound);
    }

    private float getVolume(SoundType soundType)
    {
        var volume = 0f;

        switch (soundType)
        {
            case SoundType.BGM:
                volume = musicVolume;
                break;
            case SoundType.SFX:
                volume = soundEffectVolume;
                break;
            case SoundType.AmbienceSFX:
                volume = ambienceVolume;
                break;
        }

        return volume;
    }

    #region SKSound inner class

    public class SKSound
    {
        private SoundKit _manager;
        private SoundType _soundType;

        public AudioSource audioSource;
        internal Action _completionHandler;
        internal bool _playingLoopingAudio;
        internal float _elapsedTime;

        public SKSound(SoundKit manager)
        {
            _manager = manager;
            audioSource = _manager.gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        public void setRandomPitch(float min, float max)
        {
            var rand = UnityEngine.Random.Range(min, max);
            audioSource.pitch = rand;
        }
        /// <summary>
        /// fades out the audio over duration. this will short circuit and stop immediately if the elapsedTime exceeds the clip.length
        /// </summary>
        /// <returns>The out.</returns>
        /// <param name="duration">Duration.</param>
        /// <param name="onComplete">On complete.</param>
        private IEnumerator doFadeOut(float duration, Action onComplete)
        {
            var startingVolume = audioSource.volume;

            // fade out the volume
            while (audioSource.volume > 0.0f && _elapsedTime < audioSource.clip.length)
            {
                audioSource.volume -= Time.deltaTime * startingVolume / duration;
                yield return null;
            }

            stop();

            // all done fading out
            if (onComplete != null)
                onComplete();
        }

        private IEnumerator doFadeIn(float duration, Action onComplete)
        {
            var endVolume = audioSource.volume;
            audioSource.volume = 0;

            // fade out the volume
            while (audioSource.volume < endVolume && _elapsedTime < audioSource.clip.length && audioSource.isPlaying)
            {
                var deltaDuration = Time.unscaledDeltaTime * endVolume / duration;
                audioSource.volume += deltaDuration;
                yield return null;
            }

            audioSource.volume = endVolume;

            if (onComplete != null)
                onComplete();
        }


        /// <summary>
        /// sets whether the SKSound should loop. If true, you are responsible for calling stop on the SKSound to recycle it!
        /// </summary>
        /// <returns>The SKSound.</returns>
        /// <param name="shouldLoop">If set to <c>true</c> should loop.</param>
        public SKSound setLoop(bool shouldLoop)
        {
            _playingLoopingAudio = true;
            audioSource.loop = shouldLoop;

            return this;
        }


        /// <summary>
        /// sets an Action that will be called when the clip finishes playing
        /// </summary>
        /// <returns>The SKSound.</returns>
        /// <param name="handler">Handler.</param>
        public SKSound setCompletionHandler(Action handler)
        {
            _completionHandler = handler;

            return this;
        }


        /// <summary>
        /// stops the audio clip, fires the completionHandler if necessary and recycles the SKSound
        /// </summary>
        public void stop()
        {
            audioSource.Stop();

            if (_completionHandler != null)
            {
                _completionHandler();
                _completionHandler = null;
            }

            _manager.recycleSound(this);
        }


        /// <summary>
        /// fades out the audio clip over time. Note that if the clip finishes before the fade completes it will short circuit
        /// the fade and stop playing
        /// </summary>
        /// <param name="duration">Duration.</param>
        /// <param name="handler">Handler.</param>
        public void fadeOut(float duration, Action handler = null)
        {
            _manager.StartCoroutine
            (
                doFadeOut(duration, () =>
               {
                   if (handler != null)
                       handler();
               })
            );
        }

        public void fadeIn(float duration, Action handler = null)
        {
            _manager.StartCoroutine
            (
                doFadeIn(duration, () =>
                {
                    if (handler != null)
                        handler();
                })
            );
        }


        internal void playAudioClip(AudioClip audioClip, float volume, float pitch, float pan, SoundType soundType)
        {
            _playingLoopingAudio = false;
            _elapsedTime = 0;
            _soundType = soundType;

            // setup the GameObject and AudioSource and start playing
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1
			audioSource.pan = pan; // Pan removed in version 5.2.1p2
#else
            audioSource.panStereo = pan;
#endif

            // reset some defaults in case the AudioSource was changed
            audioSource.loop = false;
            audioSource.mute = false;

            audioSource.Play();
        }

    }

    #endregion

}
