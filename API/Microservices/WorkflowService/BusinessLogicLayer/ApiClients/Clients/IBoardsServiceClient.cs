using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ApiClients.Clients
{
    [Headers("Authorization: Bearer")]
    public interface IBoardsServiceClient
    {
        [Get("/api/boards/{boardId}/has-access")]
        Task<bool> HasAccess(Guid boardId);
    }
}
