using UnityEngine;
using Zenject;

namespace GG
{
    public class CharacterMotorInstaller : Installer<GameObject, CharacterMotorInstaller>
    {
        private GameObject _owner;

        public CharacterMotorInstaller(GameObject owner)
        {
            _owner = owner;
        }

        public override void InstallBindings()
        {
            var characterController2d = _owner.GetComponent<Prime31.CharacterController2D>();
            Container.BindInstance(characterController2d);
            Container.Bind<CharacterMotor>().AsSingle();
            Container.BindAllInterfaces<CharacterMotor>().To<CharacterMotor>().AsSingle();
        }
    }
}