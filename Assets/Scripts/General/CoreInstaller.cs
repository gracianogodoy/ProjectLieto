using UnityEngine;
using Zenject;

namespace GG
{
    public class CoreInstaller : MonoInstaller<CoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindAllInterfacesAndSelf<SwitchWorld>().To<SwitchWorld>().AsSingle();
            Container.BindAllInterfacesAndSelf<GameFlow>().To<GameFlow>().AsSingle();
            Container.BindAllInterfacesAndSelf<InputHandler>().To<InputHandler>().AsSingle();

            Container.BindSignal<LietoDeathSignal>();
            Container.BindSignal<LietoResurrectSignal>();

            Container.BindInstance(GameObject.FindObjectOfType<CameraFade>()).AsSingle();
            Container.BindInstance(GameObject.FindObjectOfType<CameraFollow>()).AsSingle();
        }
    }
}