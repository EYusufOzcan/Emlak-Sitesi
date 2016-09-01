using StajProje3.Helper;
using StajProje3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Security;

namespace StajProje3.Controllers
{
    public class CreateController : Controller
    {
        //Sadece admine ait actionlar

        private EmlakEntities17 db = new EmlakEntities17();

        public ActionResult Index()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.AdminEkranı.Where(a=>a.Kullanici == "admin").ToList();
            return View(kayit);  
        }

        public ActionResult Create()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(AdminEkranı adm, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".JPG", ".PNG", ".gif", ".GIF" };
                    var extension = System.IO.Path.GetExtension(file.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(string.Empty, ConstantVariables.ResimBasarisiz);
                    }
                    else
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(Server.MapPath("~/Dosyalar"), pic);
                        file.SaveAs(path);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        adm.Resim = pic;
                        adm.Kullanici = Session["ad"].ToString();
                        db.AdminEkranı.Add(adm);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Onayla(int id)
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.OnayBekleyen.Find(id);
            return View(kayit);
        }

        [HttpPost]
        public ActionResult Onayla(AdminEkranı onlyn, int id)
        {
            if (ModelState.IsValid)
            {
                var kayit = db.OnayBekleyen.Find(id);
                db.Entry(onlyn).State = EntityState.Added;
                db.OnayBekleyen.Remove(kayit);
                db.SaveChanges();
                return RedirectToAction("Show", new { id = onlyn.ID });
            }
            return View();
        }

        public ActionResult Reddet(int id)
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.OnayBekleyen.Find(id);
            return View(kayit);
        }

        [HttpPost]
        public ActionResult Reddet(Reddedilen rddln, int id, string rednedeni)
        {
            if (ModelState.IsValid)
            {
                var kayit = db.OnayBekleyen.Find(id);
                rddln.Rednedeni = rednedeni;
                db.Entry(rddln).State = EntityState.Added;
                db.OnayBekleyen.Remove(kayit);
                db.SaveChanges();
                return RedirectToAction("Show", new { id = rddln.ID });
            }
            return View();
        }

        public ActionResult AdmRed()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.Reddedilen.ToList();
            return View(kayit);
        }

        public ActionResult AdmOnay()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.AdminEkranı.Where(a => a.Kullanici != "admin").ToList();
            return View(kayit);
        }

        public ActionResult Edit(int id)
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.AdminEkranı.Find(id);
            return View(kayit);
        }

        [HttpPost]
        public ActionResult Edit(AdminEkranı adm, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".JPG", ".PNG", ".gif", ".GIF" };
                    var extension = System.IO.Path.GetExtension(file.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(string.Empty, ConstantVariables.ResimBasarisiz);
                    }
                    else
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(Server.MapPath("~/Dosyalar"), pic);
                        file.SaveAs(path);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        adm.Resim = pic;
                        adm.Kullanici = Session["ad"].ToString();
                        db.Entry(adm).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", new { id = adm.ID });
                    }
                }
                else
                {
                    adm.Kullanici = Session["ad"].ToString();
                    db.Entry(adm).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = adm.ID });
                }
            }
            return View();
        }

        public ActionResult Delete()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                AdminEkranı data = (from item in db.AdminEkranı
                                    where item.ID == id
                                    select item).FirstOrDefault();

                db.AdminEkranı.Remove(data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete1()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Delete1(int id)
        {
            if (Session["ad"] == null)
            {
                return RedirectToAction("Login");
            }
            if (ModelState.IsValid)
            {
                Mesaj msj = (from item in db.Mesaj
                             where item.ID == id
                             select item).FirstOrDefault();

                db.Mesaj.Remove(msj);
                db.SaveChanges();
                return RedirectToAction("ContactShow");
            }
            return View();
        }

        public ActionResult ContactShow()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.Mesaj.ToList();
            return View(kayit);
        }

        public ActionResult Show()
        {
            string key = Session["ad"].ToString();
            if (key != "admin")
            {
                return RedirectToAction("Login");
            }
            var kayit = db.OnayBekleyen.ToList();
            return View(kayit);
        }

        //Herkese açık actionlar

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //HttpPost veri göndermemi sağlıyor
        [ValidateAntiForgeryToken]
        public ActionResult Login(Kullanıcılar u)
        {
             if (ModelState.IsValid) //Model uyarılarını uyup uymadığını kontrol ediyor.
                {
                    using (EmlakEntities17 db = new EmlakEntities17())
                    {
                        var v = db.Kullanıcılar.Where(a => a.KullanıcıAd.Equals(u.KullanıcıAd) && a.Sifre.Equals(u.Sifre) && a.EmailOnay == true).FirstOrDefault();

                        if (v != null)
                        {
                            Session["ad"] = u.KullanıcıAd;

                            if (u.KullanıcıAd == "admin")
                            {
                                return RedirectToAction("Create");
                            }

                            else
                            {
                                return RedirectToAction("OnayBek");
                            }
                        }

                        else
                        {
                            ModelState.AddModelError(string.Empty, ConstantVariables.GirisBasarisiz);
                        }
                    }
                }
            return View(u);
        }

        public ActionResult List()
        {
            Session["ad"] = null;
            var kayit = db.AdminEkranı.ToList();
            return View(kayit);
        }

        [HttpPost]
        public ActionResult List(string srch = null, int number1 = 0, int number2 = 0)
        {

            if(number1 == 0 && number2 == 0)
            {
                string ksrch = srch==null?string.Empty:srch.ToLower();

                var kayit = db.AdminEkranı.Where(r => ksrch == null ||
                    r.IlanBasligi.ToLower().Contains(ksrch) ||
                    r.KonutSekli.ToLower().Contains(ksrch) ||
                    r.KonutTipi.ToLower().Contains(ksrch) ||
                    r.Il.ToLower().Contains(ksrch) ||
                    r.Ilce.ToLower().Contains(ksrch));

                return View(kayit);
            }

            else if (number1 > number2)
            {
                TempData["alertMessage"] = "Ust sınır, alt sınırdan kucuk olamaz.";
                return RedirectToAction("List");
            }

            else
            {
                var result = (from a in db.AdminEkranı where a.Fiyat < number2 && a.Fiyat > number1 select a);
                return View(result);
            }     
        }

        public ActionResult Detay(int id)
        {
            var model = db.AdminEkranı.Find(id);
            return View(model);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Mesaj msj)
        {
            if (ModelState.IsValid)
            {
                db.Mesaj.Add(msj);
                db.SaveChanges();
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("stok.satis.otomasyan@gmail.com", "deli90*-");

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("stok.satis.otomasyan@gmail.com");
                Msg.To.Add(msj.Email);
                Msg.Subject = "İletişim Maili";
                Msg.Body = string.Format("Ad: {0}<BR/>Soyad: {1}<BR/>Telefon: {2}<BR/>Mesaj: {3}<BR/>", msj.Ad, msj.Soyad, msj.Telefon, msj.Mesaj1); ;
                Msg.IsBodyHtml = true;
                sc.Send(Msg);
                return RedirectToAction("List");
            }
            return View();
        }

        public ActionResult User()
        {
            return View();
        }

        [HttpPost]
        public ActionResult User(Kullanıcılar kln)
        {
            var kayit = db.Kullanıcılar.Where(a=>a.KullanıcıAd == kln.KullanıcıAd || a.Email == kln.Email).FirstOrDefault();
            if (kayit != null)
            {
                ModelState.AddModelError(string.Empty, ConstantVariables.KayitBasarisiz);
            }

            if (kln.Ad == null || kln.Soyad == null || kln.Email == null)
            {
                ModelState.AddModelError(string.Empty, ConstantVariables.VeriHatasi);
            }

            if (ModelState.IsValid)
            {
                kln.KatılımTarihi = DateTime.Now;
                kln.EmailOnay = false;
                db.Kullanıcılar.Add(kln);
                db.SaveChanges();

                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("stok.satis.otomasyan@gmail.com", "deli90*-");

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("stok.satis.otomasyan@gmail.com");
                Msg.To.Add(kln.Email);
                Msg.Subject = "Kullanıcı Onay";
                Msg.Body = string.Format("Sevgili {0} {1},<BR/> Kaydınız için teşekkürler. Üyeliğinizi tamamlamak için lütfen aşağıdaki linke tıklayın: <BR/> <a href=\"{2}\" title=\"User Email Confirm\">{2}</a>", kln.Ad, kln.Soyad, Url.Action("ConfirmEmail", "Create", new { Token = kln.ID, Email = kln.Email }, Request.Url.Scheme)); ;
                Msg.IsBodyHtml = true;
                sc.Send(Msg);
                return RedirectToAction("List");
            }
            
            return View();
        }

        public ActionResult ConfirmEmail(Kullanıcılar kln)
        {
                var item = (from data in db.Kullanıcılar where data.Email == kln.Email select data).FirstOrDefault();
                Session["ad"] = item.KullanıcıAd;
                item.EmailOnay = true;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OnayBek");
        }

        //Sadece kullanıcılar için olan actionlar

        public ActionResult Show1()
        {
            string key = Session["ad"].ToString();
            if (key == "admin" || key == null)
            {
                return RedirectToAction("Login");
            }
            var kayit = db.OnayBekleyen.Where(a => a.Kullanici == key).ToList();
            return View(kayit);
        }

        public ActionResult KulRed()
        {
            string key = Session["ad"].ToString();
            if (key == "admin" || key == null)
            {
                return RedirectToAction("Login");
            }
            var kayit = db.Reddedilen.Where(a => a.Kullanici == key).ToList();
            return View(kayit);
        }

        public ActionResult KulOnay()
        {
            string key = Session["ad"].ToString();
            if (key == "admin" || key == null)
            {
                return RedirectToAction("Login");
            }
            var kayit = db.AdminEkranı.Where(a => a.Kullanici == key).ToList();
            return View(kayit);
        }

        public ActionResult OnayBek()
        {
            string key = Session["ad"].ToString();
            if (key == "admin" || key == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult OnayBek(OnayBekleyen ony, HttpPostedFileBase file)
        {
           
                var kayit = db.OnayBekleyen.Where(a => a.IlanBasligi == ony.IlanBasligi).FirstOrDefault();
                if (kayit != null)
                {
                    ModelState.AddModelError(string.Empty, ConstantVariables.VeriGirisiBasarisiz);
                }

                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        var allowedExtensions = new[] { ".png", ".jpg", ".JPG", ".PNG", ".gif", ".GIF" };
                        var extension = System.IO.Path.GetExtension(file.FileName);
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError(string.Empty, ConstantVariables.ResimBasarisiz);
                        }
                        else
                        {
                            string pic = System.IO.Path.GetFileName(file.FileName);
                            string path = System.IO.Path.Combine(Server.MapPath("~/Dosyalar"), pic);
                            file.SaveAs(path);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file.InputStream.CopyTo(ms);
                                byte[] array = ms.GetBuffer();
                            }
                            ony.Resim = pic;
                            ony.Kullanici = Session["ad"].ToString();
                            db.OnayBekleyen.Add(ony);
                            db.SaveChanges();
                            return RedirectToAction("Show1");
                        }
                    }
            }
            return View();
        }

        public ActionResult Edit1(int id)
        {
            string key = Session["ad"].ToString();
            if (key == "admin" || key == null)
            {
                return RedirectToAction("Login");
            }
            var kayit = db.Reddedilen.Find(id);
            return View(kayit);
        }

        [HttpPost]
        public ActionResult Edit1(OnayBekleyen ony, int id, HttpPostedFileBase file)
        {
            var kayit1 = db.OnayBekleyen.Where(a => a.IlanBasligi == ony.IlanBasligi).FirstOrDefault();
            if (kayit1 != null)
            {
                ModelState.AddModelError(string.Empty, ConstantVariables.VeriGirisiBasarisiz);
            }

            if (ModelState.IsValid)
            {
               if (file != null)
                {
                    var allowedExtensions = new[] { ".png", ".jpg", ".JPG", ".PNG", ".gif", ".GIF" };
                    var extension = System.IO.Path.GetExtension(file.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(string.Empty, ConstantVariables.ResimBasarisiz);
                    }
                    else
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(Server.MapPath("~/Dosyalar"), pic);
                        file.SaveAs(path);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        ony.Resim = pic;
                        db.OnayBekleyen.Add(ony);
                        Reddedilen kayit = (from data in db.Reddedilen where data.ID == id select data).FirstOrDefault();
                        db.Reddedilen.Remove(kayit);
                        db.SaveChanges();
                        return RedirectToAction("KulOnay", new { id = ony.ID });
                    }
                }
                else
                {
                    db.OnayBekleyen.Add(ony);
                    Reddedilen kayit = (from data in db.Reddedilen where data.ID == id select data).FirstOrDefault();
                    db.Reddedilen.Remove(kayit);
                    db.SaveChanges();
                    return RedirectToAction("KulOnay", new { id = ony.ID });
                }
            }
            return View();
        }
    }
}
