using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext Context, ITokenService tokenService)
        {
            _context = Context;
            this.tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(DTOs.RegisterDto registerDto)
        {
            if (await UserExist(registerDto.UserName))
                return BadRequest("Username already exist!");



            using var hmac = new HMACSHA512();
            var user = new AppUser()
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
                
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return new UserDto { UserName=user.UserName, Token=tokenService.CreateToken(user) };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(DTOs.LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.UserName == loginDto.UserName);
            if (user == null)
                return Unauthorized("Invalid username!");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computehash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<computehash.Length;i++)
            {
                if (computehash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password !");
            }
            return new UserDto { UserName = user.UserName, Token = tokenService.CreateToken(user) };


        }




        private async Task<bool> UserExist(string username)
        {
            return await _context.Users.Where(m=>m.UserName==username.ToLower()).AnyAsync();

        }


    }

}

