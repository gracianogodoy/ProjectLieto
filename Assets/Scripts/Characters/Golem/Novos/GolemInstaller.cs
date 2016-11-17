using Zenject;

namespace GG
{
    public class GolemInstaller : MonoInstaller<GolemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).WithId(InjectId.Owner).AsSingle();

            Container.BindInstance(GetComponent<ProximitySensor>()).AsSingle();

            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            Container.BindAllInterfacesAndSelf<EnemyAttackAI>().To<EnemyAttackAI>().AsSingle();

            Container.BindAllInterfacesAndSelf<GolemDeath>().To<GolemDeath>().AsSingle();

            LifeInstaller.Install(Container, gameObject);
            AttackInstaller.Install(Container);
        }
    }
}