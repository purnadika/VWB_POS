using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;

namespace POS.Application.Features.UserManagement.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly POS.Domain.Interfaces.Repositories.IAuthLogRepository _authLogRepository;
    private readonly POS.Domain.Interfaces.Repositories.IUserRepository _userRepository;
    private readonly POS.Domain.Interfaces.Repositories.IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        POS.Domain.Interfaces.Repositories.IAuthLogRepository authLogRepository,
        POS.Domain.Interfaces.Repositories.IUserRepository userRepository,
        POS.Domain.Interfaces.Repositories.IUnitOfWork unitOfWork)
    {
        _authLogRepository = authLogRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var log = new POS.Domain.Entities.AuthLog
        {
            Email = request.Email,
            IpAddress = "Unknown", // Can be passed from controller if needed
            Timestamp = System.DateTime.UtcNow
        };

        var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);

        Result<string> result;

        if (user == null)
        {
            result = Result.Failure<string>("Invalid credentials");
            log.IsSuccess = false;
            log.Details = "User not found";
        }
        else if (!user.IsActive)
        {
            result = Result.Failure<string>("User is deactivated");
            log.IsSuccess = false;
            log.Details = "User is deactivated";
        }
        else if (!VerifyPassword(request.Password, user.PasswordHash.Hash))
        {
            result = Result.Failure<string>("Invalid credentials");
            log.IsSuccess = false;
            log.Details = "Invalid password";
        }
        else
        {
            user.RecordLoginAttempt();
            result = Result.Success("fake-jwt-token");
            log.IsSuccess = true;
            log.Details = "Login successful";
        }

        await _authLogRepository.AddAsync(log, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    private bool VerifyPassword(string password, string hash)
    {
        try 
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch 
        {
            return false;
        }
    }
}
