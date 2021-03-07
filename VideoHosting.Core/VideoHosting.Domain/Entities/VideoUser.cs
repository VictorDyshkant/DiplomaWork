using System;

namespace VideoHosting.Domain.Entities
{
    public class VideoUser
    {
        public Guid VideoId { get; set; }

        public virtual Video Video { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }


    public class UserUser
    {
        public string SubscriberId { get; set; }

        public virtual User Subscriber { get; set; }

        public string SubscripterId { get; set; }

        public virtual User Subscripter { get; set; }
    }

}
