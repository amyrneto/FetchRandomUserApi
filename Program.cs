using System.Text.Json;

namespace FetchRandomUserApi
{
    public class Program
    {
        static string url = "https://randomuser.me/api/";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching from Random User API...");

            try
            {
                var userData = await FetchRandomUserAsync();

                Console.WriteLine("Random user data fetched:");
                Console.WriteLine($"Name: {userData?.name?.title} {userData?.name?.first} {userData?.name?.last}.");
                Console.WriteLine($"E-mail: {userData?.email}.");
                Console.WriteLine($"Country: {userData?.location?.country}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception has occurred: {ex.Message}");
            }
		}

		static async Task<RandomUser?> FetchRandomUserAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch data from url: {url}");

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var randomUserResponse = JsonSerializer.Deserialize<RandomUserResponse>(jsonResponse);

            return randomUserResponse?.results?[0];
        }
    }

    // Data Classes for JSON Deserialization
    public class RandomUserResponse
    {
        public List<RandomUser> results { get; set; }
    }

    public class RandomUser
    {
		public string gender { get; set; }
		public Name name { get; set; }
        public Location location { get; set; }
        public string email { get; set; }
    }

    public class Name
    {
		public string title { get; set; }
		public string first { get; set; }
		public string last { get; set; }
	}

    public class Location
    {
		public string country { get; set; }
	}

}