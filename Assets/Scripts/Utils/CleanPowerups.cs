using UnityEngine;
using Zenject;

namespace GG
{
    public class CleanPowerups : IInitializable
    {
        private LietoResurrectSignal _signal;

        public CleanPowerups(LietoResurrectSignal signal)
        {
            _signal = signal;
        }

        public void Initialize()
        {
            _signal += onResurrect;
        }

        private void onResurrect()
        {
            var powerups = GameObject.FindObjectsOfType<Powerup>();

            for (int i = 0; i < powerups.Length; i++)
            {
                var powerup = powerups[i];

                if (powerup.PowerupType == Powerup.Type.Dropped)
                    GameObject.Destroy(powerup.gameObject);
            }
        }
    }

}