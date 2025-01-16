namespace GameLib
{
    public class WordSelectionEventArgs(WordListItem[] wordListItems, string drawer)
    {
        public WordListItem[] WordListItems { get; init; } = wordListItems;
        public string Drawer { get; init; } = drawer;
    }
}