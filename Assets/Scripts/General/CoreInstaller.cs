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
            Container.BindAllInterfacesAndSelf<CleanPowerups>().To<CleanPowerups>().AsSingle();

            Container.BindSignal<LietoDeathSignal>();
            Container.BindSignal<LietoResurrectSignal>();
            Container.BindSignal<PauseSignal>();
            Container.BindSignal<UnpauseSignal>();
            Container.BindSignal<SquireDeathSignal>();
            Container.BindSignal<BossFightStartSignal>();
            Container.BindSignal<BossDeathSignal>();
            Container.BindSignal<DisableSwitchSignal>();

            Container.BindInstance(GameObject.FindObjectOfType<CameraFade>()).AsSingle();
            Container.BindInstance(GameObject.FindObjectOfType<CameraFollow>()).AsSingle();
        }
    }
}