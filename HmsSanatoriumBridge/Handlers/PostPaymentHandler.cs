using System;
using Logus.HMS.Messages.Folio;
using NServiceBus;
using NServiceBus.Logging;

namespace HmsSanatoriumBridge.Handlers
{
    /// <summary>
    ///     Обработчик сообщений о начислении услуг из МИС Санаториум
    /// </summary>
    public class PostPaymentHandler : IHandleMessages<PostTransactionOnFolioRequest>
    {
        protected static readonly ILog Log = LogManager.GetLogger<PostPaymentHandler>();
        private readonly IBus _bus;

        public PostPaymentHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(PostTransactionOnFolioRequest message)
        {
            try
            {
                Log.InfoFormat(
                    "Получен запрос на начисление транзакций:\n PropCode:{0}\n FolioNo:{1}\n TransactionCode:{2}\n Name:{3}",
                    message.PropertyCode, message.FolioGenericNo, message.TransactionCode, message.Name);

                // TODO Начилить транзакцию из сообщения message на счёт в HMS-системе. Отправить ответ об успешном завершении, перечислив идентификаторы созданных транзакций                
                _bus.Reply(new PostTransactionResponse(message)
                {
                    Succeeded = true,
                    Ids = new long[] {1, 2, 3}
                });
            }
            catch (ApplicationException ex)
            {
                Log.Error("Ошибка при обработке запроса на начисление", ex);
                // Следует отлавливать только высокоуровневые исключения уровня приложения. 
                // В случаях с инфраструктурными проблемами (недоступен сервер, файл и т.п.) стоит выбрасывать исключение наружу, чтобы шина позаботилась о повторной доставке этого сообщения
                _bus.Reply(new PostTransactionResponse(message)
                {
                    Succeeded = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}