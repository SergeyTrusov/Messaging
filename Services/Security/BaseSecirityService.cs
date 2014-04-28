using System;

namespace Services.Security
{
    /// <summary>
    /// ������� ������ ����������
    /// </summary>
    /// <typeparam name="TSecurityObject">������ ������������</typeparam>
    /// <typeparam name="TAction">��� �������� ������������</typeparam>
    /// <typeparam name="TKey">��� ����� �������</typeparam>
    public abstract class BaseSecirityService<TSecurityObject, TAction, TKey> : ISecurityService<TSecurityObject, TAction>
    {
        /// <summary>
        /// ����������� �������� ������������
        /// </summary>
        public abstract void Check(TSecurityObject securityObject, TAction action, TKey id);

        /// <summary>
        /// ����������� �������� ������������
        /// </summary>
        public void Check(TSecurityObject securityObject, TAction action, object id)
        {
            //���� ��� ������������� ��������� ����
            if (id == null || id is TKey)
            {
                Check(securityObject, action, (TKey)id);
                return;
            }

            //���������, �������� �� ��� ������ ����������
            //todo:��� ���������� ������ ����� ��������� ���� �����
            if (!(id is string) || (typeof(TKey) != typeof(int?)))
                throw new Exception("Cannot cast id in security service!");

            //�������� object � int
            int idParsed;
            var result = int.TryParse((string)id, out idParsed);
            if (!result) throw new Exception("Cannot cast id in security service!");
            Check(securityObject, action, (TKey)(object)idParsed);
        }

        /// <summary>
        /// ����������� �������� ������������
        /// </summary>
        public void Check(TSecurityObject securityObject, TAction action)
        {
            Check(securityObject, action, null);
        }

        /// <summary>
        /// ����������� �������� ������������
        /// </summary>
        public void Check(object securityObject, object action, object id)
        {
            Check((TSecurityObject) securityObject, (TAction) action, id);
        }
    }
}