namespace NotADoctor99.MetroPlugin
{
    using System;
    using Loupedeck;

    public class MetroPlugin : Plugin
    {
        public override Boolean UsesApplicationApiOnly => true;

        public override Boolean HasNoApplication => true;

        public MetroPlugin() => PluginLog.Init(this.Log);

        public override void Load()
        {
        }

        public override void Unload()
        {
        }
    }
}
