using Domain.Repositories;

namespace Application.Services.Repositories
{
    public  class ConfigurationGameRepositoryImpl : ConfigurationGameRepository
    {
        // TODO: Load configuration from server
        public int StartLives => 10;
    }
}
