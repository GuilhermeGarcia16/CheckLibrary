namespace CheckLibrary.Data
{
    public class StatusMessage<TData>
    {
        public bool Ok { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }
        public StatusMessage(){}
        public StatusMessage(TData data, bool ok, string message)
        {
            Data = data;
            Ok = ok;
            Message = message;
        }
    }
}