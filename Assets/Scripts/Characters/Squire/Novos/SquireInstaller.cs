using UnityEngine;
using Zenject;

namespace GG
{
    public class SquireInstaller : MonoInstaller<SquireInstaller>
    {
        public override void InstallBindings()
        {
            CharacterMotorInstaller.Install(Container, gameObject);

            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            

        }
    }
}