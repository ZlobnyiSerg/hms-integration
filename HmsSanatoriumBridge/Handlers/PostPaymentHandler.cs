using System;
using Logus.HMS.Messages.Folio;
using Logus.HMS.Messages.Invoice;
using NServiceBus;
using NServiceBus.Logging;

namespace HmsSanatoriumBridge.Handlers
{
    /// <summary>
    ///     Обработчик сообщений о начислении услуг из МИС Санаториум
    /// </summary>
    public class PostPaymentHandler : IHandleMessages<PostTransactionsRequest>
    {
        protected static readonly ILog Log = LogManager.GetLogger<PostPaymentHandler>();
        private readonly IBus _bus;

        public PostPaymentHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(PostTransactionsRequest message)
        {
            try
            {
                Log.InfoFormat("Получен запрос на начисление транзакций:\n PropCode:{0}\n FolioNo:{1}", message.PropertyCode, message.FolioGenericNo);

                // TODO Начилить транзакции из сообщения message на счёт в HMS-системе.
                // Отправить ответ об успешном завершении, перечислив идентификаторы созданных транзакций, а также баланс счёта после начисления (опционально)
                _bus.Reply(new PostTransactionsResponse(message)
                {
                    Succeeded = true,
                    PostedTransactions = new[]
                    {
                        new PostTransactionResponse
                        {
                            FolioBalance = new FolioPocketInfo
                            {
                                Amount = 100.0m, // баланс счёта после начисления
                                CurrencyCode = "RUB"
                            },
                            Ids = new [] {"1", "2", "3"}
                        }
                    }                    
                });
            }
            catch (ApplicationException ex)
            {
                Log.Error("Ошибка при обработке запроса на начисление", ex);
                // Следует отлавливать только высокоуровневые исключения уровня приложения (когда данная услуга не может быть начислена вследствие некорректности параметров сообщения)
                // В случаях с инфраструктурными проблемами (недоступен сервер, файл и т.п.) стоит выбрасывать исключение наружу, чтобы шина позаботилась о повторной доставке этого сообщения
                _bus.Reply(new PostTransactionsResponse(message)
                {
                    Succeeded = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
