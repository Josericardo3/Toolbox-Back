using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.Base
{
    public class BaseResponseDTO
    {
        public BaseResponseDTO()
        {
            Exception = string.Empty;
            Confirmacion = false;
        }
        public string Exception { get; set; }
        public bool Confirmacion { get; set; }
        public string Mensaje { get; set; }
        public string? Texto { get; set; }
    }

    public class InformacionDTO<T> : BaseResponseDTO
    {
        public T Data { get; set; }
    }

    public class TablaDTO<T> : BaseResponseDTO
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
    public class BaseComboDTO<T> : BaseResponseDTO
    {
        public IEnumerable<T> Data { get; set; }
    }
    public class BaseInformacionComboDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Codigo { get; set; }
    }
}
