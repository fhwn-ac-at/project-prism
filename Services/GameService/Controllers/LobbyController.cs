namespace GameService.Controllers
{
    using MessageLib.SharedObjects;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        [HttpPost(Name = "ConnectUserToLobby")]
        public void ConnectUserToLobby(User user, string lobbyId)
        {

        }
    }
}
