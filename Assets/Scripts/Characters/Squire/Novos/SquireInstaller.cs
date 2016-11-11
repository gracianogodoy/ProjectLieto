using Zenject;

namespace GG
{
    public class SquireInstaller : MonoInstaller<SquireInstaller>
    {
        public override void InstallBindings()
        {
            CharacterMotorInstaller.Install(Container, gameObject);

            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            Container.BindAllInterfacesAndSelf<Move>().To<Move>().AsSingle();
            Container.BindAllInterfacesAndSelf<Life>().To<Life>().AsSingle();

            Container.BindAllInterfacesAndSelf<SquireAI>().To<SquireAI>().AsSingle();

            Container.BindAllInterfacesAndSelf<TestLife>().To<TestLife>().AsSingle();
        }
    }
}