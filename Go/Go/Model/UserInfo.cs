namespace Go.Model
{
    public class UserInfo
    {
        private static UserInfo _info;

        public static UserInfo User
        {
            get
            {
                if (_info == null) _info = new UserInfo();
                return _info;
            }
        }

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
