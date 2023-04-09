namespace HRTestSystem.APIResponse
{
    public class authenticatedUser
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool UserBelongsToApplication { get; set; }
    }
}
