using UnityEngine;
using Zenject;

namespace GG
{
    public class LifeInstaller : Installer<GameObject, LifeInstaller>
    {
        GameObject _owner;

        public LifeInstaller(GameObject owner)
        {
            _owner = owner;
        }

        public override void InstallBindings()
        {
            Container.Bind<DetectStrike>().FromComponent(_owner);
            Container.BindAllInterfacesAndSelf<Life>().To<Life>().AsSingle();
            Container.BindAllInterfacesAndSelf<ReceiveDamage>().To<ReceiveDamage>().AsSingle();
        }
    }
}