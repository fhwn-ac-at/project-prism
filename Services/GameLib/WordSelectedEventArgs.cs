namespace GameLib
{
    public class WordSelectedEventArgs
    {
        public required string SelectedWord { get; init; }

        public required string TextSelectedWord { get; init; }
        public required string Drawer {  get; init; }
    }
}