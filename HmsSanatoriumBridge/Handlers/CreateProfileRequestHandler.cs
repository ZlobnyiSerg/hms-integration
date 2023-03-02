using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logus.HMS.Messages.GuestProfile;
using Logus.HMS.Messages.Invoice;
using NServiceBus;
using NServiceBus.Logging;

namespace HmsSanatoriumBridge.Handlers
{
    /// <summary>
    /// Обработчик сообщений о создании/изменении данных профайлов
    /// </summary>
    public class CreateProfileRequestHandler : IHandleMessages<CreateGuestProfileRequest>
    {
        protected static readonly ILog Log = LogManager.GetLogger<CreateProfileRequestHandler>();

        private readonly IBus _bus;
        public CreateProfileRequestHandler(IBus bus)
        {
            _bus = bus;
        }


        public void Handle(CreateGuestProfileRequest message)
        {
            try
            {
                // Если GenericNo задан, то это уже существующий в HMS системе профиль (синхронизация изменений)
                Log.InfoFormat("Получен запрос на создание/изменение профиля гостя:\n LastName:{0}\n FirstName:{0}\n GenericNo:{1}",
                    message.LastName, message.FirstName, message.GenericNo);


                // TODO Создать новый профиль/изменить данные существующего 
                // Отправить ответ об успешном завершении, указав идентификатор созданного/измененного профайла

                _bus.Reply(new CreateGuestProfileResponse(message)
                {
                    Succeeded = true,
                    GenericNo = "", // Строковый идентификатор профайла в HMS системе, по нему потом МИС будет делать начисления
                });

            }
            catch (ApplicationException ex)
            {
                Log.Error("Ошибка при обработке запроса на создание профайла", ex);

                // В случаях с инфраструктурными проблемами (недоступен сервер, файл и т.п.) стоит выбрасывать исключение наружу, чтобы шина позаботилась о повторной доставке этого сообщения
                _bus.Reply(new CreateGuestProfileResponse(message)
                {
                    Succeeded = false,
                    ErrorMessage = ex.Message
                });
            }

        }
    }
}
