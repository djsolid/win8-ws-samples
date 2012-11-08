using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TwitterSample.Models
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class TwitterModel : Common.BindableBase
    {

        private long _id;
        private string _dateCreated;
        private string _profileImageUrl;
        private string _text;
        private string _fromUserName;
        private ImageSource _tileImage;

        public TwitterModel(long id,
                            string dateCreated,
                            string profileImageUrl,
                            string text,
                            string fromUserName)
        {
           _id = id;
           _dateCreated = dateCreated;
           _profileImageUrl = profileImageUrl;
           _text = text;
           _fromUserName = fromUserName;
        }
        
        public string FromUserName
        {
            get { return this._fromUserName; }
            set { this.SetProperty(ref this._fromUserName, value); }
        }

       public string DateCreated
        {
            get
            {
                var date = DateTime.Parse(_dateCreated);
                return date.ToString();
            }
            set { SetProperty(ref _dateCreated, value); }
        }

        public long Id
        {
            get { return this._id; }
            set { this.SetProperty(ref this._id, value); }
        }

        public string ProfileImageUrl
        {
            get { return this._profileImageUrl; }
            set { this.SetProperty(ref this._profileImageUrl, value); }
        }
        
        public string Text
        {
            get { return this._text; }
            set { this.SetProperty(ref this._text, value); }
        }

        public ImageSource TileImage
        {
            get
            {
                if (this._tileImage == null && this._profileImageUrl != null)
                {
                    this._tileImage = new BitmapImage(new Uri(this._profileImageUrl));
                }
                return this._tileImage;
            }
            set
            {
                this._profileImageUrl = null;
                this.SetProperty(ref this._tileImage, value);
            }
        }
    }
}
