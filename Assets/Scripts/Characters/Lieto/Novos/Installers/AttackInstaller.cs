using UnityEngine;
using Zenject;

namespace GG
{
    public class AttackInstaller : Installer<GameObject, AttackInstaller>
    {
        private GameObject _onwer;

        public AttackInstaller(GameObject owner)
        {
            _onwer = owner;
        }

        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();

            Container.BindAllInterfacesAndSelf<Attack>().To<Attack>().AsSingle();
            Container.BindInstance(_onwer).WhenInjectedInto<Attack>();
        }
    }
}