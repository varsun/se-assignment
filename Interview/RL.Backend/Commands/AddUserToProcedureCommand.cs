﻿using MediatR;
using RL.Backend.Models;

namespace RL.Backend.Commands
{
    public class AddUserToProcedureCommand : IRequest<ApiResponse<Unit>>
    {
        public int ProcedureId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }
}
