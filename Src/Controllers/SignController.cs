using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using DTOs;
using Utils;
using System.Text.Json;

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
    public ActionResult<User> SignUp(UserSignUpDTO newUser)
    {   
        User user = new(newUser.name, newUser.email, newUser.password, null);

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
    public ActionResult<string> SignIn(UserSignInDTO signData)
    {
        User user = _userRepository.GetByEmail(signData.email);;

        if (user == null) 
            return BadRequest("Wrong password or email");

        if (!_crypt.compare(signData.password, user.password)) {
            return BadRequest("Wrong password or email");
        }

        return Ok(Token.Generate(user));
    }

    [HttpPost]
    [Route("facebookAuth/{accessToken}")]
    public ActionResult FacebookAuth(string accessToken) {

        FbUserData fbUserData;

        using var client = new HttpClient();
        fbUserData = JsonSerializer.Deserialize<FbUserData>(client.GetAsync(
            $"https://graph.facebook.com/me?access_token={accessToken}&fields=id,name,email,picture.width(100).height(100)"
        ).Result.Content.ReadAsStringAsync().Result);

        User user;

        user = _userRepository.GetByEmail(fbUserData.email);

        if (user == null) {
            user = new (fbUserData.name, fbUserData.email, null, fbUserData.picture.data.url);
            _userRepository.Add(user);
            user = _userRepository.GetByEmail(user.email);
        }

        return Ok(Token.Generate(user));
    }

    [HttpPost]
    [Route("googleAuth/{accessToken}")]
    public ActionResult GoogleAuth(string idToken) {

        GoogleResponse googleResponse;

        using var client = new HttpClient();
        googleResponse = JsonSerializer.Deserialize<GoogleResponse>(client.GetAsync(
            $"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}"
        ).Result.Content.ReadAsStringAsync().Result);

        User user;

        user = _userRepository.GetByEmail(googleResponse.email);

        if (user == null) {
            user = new (googleResponse.name, googleResponse.email, null, googleResponse.picture);
            _userRepository.Add(user);
            user = _userRepository.GetByEmail(user.email);
        }

        return Ok(Token.Generate(user));
    }
}


