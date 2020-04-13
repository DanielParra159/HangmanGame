using Domain.Repositories;

namespace Application.Services.Repositories
{
    public  class ConfigurationGameRepository : IConfigurationGameRepository
    {
        // TODO: Load configuration from server
        public int StartLives => 10;
    }
}
