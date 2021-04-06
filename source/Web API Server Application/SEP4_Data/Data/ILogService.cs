using System.Runtime.CompilerServices;

namespace SEP4_Data.Data
{
    public interface ILogService
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string value);
    }
}