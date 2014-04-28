using System.Collections.Generic;
using System.Linq;
using DataBaseModel;
using DomainEntities;

namespace Repositories
{
    /// <summary>
    /// Репозитории для пользователей
    /// </summary>
    public class UserRepositories
    {
        #region Constructor & properties

        /// <summary>
        /// Контекст для работы с БД
        /// </summary>
        private readonly Context _context;

        public UserRepositories(Context context)
        {
            _context = context;
        }

        #endregion Constructor & properties

        #region Methods

        /// <summary>
        /// Получить список пользователей для отправки сообщений
        /// </summary>
        public IEnumerable<MembershipUser> GetUsersForSendMessage()
        {
            return _context.Users.ToList();
        }

        /// <summary>
        /// Получить пользователя по id
        /// </summary>
        public MembershipUser GetById(string id)
        {
            return _context.Users.FirstOrDefault(n => n.Id == id);
        }

        #endregion Methods

    }
}
