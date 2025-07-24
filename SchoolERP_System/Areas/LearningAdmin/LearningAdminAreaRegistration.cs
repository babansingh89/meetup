using System.Web.Mvc;

namespace SchoolERP_System.Areas.LearningAdmin
{
    public class LearningAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LearningAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LearningAdmin_default",
                "LearningAdmin/{controller}/{action}/{id}",
                new { controller = "Login", action = "LoginAdmin", id = UrlParameter.Optional }
            );
        }
    }
}