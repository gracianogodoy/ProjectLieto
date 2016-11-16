using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;
using MovementEffects;

namespace GG
{
    public class GameFlow : IInitializable, IDisposable
    {
        private LietoDeathSignal _deathSignal;
        private LietoRessurectSignal _ressurectSignal;
        private CameraFade _fade;
        private Settings _settings;

        public GameFlow(LietoDeathSignal deathSignal, LietoRessurectSignal ressurectSignal, CameraFade fade, Settings settings)
        {
            _deathSignal = deathSignal;
            _ressurectSignal = ressurectSignal;
            _fade = fade;
            _settings = settings;
        }

        public void Initialize()
        {
            _deathSignal += onDeathSignal;
        }

        public void Dispose()
        {
            _deathSignal -= onDeathSignal;
        }

        private void onDeathSignal()
        {
            Timing.RunCoroutine(flow());
        }

        private IEnumerator<float> flow()
        {
            yield return Timing.WaitForSeconds(_settings.afterDeathTime);

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(fadeIn()));

            _ressurectSignal.Fire();

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(fadeOut()));
        }

        private IEnumerator<float> fadeIn()
        {
            _fade.StartFade(Color.black, _settings.fadeInTime);
            yield return Timing.WaitForSeconds(_settings.fadeInTime);
        }

        private IEnumerator<float> fadeOut()
        {
            _fade.StartFade(new Color(0, 0, 0, 0), _settings.fadeInTime);
            yield return Timing.WaitForSeconds(_settings.fadeOutTime);
        }

        [Serializable]
        public class Settings
        {
            public float afterDeathTime;
            public float fadeInTime;
            public float fadeOutTime;
        }
    }
}