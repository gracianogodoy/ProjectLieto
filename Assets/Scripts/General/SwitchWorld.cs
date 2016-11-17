using MovementEffects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace GG
{
    public class SwitchWorld : IInitializable
    {
        private GameObject _world1;
        private GameObject _world2;

        private Settings _settings;
        private bool _isOnFirstWorld = true;
        private bool _isSwitching;

        public SwitchWorld(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            _world1 = GameObject.Find("world1");
            Assert.IsNotNull(_world1);

            _world2 = GameObject.Find("world2");
            Assert.IsNotNull(_world2);
        }

        public void Switch()
        {
            if (!_isSwitching)
            {
                Timing.RunCoroutine(glitch());
            }
        }

        public void SwitchTo(string world, bool clean)
        {
            if (!_isSwitching)
            {
                if ((world == "world1" && _isOnFirstWorld) ||
                   (world == "world2" && !_isOnFirstWorld))
                    return;

                if (!clean)
                    Timing.RunCoroutine(glitch());
                else
                    switchWorlds();
            }
        }

        private bool switchPositions()
        {
            var world1playing = _world1.transform.position.y == _settings.playingPositionY;
            var worldToEnable = world1playing ? _world2 : _world1;
            var worldToDisable = world1playing ? _world1 : _world2;

            changePositionY(worldToDisable.transform, _settings.hidingPositionY);
            changePositionY(worldToEnable.transform, _settings.playingPositionY);

            world1playing = _world1.transform.position.y == _settings.playingPositionY;
            return world1playing;
        }

        private void changePositionY(Transform world, float positionY)
        {
            world.position = new Vector3(world.position.x, positionY, world.position.z);
        }

        private IEnumerator<float> glitch()
        {
            _isSwitching = true;
            var currentGlitch = 0;
            var changeColor = false;

            while (currentGlitch < _settings.numberOfGlitches)
            {
                changeColor = switchPositions();

                yield return Timing.WaitForSeconds(_settings.intervalBetweenGlitches);

                if (!changeColor)
                    currentGlitch++;

                changeColor = !changeColor;
            }

            switchWorlds();

            _isSwitching = false;
        }

        private void switchWorlds()
        {
            if (!_isOnFirstWorld)
                switchPositions();

            _isOnFirstWorld = !_isOnFirstWorld;
        }

        [Serializable]
        public class Settings
        {
            public float playingPositionY;
            public float hidingPositionY;

            public int numberOfGlitches;
            public float intervalBetweenGlitches;
        }
    }
}