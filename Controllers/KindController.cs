using FurnitureStore.Data.Interfaces;
using FurnitureStore.Data.Models;
using FurnitureStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Controllers
{
    public class KindController : Controller
    {
        private readonly IRepository<FurnitureKind> _allKind;
        private readonly IWebHostEnvironment _environment;

        public KindController(IWebHostEnvironment environment,IRepository<FurnitureKind> allKind)
        {
            _allKind = allKind;
            _environment= environment;
        }
        public async Task<IActionResult> KindList()
        {
            return View(await _allKind.ListAsync());
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public IActionResult CreateList()
        {
            return View();
        }
        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Create(string kind,IFormFile pic)
        {
            FurnitureKind furnitureKind = new FurnitureKind();
            furnitureKind.Kind = kind;
            await _allKind.CreateAsync(furnitureKind);
            furnitureKind.Photo = await Photo(furnitureKind.KindID, pic);
            await _allKind.Update(furnitureKind);
            return RedirectToAction("KindList");
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Delete(int kindID)
        {   
            FileInfo fileInf = new FileInfo(_environment.WebRootPath + _allKind.GetAsync(kindID).Result.Photo);
            if (fileInf.Exists) fileInf.Delete();
            await _allKind.DeleteAsync(kindID);
            return RedirectToAction("KindList");
        }

        public async Task<string> Photo(int kindID, IFormFile pic)
        {
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
                            newPic.Save(_environment.WebRootPath + "/img/KindPhoto/" + kindID + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Close();
                            newPic.Dispose();
                        }
                    }
                }                                                                                                                                                                                                                                                    
                return "/img/KindPhoto/" + kindID + ".jpg";
            }
            return string.Empty;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        public async Task<IActionResult> Edit(int kindID)
        {
            FurnitureKind FurnitureKind = await _allKind.GetAsync(kindID);
            return View(FurnitureKind);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpPost]
        public async Task<IActionResult> Edit(FurnitureKind furnitureKind, IFormFile pic)
        {
            if(pic!=null)furnitureKind.Photo = await Photo(furnitureKind.KindID, pic);
            else if (System.IO.File.Exists(_environment.WebRootPath + "/img/KindPhoto/"+furnitureKind.KindID+ ".jpg"))
            {
                furnitureKind.Photo= "/img/KindPhoto/" + furnitureKind.KindID + ".jpg";
            }
            else furnitureKind.Photo = "/img/No Photo/no-photo.jpg";
            await _allKind.Update(furnitureKind);
            return RedirectToAction("KindList");
        }

    }
}
