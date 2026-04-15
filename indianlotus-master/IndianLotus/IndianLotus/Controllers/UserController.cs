using CarisBrook.Service.UserService;
using Newtonsoft.Json;
using PagedList;
using ResturantBooking.Utility.Helper;
using ResturantBooking.Web.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace ResturantBooking.Controllers
{
    [CustomAuthorize]
    [RoleAuthorize(AppConstant.RoleAdmin, AppConstant.RoleSuperAdmin)]
    public class UserController : Controller
    {
        UserService userService = new UserService();
        // GET: User
      
        public ActionResult ManageUsers(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageDataSize;
            var user = userService.GetUserList(Convert.ToInt32(UserAuthenticate.LogId), Convert.ToInt32(UserAuthenticate.UserTypeId)).ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Users", user) : View(user);

        }
        //[HttpGet]
        //public ActionResult AddGallery(int data)
        //{
        //    ViewBag.AlbumData = menuService.GetAllAlbum().Where(x => x.IsActive == true);
        //    GalleryViewModel objGallery = new GalleryViewModel();
        //    if (data != 0)
        //    {
        //        objGallery = menuService.GetGalleryById((int)data);
        //        return View(objGallery);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
        //[HttpPost]
        //public ActionResult AddGallery(GalleryViewModel ObjGallery)
        //{
        //    bool result;
        //    if (ObjGallery.id == 0)
        //        result = menuService.SaveGallery(ObjGallery);
        //    else
        //        result = menuService.UpdateGallery(ObjGallery);
        //    if (result)
        //    {
        //        return RedirectToAction("ManageGallery", new { });
        //    }
        //    return View();
        //}
    }
}