﻿using System;
using System.Threading.Tasks;

namespace RedditSharp
{
    public class SubredditImage
    {
        private const string DeleteImageUrl = "/api/delete_sr_img";

        private Reddit Reddit { get; set; }
        private IWebAgent WebAgent { get; set; }

        public SubredditImage(Reddit reddit, SubredditStyle subredditStyle,
            string cssLink, string name, IWebAgent webAgent)
        {
            Reddit = reddit;
            WebAgent = webAgent;
            SubredditStyle = subredditStyle;
            Name = name;
            CssLink = cssLink;
        }

        public SubredditImage(Reddit reddit, SubredditStyle subreddit,
            string cssLink, string name, string url, IWebAgent webAgent)
            : this(reddit, subreddit, cssLink, name, webAgent)
        {

            int discarded;
            if (int.TryParse(url, out discarded))
            {
                Url = new Uri(string.Format("http://thumbs.reddit.com/{0}_{1}.png", subreddit.Subreddit.FullName, url), UriKind.Absolute);
            }
            else
            {
                Url = new Uri(url);
            }
            // Handle legacy image urls
            // http://thumbs.reddit.com/FULLNAME_NUMBER.png
            
        }

        public string CssLink { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public SubredditStyle SubredditStyle { get; set; }

        public async Task Delete()
        {
            var request = WebAgent.CreatePost(DeleteImageUrl);
            WebAgent.WritePostBody(request, new
            {
                img_name = Name,
                uh = Reddit.User.Modhash,
                r = SubredditStyle.Subreddit.Name
            });
            var response = await WebAgent.GetResponseAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            SubredditStyle.Images.Remove(this);
        }
        public async Task DeleteAsync()
        {
            var request = WebAgent.CreatePost(DeleteImageUrl);
            WebAgent.WritePostBody(request, new
            {
                img_name = Name,
                uh = Reddit.User.Modhash,
                r = SubredditStyle.Subreddit.Name
            });
            var response = await WebAgent.GetResponseAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            SubredditStyle.Images.Remove(this);
        }
    }
}
