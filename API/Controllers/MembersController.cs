using API.Contracts.Members;
using Application.Members;
using Application.Members.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }


        [HttpGet("{id:guid}")]
        public async Task<MemberDetailsDto> GetById(Guid id, CancellationToken ct)
        {
            return await _memberService.GetMemberDetailsByIdAsync(id, ct);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateMemberRequest request, CancellationToken ct)
        {
            var memberId = await _memberService.CreateMemberAsync(new CreateMemberDto(request.Email), ct);

            return CreatedAtAction(
                nameof(GetById),
                new {id = memberId},
                new {id = memberId});
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateMemberRequest request, CancellationToken ct)
        {
            await _memberService.UpdateMemberAsync(id, new UpdateMemberDto(request.Email), ct);
            return NoContent();
        }
    }
}
