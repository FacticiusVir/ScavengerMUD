namespace Keeper.ScavengerMud
{
    public class LoginResult
    {
        static LoginResult()
        {
            Fail = new LoginResult(false, false, null);
        }

        public LoginResult(bool isSuccess, bool isNew, string username)
        {
            this.IsSuccess = isSuccess;
            this.IsNew = isNew;
            this.Username = username;
        }

        public bool IsSuccess
        {
            get;
            private set;
        }

        public bool IsNew
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            private set;
        }

        public static LoginResult Fail
        {
            get;
            private set;
        }
    }
}