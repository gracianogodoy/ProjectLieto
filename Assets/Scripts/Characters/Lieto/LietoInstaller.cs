using UnityEngine;
using Zenject;
using GG;

public class LietoInstaller : MonoInstaller<LietoInstaller>
{
    public InputHandler.Settings InputSetttings;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().AsSingle();
        Container.BindInstance(InputSetttings);

        Container.BindAllInterfaces<InputHandler>().To<InputHandler>().AsSingle();
        Container.Bind<GG.Move>().AsSingle();

        bindInputCommands();
    }

    private void bindInputCommands()
    {
        Container.BindCommand<InputCommands.MoveCommand, int>().To<GG.Move>(m => m.OnMove).AsSingle();
        Container.BindCommand<InputCommands.JumpCommand>().To<GG.Move>(m => m.OnJump).AsSingle();
        Container.BindCommand<InputCommands.StopJumpCommand>().To<GG.Move>(m => m.OnJump).AsSingle();
        Container.BindCommand<InputCommands.AttackCommand>().To<GG.Move>(m => m.OnJump).AsSingle();
        Container.BindCommand<InputCommands.SwitchCommand>().To<GG.Move>(m => m.OnJump).AsSingle();
    }
}