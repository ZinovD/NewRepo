using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FurnitureStore.Data.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace FurnitureStore.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(IWebHostEnvironment IWebHostEnvironment, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _environment = IWebHostEnvironment;
            _roleManager = roleManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, SurName = model.SurName, Name = model.Name, UserName = model.Email, MiddleName = model.MiddleName, Address = model.Address };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id, Email = user.Email, SurName = user.SurName, Name = user.Name,
                MiddleName = user.MiddleName, Address = user.Address,
                AllRoles = _roleManager.Roles.ToList(),
                UserRoles = await _userManager.GetRolesAsync(user)
        };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model, List<string> roles)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.SurName = model.SurName;
                    user.Name = model.Name;
                    user.MiddleName = model.MiddleName;
                    user.Address = model.Address;
                    var userRoles = await _userManager.GetRolesAsync(user);
                    // получаем все роли
                    var allRoles = _roleManager.Roles.ToList();
                    // получаем список ролей, которые были добавлены
                    var addedRoles = roles.Except(userRoles);
                    // получаем роли, которые были удалены
                    var removedRoles = userRoles.Except(roles);

                    await _userManager.AddToRolesAsync(user, addedRoles);

                    await _userManager.RemoveFromRolesAsync(user, removedRoles);

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {

            User user = await _userManager.FindByIdAsync(id);
            if (user != null && _userManager.FindByIdAsync(id).Result.Email != User.Identity.Name)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ChangeUserPassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile", new { UserName = user.Email });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Profile(string UserName)
        {
            User user = await _userManager.FindByEmailAsync(UserName);
            EditUserViewModel model = new EditUserViewModel
            { 
                Id = user.Id, Email = user.Email, SurName = user.SurName, Name = user.Name,
                MiddleName = user.MiddleName, Address = user.Address,Avatar= user.Avatar
            };
            System.Console.WriteLine(model.Avatar);
            if (user == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            { 
                Id = user.Id, Email = user.Email, SurName = user.SurName, Name = user.Name,
                MiddleName = user.MiddleName, Address = user.Address,Avatar=user.Avatar 
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(EditUserViewModel model,IFormFile pic)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            string path = user.Avatar;
            if (pic != null)
            {
                await using (var memoryStream = new MemoryStream())
                {
                    await pic.CopyToAsync(memoryStream);
                    var img = Image.FromStream(memoryStream);
                    using (Bitmap newPic = new Bitmap(350, 300))
                    {
                        using (Graphics gr = Graphics.FromImage(newPic))
                        {
                            gr.DrawImage(img, 0, 0, 350, 300);
                            newPic.Save(_environment.WebRootPath + "/users/" + model.Email+ ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }
                path= "/users/" + model.Email + ".jpg";
            }
            if (ModelState.IsValid)
            {
                user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.SurName = model.SurName;
                    user.Name = model.Name;
                    user.MiddleName = model.MiddleName;
                    user.Address = model.Address;
                    user.Avatar = path;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile", new { UserName = user.Email });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
    }
}
