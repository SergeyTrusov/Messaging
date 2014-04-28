using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using DataBaseModel;
using DomainEntities;
using Microsoft.AspNet.Identity;
using Repositories.QueryObject;

namespace Repositories
{
    /// <summary>
    /// Репозиторий для сообщений
    /// </summary>
    public class MessagesRepository
    {
        #region Constructor & properties

        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly Context _context;

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private readonly IPrincipal _user;

        public MessagesRepository(Context context, IPrincipal user)
        {
            _context = context;
            _user = user;
        }

        #endregion Constructor & properties

        #region Methods

        /// <summary>
        /// Получить входящии сообщения текущего пользоватлея
        /// </summary>
        public IEnumerable<Message> GetInput(QueryObject<Message> queryObject)
        {
            var userid = _user.Identity.GetUserId();
            var query = _context.Messages.Where(n => n.ReceiverId == userid && !n.DeleteReceiver);
            query = queryObject.Query(query).Include(n => n.Receiver).Include(n => n.Sender);
            return query.ToList();
        }

        /// <summary>
        /// Получить исходящии сообщения текущего пользователя
        /// </summary>
        public IEnumerable<Message> GetOutput(QueryObject<Message> queryObject)
        {
            var userid = _user.Identity.GetUserId();
            var query = _context.Messages.Where(n => n.SenderId == userid && !n.DeleteSender);
            query = queryObject.Query(query).Include(n => n.Receiver).Include(n => n.Sender);
            return query.ToList();
        }

        /// <summary>
        /// Получить сообщения по id
        /// todo:Вынести в базовый репощиторий при расшиирени
        /// </summary>
        public Message GetById(int id)
        {
            return _context.Messages.FirstOrDefault(n => n.Id == id);
        }

        #endregion Methods
    }
}
