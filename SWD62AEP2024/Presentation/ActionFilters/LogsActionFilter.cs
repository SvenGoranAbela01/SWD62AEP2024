using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis;

namespace Presentation.ActionFilters
{
    public class LogsActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Log myLog = new Log();
            myLog.Message = $"Timestamp: {DateTime.Now}, Action: {context.HttpContext.Request.Path}, " + 
                $"Querystring: {context.HttpContext.Request.QueryString}";

            myLog.User = "Anonymous User";
            if (context.HttpContext.User != null)
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    myLog.User = context.HttpContext.User.Identity.Name;
                }
            }
            myLog.IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString(); //[::1] if local

            //What if i like to change the destination of these togs with the minimal effort possible?
            //also keeping the same code efficiency...

            //answer: using the interface (base type of the implementations) in the code makes your code
            //        open to any implemented solution you choose without needing to edit the code at a later stage

            ILogsRepository logsRepository = context.HttpContext.RequestServices.GetService<ILogsRepository>();
            logsRepository.AddLog(myLog);

            base.OnActionExecuting(context);//if you want to keep running the next code smoothly don't delete this line
        }
    }
}
