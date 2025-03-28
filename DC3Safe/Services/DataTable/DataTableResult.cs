namespace DC3Safe.Services.DataTable
{
    public class DataTableResult<T> : DataTableResultBase where T : class
    {
        public IList<T> data { get; set; }

        public DataTableResult()
        {
            data = new List<T>();
        }
    }
}
