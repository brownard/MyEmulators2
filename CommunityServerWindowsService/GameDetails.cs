using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CommunityServerWindowsService
{
    [DataContract(Namespace="http://myemulators2/v1", Name="GameDetails")]
    public class GameDetails : IExtensibleDataObject
    {
        private string _title = string.Empty;
        private string _yearMade = string.Empty;
        private string _grade = string.Empty;
        private string _genre = string.Empty;
        private string _imageFront = string.Empty;
        private string _imageBack = string.Empty;
        private string _imageTitleScreen = string.Empty;
        private string _imageIngame = string.Empty;
        private string _imageFanart = string.Empty;
        private string _manual = string.Empty;
        private string _hash = string.Empty;
        private string _filename = string.Empty;
        private string _description = string.Empty;
        private ExtensionDataObject _extensionData;

        [DataMember(Name = "Title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [DataMember(Name = "Year")]
        public string YearMade
        {
            get { return _yearMade; }
            set { _yearMade = value; }
        }

        [DataMember(Name = "Grade")]
        public string Grade
        {
            get { return _grade; }
            set { _grade = value; }
        }

        [DataMember(Name = "Description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DataMember(Name = "Genre")]
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        [DataMember(Name = "ImageFront")]
        public string ImageFront
        {
            get { return _imageFront; }
            set { _imageFront = value; }
        }

        [DataMember(Name = "ImageBack")]
        public string ImageBack
        {
            get { return _imageBack; }
            set { _imageBack = value; }
        }

        [DataMember(Name = "ImageTitleScreen")]
        public string ImageTitleScreen
        {
            get { return _imageTitleScreen; }
            set { _imageTitleScreen = value; }
        }

        [DataMember(Name = "ImageIngame")]
        public string ImageIngame
        {
            get { return _imageIngame; }
            set { _imageIngame = value; }
        }

        [DataMember(Name = "ImageFanart")]
        public string ImageFanart
        {
            get { return _imageFanart; }
            set { _imageFanart = value; }
        }

        [DataMember(Name = "Manual")]
        public string Manual
        {
            get { return _manual; }
            set { _manual = value; }
        }

        [DataMember(Name = "Hash")]
        public string Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        [DataMember(Name = "Filename")]
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public ExtensionDataObject ExtensionData
        {
            get { return _extensionData; }
            set { _extensionData = value; }
        }
    }
}
