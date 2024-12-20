namespace GameService.Controllers
{
    using MessageLib.SharedObjects;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        [HttpPost(Name = "ConnectUserToLobby")]
        public void ConnectUserToLobby(User user, string lobbyId)
        {
            // TODO check if lobby exists if not create new lobby

            // TODO create lobby object, when all user leave delete it
        }
    }
}
