using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IUserManagmentService
    {
        Task<Guid> RegisterUser(UserRegistrationDTO userRegistrationDTO);
        Task<bool> DeleteUserByUsername(string username);
    }
}
