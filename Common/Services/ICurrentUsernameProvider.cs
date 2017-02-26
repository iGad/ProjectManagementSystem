namespace Common.Services
{
    /// <summary>
    /// Поставщик идентификатора текущего пользователя
    /// </summary>
    public interface ICurrentUsernameProvider
    {
        /// <summary>
        /// Получить идентификатор текущего пользователя
        /// </summary>
        /// <returns>Username для пользователя</returns>
        string GetCurrentUsername();
    }
}
