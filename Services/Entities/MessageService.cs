using System;
using System.Security.Principal;
using DataBaseModel;
using DomainEntities;
using Microsoft.AspNet.Identity;
using Repositories;
using Services.Contracts;
using Services.Security;
using Services.Security.SecurityActions;

namespace Services.Entities
{
    /// <summary>
    /// Сервис сообщений
    /// </summary>
    public class MessageService
    {
        #region Constructor & properties

        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly Context _context;

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private readonly IPrincipal _principal;

        /// <summary>
        /// Репозитрии сообщений
        /// </summary>
        private readonly MessagesRepository _messagesRepository;
        
        /// <summary>
        /// Сервис безопастности для сообщений
        /// </summary>
        private readonly ISecurityService<Message, MessageSecurityActions> _securityService;

        /// <summary>
        /// Контракт на уведомлении о сообщении
        /// </summary>
        private readonly ISendMessageNotification _messageNotification;

        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        private readonly UserRepositories _userRepositories;

        public MessageService(
            Context context,
            IPrincipal principal,
            MessagesRepository messagesRepository,
            ISecurityService<Message,
                MessageSecurityActions> securityService,
            ISendMessageNotification messageNotification,
            UserRepositories userRepositories)
        {
            _context = context;
            _principal = principal;
            _messagesRepository = messagesRepository;
            _securityService = securityService;
            _messageNotification = messageNotification;
            _userRepositories = userRepositories;
        }

        #endregion  Constructor & properties

        #region Public Methods

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        public void SendMessage(Message message)
        {
            message.SenderId = _principal.Identity.GetUserId();
            _securityService.Check(message, MessageSecurityActions.Send);
            //Проверить доступ

            //Добавить сообщение в БД
            message.SendDate = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();
            
            //Загрузить не хватающии данные и уведомить получателя
            message.Sender = _userRepositories.GetById(message.SenderId);
            message.Receiver = _userRepositories.GetById(message.ReceiverId);
            _messageNotification.Noty(message);
        }

        /// <summary>
        /// Удалить входящее сообщение
        /// </summary>
        public void DeleteInputMessage(int id)
        {
            var message = _messagesRepository.GetById(id);
            _securityService.Check(message, MessageSecurityActions.DeleteInput);
            //Проверить доступ

            //Пометить как удалёное
            _context.Messages.Attach(message);
            message.DeleteReceiver = true;
            _context.SaveChanges();
        }

        /// <summary>
        /// Удалить исходящее сообщение
        /// </summary>
        public void DeleteOutputMessage(int id)
        {
            var message = _messagesRepository.GetById(id);
            _securityService.Check(message, MessageSecurityActions.DeleteOutput);
            //Проверить доступ

            //Пометить как удалёное
            _context.Messages.Attach(message);
            message.DeleteSender = true;
            _context.SaveChanges();
        }

        #endregion  Public Methods
    }
}

