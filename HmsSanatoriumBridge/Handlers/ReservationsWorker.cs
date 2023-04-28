using System;
using System.Threading;
using Logus.HMS.Messages.Booking;
using NServiceBus;
using NServiceBus.Logging;

namespace HmsSanatoriumBridge.Handlers
{
    /// <summary>
    ///     Класс содержит реализацию проверки новых броней, изменений в существующих бронях и т.п.
    /// </summary>
    public class ReservationsWorker
    {
        protected static readonly ILog Log = LogManager.GetLogger<ReservationsWorker>();
        private readonly IBus _bus;
        private volatile bool _shouldStop;

        public ReservationsWorker(IBus bus)
        {
            _bus = bus;
        }

        /// <summary>
        ///     Метод должен осуществлять проверку наличия новых броней или изменений в существующих.
        ///     Веротяно он должен "слушать" внешний источник (по Tcp/ip) и перенаправлять сообщения в сервисную шину
        /// </summary>
        public void MonitorReservations()
        {
            Log.InfoFormat("Reservations monitor started...");
            while (!_shouldStop)
            {
                Thread.Sleep(500);
            }
            Log.InfoFormat("Reservations monitor stopped...");
        }

        /// <summary>
        ///     Обработка создания/изменения брони
        /// </summary>
        private void ProcessReservation(bool reservationIsNew)
        {
            var message = new ReservationUpdatedMessage();

            message.GenericNo = "100001";
            message.ArrivalDate = DateTime.Now;

            _bus.Publish(message);
        }

        /// <summary>
        ///     Запрос остановки
        /// </summary>
        public void RequestStop()
        {
            _shouldStop = true;
        }
    }
}