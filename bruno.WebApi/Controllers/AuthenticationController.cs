using bruno.Application.Authentication;
using bruno.Application.Authentication.Commands.Register;
using bruno.Application.Authentication.Queries.Login;
using bruno.Application.Common.Errors;
using bruno.Contracts.Authentication;
using bruno.WebApi.Filter;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bruno.WebApi.Controllers
{
    [Route("auth")]
    [ApiController]
    [ErrorHandlingFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public AuthenticationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);

            Result<AuthenticationResult> authResult = await _mediator.Send(command);

            if (authResult.IsSuccess)
            {
                return Ok(_mapper.Map<AuthenticationResponse>(authResult.Value));
            }

            var firstError = authResult.Errors[0];
            if (firstError is DuplicateEmailError) 
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);
            }

            return Problem($"Unexpected error: {firstError.Message}");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);

            var authResult = await _mediator.Send(query);

            if (authResult.IsSuccess)
            {
                return Ok(_mapper.Map<AuthenticationResponse>(authResult.Value));
            }

            var firstError = authResult.Errors[0];
            if (firstError is DuplicateEmailError)
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);
            }

            return Problem($"Unexpected error: {firstError.Message}");
        }
    }
}
