using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Controllers
{
    public class TypeController : Controller
    {
        private readonly IRepository<FurnitureType> _allType;

        public TypeController(IRepository<FurnitureType> allType)
        {
            _allType = allType;
        }
        public async Task<IActionResult> TypeList()
        {
            return View(await _allType.ListAsync());
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public IActionResult CreateList()
        {
            return View();
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Create(string type)
        {
            await _allType.CreateAsync(new FurnitureType { Type = type });
            await _allType.SaveAsync();
            return RedirectToAction("TypeList");
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(int typeID)
        {
            await _allType.DeleteAsync(typeID);
            await _allType.SaveAsync();
            return RedirectToAction("TypeList");
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Edit(int typeID)
        {
            FurnitureType FurnitureType = await _allType.GetAsync(typeID);

            return View(FurnitureType);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Update(int typeID, string type)
        {
            try
            {
                await _allType.Update(new FurnitureType { TypeID = typeID, Type = type });
                await _allType.SaveAsync();
                return RedirectToAction("TypeList");
            }
            catch
            {
                return RedirectToAction("TypeList");
            }
        }
    }
}
