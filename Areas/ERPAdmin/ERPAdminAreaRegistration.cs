using System.Web.Mvc;

namespace SchoolERP_System.Areas.ERPAdmin
{
    public class ERPAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ERPAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ERPAdmin_default",
                "ERPAdmin/{controller}/{action}/{id}",
              new { controller = "LoginERP", action = "LoginERPAdmin", id = UrlParameter.Optional }
            );
        }
    }
}