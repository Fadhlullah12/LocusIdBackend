using BCrypt.Net;
using LocusIDBackend.Dtos;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IStudentRepository studentRepository, ILecturerRepository lecturerRepository, IUserRepository userRepository, ITokenService tokenService, IDirectorRepository directorRepository)
        {
            _studentRepository = studentRepository;
            _lecturerRepository = lecturerRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _directorRepository = directorRepository;
        }

        public async Task<BaseResponse<LoginResponseDto>> Login(LoginRequestDto request)
        {
            try
            {
              var user = await _userRepository.Get(a => a.Email == request.Email);
                if (user == null)
                    {
                      return new BaseResponse<LoginResponseDto>
                        {
                            Message = "Sorry Email does not Exist",
                            Success = false,
                        };
                    }
                     // Verify password
                        if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                        {
                            var token = _tokenService.GenerateToken(user.Id, user.Role, $"{user.FirstName} {user.LastName}");
                            var response = new LoginResponseDto
                            {
                                UserId = user.Id,
                                Role = user.Role,
                                Token = token
                            };
                            return BaseResponse<LoginResponseDto>.SuccessResponse(response, "Login successful");
                        }
                        
                        else
                        {
                            return new BaseResponse<LoginResponseDto>
                            {
                                Message = "Invalid password",
                                Success = false,
                            };
                        }
                   
            }
            catch (Exception ex)
            {
                return BaseResponse<LoginResponseDto>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}
