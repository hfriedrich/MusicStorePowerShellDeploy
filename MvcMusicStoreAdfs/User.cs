namespace MvcMusicStoreAdfs
{
    public class User
    {
        public string Name { get; set; }

        public User(Claim claim)
        {
            Name = claim.UserName;
        }

        public static implicit operator Claim(User user)
        {
            return new Claim
            {
                UserName = user.Name,
            };
        }
    }
}