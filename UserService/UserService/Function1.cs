using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace UserService
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public static List<User> Users = new List<User>();

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;

            Users.Add(new User { Email = "abc@uuu.com", Id = Guid.NewGuid(), Name = "Farid" });
            Users.Add(new User { Email = "abc2@uuu.com", Id = Guid.NewGuid(), Name = "Farid2" });
            Users.Add(new User { Email = "abc3@uuu.com", Id = Guid.NewGuid(), Name = "Farid3" });
            Users.Add(new User { Email = "abc4@uuu.com", Id = Guid.NewGuid(), Name = "Farid4" });
        }

        [Function("Users")]
        public IActionResult GetUsers([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHeeeeeeeeeyyyyy");


            return new OkObjectResult(Users);
        }

        [Function("User")]
        public async Task<IActionResult> SaveUser([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHeeeeeeeeeyyyyy");
            try
            {
                
                var user = await req.ReadFromJsonAsync<User>();

                Users.Add(user);

                var serviceBusPublisher = new ServiceBusPublisher();

                var userJson = JsonSerializer.Serialize(user);
                
                await serviceBusPublisher.PublishMessage(userJson);

                return new OkResult();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }


            
        }
    }

    public class User
    {
        private Guid _id;

        public Guid Id
        {
            get => _id;
            set => _id = value == Guid.Empty ? Guid.NewGuid() : value;
        }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
