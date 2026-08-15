using MediatR;
using POS.Application.Features.UserManagement.DTOs;
using POS.Application.Features.UserManagement.Queries;
using POS.Domain.Common;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Services;

namespace POS.Application.Features.UserManagement.Handlers;

/// <summary>
/// Handler for GetUserQuery - retrieves a single user by ID.
/// </summary>
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILocalizationService _localizationService;

    public GetUserQueryHandler(IUserRepository userRepository, ILocalizationService localizationService)
    {
        _userRepository = userRepository;
        _localizationService = localizationService;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            var errorMessage = _localizationService.GetString(
                "user_not_found",
                request.PreferredLanguage);

            return Result<UserResponse>.Failure(errorMessage);
        }

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Email = user.Email.Value,
            FullName = user.FullName,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastModifiedAt = user.LastModifiedAt,
            PhoneNumber = user.PhoneNumber
        };

        return Result<UserResponse>.Success(userResponse);
    }
}
