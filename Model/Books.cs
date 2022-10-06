namespace BookLibrary1.Model
{
    public class Books
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        private string _BookImageURL;

        public string BookImageURL
        {
            get { return AppSettings.DefaultEndpoint+_BookImageURL; }
            set { _BookImageURL = value; }
        }
        public int BookCount { get; set; }
        public BookType Category { get; set; }
        public KeepType KeepType { get; set; }
        public string InsertedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }

    public enum BookType
    {
        Finance,
        Programming,
        Language,
        Story,
        Novels

    }

    public enum KeepType
    {
        Rent,
        Purchase
    }
}
