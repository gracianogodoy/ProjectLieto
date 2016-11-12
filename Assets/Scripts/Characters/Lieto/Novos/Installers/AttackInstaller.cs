using UnityEngine;
using Zenject;

namespace GG
{
    public class AttackInstaller : Installer<AttackInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<Attack>().To<Attack>().AsSingle();
            Container.BindAllInterfacesAndSelf<Strike>().To<Strike>().AsSingle();
        }
    }
}