﻿using ECommerce.Core.Entities;

namespace ECommerce.Core.Repositories;

public interface IUsersRepository
{
    /// <summary>
    /// Add new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns>AppUser</returns>
    Task<AppUser> Add(AppUser user);

    /// <summary>
    /// Get user by email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns>AppUser?</returns>
    Task<AppUser?> Get(string email, string password);

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id">id of the user</param>
    /// <returns>Object of type AppUser?</returns>
    Task<AppUser?> GetById(Guid id);
}
