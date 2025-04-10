using Newtonsoft.Json.Linq;

namespace Libs.Common
{
    public class HasCredentialAttribute : Attribute, IAuthorizationFilter
    {
        public string RoleID { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session.GetString(CommonConstants.USER_SESSION);
            Console.WriteLine("Session: " + session);

            if (string.IsNullOrEmpty(session))
            {
                // Nếu người dùng chưa đăng nhập
                context.Result = new RedirectToActionResult("Login_New", "Account_App_New", null);
                return;
            }

            try
            {
                var userSession = new UserLoginSec { UserName = session }; // Tạo đối tượng UserLoginSec từ session

                // Lấy quyền từ session và phân tích JSON thành danh sách quyền
                var credentialsJson = context.HttpContext.Session.GetString(CommonConstants.SESSION_CREDENTIALS);
                if (string.IsNullOrEmpty(credentialsJson))
                {
                    Console.WriteLine("No credentials found in session.");
                    context.Result = new RedirectToActionResult("Login_New", "Account_App_New", null);
                    return;
                }

                // Phân tích chuỗi JSON thành danh sách quyền
                var userCredentials = JsonConvert.DeserializeObject<List<string>>(credentialsJson);
                Console.WriteLine("User Credentials: " + string.Join(",", userCredentials));  // Debug quyền

                // Kiểm tra quyền truy cập
                if (!userCredentials.Contains(RoleID) && userSession.GroupID != CommonConstants.ADMIN_GROUP)
                {
                    context.Result = new RedirectToActionResult("Error401", "Home", null);  // Quyền không hợp lệ
                }
            }
            catch (JsonReaderException ex)
            {
                // Handle the exception and provide more information about the invalid JSON
                Console.WriteLine("Error parsing JSON: " + ex.Message);
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
            }
        }

        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            // Lấy danh sách quyền từ session
            var credentialsJson = HttpContextHelper.GetSession(CommonConstants.SESSION_CREDENTIALS);
            if (string.IsNullOrEmpty(credentialsJson))
            {
                Console.WriteLine("No credentials found in session.");  // Debug
                return new List<string>();
            }

            var credentials = JsonConvert.DeserializeObject<List<string>>(credentialsJson);
            Console.WriteLine("Credentials from session: " + string.Join(",", credentials));  // Debug quyền
            return credentials ?? new List<string>();
        }
    }


    public static class HttpContextHelper
    {
        public static string GetSession(string key)
        {
            return new HttpContextAccessor().HttpContext?.Session.GetString(key);
        }

        public static void SetSession(string key, string value)
        {
            new HttpContextAccessor().HttpContext?.Session.SetString(key, value);
        }
    }
}
