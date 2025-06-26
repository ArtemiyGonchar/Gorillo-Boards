using AutoMapper;
using BusinessLogicLayer.ApiClients.Clients;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Entites;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Classes
{
    public class StateManagementService : IStateManagementService
    {
        private readonly IBoardsServiceClient _boardsServiceClient;
        private readonly IStateRepository _stateRepository;
        private readonly IMapper _mapper;

        public StateManagementService(IStateRepository stateRepository, IMapper mapper, IBoardsServiceClient boardsServiceClient)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
            _boardsServiceClient = boardsServiceClient;
        }

        public async Task<Guid> CreateState(StateCreateDTO stateCreateDTO)
        {
            // todo add validation

            var hasAccess = await _boardsServiceClient.HasAccess(stateCreateDTO.BoardId);

            if (!hasAccess)
            {
                throw new Exception("Unuthorized");
            }
            var stateMapped = _mapper.Map<State>(stateCreateDTO);
            var stateId = await _stateRepository.CreateAsync(stateMapped);
            return stateId;
        }
    }
}
