using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Controllers
{
    public class ColorController : Controller
    {
        private readonly IRepository<FurnitureColor> _allColor;

        public ColorController(IRepository<FurnitureColor> allColor)
        {
            _allColor = allColor;
        }
        public async Task<IActionResult> ColorList()
        {
            return View(await _allColor.ListAsync());
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public IActionResult CreateList()
        {
            return View();
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Create(string colorName, string color)
        {
            await _allColor.CreateAsync(new FurnitureColor { ColorName = colorName, Color=color});
            await _allColor.SaveAsync();
            return RedirectToAction("ColorList");
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(int colorID)
        {
            await _allColor.DeleteAsync(colorID);
            await _allColor.SaveAsync();
            return RedirectToAction("ColorList");
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Edit(int colorID)
        {
            FurnitureColor FurnitureColor = await _allColor.GetAsync(colorID);

            return View(FurnitureColor);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Update(int colorID, string colorName,string color)
        {
            try
            {
                await _allColor.Update(new FurnitureColor { ColorID = colorID, ColorName = colorName, Color = color });
                await _allColor.SaveAsync();
                return RedirectToAction("ColorList");
            }
            catch
            {
                return RedirectToAction("ColorList");
            }
        }
    }
}
