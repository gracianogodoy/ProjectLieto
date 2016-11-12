using Zenject;

namespace GG
{
    public class GolemInstaller : MonoInstaller<GolemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).WithId(InjectId.Owner).AsSingle();

            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>();

            LifeInstaller.Install(Container, gameObject);
            AttackInstaller.Install(Container);
        }
    }
}