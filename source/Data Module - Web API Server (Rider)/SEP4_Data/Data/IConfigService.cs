using System.Runtime.CompilerServices;

namespace SEP4_Data.Data
{
    public interface IConfigService
    {
        public int Port { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public bool Https { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public string DbHost { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public int DbPort { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public string DbName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public string DbUser { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public string DbPassword { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public byte[] JwtKey { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public byte[] Salt { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public int UserPostPermissionLevel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public int TokenExpire { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public bool Swagger { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public bool ReInitializeDb { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
        public int SampleInterval { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
    }
}