namespace DapperApi.Models
{
    public class GenelModel
    {
        public bool durum { get; set; } = true;
        public string mesaj { get; set; } = "Başarılı";
        public string hatamesaj { get; set; } = "";

        public object Data { get; set; }
    }
}
