namespace LargeFileViewer.ViewModel.ViewStates
{
    class InitialViewModel : ViewStateViewModel
    {
        private const string Info = "Drop your file here";

        public string Message
        {
            get { return Info; }
        }
    }
}
