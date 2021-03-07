using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoHosting.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool Sex { get; set; }

        public string Faculty { get; set; }

        public string Group { get; set; }

        public int? TempPassword { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string PhotoPath { get; set; }

        public virtual List<Video> Videos { get; set; }

        public virtual List<Commentary> Commentaries { get; set; }

        public virtual List<UserUser> Subscribers { get; set; }

        public virtual List<UserUser> Subscriptions { get; set; }

        public virtual List<VideoUser> Likes { get; set; }

        public virtual List<VideoUser> Dislikes { get; set; }

        public User()
        {
            Videos = new List<Video>();
            Commentaries = new List<Commentary>();

            Subscribers = new List<UserUser>();
            Subscriptions = new List<UserUser>();

            Likes = new List<VideoUser>();
            Dislikes = new List<VideoUser>();
        }

        public void AddLike(Video video)
        {
            if (Dislikes.FirstOrDefault(x => x.Video == video) != null)
            {
                Dislikes.Remove(Dislikes.FirstOrDefault(x => x.Video == video));
            }
            Likes.Add(new VideoUser() { Video = video, User = this });
        }

        public void DeleteLike(Video video)
        {
            Dislikes.Remove(Dislikes.FirstOrDefault(x => x.Video == video));
        }

        public void AddDislike(Video video)
        {
            if (Likes.FirstOrDefault(x => x.Video == video) != null)
            {
                Likes.Remove(Likes.FirstOrDefault(x => x.Video == video));
            }

            Dislikes.Add(new VideoUser { Video = video, User = this });
        }

        public void DeleteDislike(Video video)
        {
            Dislikes.Remove(Likes.FirstOrDefault(x => x.Video == video));
        }

        public void Subscribe(User user)
        {
            if (Subscriptions.FirstOrDefault(x => x.Subscripter == user) == null)
            {
                Subscriptions.Add(new UserUser { Subscriber = this, Subscripter = user });
            }
        }

        public void Unsubscribe(User user)
        {
            if (Subscriptions.FirstOrDefault(x => x.Subscripter == user) != null)
            {
                Subscribers.Remove(Subscriptions.FirstOrDefault(x => x.Subscripter == user));
            }
        }
    }
}
