using System;

namespace Commands.Project.Scripts.Modules.Commands.Core
{
    public interface ICommandInfo
    {
        Type BindedType { get; }
    }
}