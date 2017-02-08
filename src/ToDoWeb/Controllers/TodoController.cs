using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Net;
using ToDoWeb.Models;
using ToDoWeb.Configuration;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoWeb.Controllers
{
    public class TodoController : Controller
    {
        public UrlConfig UrlConfig { get; }
        public TodoController(Microsoft.Extensions.Options.IOptions<UrlConfig> urlConfig)
        {
            UrlConfig = urlConfig.Value;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ToDoList()
        {
            //Need to get list of items to display
            string ApiGetUrl = String.Empty;
            string result = String.Empty;
            TodoItem items = new TodoItem();
            List<TodoItem> todoitems = new List<TodoItem>();
            //items = null;

            ApiGetUrl = UrlConfig.PostUrl;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiGetUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //var output = JsonConvert.SerializeObject(items);
                HttpResponseMessage response = await client.GetAsync(ApiGetUrl);
                if (response.IsSuccessStatusCode)
                {

                    todoitems = JsonConvert.DeserializeObject<List<TodoItem>>(await response.Content.ReadAsStringAsync());
                    //var test2 = await response.Content.ReadAsStringAsync();

                    return View(todoitems);
                   
                }
                else
                {
                    //Need some error handling here
                    return View("PostError");
                }
            }

        }

        [HttpPost]
        public ActionResult ToDoPost(TodoItem items)
        {
            if (items == null)
            {
                return RedirectToAction("Index");
            }

            if (String.IsNullOrEmpty(items.name))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("PostToApi",items);
        }

        public async Task<ActionResult> PostToApi(TodoItem model)
        {
            string ApiPostUrl = String.Empty;
            string result = String.Empty;
            
            ApiPostUrl = UrlConfig.PostUrl;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiPostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var output = JsonConvert.SerializeObject(model);
                HttpResponseMessage response = await client.PostAsync(ApiPostUrl, new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json"));
                //HttpResponseMessage response = await client.PostAsync(ApiPostUrl, new StringContent(JsonConvert.SerializeObject({ "name":"Walk the dog"},{"IsComplete":"No"}), System.Text.Encoding.UTF8, "application/json"));

                var responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    //Post was a success
                    return View("PostSuccess", responseJson);
                }
                else
                {
                    //There was an error posting the new tasks
                    return View("PostError");
                }


                //Need to call a view here that handles the success or error message that is returned
               // return response;

            }

        }
    }

    
}
