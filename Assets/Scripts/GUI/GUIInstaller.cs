using Zenject;

namespace GG
{
    public class GUIInstaller : MonoInstaller<GUIInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<PauseControl>().To<PauseControl>().AsSingle() ;
        }
    } 
}