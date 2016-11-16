using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "GeneralSettingsInstaller", menuName = "Installers/GeneralSettingsInstaller")]
    public class GeneralSettingsInstaller : ScriptableObjectInstaller<GeneralSettingsInstaller>
    {
        public DamageBlink.Settings DamageBlinkSettings;
        public SwitchWorld.Settings SwitchWorldSettings;
        public GameFlow.Settings GameFlowSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(DamageBlinkSettings);
            Container.BindInstance(SwitchWorldSettings);
            Container.BindInstance(GameFlowSettings);
        }
    }
}