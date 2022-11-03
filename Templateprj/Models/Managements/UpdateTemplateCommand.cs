namespace Templateprj.Models.Managements
{
    public class UpdateTemplateCommand
    {
        public int Id { get; set; }    
        public string BlackListedBy { get; set; }
        public int Status { get; set; }
    }
}