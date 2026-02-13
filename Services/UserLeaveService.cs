using AutoMapper;
using ELTBackend.Constants;
using ELTBackend.DTOs;
using ELTBackend.Exceptions;
using ELTBackend.Models;
using ELTBackend.Repositories;

namespace ELTBackend.Services
{
    public class UserLeaveService : IUserLeaveService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILeaveRepository _leaveRepository;

        public UserLeaveService(
            IMapper mapper,
            IUserRepository userRepository,
            ILeaveRepository leaveRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _leaveRepository = leaveRepository;
        }

        public async Task<LeaveReadDto> CreateLeaveAsync(LeaveCreateDto leaveCreateDto)
        {
            await EnsureUserExistsByIdOrThrowAsync(leaveCreateDto.UserId);
            var leaveEntity = _mapper.Map<Leave>(leaveCreateDto);
            await _leaveRepository.CreateAsync(leaveEntity);
            await _leaveRepository.SaveChangesAsync();
            return _mapper.Map<LeaveReadDto>(leaveEntity);
        }

        public async Task DeletePendingLeaveAsync(Guid leaveId, Guid userId)
        {
            var leave = await GetPendingLeaveByIdForAnEmployeeOrThrowAsync(leaveId, userId);
            leave.IsDeleted = true;
            await _leaveRepository.SaveChangesAsync();
        }

        public async Task<LeaveReadDto> ApprovePendingLeaveAsync(Guid leaveId, Guid userId)
        {
            var leave = await GetPendingLeaveByIdForAnEmployeeOrThrowAsync(leaveId, userId);
            leave.Status = Status.Approved;
            await _leaveRepository.SaveChangesAsync();
            return _mapper.Map<LeaveReadDto>(leave);
        }

        public async Task<LeaveReadDto> RejectPendingLeaveAsync(Guid leaveId, Guid userId)
        {
            var leave = await GetPendingLeaveByIdForAnEmployeeOrThrowAsync(leaveId, userId);
            leave.Status = Status.Rejected;
            await _leaveRepository.SaveChangesAsync();
            return _mapper.Map<LeaveReadDto>(leave);
        }

        public async Task<IEnumerable<LeaveReadDetailsDto>> GetAllLeavesWithEmployeeAsync()
        {
            return _mapper.Map<IEnumerable<LeaveReadDetailsDto>>(
                await _leaveRepository.GetAllLeavesWithEmployeeAsync()
            );
        }

        #region Private Methods
        private async Task EnsureUserExistsByIdOrThrowAsync(Guid userId)
        {
            var exists = await _userRepository.ExistsByIdAsync(userId);

            if (!exists)
            {
                throw new NotFoundException(BusinessErrorMessages.UserNotFound);
            }
        }

        private async Task<Leave> GetPendingLeaveByIdForAnEmployeeOrThrowAsync(
            Guid leaveId,
            Guid userId
        )
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);

            if (leave == null || leave.UserId != userId || leave.Status != Status.Pending)
            {
                throw new NotFoundException(BusinessErrorMessages.PendingLeaveNotFound);
            }

            return leave;
        }
        #endregion
    }
}
