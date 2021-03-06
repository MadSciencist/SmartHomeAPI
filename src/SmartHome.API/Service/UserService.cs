﻿using AutoMapper;
using Matty.Framework;
using Matty.Framework.Enums;
using Matty.Framework.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.API.Dto;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Enums;
using SmartHome.Core.Entities.User;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome.API.Service
{
    public class UserService : IUserService
    {
        public ClaimsPrincipal Principal { get; set; }
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenBuilder tokenBuilder,
            IPasswordValidator<AppUser> passwordValidator, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenBuilder = tokenBuilder;
            _passwordValidator = passwordValidator;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserDto>> GetUserAsync(int userId)
        {
            var response = new ServiceResult<UserDto>(Principal);

            var user = await _userManager.Users
                .Include(x => x.CreatedNodes)
                .Include(x => x.CreatedControlStrategies)
                .Include(x => x.EligibleNodes)
                .Include(x => x.ActivatedBy)
                .Include(x => x.UiConfiguration)
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                response.Alerts.Add(new Alert($"User does not exist", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            // only admin or user itself can access
            if (ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin) || ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId))
            {
                response.Data = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    EmailConformed = user.EmailConfirmed,
                    ActivatedById = user.ActivatedBy?.Id ?? 0,
                    ActivationDate = user.ActivationDate,
                    IsActive = user.IsActive,
                    CreatedControlStrategies = user.CreatedControlStrategies?.Select(x => x.Id).ToList(),
                    CreatedNodes = user.CreatedNodes?.Select(x => x.Id).ToList(),
                    EligibleNodes = user.EligibleNodes?.Select(x => x.Id).ToList(),
                    UiConfigurations = _mapper.Map<List<UiConfigurationDto>>(user.UiConfiguration)
                };

                return response;
            }

            throw new SmartHomeUnauthorizedException($"Unauthorized");
        }

        public async Task<ServiceResult<TokenDto>> LoginAsync(LoginDto login)
        {
            var response = new ServiceResult<TokenDto>(Principal);

            var user = await _userManager.FindByNameAsync(login.Login);

            if (user is null)
            {
                response.Alerts.Add(new Alert($"User { login.Login } does not exist", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            if (!user.IsActive)
            {
                response.Alerts.Add(new Alert($"User {login.Login} is not active", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status400BadRequest;
                return response;
            }

            await _signInManager.SignOutAsync(); // terminate existing session

            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);

            if (!signInResult.Succeeded)
            {
                response.Alerts.Add(new Alert("Incorrect login or password", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status401Unauthorized;
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var (token, validTo) = _tokenBuilder.Build(user, roles);

            response.Data = new TokenDto
            {
                Issued = DateTime.UtcNow,
                Schema = "Bearer",
                Token = token,
                ValidTo = validTo
            };

            return response;
        }

        public async Task<ServiceResult<TokenDto>> RegisterAsync(RegisterDto register)
        {
            var response = new ServiceResult<TokenDto>(Principal);

            if (await _userManager.FindByNameAsync(register.Login) != null)
            {
                response.Alerts.Add(new Alert($"User {register.Login} already exist", MessageType.Error));
                return response;
            }

            var user = new AppUser
            {
                Email = register.Email,
                UserName = register.Login,
                IsActive = false
            };

            var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, register.Password);
            if (!passwordValidationResult.Succeeded)
            {
                response.AddAlerts(passwordValidationResult.Errors.Select(x => new Alert(x.Description, MessageType.Error)));
                response.ResponseStatusCodeOverride = StatusCodes.Status400BadRequest;
                return response;
            }

            var createResult = await _userManager.CreateAsync(user, register.Password);
            if (!createResult.Succeeded)
            {
                response.AddAlerts(createResult.Errors.Select(x => new Alert(x.Description, MessageType.Error)));
                response.ResponseStatusCodeOverride = StatusCodes.Status400BadRequest;
                return response;
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.User);
            if (!addToRoleResult.Succeeded)
            {
                response.AddAlerts(addToRoleResult.Errors.Select(x => new Alert(x.Description, MessageType.Error)));
                response.ResponseStatusCodeOverride = StatusCodes.Status400BadRequest;
                return response;
            }

            // SingIn user and create token
            await _signInManager.PasswordSignInAsync(user, register.Password, false, false);
            var roles = await _userManager.GetRolesAsync(user);
            var (token, validTo) = _tokenBuilder.Build(user, roles);

            response.Data = new TokenDto
            {
                Issued = DateTime.UtcNow,
                Schema = "Bearer",
                Token = token,
                ValidTo = validTo
            };

            return response;
        }

        public async Task<ServiceResult<object>> DeleteAsync(int userId)
        {
            var response = new ServiceResult<object>(Principal);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                response.Alerts.Add(new Alert($"User does not exist", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;
                return response;
            }

            // only admin or user itself can access
            if (ClaimsPrincipalHelper.IsInRole(Principal, Roles.Admin) || ClaimsPrincipalHelper.HasUserClaimedIdentifier(Principal, userId.ToString()))
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                if (deleteResult.Succeeded)
                {
                    response.Alerts.Add(new Alert("Successfully deleted", MessageType.Success));
                }
                else
                {
                    deleteResult.Errors.Select(x => new Alert(x.Description, MessageType.Error)).ToList();
                    response.ResponseStatusCodeOverride = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                response.Alerts.Add(new Alert($"User does not have permissions for requested operation", MessageType.Error));
                response.ResponseStatusCodeOverride = StatusCodes.Status401Unauthorized;
            }

            return response;
        }

        public Task<ServiceResult<object>> UpdateAsync(RegisterDto update, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
