using System;

namespace Services.Security
{
    /// <summary>
    /// Базовый сервис безосности
    /// </summary>
    /// <typeparam name="TSecurityObject">Объект безопасности</typeparam>
    /// <typeparam name="TAction">Тип действий безопасности</typeparam>
    /// <typeparam name="TKey">Тип ключа объекта</typeparam>
    public abstract class BaseSecirityService<TSecurityObject, TAction, TKey> : ISecurityService<TSecurityObject, TAction>
    {
        /// <summary>
        /// Осуществить проверку безопасности
        /// </summary>
        public abstract void Check(TSecurityObject securityObject, TAction action, TKey id);

        /// <summary>
        /// Осуществить проверку безопасности
        /// </summary>
        public void Check(TSecurityObject securityObject, TAction action, object id)
        {
            //Если нет необходимости кастовать ключ
            if (id == null || id is TKey)
            {
                Check(securityObject, action, (TKey)id);
                return;
            }

            //Проверяем, известен ли нам способ приведения
            //todo:при добавлении других типов расширить этот метод
            if (!(id is string) || (typeof(TKey) != typeof(int?)))
                throw new Exception("Cannot cast id in security service!");

            //Приводим object к int
            int idParsed;
            var result = int.TryParse((string)id, out idParsed);
            if (!result) throw new Exception("Cannot cast id in security service!");
            Check(securityObject, action, (TKey)(object)idParsed);
        }

        /// <summary>
        /// Осуществить проверку безопасности
        /// </summary>
        public void Check(TSecurityObject securityObject, TAction action)
        {
            Check(securityObject, action, null);
        }

        /// <summary>
        /// Осуществить проверку безопасности
        /// </summary>
        public void Check(object securityObject, object action, object id)
        {
            Check((TSecurityObject) securityObject, (TAction) action, id);
        }
    }
}