
using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;
namespace RL.Backend.Commands.Handlers.Procedures
{

    public class AddUserToProcedureCommandHandler : IRequestHandler<AddUserToProcedureCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public AddUserToProcedureCommandHandler(RLContext context)
        {
            _context = context;
        }


        //public async Task<ApiResponse<Unit>> Handle(AddUserToProcedureCommand request, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        //Validate request
        //        if (request.ProcedureId < 1)
        //            return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));
        //        if (request.UserId < 1)
        //            return ApiResponse<Unit>.Fail(new BadRequestException("Invalid UserId"));

        //        var procedure = await _context.Procedures
        //            .Include(p => p.ProcedureUsers)
        //            .ThenInclude(pu => pu.User)
        //            .FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);
        //        var user = await _context.Users.FirstOrDefaultAsync(p => p.UserId == request.UserId);



        //        if (procedure is null)
        //            return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));
        //        if (user is null)
        //            return ApiResponse<Unit>.Fail(new NotFoundException($"UserId: {request.UserId} not found"));

        //        //Already has the procedure, so just succeed
        //        if (procedure.ProcedureUsers.Any(p => p.ProcedureId == procedure.ProcedureId))
        //            return ApiResponse<Unit>.Succeed(new Unit());

        //        procedure.ProcedureUsers.Add(new ProcedureUser
        //        {
        //            UserId = user.UserId,
        //            User = user,
        //        });

        //        await _context.SaveChangesAsync();

        //        return ApiResponse<Unit>.Succeed(new Unit());
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponse<Unit>.Fail(e);
        //    }
        //}

        public async Task<ApiResponse<Unit>> Handle(AddUserToProcedureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate request
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));
                if (request.UserIds == null || !request.UserIds.Any())
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid UserIds"));

                var procedure = await _context.Procedures
                    .Include(p => p.ProcedureUsers)
                    .FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);

                if (procedure is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));

                var users = await _context.Users
                    .Where(u => request.UserIds.Contains(u.UserId))
                    .ToListAsync();

                if (!users.Any())
                    return ApiResponse<Unit>.Fail(new NotFoundException($"No valid users found for given UserIds"));

                // Add users to the procedure
                foreach (var user in users)
                {
                    if (!procedure.ProcedureUsers.Any(pu => pu.UserId == user.UserId))  // Avoid duplicates
                    {
                        procedure.ProcedureUsers.Add(new ProcedureUser
                        {
                            UserId = user.UserId,
                            ProcedureId = request.ProcedureId
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception e)
            {
                return ApiResponse<Unit>.Fail(e);
            }
        }

    }
}
