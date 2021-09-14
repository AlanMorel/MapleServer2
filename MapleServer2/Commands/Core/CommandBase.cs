using NLog;

namespace MapleServer2.Commands.Core
{
    public abstract class CommandBase
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public List<string> Aliases { get; protected set; }
        public string Description { get; set; }
        public List<IParameter> Parameters { get; protected set; }
        public string Usage { get; protected set; }

        public CommandBase()
        {
            Parameters = new List<IParameter>();
        }

        public abstract void Execute(CommandTrigger trigger);

        public override string ToString() => GetType().Name;
    }
}
