using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "LietoSettings", menuName = "Installers/LietoSettings")]
    public class LietoSettings : ScriptableObjectInstaller<LietoSettings>
    {
        public InputHandler.Settings InputSetttings;
        public CharacterMotor.Settings MotorSettings;
        public Move.Settings MoveSettings;
        public Jump.Settings JumpSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(MotorSettings);
            Container.BindInstance(InputSetttings);
            Container.BindInstance(MoveSettings);
            Container.BindInstance(JumpSettings);
        }
    }
}