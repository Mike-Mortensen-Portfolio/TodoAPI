namespace TodoClient.Services
{
    public class TodoConstants
    {
        /// <summary>
        /// A test interface. Should target a <strong>GET</strong> request method
        /// </summary>
        public const string API_TODO_TEST = "/";
        /// <summary>
        /// The root URL
        /// </summary>
        public const string API_TODO_BASE = "/todoitems";
        /// <summary>
        /// Create to-do item endpoint. Should target a <strong>POST</strong> request method
        /// </summary>
        public const string API_TODO_CREATE = "/todoitems";
        /// <summary>
        /// Enpoint for getting a specific to-do item. Should target a <strong>GET</strong> request method
        /// </summary>
        public const string API_TODO_GET = "/todoitems/";
        /// <summary>
        /// The enpoint for getting all uncompleted to-do items. Should target a <strong>GET</strong> request method
        /// </summary>
        public const string API_TODO_GET_UNCOMPLETED = "/todoitems";
        /// <summary>
        /// An enpoint for getting all to-do items. Should target a <strong>GET</strong> request method
        /// </summary>
        public const string API_TODO_GET_ALL = "/todoitems/all";
        /// <summary>
        /// The endpoint for updating to-do items. Should target a <strong>PUT</strong> request method
        /// </summary>
        public const string API_TODO_UPDATE = "/todoitems/";
        /// <summary>
        /// Delete (soft) to-do item endpoint. Should target a <strong>DELETE</strong> request method
        /// </summary>
        public const string API_TODO_SOFT_DELETE = "/todoitems/soft/";
        /// <summary>
        /// Delete to-do item endpoint. Should target a <strong>DELETE</strong> request method
        /// </summary>
        public const string API_TODO_DELETE = "/todoitems/";
    }
}
