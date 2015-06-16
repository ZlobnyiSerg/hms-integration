using System;
using System.Threading;
using HmsSanatoriumBridge.Handlers;
using NServiceBus;
using NServiceBus.Logging;
using Topshelf;

namespace HmsSanatoriumBridge
{
    public class BridgeService : ServiceControl
    {
        protected static readonly ILog Log = LogManager.GetLogger<BridgeService>();

        private IStartableBus _bus;
        private ReservationsWorker _reservationsWorker;
        private Thread _reservationsWorkerThread;

        public bool Start(HostControl hostControl)
        {
            Log.Info("Starting bridge service...");
            _bus = (IStartableBus) BusFactory.Create();
            _bus.Start();

            _reservationsWorker = new ReservationsWorker(_bus);
            _reservationsWorkerThread = new Thread(_reservationsWorker.MonitorReservations);
            _reservationsWorkerThread.Start();

            Log.Info("Bridge service started.");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log.Info("Stopping bridge service...");

            _reservationsWorker.RequestStop();
            _reservationsWorkerThread.Join(TimeSpan.FromSeconds(20));

            if (_bus != null)
            {
                _bus.Dispose();
                _bus = null;
            }

            Log.Info("Bridge service stopped.");

            return true;
        }
    }
}