﻿using ECommerce.Core.DTOs;

namespace ECommerce.Core.ServiceContracts;

/// <summary>
/// Contains usecases of users
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Handle user login
    /// </summary>
    /// <param name="loginDTO"></param>
    /// <returns>AuthResponseDTO?</returns>
    Task<AuthResponseDTO?> Login(LoginDTO loginDTO);
    /// <summary>
    /// Handle registeration of new user
    /// </summary>
    /// <param name="registerDTO"></param>
    /// <returns>AuthResponseDTO?</returns>
    Task<AuthResponseDTO?> Register(RegisterDTO registerDTO);
}
