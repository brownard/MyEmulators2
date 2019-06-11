using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MyEmulators2.Import.TheGamesDb
{
    public class ImageResult : ApiResult<ImageData>
    {
    }

    [DataContract]
    public class ImageData : ApiData
    {
        [DataMember(Name = "base_url")]
        public ImageBaseUrl BaseUrl { get; set; }

        [DataMember(Name = "images")]
        public Dictionary<string, Image[]> Images { get; set; }
    }

    [DataContract]
    public class ImageBaseUrl
    {
        [DataMember(Name = "original")]
        public string Original { get; set; }

        [DataMember(Name = "small")]
        public string Small { get; set; }

        [DataMember(Name = "thumb")]
        public string Thumb { get; set; }

        [DataMember(Name = "cropped_center_thumb")]
        public string CroppedCenterThumb { get; set; }

        [DataMember(Name = "medium")]
        public string Medium { get; set; }

        [DataMember(Name = "large")]
        public string Large { get; set; }
    }

    [DataContract]
    public class Image
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "side")]
        public string Side { get; set; }

        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        [DataMember(Name = "resolution")]
        public string Resolution { get; set; }
    }
}
