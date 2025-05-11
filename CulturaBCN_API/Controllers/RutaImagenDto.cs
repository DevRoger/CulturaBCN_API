using Newtonsoft.Json;

namespace CulturaBCN_API.Controllers
{

    public class RutaImagenDto
    {
        [JsonProperty("foto_url")]
        public string Foto_url { get; set; }
        public RutaImagenDto() { }
        public RutaImagenDto(string foto_url)
        {
            Foto_url = foto_url;
        }
    } 
}