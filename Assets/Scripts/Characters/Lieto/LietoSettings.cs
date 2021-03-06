using UnityEngine;
using Zenject;

namespace GG
{
    [CreateAssetMenu(fileName = "LietoSettings", menuName = "Installers/LietoSettings")]
    public class LietoSettings : ScriptableObjectInstaller<LietoSettings>
    {
        public CharacterMotor.Settings MotorSettings;
        public Move.Settings MoveSettings;
        public Jump.Settings JumpSettings;
        public Attack.Settings AttackSettings;
        public FaceDirection.Settings FaceDirectionSettings;
        public Life.Settings LifeSettings;
        public Strike.Settings StrikeSettings;
        public Pushback.Settings PushbackSettings;
        public Switch.Settings SwitchSettings;
        public Hazard.Settings HazardSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(MotorSettings);
            Container.BindInstance(MoveSettings);
            Container.BindInstance(JumpSettings);
            Container.BindInstance(AttackSettings);
            Container.BindInstance(FaceDirectionSettings);
            Container.BindInstance(LifeSettings);
            Container.BindInstance(StrikeSettings);
            Container.BindInstance(PushbackSettings);
            Container.BindInstance(SwitchSettings);
            Container.BindInstance(HazardSettings);
        }
    }
}