using System;

namespace Commands.Project.Scripts.Modules.Commands.Core.Impl
{
    public class CommandInfo : ICommandInfo
    {
        public Type BindedType { get; }
        
        public CommandInfo(Type bindedType)
        {
            BindedType = bindedType;
        }
    }
}