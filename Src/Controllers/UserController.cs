using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace loginExercise.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase {
    private readonly IUserRepository _userRepository;
    private readonly IStorage _storage;
    public UserController (IUserRepository userRepository, IStorage storage) {
        _userRepository = userRepository;
        _storage = storage;
    }

    [HttpPut]
    [Route("{id}/photo")]
    [Authorize]
    public ActionResult ChangePhoto([FromForm] IFormFile image, string id) {
        
        if (!image.ContentType.Contains("image")) {
            return BadRequest("Not a image");
        }
        
        string savedFileName = _storage.upload(image);
        
        User user = _userRepository.GetById(int.Parse(id));
        
        user.photo = savedFileName;
        bool isUpdate = _userRepository.Edit(user);

        return isUpdate ? Ok(user) : BadRequest("Failed to save");
    }
}