namespace TestTask.ViewModels {
    /// <summary>
    /// Ответ сервиса.
    /// </summary>
    public class ResponseVM {
        /// <summary>
        /// Успешно ли прошло выполнение запроса.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Код, с которым завершился запрос.
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Отправляемые данные (могут отсутствовать).
        /// </summary>
        public object Data { get; set; }
    }
}
