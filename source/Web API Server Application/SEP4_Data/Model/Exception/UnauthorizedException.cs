namespace SEP4_Data.Model.Exception
{
    public class UnauthorizedException : System.Exception
    {
        public UnauthorizedException(string message) : base(message) {}
    }
}