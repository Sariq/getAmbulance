namespace getAmbulance
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AspNet.Identity.MongoDB;
    using Models;
    using MongoDB.Driver;
    using static WhiteLabel.WhiteLabelModel;
    public class ApplicationIdentityContext : IDisposable
	{
		public static ApplicationIdentityContext Create()
		{
			// todo add settings where appropriate to switch server & database in your own application
			var client = new MongoClient("mongodb://localhost:27017");
			var database = client.GetDatabase("mydbtest");
			var users = database.GetCollection<ApplicationUser>("users");
			var roles = database.GetCollection<IdentityRole>("roles");
            var browserClients = database.GetCollection<BrowserClient>("browserClients");
            var refreshTokens = database.GetCollection<RefreshToken>("refreshTokens");
            var reservations = database.GetCollection<ReservationEntity>("Reservations");
            var whiteLabels = database.GetCollection<WhiteLabelEntity>("WhiteLabels");

            return new ApplicationIdentityContext(users, roles, browserClients, refreshTokens, reservations, whiteLabels);
		}
        public ApplicationIdentityContext()
        {
            Create();
        }

        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles, IMongoCollection<BrowserClient> clients, IMongoCollection<RefreshToken> refreshTokens, IMongoCollection<ReservationEntity>  reservations, IMongoCollection<WhiteLabelEntity> whiteLabels)
		{
			Users = users;
			Roles = roles;
            BrowserClients = clients;
            RefreshTokens = refreshTokens;
            Reservations = reservations;
            WhiteLabels = whiteLabels;
            
        }
        public IMongoCollection<ApplicationUser> Users { get; set; }
        public IMongoCollection<IdentityRole> Roles { get; set; }
        public IMongoCollection<BrowserClient> BrowserClients { get; set; }
        public IMongoCollection<RefreshToken> RefreshTokens { get; set; }
        public IMongoCollection<ReservationEntity> Reservations { get; set; }
        public IMongoCollection<WhiteLabelEntity> WhiteLabels { get; set; }

        public Task<List<IdentityRole>> AllRolesAsync()
		{
			return Roles.Find(r => true).ToListAsync();
		}


        public void Dispose()
		{
		}
	}
}