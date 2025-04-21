using LetsChat.Intefaces;

namespace LetsChat.Users.UpdateUser;

public record UpdateUserRequest(UserDto UserDto) : IRequest<UpdateUserResult>;
public record UpdateUserResult(UserDto UserDto);
public class UpdateUserHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserRequest, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var updatedUser = await userRepository.UpdateUser(MapToUser(request.UserDto), cancellationToken);
        return new UpdateUserResult(MapToDto(updatedUser));
    }

    //TODO: use automapper
    private User MapToUser(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            Username = userDto.Username,
            Password = userDto.Password,
            Surname = userDto.Surename
        };
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Username = user.Username,
            Password = user.Password,
            Surename = user.Surname
        };
    }
}
