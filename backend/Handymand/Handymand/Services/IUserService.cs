using Handymand.Models;
using Handymand.Models.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public interface IUserService
    {
        //Auth

        Task<ServiceResponse<UserResponseDTO>> Authenticate(UserRequestDTO model);

        //GetAll
        Task<List<UserDTO>> GetAllUsers();
        Task<List<UserDTO>> GetAllUsers(int pageNr, int noElements, FilterUsersDTO filter);
        Task<UserInfoBarDTO> GetUserInfoBar(int id);

        Task<int> GetNrOfNotifications(int id);

        Task<int> GetTotalNrOfUsers(FilterUsersDTO filter);
        //GetById

        Task<ServiceResponse<UserDTO>> GetById(int Id);

        Task<ServiceResponse<MyUserDTO>> GetMyUser(int Id);

        Task<ServiceResponse<byte[]>> GetProfileImage(int Id);

        //Create

        Task CreateUser(UserDTO user);

        UserDTO ConvertToDTOForCreate(User user);

        //Update
        Task<ServiceResponse<MyUserDTO>> UpdateUser(UpdateUserDTO dto);

        Task<bool> UpdateUserAdmin(UserDTO dto);

        Task<bool> ViewNotificationOnJobOffer(int loggedinId, int jobOfferId, int notificationTypeId);

        //Delete

        Task<bool> Delete(UserDTO dto);
        Task<bool> Block(UserDTO dto);

        //Crypt and Decrypt

        string DecryptStringAES(string encryptedValue);
        string EncryptString(string key, string plainText);



    }
}
