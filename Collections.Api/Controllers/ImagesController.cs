using Collections.Api.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[Authorize]
[ApiController]
[Route("CollectionsApi/[controller]")]
public class ImagesController : ControllerBase
{
}