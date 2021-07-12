using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnline.Models;

namespace WebOnline.Dao
{
    public class UserDao
    {
        DatabaseEntities db = null;
        public UserDao()
        {
            db = new DatabaseEntities();
        }

        public String Insert(TaiKhoan entity)
        {
            entity.MaNguoi = InsertPer();
            entity.Status = true;
            db.TaiKhoan.Add(entity);
            db.SaveChanges();
            return entity.TaiKhoan1;
        }

        public int InsertPer()
        {
            Nguoi person = new Nguoi();
            db.Nguoi.Add(person);
            db.SaveChanges();
            var per = (from h in db.Nguoi orderby h.MaNguoi descending select h).FirstOrDefault();
            return per.MaNguoi;
        }

        public IEnumerable<TaiKhoan> ListAllPaging(string searchString, int page, int pageSize)
        {

            IQueryable<TaiKhoan> model = db.TaiKhoan;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TaiKhoan1.Contains(searchString));
            }
            return model.OrderByDescending(x => x.MaTaiKhoan).ToPagedList(page, pageSize);
        }

        public int Login(String UserName, string PassWord)
        {
            var res = db.TaiKhoan.SingleOrDefault(x => x.TaiKhoan1 == UserName);

            if (res == null)
            {
                return 0;
            }
            else
            {
                if (res.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (res.MatKhau == PassWord)
                        return 1;
                    else
                        return -2;
                }
            }

        }

        public TaiKhoan GetByID(string userName)
        {
            return db.TaiKhoan.SingleOrDefault(x => x.TaiKhoan1 == userName);
        }

        public TaiKhoan ViewDetail(int id)
        {
            return db.TaiKhoan.Find(id);
        }


        public bool Update(TaiKhoan entity)
        {
            try
            {
                var user = db.TaiKhoan.Find(entity.MaTaiKhoan);
                user.TaiKhoan1 = entity.TaiKhoan1;
                if (!string.IsNullOrEmpty(entity.MatKhau))
                {
                    user.MatKhau = entity.MatKhau;
                }
                user.MatKhau = entity.MatKhau;
                user.RoleID = entity.RoleID;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.TaiKhoan.Find(id);
                db.TaiKhoan.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}