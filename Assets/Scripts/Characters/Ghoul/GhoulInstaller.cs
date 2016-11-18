using Zenject;

namespace GG
{
    public class GhoulInstaller : MonoInstaller<GhoulInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).WithId(InjectId.Owner).AsSingle();

            Container.BindInstance(GetComponent<ProximitySensor>()).AsSingle();

            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            Container.BindAllInterfacesAndSelf<EnemyAttackAI>().To<EnemyAttackAI>().AsSingle();
            Container.BindAllInterfacesAndSelf<Pushback>().To<Pushback>().AsSingle();
            Container.BindAllInterfacesAndSelf<DropPowerup>().To<DropPowerup>().AsSingle();

            Container.BindAllInterfacesAndSelf<GhoulAI>().To<GhoulAI>().AsSingle();
            Container.BindAllInterfacesAndSelf<GhoulResurrect>().To<GhoulResurrect>().AsSingle();
            Container.BindAllInterfacesAndSelf<GhoulDeath>().To<GhoulDeath>().AsSingle();


            LifeInstaller.Install(Container, gameObject);
            CharacterMotorInstaller.Install(Container, gameObject);
            AttackInstaller.Install(Container);
        }
    } 
}