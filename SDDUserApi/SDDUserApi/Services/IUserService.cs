using SDDUserApi.Data.DTO;
using SDDUserApi.Data.Model;

namespace SDDUserApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(UserDto userDto, string hashPwd);
        //Task AddUserAsync(UserDto userDto, string confirmpassword);
        Task UpdateUserAsync(int id, UserDto userDto);
        Task DeleteUserAsync(int id);
    }
}
