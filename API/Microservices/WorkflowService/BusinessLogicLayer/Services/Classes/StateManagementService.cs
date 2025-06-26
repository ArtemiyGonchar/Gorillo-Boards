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

        public async Task<bool> ChangeOrderState(Guid stateId, int targetOrder)
        {
            var state = await _stateRepository.GetAsync(stateId);

            if (state == null)
            {
                throw new Exception("Such state not exists");
            }

            var statesByBoard = await _stateRepository.GetStatesByBoardId(state.BoardId);

            if (state.Order == targetOrder)
            {
                return false;
            }

            if (state.Order < targetOrder)
            {
                foreach (var stateInList in statesByBoard)
                {
                    if (stateInList.Order > state.Order && stateInList.Order <= targetOrder)
                    {
                        stateInList.Order--;
                    }
                }
            }
            else
            {
                foreach (var stateInList in statesByBoard)
                {
                    if (stateInList.Order >= targetOrder && stateInList.Order < state.Order)
                    {
                        stateInList.Order++;
                    }
                }
            }

            state.Order = targetOrder;
            await _stateRepository.UpdateAsync(state);
            await _stateRepository.UpdateManyStates(statesByBoard);
            return true;
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
            var stateId = await _stateRepository.CreateStateWithOder(stateMapped);
            return stateId;
        }

        public async Task<bool> DeleteState(Guid stateId)
        {
            var inDb = await _stateRepository.GetAsync(stateId);
            if (inDb == null)
            {
                throw new Exception("Such state not exists");
            }

            var isDeleted = await _stateRepository.DeleteAsync(stateId);
            return isDeleted;
        }
    }
}
