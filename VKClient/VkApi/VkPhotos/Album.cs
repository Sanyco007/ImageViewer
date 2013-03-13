using System;

namespace VKClient.VkApi.VkPhotos
{
    public struct Album
    {
        public int Aid            { get; set; }
        public int ThumbId        { get; set; }
        public int OwnerId        { get; set; }
        public string Title       { get; set; }
        public string Description { get; set; }
        public DateTime Created   { get; set; }
        public DateTime Updated   { get; set; }
        public int Size           { get; set; }
        public int Privacy        { get; set; }
        public string ThumbSrc    { get; set; }
    }  

}
