using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UserLoginASPCoreApp.Models;

namespace UserLoginASPCoreApp.Controllers
{
    public class UserLoginController : Controller
    {

        private string _url = "https://localhost:44305/api/LoginAPI/";
        private HttpClient _client = new HttpClient();
        [HttpGet]
        public IActionResult Index()
        {
            List<User> user = new List<User>();
            HttpResponseMessage response = _client.GetAsync(_url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<User>>(result);
                if (data != null)
                {
                    user = data;
                }
            }
            return View(user);
        }

        public List<User> getuser()
        {
            List<User> user = new List<User>();
            HttpResponseMessage response = _client.GetAsync(_url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<User>>(result);
                if (data != null)
                {
                    user = data;
                }
            }
            return user;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            //User _user = new User();
            var existuser = getuser();
            int cout = 0;
            int _id = 0;
            string _session = "";
            for (int i = 0; i < existuser.Count; i++)
            {
                if (existuser[i].userName == user.userName && existuser[i].password == user.password)
                {
                    cout = 1;
                    _id = existuser[i].id;
                    _session = existuser[i].session;
                }
            }
            
            if (cout == 1)
            {
                if (_session == "inactive") {
                    user.id = _id;
                    int success = EditUserById(user);
                    if (success == 1)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("UserAlredyLogin");
                }
                
                
            }
            else {
                user.session = "active";
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/Json");
                HttpResponseMessage responce = _client.PostAsync(_url, content).Result;
                if (responce.IsSuccessStatusCode)
                {
                    TempData["insert_message"] = "User added....";
                    return RedirectToAction("Index");
                }
            }
            
            return View(user);
        }

        public IActionResult UserAlredyLogin()
        {
            
            return View();
        }

        public int EditUserById(User user) {
            user.session = "active";
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/Json");
            HttpResponseMessage responce = _client.PutAsync(_url + user.id, content).Result;
            if (responce.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
           User user = new User();
            HttpResponseMessage response = _client.GetAsync(_url+id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result.Trim('[', ']'); 
                var data = JsonConvert.DeserializeObject<User>(result); ;
                if (data != null)
                {
                    user = data;
                }
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            user.session = "inactive";
            string data = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/Json");
            HttpResponseMessage responce = _client.PutAsync(_url+ user.id, content).Result;
            if (responce.IsSuccessStatusCode)
            {
                TempData["upadte_message"] = "User Logout....";
                return RedirectToAction("Create");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            User user = new User();
            HttpResponseMessage response = _client.GetAsync(_url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result.Trim('[', ']');
                var data = JsonConvert.DeserializeObject<User>(result); ;
                if (data != null)
                {
                    user = data;
                }
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            User user = new User();
            HttpResponseMessage response = _client.GetAsync(_url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result.Trim('[', ']');
                var data = JsonConvert.DeserializeObject<User>(result); ;
                if (data != null)
                {
                    user = data;
                }
            }
            return View(user);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            
            HttpResponseMessage response = _client.DeleteAsync(_url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Deleted_message"] = "User deleted....";
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
