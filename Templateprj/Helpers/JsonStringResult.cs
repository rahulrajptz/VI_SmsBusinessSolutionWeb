using System.Web.Mvc;
namespace Templateprj.Helpers
{
    public class JsonStringResult : ContentResult
    {
        public JsonResult JsonStringResult1(string json)
        {
            Content = json;
            ContentType = "application/json";
            return new JsonResult();
        }
    }
}
