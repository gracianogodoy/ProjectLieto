using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "InputSettingsInstaller", menuName = "Installers/InputSettingsInstaller")]
    public class InputSettingsInstaller : ScriptableObjectInstaller<InputSettingsInstaller>
    {
        public InputHandler.Settings InputSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(InputSettings);
        }
    }
}