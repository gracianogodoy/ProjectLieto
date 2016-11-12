using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "GeneralSettingsInstaller", menuName = "Installers/GeneralSettingsInstaller")]
    public class GeneralSettingsInstaller : ScriptableObjectInstaller<GeneralSettingsInstaller>
    {
        public DamageBlink.Settings DamageBlinkSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(DamageBlinkSettings);
        }
    }
}