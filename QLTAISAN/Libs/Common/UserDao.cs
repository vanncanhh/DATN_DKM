namespace Libs.Common
{
    public class UserDao
    {

        QuanLyTaiSanCtyDATNContext db = null;
        public UserDao()
        {
            db = new QuanLyTaiSanCtyDATNContext();
        }

        public UserLogin GetById(string userName)
        {
            return db.UserLogins.SingleOrDefault(x => x.UserName == userName);
        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.UserLogins.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupId equals b.Id
                        join c in db.Roles on a.RoleId equals c.Id
                        where b.Id == user.GroupId
                        select new
                        {
                            RoleId = a.RoleId,
                            UserGroupId = a.UserGroupId
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleId = x.RoleId,
                            UserGroupId = x.UserGroupId
                        });
            return data.Select(x => x.RoleId).ToList();

        }
        public int Login(string userName, string passWord/*, bool isLoginAdmin = false*/)
        {
            var result = db.UserLogins.SingleOrDefault(x => x.UserName.Trim() == userName && x.IsDeleted == false);
            if (result == null)
            {
                return 0;
            }
            else
            {
                string hashedPassword = Encryptor.MD5Hash(passWord.Trim());
                if (result.PassWord.Trim() == hashedPassword)
                {
                    return 1;
                }
                else
                {
                    return -2;
                }
            }
        }

        public bool AddUserGroup(string Id, string Name)
        {
            bool a = true;
            var Check = db.UserGroups.Where(x => x.Id == Id).Count();
            if (Check > 0)
            {
                a = false;
            }
            else
            {
                var entity = new UserGroup { Id = Id, Name = Name };
                db.UserGroups.Add(entity);
                db.SaveChanges();
                a = true;
            }
            return a;
        }

        public bool DeleteUserGroup(string Id)
        {
            bool a = true;
            var Check = db.Credentials.Where(x => x.UserGroupId == Id).Count();
            var Check_1 = db.UserLogins.Where(x => x.GroupId == Id).Count();
            if (Check > 0 || Check_1 > 0)
            {
                a = false;
            }
            else
            {

                var UserGroup = db.UserGroups.Find(Id);
                db.UserGroups.Remove(UserGroup);
                db.SaveChanges();
                a = true;
            }
            return a;
        }

        public bool AddRoleForGroup(string lstRoleId, string GroupId)
        {
            bool a = true;
            var lstRole = lstRoleId.Split(',');
            for (int i = 0; i < lstRole.Length; i++)
            {
                if (!lstRole[i].Equals(""))
                {
                    var entity = new Credential { RoleId = lstRole[i].Trim(), UserGroupId = GroupId };
                    db.Credentials.Add(entity);
                    db.SaveChanges();
                }
            }

            return a;
        }


        public bool UpdateRoleUser(string FullName, string UserName, string Role, string PassWord)
        {
            bool result = true;
            //Check đã tồn tại username
            var Check = db.UserLogins.Where(x => x.UserName == UserName & x.IsDeleted == false).Count();
            if (Check > 0)
            {
                result = false;
            }
            else
            {
                var entity = new UserLogin { FullName = FullName, UserName = UserName, GroupId = Role, PassWord = PassWord, Email = null, IsDeleted = false, Status = null };
                db.UserLogins.Add(entity);
                db.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UpdateRole(int Id, string FullName, string UserName, string Role, string PassWord)
        {
            bool result = true;
            var user = db.UserLogins.Find(Id);
            user.FullName = FullName;
            user.GroupId = Role;
            //user.PassWord = PassWord; 
            db.SaveChanges();
            return result;
        }

        public bool DeleteRoleUser(int Id)
        {
            bool result = true;

            var user = db.UserLogins.Find(Id);
            user.IsDeleted = true;

            db.SaveChanges();
            return result;
        }
    }
}
