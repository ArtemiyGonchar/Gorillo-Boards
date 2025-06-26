using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IBoardManagementService
    {
        Task<Guid> BoardCreate(BoardCreatedDTO boardCreatedDTO);
        Task<bool> BoardDelete(string title);
    }
}
