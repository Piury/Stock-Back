﻿using Microsoft.AspNetCore.Mvc;
using Stock_Back.Controllers.UserApiControllers;
using Stock_Back.DAL;
using Stock_Back.DAL.Controller;
using Stock_Back.DAL.Data;
using Stock_Back.DAL.Interfaces;
using Stock_Back.DAL.Models;
using Stock_Back.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stock_Back.Controllers
{
    
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserController _userController;
        private readonly UserGetAll userGetAll;
        private readonly UserGetById userGetById;
        private readonly UserPost userPost;
        private readonly UserUpdate userUpdate;
        private readonly UserDelete userDelete;
        public UserApiController(AppDbContext dbContext)
        {
            _userController = new UserController(dbContext);
            userGetAll = new UserGetAll(_userController);
            userGetById = new UserGetById(_userController);
            userPost = new UserPost(_userController);
            userUpdate = new UserUpdate(_userController);
            userDelete = new UserDelete(_userController);
        }

        // GET all or by id
        [HttpGet]
        [Route("api/[controller]/GetUsers/{id?}")]
        public async Task<IActionResult> GetUsers(int? id)
        {
            // Si se proporciona un ID, obtener el usuario específico.
            if (id.HasValue)
            {
                return await userGetById.GetUserById(id.Value);
            }
            // Si no, obtener todos los usuarios.
            else
            {
                return await userGetAll.GetUsers();
            }
        }

        // POST api/<UserApiController>
        [HttpPost]
        [Route("api/[controller]/InsertUser")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            // Extrae la claim de SuperAdmin del token.
            var isSuperAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "SuperAdmin")?.Value;

            return await userPost.InsertUser(user, isSuperAdmin);
        }

        // PUT api/<UserApiController>/5
        [HttpPut]
        [Route("api/[controller]/UpdateUser")]
        public async Task<IActionResult> Put([FromBody] User userEdited)
        {
            return await userUpdate.UpdateUser(userEdited);
        }

        // DELETE api/<UserApiController>/5
        [HttpDelete]
        [Route("api/[controller]/DeleteUser/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await userDelete.DeleteUser(id);
        }
    }
}
