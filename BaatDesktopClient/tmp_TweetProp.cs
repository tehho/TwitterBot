using System;
using System.Collections.Generic;
using System.Text;

namespace BaatDesktopClient
{
        public class Rootobject
        {
            public Class1[] Property1 { get; set; }
        }

        public class Class1
        {
            public long id { get; set; }
            public string id_str { get; set; }
            public object text { get; set; }
            public string full_text { get; set; }
            public int[] display_text_range { get; set; }
            public object extended_tweet { get; set; }
            public bool favorited { get; set; }
            public int favorite_count { get; set; }
            public User user { get; set; }
            public object current_user_retweet { get; set; }
            public object coordinates { get; set; }
            public Entities1 entities { get; set; }
            public object extended_entities { get; set; }
            public DateTime created_at { get; set; }
            public bool truncated { get; set; }
            public object in_reply_to_status_id { get; set; }
            public object in_reply_to_status_id_str { get; set; }
            public object in_reply_to_user_id { get; set; }
            public object in_reply_to_user_id_str { get; set; }
            public object in_reply_to_screen_name { get; set; }
            public object quoted_status_id { get; set; }
            public object quoted_status_id_str { get; set; }
            public object quoted_status { get; set; }
            public int retweet_count { get; set; }
            public bool retweeted { get; set; }
            public Retweeted_Status retweeted_status { get; set; }
            public bool possibly_sensitive { get; set; }
            public int lang { get; set; }
            public object contributorsIds { get; set; }
            public object contributors { get; set; }
            public string source { get; set; }
            public object place { get; set; }
            public object scopes { get; set; }
            public object filter_level { get; set; }
            public bool withheld_copyright { get; set; }
            public object withheld_in_countries { get; set; }
            public object withheld_scope { get; set; }
        }

        public class User
        {
            public string name { get; set; }
            public object status { get; set; }
            public string description { get; set; }
            public DateTime created_at { get; set; }
            public string location { get; set; }
            public bool geo_enabled { get; set; }
            public string url { get; set; }
            public int lang { get; set; }
            public object email { get; set; }
            public int statuses_count { get; set; }
            public int followers_count { get; set; }
            public int friends_count { get; set; }
            public bool following { get; set; }
            public bool _protected { get; set; }
            public bool verified { get; set; }
            public Entities entities { get; set; }
            public bool notifications { get; set; }
            public string profile_image_url { get; set; }
            public string profile_image_url_https { get; set; }
            public bool follow_request_sent { get; set; }
            public bool default_profile { get; set; }
            public bool default_profile_image { get; set; }
            public int favourites_count { get; set; }
            public int listed_count { get; set; }
            public string profile_sidebar_fill_color { get; set; }
            public string profile_sidebar_border_color { get; set; }
            public bool profile_background_tile { get; set; }
            public string profile_background_color { get; set; }
            public string profile_background_image_url { get; set; }
            public string profile_background_image_url_https { get; set; }
            public string profile_banner_url { get; set; }
            public string profile_text_color { get; set; }
            public string profile_link_color { get; set; }
            public bool profile_use_background_image { get; set; }
            public bool is_translator { get; set; }
            public bool contributors_enabled { get; set; }
            public int utc_offset { get; set; }
            public string time_zone { get; set; }
            public object withheld_in_countries { get; set; }
            public object withheld_scope { get; set; }
            public int Id { get; set; }
            public string id_str { get; set; }
            public string screen_name { get; set; }
        }

        public class Entities
        {
            public Url url { get; set; }
            public Description description { get; set; }
        }

        public class Url
        {
            public Url1[] urls { get; set; }
        }

        public class Url1
        {
            public string url { get; set; }
            public string display_url { get; set; }
            public string expanded_url { get; set; }
            public int[] indices { get; set; }
        }

        public class Description
        {
            public object[] urls { get; set; }
        }

        public class Entities1
        {
            public object[] urls { get; set; }
            public User_Mentions[] user_mentions { get; set; }
            public object[] hashtags { get; set; }
            public object[] symbols { get; set; }
            public object media { get; set; }
        }

        public class User_Mentions
        {
            public int id { get; set; }
            public string id_str { get; set; }
            public string screen_name { get; set; }
            public string name { get; set; }
            public int[] indices { get; set; }
        }

        public class Retweeted_Status
        {
            public long id { get; set; }
            public string id_str { get; set; }
            public object text { get; set; }
            public string full_text { get; set; }
            public int[] display_text_range { get; set; }
            public object extended_tweet { get; set; }
            public bool favorited { get; set; }
            public int favorite_count { get; set; }
            public User1 user { get; set; }
            public object current_user_retweet { get; set; }
            public object coordinates { get; set; }
            public Entities3 entities { get; set; }
            public Extended_Entities extended_entities { get; set; }
            public DateTime created_at { get; set; }
            public bool truncated { get; set; }
            public object in_reply_to_status_id { get; set; }
            public object in_reply_to_status_id_str { get; set; }
            public object in_reply_to_user_id { get; set; }
            public object in_reply_to_user_id_str { get; set; }
            public object in_reply_to_screen_name { get; set; }
            public object quoted_status_id { get; set; }
            public object quoted_status_id_str { get; set; }
            public object quoted_status { get; set; }
            public int retweet_count { get; set; }
            public bool retweeted { get; set; }
            public object retweeted_status { get; set; }
            public bool possibly_sensitive { get; set; }
            public int lang { get; set; }
            public object contributorsIds { get; set; }
            public object contributors { get; set; }
            public string source { get; set; }
            public object place { get; set; }
            public object scopes { get; set; }
            public object filter_level { get; set; }
            public bool withheld_copyright { get; set; }
            public object withheld_in_countries { get; set; }
            public object withheld_scope { get; set; }
        }

        public class User1
        {
            public string name { get; set; }
            public object status { get; set; }
            public string description { get; set; }
            public DateTime created_at { get; set; }
            public string location { get; set; }
            public bool geo_enabled { get; set; }
            public string url { get; set; }
            public int lang { get; set; }
            public object email { get; set; }
            public int statuses_count { get; set; }
            public int followers_count { get; set; }
            public int friends_count { get; set; }
            public bool following { get; set; }
            public bool _protected { get; set; }
            public bool verified { get; set; }
            public Entities2 entities { get; set; }
            public bool notifications { get; set; }
            public string profile_image_url { get; set; }
            public string profile_image_url_https { get; set; }
            public bool follow_request_sent { get; set; }
            public bool default_profile { get; set; }
            public bool default_profile_image { get; set; }
            public int favourites_count { get; set; }
            public int listed_count { get; set; }
            public string profile_sidebar_fill_color { get; set; }
            public string profile_sidebar_border_color { get; set; }
            public bool profile_background_tile { get; set; }
            public string profile_background_color { get; set; }
            public string profile_background_image_url { get; set; }
            public string profile_background_image_url_https { get; set; }
            public string profile_banner_url { get; set; }
            public string profile_text_color { get; set; }
            public string profile_link_color { get; set; }
            public bool profile_use_background_image { get; set; }
            public bool is_translator { get; set; }
            public bool contributors_enabled { get; set; }
            public int utc_offset { get; set; }
            public string time_zone { get; set; }
            public object withheld_in_countries { get; set; }
            public object withheld_scope { get; set; }
            public int Id { get; set; }
            public string id_str { get; set; }
            public string screen_name { get; set; }
        }

        public class Entities2
        {
            public Url2 url { get; set; }
            public Description1 description { get; set; }
        }

        public class Url2
        {
            public Url3[] urls { get; set; }
        }

        public class Url3
        {
            public string url { get; set; }
            public string display_url { get; set; }
            public string expanded_url { get; set; }
            public int[] indices { get; set; }
        }

        public class Description1
        {
            public object[] urls { get; set; }
        }

        public class Entities3
        {
            public object[] urls { get; set; }
            public User_Mentions1[] user_mentions { get; set; }
            public object[] hashtags { get; set; }
            public object[] symbols { get; set; }
            public Medium[] media { get; set; }
        }

        public class User_Mentions1
        {
            public int id { get; set; }
            public string id_str { get; set; }
            public string screen_name { get; set; }
            public string name { get; set; }
            public int[] indices { get; set; }
        }

        public class Medium
        {
            public long id { get; set; }
            public string id_str { get; set; }
            public string url { get; set; }
            public string display_url { get; set; }
            public string expanded_url { get; set; }
            public string media_url { get; set; }
            public string media_url_https { get; set; }
            public string type { get; set; }
            public int[] indices { get; set; }
            public Sizes sizes { get; set; }
            public object video_info { get; set; }
        }

        public class Sizes
        {
            public Thumb thumb { get; set; }
            public Small small { get; set; }
            public Large large { get; set; }
            public Medium1 medium { get; set; }
        }

        public class Thumb
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Small
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Large
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Medium1
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Extended_Entities
        {
            public object urls { get; set; }
            public object user_mentions { get; set; }
            public object hashtags { get; set; }
            public object symbols { get; set; }
            public Medium2[] media { get; set; }
        }

        public class Medium2
        {
            public long id { get; set; }
            public string id_str { get; set; }
            public string url { get; set; }
            public string display_url { get; set; }
            public string expanded_url { get; set; }
            public string media_url { get; set; }
            public string media_url_https { get; set; }
            public string type { get; set; }
            public int[] indices { get; set; }
            public Sizes1 sizes { get; set; }
            public object video_info { get; set; }
        }

        public class Sizes1
        {
            public Thumb1 thumb { get; set; }
            public Small1 small { get; set; }
            public Large1 large { get; set; }
            public Medium3 medium { get; set; }
        }

        public class Thumb1
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Small1
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Large1
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

        public class Medium3
        {
            public int w { get; set; }
            public int h { get; set; }
            public string resize { get; set; }
        }

    
}
