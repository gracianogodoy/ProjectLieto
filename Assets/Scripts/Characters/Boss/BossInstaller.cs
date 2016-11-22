using Zenject;

namespace GG
{
    public class BossInstaller : MonoInstaller<BossInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<BossAI>().To<BossAI>().AsSingle();
            LifeInstaller.Install(Container, gameObject);
        }
    } 
}