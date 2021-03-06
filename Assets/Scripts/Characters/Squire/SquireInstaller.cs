using Zenject;

namespace GG
{
    public class SquireInstaller : MonoInstaller<SquireInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).WithId(InjectId.Owner).AsSingle();
            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            Container.BindAllInterfacesAndSelf<Move>().To<Move>().AsSingle();
            Container.BindAllInterfacesAndSelf<Pushback>().To<Pushback>().AsSingle();
            Container.BindAllInterfacesAndSelf<DropPowerup>().To<DropPowerup>().AsSingle();

            //Container.BindAllInterfacesAndSelf<SquireResurrect>().To<SquireResurrect>().AsSingle();
            Container.BindAllInterfacesAndSelf<SquireDeath>().To<SquireDeath>().AsSingle();
            Container.BindAllInterfacesAndSelf<SquireAI>().To<SquireAI>().AsSingle();

            LifeInstaller.Install(Container, gameObject);
            CharacterMotorInstaller.Install(Container, gameObject);
        }
    }
}