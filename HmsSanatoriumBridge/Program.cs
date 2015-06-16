using Topshelf;

namespace HmsSanatoriumBridge
{
    public class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();
                x.StartAutomatically();                
                x.SetDescription("HMS <-> Sanatorium Integration Bridge");
                x.Service<BridgeService>();                
            });
        }
    }
}
