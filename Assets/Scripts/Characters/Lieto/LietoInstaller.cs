using Zenject;

namespace GG
{
    public class LietoInstaller : MonoInstaller<LietoInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(gameObject).WithId(InjectId.Owner).AsSingle();
            Container.BindAllInterfacesAndSelf<WireEvents>().To<WireEvents>().AsSingle();

            Container.Bind<Move>().AsSingle();

            Container.BindAllInterfacesAndSelf<Jump>().To<Jump>().AsSingle();
            Container.BindAllInterfacesAndSelf<FaceDirection>().To<FaceDirection>().AsSingle();
            Container.BindAllInterfacesAndSelf<Pushback>().To<Pushback>().AsSingle();
            Container.BindAllInterfacesAndSelf<Switch>().To<Switch>().AsSingle();
            Container.BindAllInterfacesAndSelf<Hazard>().To<Hazard>().AsSingle();
            Container.BindAllInterfacesAndSelf<ConsumePowerup>().To<ConsumePowerup>().AsSingle();

            Container.BindAllInterfacesAndSelf<LietoPushed>().To<LietoPushed>().AsSingle();
            Container.BindAllInterfacesAndSelf<LietoResurrect>().To<LietoResurrect>().AsSingle();
            Container.BindAllInterfacesAndSelf<LietoDeath>().To<LietoDeath>().AsSingle();

            Container.BindAllInterfacesAndSelf<TestLife>().To<TestLife>().AsSingle();

            Container.Bind<DetectCheckpoint>().FromComponent(gameObject).AsSingle();
            Container.Bind<DetectPowerup>().FromComponent(gameObject).AsSingle();

            AttackInstaller.Install(Container);
            LifeInstaller.Install(Container, gameObject);
            CharacterMotorInstaller.Install(Container, gameObject);
        }

        public class WireEvents : IInitializable
        {
            #region Lieto Components
            [Inject]
            private Move _move;
            [Inject]
            private Jump _jump;
            [Inject]
            private Attack _attack;
            [Inject]
            private FaceDirection _faceDirection;
            [Inject]
            private InputHandler _input;
            [Inject]
            private Switch _switch;
            [Inject]
            private DetectCheckpoint _detectCheckpoint;
            #endregion

            public void Initialize()
            {
                wireInputs();
                wireJump();
            }

            private void wireInputs()
            {
                _input.OnMove += _move.OnMove;
                _input.OnMove += _faceDirection.SetDirection;
                _input.OnJump += _jump.StartJump;
                _input.OnAttack += _attack.OnAttack;
                _input.OnStopJump += _jump.StopJump;
                _input.OnSwitch += _switch.OnSwitch;
            }

            private void wireJump()
            {
                _jump.OnJump += () => _attack.SetEnable(false);
                _jump.OnStopJump += () => _attack.SetEnable(true);
            }
        }
    }
}