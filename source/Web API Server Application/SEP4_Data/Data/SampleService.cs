namespace SEP4_Data.Data
{
    public class SampleService : ISampleService
    {
        private readonly IConfigService _config;
        
        public SampleService(IConfigService config)
        {
            _config = config;
        }
    }
}