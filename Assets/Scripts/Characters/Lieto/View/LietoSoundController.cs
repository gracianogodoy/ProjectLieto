using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class LietoSoundController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _attackSound;
        [SerializeField]
        private AudioClip _jumpSound;
        [SerializeField]
        private AudioClip _attackHitSound;
        [SerializeField]
        private AudioClip _landedSound;
        [SerializeField]
        private AudioClip _strikedSound;
        public AudioClip consumePowerupSound;

        private Jump _jump;
        private Attack _attack;
        private Life _life;
        private ConsumePowerup _consumePowerup;

        [Inject]
        public void Construct(Jump jump, Attack attack, Life life, ConsumePowerup consumePowerup)
        {
            _jump = jump;
            _attack = attack;
            _life = life;
            _consumePowerup = consumePowerup;
        }

        void Start()
        {
            _jump.OnJump += onJump;
            _jump.OnStopJump += onStopJump;

            _attack.OnAttack += onAttack;
            _attack.OnAttackHit += (hits) => onHit();

            _life.OnTakeDamageFromPoint += onTakeDamage;

            _consumePowerup.OnConsume += onConsumePowerup;
        }

        private void onConsumePowerup()
        {
            SoundKit.instance.playSound(consumePowerupSound);
        }

        private void onTakeDamage(int arg1, Vector2 arg2)
        {
            SoundKit.instance.playSound(_strikedSound);
        }

        private void onStopJump()
        {
            SoundKit.instance.playSound(_landedSound, 0.4f).fadeOut(0.1f);
        }

        private void onJump()
        {
            SoundKit.instance.playSound(_jumpSound);
        }

        private void onAttack()
        {
            SoundKit.instance.playSound(_attackSound, 1, 1, 0);
        }

        private void onHit()
        {
            SoundKit.instance.playSound(_attackHitSound);
        }
    }
}
