using UnityEngine;
using Zenject;

namespace GG
{
    public class SquireDeathSignal : Signal<SquireDeathSignal> { }

    public class CountDeadSquires : IInitializable, System.IDisposable
    {
        private SquireDeathSignal _deathSignal;
        private int _count;
        private int _totalSquires;

        public float Count
        {
            get
            {
                return _count;
            }
        }

        public int TotalSquires
        {
            get
            {
                return _totalSquires;
            }
        }

        public CountDeadSquires(SquireDeathSignal deathSignal)
        {
            _deathSignal = deathSignal;
        }

        public void Dispose()
        {
            _deathSignal -= squireDeath;
        }

        public void Initialize()
        {
            _totalSquires = GameObject.FindGameObjectsWithTag("Squire").Length;
            _deathSignal += squireDeath;
        }

        private void squireDeath()
        {
            _count++;
        }
    }
}