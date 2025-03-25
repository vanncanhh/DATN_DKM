namespace Libs.Common
{
    public class HasCredentialAttribute : Attribute, IAuthorizationFilter
    {
        public string RoleID { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session.GetString(CommonConstants.USER_SESSION);

            if (string.IsNullOrEmpty(session))
            {
                // Người dùng chưa đăng nhập
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
                return;
            }

            var userSession = JsonConvert.DeserializeObject<UserLoginSec>(session);
            if (userSession == null)
            {
                // Nếu không có phiên làm việc
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
                return;
            }

            var privilegeLevels = GetCredentialByLoggedInUser(userSession.UserName.Trim());

            if (privilegeLevels.Contains(this.RoleID) || userSession.GroupID == CommonConstants1.ADMIN_GROUP)
            {
                return;
            }

            // Nếu không có quyền
            context.Result = new RedirectToActionResult("Error401", "Home", null);
        }

        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            // Lấy danh sách quyền từ session
            var credentialsJson = HttpContextHelper.GetSession(CommonConstants.SESSION_CREDENTIALS);
            if (string.IsNullOrEmpty(credentialsJson))
            {
                return new List<string>();
            }

            var credentials = JsonConvert.DeserializeObject<List<string>>(credentialsJson);
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
