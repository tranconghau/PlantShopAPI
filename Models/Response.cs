namespace PlantShopAPI.Models
{
    public class Response
    {
        public bool status {  get; set; }
        public string? message { get; set; }

        public object? data { get; set; }
    }

    public class UserResponse
    {
        public string? username { get; set; }

        public string? role { get; set; }
    }
}
