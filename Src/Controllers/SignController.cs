using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dtos;
using Utils;

namespace loginExercise.Controllers;

[ApiController]
[Route("api")]
public class SignController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICrypt _crypt;
    
    public SignController(IUserRepository userRepository, ICrypt crypt) {
        _userRepository = userRepository ?? throw new ArgumentNullException();
        _crypt = crypt ?? throw new ArgumentNullException();
    }

    [HttpPost]
    [Route("signUp")]
    public ActionResult<User> Post(UserSignUpDTO newUser)
    {   
        User user = new(newUser.name, newUser.email, newUser.password);

        User emailAlreadyInUse = _userRepository.GetByEmail(user.email);
        
        if (emailAlreadyInUse != null)
            return BadRequest("Email already in use");

        user.password = _crypt.encrypt(user.password);
        
        _userRepository.Add(user);
        user = _userRepository.GetByEmail(user.email);
        
        return Ok(user);
    }

    [HttpPost]
    [Route("signIn")]
    public ActionResult<string> Get(UserSignInDTO signData)
    {
        User user = _userRepository.GetByEmail(signData.email);;

        if (user == null) 
            return BadRequest("Wrong password or email");

        if (!_crypt.compare(signData.password, user.password)) {
            return BadRequest("Wrong password or email");
        }

        return Ok(Token.Generate(user));
    }
}
