﻿using Blogg.Models.Entities;

namespace Blogg.Repository.interfaces;

public interface IUserRepository : IRepository<User>
{
	Task<User?> RegisterAsync(User user);

	Task<User?> GetUserByNameAsync(string username);
}
